unit main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, wclBluetooth, wclWeDoWatcher, wclWeDoHub;

type
  TfmMain = class(TForm)
    btConnect: TButton;
    btDisconnect: TButton;
    laStatus: TLabel;
    laIoState: TLabel;
    laCurrentTitle: TLabel;
    laCurrent: TLabel;
    laMA: TLabel;
    laVoltageTitle: TLabel;
    laVoltage: TLabel;
    laMV: TLabel;
    laDirection: TLabel;
    cbDirection: TComboBox;
    laPower: TLabel;
    edPower: TEdit;
    btStart: TButton;
    btBrake: TButton;
    btDrift: TButton;
    laHighCurrent: TLabel;
    laLowVoltage: TLabel;
    procedure FormCreate(Sender: TObject);
    procedure btDisconnectClick(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btConnectClick(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure btBrakeClick(Sender: TObject);
    procedure btDriftClick(Sender: TObject);

  private
    FManager: TwclBluetoothManager;
    FWatcher: TwclWeDoWatcher;
    FHub: TwclWeDoHub;
    FMotor: TwclWeDoMotor;
    FCurrent: TwclWeDoCurrentSensor;
    FVoltage: TwclWeDoVoltageSensor;

    procedure FHub_OnLowVoltageAlert(Sender: TObject; const Alert: Boolean);
    procedure FHub_OnHighCurrentAlert(Sender: TObject; const Alert: Boolean);
    procedure FHub_OnDeviceDetached(Sender: TObject; const Device: TwclWeDoIo);
    procedure FHub_OnDeviceAttached(Sender: TObject; const Device: TwclWeDoIo);
    procedure FHub_OnDisconnected(Sender: TObject; const Reason: Integer);
    procedure FHub_OnConnected(Sender: TObject; const Error: Integer);

    procedure FVoltage_OnVoltageChanged(Sender: TObject);
    procedure FCurrent_OnCurrentChanged(Sender: TObject);

    procedure FWatcher_OnHubFound(Sender: TObject; const Address: Int64;
      const Name: string);

    procedure EnablePlay(const Attached: Boolean);
    procedure EnableConnect(Connected: Boolean);

    procedure Disconnect;
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors;

{$R *.dfm}

procedure TfmMain.btBrakeClick(Sender: TObject);
var
  Res: Integer;
begin
  if FMotor = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotor.Brake;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Brake motor failed: 0x' + IntToHex(Res, 8));
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

procedure TfmMain.btDriftClick(Sender: TObject);
var
  Res: Integer;
begin
  if FMotor = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotor.Drift;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Drift motor failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btStartClick(Sender: TObject);
var
  Dir: TwclWeDoMotorDirection;
  Res: Integer;
begin
  if FMotor = nil then
    ShowMessage('Device is not attached')
  else begin
    case cbDirection.ItemIndex of
      0: Dir := mdRight;
      1: Dir := mdLeft;
      else Dir := mdUnknown;
    end;
    Res := FMotor.Run(Dir, StrToInt(edPower.Text));
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Start motor failed: 0x' + IntToHex(Res, 8));
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

procedure TfmMain.EnablePlay(const Attached: Boolean);
begin
  if Attached then
    laIoState.Caption := 'Attached'
  else begin
    laIoState.Caption := 'Dectahed';

    laCurrent.Caption := '0';
    laVoltage.Caption := '0';

    laHighCurrent.Visible := False;
    laLowVoltage.Visible := False;
  end;

  laDirection.Enabled := Attached;
  cbDirection.Enabled := Attached;
  laPower.Enabled := Attached;
  edPower.Enabled := Attached;

  btStart.Enabled := Attached;
  btBrake.Enabled := Attached;
  btDrift.Enabled := Attached;

  laCurrentTitle.Enabled := Attached;
  laCurrent.Enabled := Attached;
  laMA.Enabled := Attached;

  laVoltageTitle.Enabled := Attached;
  laVoltage.Enabled := Attached;
  laMV.Enabled := Attached;
end;

procedure TfmMain.FCurrent_OnCurrentChanged(Sender: TObject);
begin
  if FCurrent <> nil then
    laCurrent.Caption := FloatToStr(FCurrent.Current);
end;

procedure TfmMain.FHub_OnConnected(Sender: TObject; const Error: Integer);
begin
  if Error <> WCL_E_SUCCESS then begin
    ShowMessage('Connect failed: 0x' + IntToHex(Error, 8));
    EnableConnect(False);
    FManager.Close;
  end else
    EnableConnect(True);
end;

procedure TfmMain.FHub_OnDeviceAttached(Sender: TObject;
  const Device: TwclWeDoIo);
begin
  // This demo supports only single motor.
  if FMotor = nil then begin
    if Device.DeviceType = iodMotor then begin
      FMotor := TwclWeDoMotor(Device);
      EnablePlay(True);
    end;
  end;

  if FCurrent = nil then begin
    if Device.DeviceType = iodCurrentSensor then begin
      FCurrent := TwclWeDoCurrentSensor(Device);
      FCurrent.OnCurrentChanged := FCurrent_OnCurrentChanged;
    end;
  end;

  if FVoltage = nil then begin
    if Device.DeviceType = iodVoltageSensor then begin
      FVoltage := TwclWeDoVoltageSensor(Device);
      FVoltage.OnVoltageChanged := FVoltage_OnVoltageChanged;
    end;
  end;
end;

procedure TfmMain.FHub_OnDeviceDetached(Sender: TObject;
  const Device: TwclWeDoIo);
begin
  if (Device.DeviceType = iodMotor) and (FMotor <> nil) and (Device.ConnectionId = FMotor.ConnectionId) then begin
    FMotor := nil;
    EnablePlay(False);
  end;
  if Device.DeviceType = iodCurrentSensor then
    FCurrent := nil;
  if Device.DeviceType = iodVoltageSensor then
    FVoltage := nil;
end;

procedure TfmMain.FHub_OnDisconnected(Sender: TObject; const Reason: Integer);
begin
  EnableConnect(False);
  FManager.Close;
end;

procedure TfmMain.FHub_OnHighCurrentAlert(Sender: TObject;
  const Alert: Boolean);
begin
  laHighCurrent.Visible := Alert;
end;

procedure TfmMain.FHub_OnLowVoltageAlert(Sender: TObject; const Alert: Boolean);
begin
  laLowVoltage.Visible := Alert;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  cbDirection.ItemIndex := 0;

  FManager := TwclBluetoothManager.Create(nil);

  FWatcher := TwclWeDoWatcher.Create(nil);
  FWatcher.OnHubFound := FWatcher_OnHubFound;

  FHub := TwclWeDoHub.Create(nil);
  FHub.OnConnected := FHub_OnConnected;
  FHub.OnDisconnected := FHub_OnDisconnected;
  FHub.OnDeviceAttached := FHub_OnDeviceAttached;
  FHub.OnDeviceDetached := FHub_OnDeviceDetached;
  FHub.OnHighCurrentAlert := FHub_OnHighCurrentAlert;
  FHub.OnLowVoltageAlert := FHub_OnLowVoltageAlert;

  FMotor := nil;
  FCurrent := nil;
  FVoltage := nil;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  Disconnect;

  FManager.Free;
  FWatcher.Free;
  FHub.Free;
end;

procedure TfmMain.FVoltage_OnVoltageChanged(Sender: TObject);
begin
  if FVoltage <> nil then
    laVoltage.Caption := FloatToStr(FVoltage.Voltage);
end;

procedure TfmMain.FWatcher_OnHubFound(Sender: TObject; const Address: Int64;
  const Name: string);
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
