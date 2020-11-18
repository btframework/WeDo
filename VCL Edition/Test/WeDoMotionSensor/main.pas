unit main;

{$I ..\..\..\..\..\..\WCL7\VCL\Source\wcl.inc}

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, wclBluetooth, wclWeDoWatcher, wclWeDoHub;

type
  TfmMain = class(TForm)
    laStatus: TLabel;
    laIoState: TLabel;
    btConnect: TButton;
    btDisconnect: TButton;
    laMode: TLabel;
    cbMode: TComboBox;
    btChange: TButton;
    btReset: TButton;
    laCountTitle: TLabel;
    laDistanceTitle: TLabel;
    laCount: TLabel;
    laDistance: TLabel;
    procedure FormCreate(Sender: TObject);
    procedure btDisconnectClick(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btConnectClick(Sender: TObject);
    procedure btChangeClick(Sender: TObject);
    procedure btResetClick(Sender: TObject);

  private
    FManager: TwclBluetoothManager;
    FWatcher: TwclWeDoWatcher;
    FHub: TwclWeDoHub;
    FMotion: TwclWeDoMotionSensor;

    procedure EnableControl(Attached: Boolean);
    procedure EnableConnect(Connected: Boolean);

    procedure FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDisconnected(Sender: TObject; Reason: Integer);
    procedure FHub_OnConnected(Sender: TObject; Error: Integer);

    procedure FWatcher_OnHubFound(Sender: TObject; Address: Int64;
      Name: string);

    procedure FMotion_OnModeChanged(Sender: TObject);
    procedure FMotion_OnDistanceChanged(Sender: TObject);
    procedure FMotion_OnCountChanged(Sender: TObject);

    procedure Disconnect;
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors;

{$R *.dfm}

procedure TfmMain.btChangeClick(Sender: TObject);
var
  Mode: TwclWeDoMotionSensorMode;
  Res: Integer;
begin
  if FMotion = nil then
    ShowMessage('Device is not attached')
  else begin
    case cbMode.ItemIndex of
      0: Mode := mmDetect;
      1: Mode := mmCount;
      else Mode := mmUnknown;
    end;
    if Mode = mmUnknown then
      ShowMessage('Invalid mode.')
    else begin
      Res := FMotion.SetMode(Mode);
      if Res <> WCL_E_SUCCESS then
        ShowMessage('Mode change failed: 0x' + IntToHex(Res, 8));
    end;
  end;
end;

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

procedure TfmMain.btResetClick(Sender: TObject);
var
  Res: Integer;
begin
  if FMotion = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotion.Reset;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Mode change failed: 0x' + IntToHex(Res, 8));
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

procedure TfmMain.EnableControl(Attached: Boolean);
begin
  if Attached then
    laIoState.Caption := 'Attached'
  else begin
    laIoState.Caption := 'Dectahed';

    laCount.Caption := '0';
    laDistance.Caption := '0';
  end;

  laMode.Enabled := Attached;
  cbMode.Enabled := Attached;
  btChange.Enabled := Attached;
  laCountTitle.Enabled := Attached;
  laCount.Enabled := Attached;
  laDistanceTitle.Enabled := Attached;
  laDistance.Enabled := Attached;
  btReset.Enabled := Attached;
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
  if FMotion = nil then begin
    if Device.DeviceType = iodWeDo20MotionSensor then begin
      FMotion := TwclWeDoMotionSensor(Device);
      FMotion.OnCountChanged := FMotion_OnCountChanged;
      FMotion.OnDistanceChanged := FMotion_OnDistanceChanged;
      FMotion.OnModeChanged := FMotion_OnModeChanged;
      EnableControl(True);
    end;
  end;
end;

procedure TfmMain.FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
begin
  if (Device.DeviceType = iodWeDo20MotionSensor) and (FMotion <> nil) and (Device.ConnectionId = FMotion.ConnectionId) then begin
    FMotion := nil;
    EnableControl(False);
  end;
end;

procedure TfmMain.FHub_OnDisconnected(Sender: TObject; Reason: Integer);
begin
  EnableConnect(False);
  FManager.Close;
end;

procedure TfmMain.FMotion_OnCountChanged(Sender: TObject);
begin
  laCount.Caption := IntToStr(FMotion.Count);
end;

procedure TfmMain.FMotion_OnDistanceChanged(Sender: TObject);
begin
  laDistance.Caption := FloatToStr(FMotion.Distance);
end;

procedure TfmMain.FMotion_OnModeChanged(Sender: TObject);
begin
  case FMotion.Mode of
    mmDetect: cbMode.ItemIndex := 0;
    mmCount: cbMode.ItemIndex := 1;
    else cbMode.ItemIndex := -1;
  end;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  cbMode.ItemIndex := 0;

  FManager := TwclBluetoothManager.Create(nil);

  FWatcher := TwclWeDoWatcher.Create(nil);
  FWatcher.OnHubFound := FWatcher_OnHubFound;

  FHub := TwclWeDoHub.Create(nil);
  FHub.OnConnected := FHub_OnConnected;
  FHub.OnDisconnected := FHub_OnDisconnected;
  FHub.OnDeviceAttached := FHub_OnDeviceAttached;
  FHub.OnDeviceDetached := FHub_OnDeviceDetached;

  FMotion := nil;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  Disconnect;

  FManager.Free;
  FWatcher.Free;
  FHub.Free;
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

end.
