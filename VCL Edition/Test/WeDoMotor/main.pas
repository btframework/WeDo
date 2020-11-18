unit main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, wclBluetooth, wclWeDoWatcher, wclWeDoHub, Vcl.ComCtrls;

type
  TfmMain = class(TForm)
    btConnect: TButton;
    btDisconnect: TButton;
    laStatus: TLabel;
    laCurrentTitle: TLabel;
    laCurrent: TLabel;
    laMA: TLabel;
    laVoltageTitle: TLabel;
    laVoltage: TLabel;
    laMV: TLabel;
    laHighCurrent: TLabel;
    laLowVoltage: TLabel;
    PageControl: TPageControl;
    tsMotor1: TTabSheet;
    laIoState1: TLabel;
    laDirection1: TLabel;
    laPower1: TLabel;
    cbDirection1: TComboBox;
    edPower1: TEdit;
    btStart1: TButton;
    btBrake1: TButton;
    btDrift1: TButton;
    tsMotor2: TTabSheet;
    laIoState2: TLabel;
    laDirection2: TLabel;
    cbDirection2: TComboBox;
    laPower2: TLabel;
    edPower2: TEdit;
    btStart2: TButton;
    btDrift2: TButton;
    btBrake2: TButton;
    procedure FormCreate(Sender: TObject);
    procedure btDisconnectClick(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btConnectClick(Sender: TObject);
    procedure btStart1Click(Sender: TObject);
    procedure btBrake1Click(Sender: TObject);
    procedure btDrift1Click(Sender: TObject);
    procedure btBrake2Click(Sender: TObject);
    procedure btDrift2Click(Sender: TObject);
    procedure btStart2Click(Sender: TObject);

  private
    FManager: TwclBluetoothManager;
    FWatcher: TwclWeDoWatcher;
    FHub: TwclWeDoHub;
    FMotor1: TwclWeDoMotor;
    FMotor2: TwclWeDoMotor;
    FCurrent: TwclWeDoCurrentSensor;
    FVoltage: TwclWeDoVoltageSensor;

    procedure FHub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
    procedure FHub_OnHighCurrentAlert(Sender: TObject; Alert: Boolean);
    procedure FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDisconnected(Sender: TObject; Reason: Integer);
    procedure FHub_OnConnected(Sender: TObject; Error: Integer);

    procedure FVoltage_OnVoltageChanged(Sender: TObject);
    procedure FCurrent_OnCurrentChanged(Sender: TObject);

    procedure FWatcher_OnHubFound(Sender: TObject; Address: Int64;
      Name: string);

    procedure EnablePlay;
    procedure EnablePlay1(Attached: Boolean);
    procedure EnablePlay2(Attached: Boolean);
    procedure EnableConnect(Connected: Boolean);

    procedure Disconnect;
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors;

{$R *.dfm}

procedure TfmMain.btBrake1Click(Sender: TObject);
var
  Res: Integer;
begin
  if FMotor1 = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotor1.Brake;
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

procedure TfmMain.btDrift1Click(Sender: TObject);
var
  Res: Integer;
begin
  if FMotor1 = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotor1.Drift;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Drift motor failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btStart1Click(Sender: TObject);
var
  Dir: TwclWeDoMotorDirection;
  Res: Integer;
begin
  if FMotor1 = nil then
    ShowMessage('Device is not attached')
  else begin
    case cbDirection1.ItemIndex of
      0: Dir := mdRight;
      1: Dir := mdLeft;
      else Dir := mdUnknown;
    end;
    Res := FMotor1.Run(Dir, StrToInt(edPower1.Text));
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Start motor failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btStart2Click(Sender: TObject);
var
  Dir: TwclWeDoMotorDirection;
  Res: Integer;
begin
  if FMotor2 = nil then
    ShowMessage('Device is not attached')
  else begin
    case cbDirection2.ItemIndex of
      0: Dir := mdRight;
      1: Dir := mdLeft;
      else Dir := mdUnknown;
    end;
    Res := FMotor2.Run(Dir, StrToInt(edPower2.Text));
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

procedure TfmMain.EnablePlay;
var
  Attached: Boolean;
begin
  Attached := (FMotor1 <> nil) or (FMotor2 <> nil);

  laCurrentTitle.Enabled := Attached;
  laCurrent.Enabled := Attached;
  laMA.Enabled := Attached;

  laVoltageTitle.Enabled := Attached;
  laVoltage.Enabled := Attached;
  laMV.Enabled := Attached;

  if not Attached then begin
    laCurrent.Caption := '0';
    laVoltage.Caption := '0';

    laHighCurrent.Visible := False;
    laLowVoltage.Visible := False;
  end;
end;

procedure TfmMain.EnablePlay1(Attached: Boolean);
begin
  if Attached then
    laIoState1.Caption := 'Attached'
  else
    laIoState1.Caption := 'Dectahed';

  laDirection1.Enabled := Attached;
  cbDirection1.Enabled := Attached;
  laPower1.Enabled := Attached;
  edPower1.Enabled := Attached;

  btStart1.Enabled := Attached;
  btBrake1.Enabled := Attached;
  btDrift1.Enabled := Attached;

  EnablePlay;
end;

procedure TfmMain.EnablePlay2(Attached: Boolean);
begin
  if Attached then
    laIoState2.Caption := 'Attached'
  else
    laIoState2.Caption := 'Dectahed';

  laDirection2.Enabled := Attached;
  cbDirection2.Enabled := Attached;
  laPower2.Enabled := Attached;
  edPower2.Enabled := Attached;

  btStart2.Enabled := Attached;
  btBrake2.Enabled := Attached;
  btDrift2.Enabled := Attached;

  EnablePlay;
end;

procedure TfmMain.FCurrent_OnCurrentChanged(Sender: TObject);
begin
  if FCurrent <> nil then
    laCurrent.Caption := FloatToStr(FCurrent.Current);
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
  // This demo supports only single motor.
  if Device.DeviceType = iodMotor then begin
    if FMotor1 = nil then begin
      FMotor1 := TwclWeDoMotor(Device);
      EnablePlay1(True);
    end else begin
      if FMotor2 = nil then begin
        FMotor2 := TwclWeDoMotor(Device);
        EnablePlay2(True);
      end
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

procedure TfmMain.FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
begin
  if (Device.DeviceType = iodMotor) then begin
    if (FMotor1 <> nil) and (Device.ConnectionId = FMotor1.ConnectionId) then begin
      FMotor1 := nil;
      EnablePlay1(False);
    end;

    if (FMotor2 <> nil) and (Device.ConnectionId = FMotor2.ConnectionId) then begin
      FMotor2 := nil;
      EnablePlay2(False);
    end;
  end;
  if Device.DeviceType = iodCurrentSensor then
    FCurrent := nil;
  if Device.DeviceType = iodVoltageSensor then
    FVoltage := nil;
end;

procedure TfmMain.FHub_OnDisconnected(Sender: TObject; Reason: Integer);
begin
  EnableConnect(False);
  FManager.Close;
end;

procedure TfmMain.FHub_OnHighCurrentAlert(Sender: TObject; Alert: Boolean);
begin
  laHighCurrent.Visible := Alert;
end;

procedure TfmMain.FHub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
begin
  laLowVoltage.Visible := Alert;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  cbDirection1.ItemIndex := 0;
  cbDirection2.ItemIndex := 0;

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

  FMotor1 := nil;
  FMotor2 := nil;
  FCurrent := nil;
  FVoltage := nil;

  PageControl.ActivePageIndex := 0;
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

procedure TfmMain.btBrake2Click(Sender: TObject);
var
  Res: Integer;
begin
  if FMotor2 = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotor2.Brake;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Brake motor failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btDrift2Click(Sender: TObject);
var
  Res: Integer;
begin
  if FMotor2 = nil then
    ShowMessage('Device is not attached')
  else begin
    Res := FMotor2.Drift;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Drift motor failed: 0x' + IntToHex(Res, 8));
  end;
end;

end.
