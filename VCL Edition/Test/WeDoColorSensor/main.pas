unit main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, wclWeDoHub, Vcl.StdCtrls, wclBluetooth,
  wclWeDoWatcher;

type
  TfmMain = class(TForm)
    btConnect: TButton;
    btDisconnect: TButton;
    laStatus: TLabel;
    laIoState: TLabel;
    procedure btConnectClick(Sender: TObject);
    procedure btDisconnectClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);

  private
    FManager: TwclBluetoothManager;
    FWatcher: TwclWeDoWatcher;
    FHub: TwclWeDoHub;
    FColor: TwclWeDoColorSensor;
    FRgb: TwclWeDoRgbLight;

    procedure EnableControl(Attached: Boolean);
    procedure EnableConnect(Connected: Boolean);

    procedure FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDisconnected(Sender: TObject; Reason: Integer);
    procedure FHub_OnConnected(Sender: TObject; Error: Integer);

    procedure FWatcher_OnHubFound(Sender: TObject; Address: Int64;
      Name: string);

    procedure FColor_OnColorDetected(Sender: TObject;
      Color: TwclWeDoColorSensorColor);

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
  else
    laIoState.Caption := 'Dectahed';
end;

procedure TfmMain.FColor_OnColorDetected(Sender: TObject;
  Color: TwclWeDoColorSensorColor);
begin
  if FRgb <> nil then begin
    case Color of
      ccBlue:
        FRgb.SetColorIndex(TwclWeDoColor.wclBlue);
      ccGreen:
        FRgb.SetColorIndex(TwclWeDoColor.wclGreen);
      ccRed:
        FRgb.SetColorIndex(TwclWeDoColor.wclRed);
      ccWhite:
        FRgb.SetColorIndex(TwclWeDoColor.wclWhite);
      ccYellow:
        FRgb.SetColorIndex(TwclWeDoColor.wclYellow);
      else
        FRgb.SetColorIndex(TwclWeDoColor.wclBlack);
    end;
  end;
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
  if FColor = nil then begin
    if Device.DeviceType = iodColorSensor then begin
      FColor := TwclWeDoColorSensor(Device);
      FColor.OnColorDetected := FColor_OnColorDetected;
      EnableControl(True);
    end;
  end;

  if Device.DeviceType = iodRgb then
    FRgb := TwclWeDoRgbLight(Device);
end;

procedure TfmMain.FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
begin
  if (Device.DeviceType = iodColorSensor) and (FColor <> nil) and (Device.ConnectionId = FColor.ConnectionId) then begin
    FColor := nil;
    EnableControl(False);
  end;

  if Device.DeviceType = iodRgb then
    FRgb := nil;
end;

procedure TfmMain.FHub_OnDisconnected(Sender: TObject; Reason: Integer);
begin
  EnableConnect(False);
  FManager.Close;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  FManager := TwclBluetoothManager.Create(nil);

  FWatcher := TwclWeDoWatcher.Create(nil);
  FWatcher.OnHubFound := FWatcher_OnHubFound;

  FHub := TwclWeDoHub.Create(nil);
  FHub.OnConnected := FHub_OnConnected;
  FHub.OnDisconnected := FHub_OnDisconnected;
  FHub.OnDeviceAttached := FHub_OnDeviceAttached;
  FHub.OnDeviceDetached := FHub_OnDeviceDetached;

  FColor:= nil;
  FRgb := nil;
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
