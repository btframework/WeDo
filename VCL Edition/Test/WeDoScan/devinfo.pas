unit devinfo;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, Vcl.ComCtrls, wclBluetooth, wclWeDoHub;

type
  TfmDevInfo = class(TForm)
    laDeviceInformationTitle: TLabel;
    laFirmwareVersionTitle: TLabel;
    laHardwareVersionTitle: TLabel;
    laSoftwareVersionTitle: TLabel;
    laManufacturerNameTitle: TLabel;
    laFirmwareVersion: TLabel;
    laHardwareVersion: TLabel;
    laSoftwareVersion: TLabel;
    laManufacturerName: TLabel;
    lvAttachedDevices: TListView;
    laBattLevelTitle: TLabel;
    laBattLevel: TLabel;
    laDeviceName: TLabel;
    pbBattLevel: TProgressBar;
    edDeviceName: TEdit;
    btSetDeviceName: TButton;
    laLowVoltage: TLabel;
    laLowSignal: TLabel;
    btTurnOff: TButton;
    btClose: TButton;
    procedure btCloseClick(Sender: TObject);
    procedure btSetDeviceNameClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure btTurnOffClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure FormDestroy(Sender: TObject);

  private
    // The radio object used to connect to WeDo Hub.
    FRadio: TwclBluetoothRadio;
    // The Hub MAC address.
    FAddress: Int64;
    // WeDo Hub instance.
    FHub: TwclWeDoHub;

    procedure DisplayDeviceInfoValue(Label_: TLabel; Value: string;
      Res: Integer);
    procedure UpdateBattLevel(Level: Byte);

    procedure ReadDeviceInformation;
    procedure ReadDeviceName;
    procedure ReadBatteryLevel;

    procedure FHub_OnLowSignalAlert(Sender: TObject; Alert: Boolean);
    procedure FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
    procedure FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
    procedure Hub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
    procedure FHub_OnDisconnected(Sender: TObject; Reason: Integer);
    procedure FHub_OnConnected(Sender: TObject; Error: Integer);

    procedure BatteryLevel_OnBatteryLevelChanged(Sender: TObject;
      Level: Byte);

  public
    constructor Create(AOwner: TComponent; Radio: TwclBluetoothRadio;
      Address: Int64); reintroduce;
  end;

var
  fmDevInfo: TfmDevInfo;

implementation

uses
  wclErrors, wclBluetoothErrors;

{$R *.dfm}

{ TfmDevInfo }

procedure TfmDevInfo.BatteryLevel_OnBatteryLevelChanged(Sender: TObject;
  Level: Byte);
begin
  // Update battery level.
  UpdateBattLevel(Level);
end;

procedure TfmDevInfo.btCloseClick(Sender: TObject);
begin
  // Disconnect from Hub.
  FHub.Disconnect;
end;

procedure TfmDevInfo.btSetDeviceNameClick(Sender: TObject);
var
  Res: Integer;
begin
  // Try to change Hub name.
  Res := FHub.WriteDeviceName(edDeviceName.Text);
  if Res <> WCL_E_SUCCESS then
    ShowMessage('Write hub name failed with error: 0x' + IntToHex(Res, 8));
end;

procedure TfmDevInfo.btTurnOffClick(Sender: TObject);
var
  Res: Integer;
begin
  Res := FHub.TurnOff;
  if Res <> WCL_E_SUCCESS then
    ShowMessage('Turn Off failed: 0x' + IntToHex(Res, 8));
end;

constructor TfmDevInfo.Create(AOwner: TComponent;Radio: TwclBluetoothRadio;
  Address: Int64);
begin
  FRadio := Radio;
  FAddress := Address;

  inherited Create(AOwner);
end;

procedure TfmDevInfo.DisplayDeviceInfoValue(Label_: TLabel; Value: string;
  Res: Integer);
begin
  // Helper function to show information value.
  if Res = WCL_E_SUCCESS then
    Label_.Caption := Value
  else begin
    if Res = WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND then
      Label_.Caption := '<unspecified>'
    else
      Label_.Caption := 'Read error: 0x' + IntToHex(Res, 8);
  end;
end;

procedure TfmDevInfo.FHub_OnConnected(Sender: TObject; Error: Integer);
begin
  if Error = WCL_E_SUCCESS then begin
    // If connection was success we can read Hub information.
    ReadDeviceInformation;
    ReadDeviceName;
    ReadBatteryLevel;
  end else begin
    // Show error message to user and close the form.
    ShowMessage('Connection error: 0x' + IntToHex(Error, 8));
    Close;
  end;
end;

procedure TfmDevInfo.FHub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
var
  Item: TListItem;
begin
  Item := lvAttachedDevices.Items.Add();
  Item.Caption := IntToStr(Device.ConnectionId);
  case Device.DeviceType of
    iodMotor: Item.SubItems.Add('Motor');
    iodVoltageSensor: Item.SubItems.Add('Voltage Sensor');
    iodCurrentSensor: Item.SubItems.Add('Current Sensor');
    iodPiezo: Item.SubItems.Add('Piezo');
    iodRgb: Item.SubItems.Add('RGB');
    iodTiltSensor: Item.SubItems.Add('Tilt Sensor');
    iodMotionSensor: Item.SubItems.Add('Motion Sensor');
    else Item.SubItems.Add('Unknown');
  end;
  Item.SubItems.Add(Device.FirmwareVersion.ToString());
  Item.SubItems.Add(Device.HardwareVersion.ToString());
  Item.SubItems.Add(BoolToStr(Device.Internal));
  Item.SubItems.Add(IntToStr(Device.PortId));
end;

procedure TfmDevInfo.FHub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
var
  Item: TListItem;
begin
  for Item in lvAttachedDevices.Items do begin
    if Item.Caption = IntToStr(Device.ConnectionId) then begin
      lvAttachedDevices.Items.Delete(lvAttachedDevices.Items.IndexOf(Item));
      Break;
    end;
  end;
end;

procedure TfmDevInfo.FHub_OnDisconnected(Sender: TObject; Reason: Integer);
begin
  // When disconnected from Hub close the form.
  Close();
end;

procedure TfmDevInfo.FHub_OnLowSignalAlert(Sender: TObject; Alert: Boolean);
begin
  laLowSignal.Visible := Alert;
end;

procedure TfmDevInfo.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  // Disconnect from Hub.
  FHub.Disconnect;
end;

procedure TfmDevInfo.FormCreate(Sender: TObject);
var
  Res: Integer;
begin
  // Create WeDo Hub instance.
  FHub := TwclWeDoHub.Create(nil);
  // Setup its event handlers. We are interested in batt level only for now.
  FHub.BatteryLevel.OnBatteryLevelChanged := BatteryLevel_OnBatteryLevelChanged;
  FHub.OnLowVoltageAlert := Hub_OnLowVoltageAlert;
  FHub.OnConnected := FHub_OnConnected;
  FHub.OnDisconnected := FHub_OnDisconnected;
  FHub.OnDeviceAttached := FHub_OnDeviceAttached;
  FHub.OnDeviceDetached := FHub_OnDeviceDetached;
  FHub.OnLowSignalAlert := FHub_OnLowSignalAlert;
  // Try to connect. We will use the same Bluetooth Radio object that is used by
  // the WeDo Watcher.
  Res := FHub.Connect(FRadio, FAddress);
  if Res <> WCL_E_SUCCESS then begin
    // If something went wrong show message and close the form.
    ShowMessage('Connect to Hub failed: 0x' + IntToHex(Res, 8));
    Close;
  end;
end;

procedure TfmDevInfo.FormDestroy(Sender: TObject);
begin
  FHub.Free;
end;

procedure TfmDevInfo.Hub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
begin
  // Show Low Voltage Warning when alert received.
  laLowVoltage.Visible := Alert;
end;

procedure TfmDevInfo.ReadBatteryLevel;
var
  Level: Byte;
  Res: Integer;
begin
  // Helper function reads battery level.
  Res := FHub.BatteryLevel.ReadBatteryLevel(Level);
  if Res = WCL_E_SUCCESS then
    UpdateBattLevel(Level);
end;

procedure TfmDevInfo.ReadDeviceInformation;
var
  Value: string;
  Res: Integer;
begin
  // Read all possible information from Hub.
  Res := FHub.DeviceInformation.ReadFirmwareVersion(Value);
  DisplayDeviceInfoValue(laFirmwareVersion, Value, Res);
  Res := FHub.DeviceInformation.ReadHardwareVersion(Value);
  DisplayDeviceInfoValue(laHardwareVersion, Value, Res);
  Res := FHub.DeviceInformation.ReadSoftwareVersion(Value);
  DisplayDeviceInfoValue(laSoftwareVersion, Value, Res);
  Res := FHub.DeviceInformation.ReadManufacturerName(Value);
  DisplayDeviceInfoValue(laManufacturerName, Value, Res);
end;

procedure TfmDevInfo.ReadDeviceName;
var
  Name: string;
  Res: Integer;
begin
  // Read Hub name.
  Res := FHub.ReadDeviceName(Name);
  if Res = WCL_E_SUCCESS then
    edDeviceName.Text := Name
  else begin
    if Res = WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND then
      edDeviceName.Text := '<unsupported>'
    else
      edDeviceName.Text := '<error>';
  end;
end;

procedure TfmDevInfo.UpdateBattLevel(Level: Byte);
begin
  pbBattLevel.Position := Level;
  laBattLevel.Caption := IntToStr(Level) + ' %';
end;

end.
