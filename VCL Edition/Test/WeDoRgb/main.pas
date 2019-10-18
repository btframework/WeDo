unit main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  wclBluetooth, wclWeDoWatcher, wclWeDoHub, Vcl.StdCtrls;

type
  TfmMain = class(TForm)
    btConnect: TButton;
    btDisconnect: TButton;
    laStatus: TLabel;
    laIoState: TLabel;
    laColorMode: TLabel;
    cbColorMode: TComboBox;
    laR: TLabel;
    edR: TEdit;
    laG: TLabel;
    edG: TEdit;
    laB: TLabel;
    edB: TEdit;
    btSetRgb: TButton;
    laColorIndex: TLabel;
    cbColorIndex: TComboBox;
    btSetIndex: TButton;
    btSetDefault: TButton;
    btTurnOff: TButton;
    btSetMode: TButton;
    procedure FormCreate(Sender: TObject);
    procedure btDisconnectClick(Sender: TObject);
    procedure btConnectClick(Sender: TObject);
    procedure btSetRgbClick(Sender: TObject);
    procedure btSetDefaultClick(Sender: TObject);
    procedure btTurnOffClick(Sender: TObject);
    procedure btSetIndexClick(Sender: TObject);
    procedure btSetModeClick(Sender: TObject);

  private
    FManager: TwclBluetoothManager;
    FWatcher: TwclWeDoWatcher;
    FHub: TwclWeDoHub;
    FRgb: TwclWeDoRgbLight;

    procedure UpdateMode;
    procedure UpdateRgb;
    procedure UpdateIndex;
    procedure UpdateValues;

    procedure EnableSetColors(Attached: Boolean);
    procedure EnableConnect(Connected: Boolean);
    procedure EnableColorControls;

    procedure FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDisconnected(Sender: TObject; Reason: Integer);
    procedure FHub_OnConnected(Sender: TObject; Error: Integer);

    procedure FRgb_OnColorChanged(Sender: TObject);
    procedure FRgb_OnModeChanged(Sender: TObject);

    procedure FWatcher_OnHubFound(Sender: TObject; Address: Int64;
      Name: string);

    procedure Disconnect;
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors;

{$R *.dfm}

procedure TfmMain.btConnectClick(Sender: TObject);
var
  Res: Integer;
  Radio: TwclBluetoothRadio;
  i: Integer;
begin
  // The very first thing we have to do is to open Bluetooth Manager.
  // That initializes the underlying drivers and allows us to work with
  // Bluetooth.

  // Always check result!
  Res := FManager.Open;
  if Res <> WCL_E_SUCCESS then
    // It should never happen but if it does notify user.
    ShowMessage('Unable to open Bluetooth Manager: 0x' + IntToHex(Res, 8))

  else begin
    // Assume that no one Bluetooth Radio available.
    Radio := nil;

    // Check that at least one Bluetooth Radio exists (or at least Bluetooth
    // drivers installed).
    if FManager.Count = 0 then
      // No one, even drivers?

      ShowMessage('No Bluetooth Hardware installed')
    else begin
      // Ok, at least one Bluetooth Radio module should be available.
      for i := 0 to FManager.Count - 1 do begin
        // Check if current Radio module is available (plugged in and turned ON).
        if FManager[i].Available then begin
          // Looks like we have Bluetooth on this PC!
          Radio := FManager[i];
          // Terminate the loop.
          Break;
        end;
      end;

      // Check that we found the Bluetooth Radio module.
      if Radio = nil then
        // If not, let user know that he has no Bluetooth.
        ShowMessage('No available Bluetooth Radio found')

      else begin
        // If found, try to start discovering.
        Res := FWatcher.Start(Radio);
        if Res <> WCL_E_SUCCESS then begin
          // It is something wrong with discovering starting. Notify user about
          // the error.
          ShowMessage('Unable to start discovering: 0x' + IntToHex(Res, 8));
          // Also clean up found Radio variable so we can check it later.
          Radio := nil;

        end else begin
          btConnect.Enabled := False;
          btDisconnect.Enabled := True;
          laStatus.Caption := 'Searching...';
        end;
      end;
    end;

    // Again, check the found Radio.
    if Radio = nil then
      // And if it is null (not found or discovering was not started
      // close the Bluetooth Manager to release all the allocated resources.
      FManager.Close;
  end;
end;

procedure TfmMain.btDisconnectClick(Sender: TObject);
begin
  Disconnect;
end;

procedure TfmMain.btSetDefaultClick(Sender: TObject);
var
  Res: Integer;
begin
  if FRgb = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FRgb.SwitchToDefaultColor;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set default color: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btSetIndexClick(Sender: TObject);
var
  Res: Integer;
begin
  if FRgb = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FRgb.SetColorIndex(TwclWeDoColor(cbColorIndex.ItemIndex));
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set default color: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btSetModeClick(Sender: TObject);
var
  Res: Integer;
  RgbEnabled: Boolean;
  IndexEnabled: Boolean;
begin
  if FRgb = nil then
    ShowMessage('Device is not attached')

  else begin
    RgbEnabled := cbColorMode.ItemIndex = 1;
    IndexEnabled := cbColorMode.ItemIndex = 0;

    Res := WCL_E_SUCCESS;
    if RgbEnabled then
      Res := FRgb.SetMode(lmAbsolute)
    else begin
      if IndexEnabled then
        Res := FRgb.SetMode(lmDiscrete);
    end;

    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to change color mode: 0x' + IntToHex(Res, 8))

    else begin
      EnableColorControls;

      if RgbEnabled then
        UpdateRgb
      else begin
        if IndexEnabled then
          UpdateIndex;
      end;
    end;
  end;
end;

procedure TfmMain.btSetRgbClick(Sender: TObject);
var
  c: TColor;
  Res: Integer;
begin
  if FRgb = nil then
    ShowMessage('Device is not attached')
  else begin
    c := Rgb(StrToInt(edR.Text), StrToInt(edG.Text), StrToInt(edB.Text));
    Res := FRgb.SetColor(c);
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set color: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btTurnOffClick(Sender: TObject);
var
  Res: Integer;
begin
  if FRgb = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FRgb.SwitchOff;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set color: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.Disconnect;
begin
  FWatcher.Stop;
  FHub.Disconnect;
  FManager.Close;

  btConnect.Enabled := True;
  btDisconnect.Enabled := False;
end;

procedure TfmMain.EnableColorControls;
var
  RgbEnabled: Boolean;
  IndexEnabled: Boolean;
begin
  RgbEnabled := cbColorMode.ItemIndex = 1;
  IndexEnabled := cbColorMode.ItemIndex = 0;

  laR.Enabled := RgbEnabled;
  laG.Enabled := RgbEnabled;
  laB.Enabled := RgbEnabled;
  edR.Enabled := RgbEnabled;
  edG.Enabled := RgbEnabled;
  edB.Enabled := RgbEnabled;
  btSetRgb.Enabled := RgbEnabled;

  laColorIndex.Enabled := IndexEnabled;
  cbColorIndex.Enabled := IndexEnabled;
  btSetIndex.Enabled := IndexEnabled;
end;

procedure TfmMain.EnableConnect(Connected: Boolean);
begin
  if Connected then begin
    btConnect.Enabled := False;
    btDisconnect.Enabled := True;
    laStatus.Caption := 'Connected';
  end else begin
    btConnect.Enabled := True;
    btDisconnect.Enabled := False;
    laStatus.Caption := 'Disconnected';
  end;
end;

procedure TfmMain.EnableSetColors(Attached: Boolean);
begin
  if Attached then
    laIoState.Caption := 'Attached'
  else begin
    laIoState.Caption := 'Dectahed';

    edR.Text := '';
    edG.Text := '';
    edB.Text := '';

    cbColorIndex.ItemIndex := -1;
    cbColorMode.ItemIndex := -1;
  end;

  cbColorMode.Enabled := Attached;
  laColorMode.Enabled := Attached;

  laR.Enabled := Attached;
  laG.Enabled := Attached;
  laB.Enabled := Attached;
  edR.Enabled := Attached;
  edG.Enabled := Attached;
  edB.Enabled := Attached;
  btSetRgb.Enabled := Attached;

  laColorIndex.Enabled := Attached;
  cbColorIndex.Enabled := Attached;
  btSetIndex.Enabled := Attached;

  btSetDefault.Enabled := Attached;
  btTurnOff.Enabled := Attached;
  btSetMode.Enabled := Attached;

  if Attached then
    UpdateValues;
end;

procedure TfmMain.FHub_OnConnected(Sender: TObject; Error: Integer);
begin
  if Error <> WCL_E_SUCCESS then begin
    ShowMessage('Connect failed: 0x' + IntToHex(Error, 8));
    EnableConnect(False);
    FManager.Close;
  end else
    EnableConnect(True);
end;

procedure TfmMain.FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
begin
  if Device.DeviceType = iodRgb then begin
    FRgb := TwclWeDoRgbLight(Device);
    FRgb.OnColorChanged := FRgb_OnColorChanged;
    FRgb.OnModeChanged := FRgb_OnModeChanged;

    EnableSetColors(True);
    EnableColorControls;
  end;
end;

procedure TfmMain.FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
begin
  if Device.DeviceType = iodRgb then begin
    FRgb := nil;
    EnableSetColors(False);
  end;
end;

procedure TfmMain.FHub_OnDisconnected(Sender: TObject; Reason: Integer);
begin
  EnableConnect(False);
  FManager.Close;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  cbColorMode.ItemIndex := -1;

  FManager := TwclBluetoothManager.Create(nil);

  FWatcher := TwclWeDoWatcher.Create(nil);
  FWatcher.OnHubFound := FWatcher_OnHubFound;

  FHub := TwclWeDoHub.Create(nil);
  FHub.OnConnected := FHub_OnConnected;
  FHub.OnDisconnected := FHub_OnDisconnected;
  FHub.OnDeviceAttached := FHub_OnDeviceAttached;
  FHub.OnDeviceDetached := FHub_OnDeviceDetached;

  FRgb := nil;
end;

procedure TfmMain.FRgb_OnColorChanged(Sender: TObject);
begin
  UpdateRgb;
  UpdateIndex;
end;

procedure TfmMain.FRgb_OnModeChanged(Sender: TObject);
begin
  UpdateMode;
end;

procedure TfmMain.FWatcher_OnHubFound(Sender: TObject; Address: Int64;
  Name: string);
var
  Radio: TwclBluetoothRadio;
  Res: Integer;
begin
  Radio := FWatcher.Radio;
  FWatcher.Stop;
  Res := FHub.Connect(Radio, Address);
  if Res <> WCL_E_SUCCESS then begin
    ShowMessage('Connect failed: 0x' + IntToHex(Res, 8));
    EnableConnect(False);
  end else
    laStatus.Caption := 'Connecting';
end;

procedure TfmMain.UpdateIndex;
begin
  cbColorIndex.ItemIndex := Integer(FRgb.ColorIndex);
end;

procedure TfmMain.UpdateMode;
begin
  if FRgb <> nil then begin
    case FRgb.Mode of
      lmDiscrete:
        cbColorMode.ItemIndex := 0;
      lmAbsolute:
        cbColorMode.ItemIndex := 1;
      else
        cbColorMode.ItemIndex := -1;
    end;
  end;
end;

procedure TfmMain.UpdateRgb;
var
  c: TColor;
begin
  c := FRgb.Color;
  edR.Text := IntToStr(c and $000000FF);
  edG.Text := IntToStr((c shr 8) and $000000FF);
  edB.Text := IntToStr((c shr 16) and $000000FF);
end;

procedure TfmMain.UpdateValues;
begin
  UpdateRgb;
  UpdateIndex;
  UpdateMode;
end;

end.
