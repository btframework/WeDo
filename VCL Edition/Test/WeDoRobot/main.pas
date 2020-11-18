unit main;

interface

uses
  Vcl.Forms, System.Classes, Vcl.Controls, Vcl.StdCtrls, Vcl.ComCtrls,
  wclWeDoRobot, wclWeDoHub;

type
  TfmMain = class(TForm)
    btStart: TButton;
    btStop: TButton;
    lvHubs: TListView;
    btDisconnect: TButton;
    pcHub: TPageControl;
    tsHubInfo: TTabSheet;
    laDeviceInformationTitle: TLabel;
    laFirmwareVersionTitle: TLabel;
    laHardwareVersionTitle: TLabel;
    laSoftwareVersionTitle: TLabel;
    laManufacturerNameTitle: TLabel;
    laFirmwareVersion: TLabel;
    laHardwareVersion: TLabel;
    laSoftwareVersion: TLabel;
    laManufacturerName: TLabel;
    laBattTypeTitle: TLabel;
    laBatteryType: TLabel;
    laBattLevelTitle: TLabel;
    laBattLevel: TLabel;
    laDeviceNameCaption: TLabel;
    laDeviceName: TLabel;
    laNewName: TLabel;
    edDeviceName: TEdit;
    btSetDeviceName: TButton;
    btClear: TButton;
    lbLog: TListBox;
    laLowVoltage: TLabel;
    laLowSignal: TLabel;
    laHighCurrent: TLabel;
    laButtonPressed: TLabel;
    tsCurrent: TTabSheet;
    laCurrentDeviceInfo: TLabel;
    laCurrentVersionCaption: TLabel;
    laCurrentDeviceTypeCaption: TLabel;
    laCurrentConnectionIdCaption: TLabel;
    laCurrentVersion: TLabel;
    laCurrentDeviceType: TLabel;
    laCurrentConnectionId: TLabel;
    laCurrentCaption: TLabel;
    laCurrent: TLabel;
    tsVoltage: TTabSheet;
    laVotageDeviceInformation: TLabel;
    laVoltageVersionCaption: TLabel;
    laVoltageDeviceTypeCaption: TLabel;
    laVoltageConnectionIdCaption: TLabel;
    laVoltageConnectionId: TLabel;
    laVoltageDeviceType: TLabel;
    laVoltageVersion: TLabel;
    laVoltageCaption: TLabel;
    laVoltage: TLabel;
    tsPiezo: TTabSheet;
    laPiezoDeviceInformation: TLabel;
    laPiezoVersionCaption: TLabel;
    laPiezoDeviceTypeCaption: TLabel;
    laPiezoConnectionIdCaption: TLabel;
    laPiezoConnectionId: TLabel;
    laPiezoDeviceType: TLabel;
    laPiezoVersion: TLabel;
    laNote: TLabel;
    cbNote: TComboBox;
    laOctave: TLabel;
    cbOctave: TComboBox;
    laDuration: TLabel;
    edDuration: TEdit;
    btPlay: TButton;
    btStopSound: TButton;
    tsRgb: TTabSheet;
    laRgbDeviceInformation: TLabel;
    laRgbVersionCaption: TLabel;
    laRgbDeviceTypeCaption: TLabel;
    laRgbConnectionIdCaption: TLabel;
    laRgbConnectionId: TLabel;
    laRgbDeviceType: TLabel;
    laRgbVersion: TLabel;
    laColorMode: TLabel;
    cbColorMode: TComboBox;
    btSetMode: TButton;
    laR: TLabel;
    edR: TEdit;
    edG: TEdit;
    laG: TLabel;
    edB: TEdit;
    laB: TLabel;
    btSetRgb: TButton;
    laColorIndex: TLabel;
    cbColorIndex: TComboBox;
    btSetIndex: TButton;
    btSetDefault: TButton;
    btTurnOff: TButton;
    tsMotion1: TTabSheet;
    laMotionDeviceInformation1: TLabel;
    laMotionVersionCaption1: TLabel;
    laMotionDeviceTypeCaption1: TLabel;
    laMotionConnectionIdCaption1: TLabel;
    laMotionConnectionId1: TLabel;
    laMotionDeviceType1: TLabel;
    laMotionVersion1: TLabel;
    laMode1: TLabel;
    cbMode1: TComboBox;
    btChange1: TButton;
    btReset1: TButton;
    laCountTitle1: TLabel;
    laDistanceTitle1: TLabel;
    laCount1: TLabel;
    laDistance1: TLabel;
    tsMotion2: TTabSheet;
    laMotionDeviceInformation2: TLabel;
    laMotionVersionCaption2: TLabel;
    laMotionDeviceTypeCaption2: TLabel;
    laMotionConnectionIdCaption2: TLabel;
    laMotionConnectionId2: TLabel;
    laMotionDeviceType2: TLabel;
    laMotionVersion2: TLabel;
    laMode2: TLabel;
    cbMode2: TComboBox;
    btChange2: TButton;
    btReset2: TButton;
    laCountTitle2: TLabel;
    laDistanceTitle2: TLabel;
    laDistance2: TLabel;
    laCount2: TLabel;
    tsTilt1: TTabSheet;
    laTiltDeviceInformation1: TLabel;
    laTiltVersionCaption1: TLabel;
    laTiltDeviceTypeCaption1: TLabel;
    laTiltConnectionCaption1: TLabel;
    laTiltConnectionId1: TLabel;
    laTiltDeviceType1: TLabel;
    laTiltVersion1: TLabel;
    laTiltMode1: TLabel;
    cbTiltMode1: TComboBox;
    btChangeTilt1: TButton;
    btResetTilt1: TButton;
    laDirectionTitle1: TLabel;
    laDirection1: TLabel;
    laXTitle1: TLabel;
    laYTitle1: TLabel;
    laZTitle1: TLabel;
    laX1: TLabel;
    laY1: TLabel;
    laZ1: TLabel;
    tsTilt2: TTabSheet;
    laTiltDeviceInformation2: TLabel;
    laTiltVersionCaption2: TLabel;
    laTiltDeviceTypeCaption2: TLabel;
    laTiltConnectionCaption2: TLabel;
    laTiltConnectionId2: TLabel;
    laTiltDeviceType2: TLabel;
    laTiltVersion2: TLabel;
    laTiltMode2: TLabel;
    cbTiltMode2: TComboBox;
    btChangeTilt2: TButton;
    btResetTilt2: TButton;
    laDirection2: TLabel;
    laDirectionTitle2: TLabel;
    laXTitle2: TLabel;
    laYTitle2: TLabel;
    laZTitle2: TLabel;
    laZ2: TLabel;
    laY2: TLabel;
    laX2: TLabel;
    tsMotor1: TTabSheet;
    laMotorDeviceInformation1: TLabel;
    laMotorVersionCaption1: TLabel;
    laMotorDeviceTypeCaption1: TLabel;
    laMotorConnectionIdCaption1: TLabel;
    laMotorConnectionId1: TLabel;
    laMotorDeviceType1: TLabel;
    laMotorVersion1: TLabel;
    laMotorDirectionCaption1: TLabel;
    cbMotorDirection1: TComboBox;
    laPower1: TLabel;
    edPower1: TEdit;
    btStart1: TButton;
    btBrake1: TButton;
    btDrift1: TButton;
    tsMotor2: TTabSheet;
    laMotorDeviceInformation2: TLabel;
    laMotorVersionCaption2: TLabel;
    laMotorDeviceTypeCaption2: TLabel;
    laMotorConnectionIdCaption2: TLabel;
    laMotorConnectionId2: TLabel;
    laMotorDeviceType2: TLabel;
    laMotorVersion2: TLabel;
    laMotorDirectionCaption2: TLabel;
    cbMotorDirection2: TComboBox;
    laPower2: TLabel;
    edPower2: TEdit;
    btStart2: TButton;
    btDrift2: TButton;
    btBrake2: TButton;
    procedure FormCreate(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btClearClick(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure btStopClick(Sender: TObject);
    procedure lvHubsSelectItem(Sender: TObject; Item: TListItem;
      Selected: Boolean);
    procedure btDisconnectClick(Sender: TObject);
    procedure btSetDeviceNameClick(Sender: TObject);
    procedure btSetRgbClick(Sender: TObject);
    procedure btSetDefaultClick(Sender: TObject);
    procedure btTurnOffClick(Sender: TObject);
    procedure btSetIndexClick(Sender: TObject);
    procedure btSetModeClick(Sender: TObject);
    procedure btStopSoundClick(Sender: TObject);
    procedure btPlayClick(Sender: TObject);
    procedure btChange1Click(Sender: TObject);
    procedure btReset1Click(Sender: TObject);
    procedure btChange2Click(Sender: TObject);
    procedure btReset2Click(Sender: TObject);
    procedure btChangeTilt1Click(Sender: TObject);
    procedure btChangeTilt2Click(Sender: TObject);
    procedure btResetTilt1Click(Sender: TObject);
    procedure btResetTilt2Click(Sender: TObject);
    procedure btStart1Click(Sender: TObject);
    procedure btStart2Click(Sender: TObject);
    procedure btBrake1Click(Sender: TObject);
    procedure btBrake2Click(Sender: TObject);
    procedure btDrift1Click(Sender: TObject);
    procedure btDrift2Click(Sender: TObject);

  private
    FRobot: TwclWeDoRobot;

    function GetHub: TwclWeDoHub;
    function GetSelectedHub: TwclWeDoHub;
    function GetMotionSensor(Port: Byte): TwclWeDoMotionSensor;
    function GetTiltSensor(Port: Byte): TwclWeDoTiltSensor;
    function GetMotor(Port: Byte): TwclWeDoMotor;

    procedure RemoveTabs;
    procedure DisplayDeviceInforValue(_label: TLabel; Value: String;
      Res: Integer);
    procedure UpdateBattLevel(Level: Byte);
    procedure UpdateHubInfo(Hub: TwclWeDoHub);
    procedure UpdateDeviceInfo(Version: TLabel; Internal: TLabel;
      ConnectionId: TLabel; Device: TwclWeDoIo);
    procedure UpdateCurrent(Sensor: TwclWeDoCurrentSensor);
    procedure UpdateVoltage(Sensor: TwclWeDoVoltageSensor);
    procedure UpdateRgbMode(Rgb: TwclWeDoRgbLight);
    procedure UpdateRgbColors(Rgb: TwclWeDoRgbLight);
    procedure UpdateRgbIndex(Rgb: TwclWeDoRgbLight);
    procedure UpdateRgb(Sensor: TwclWeDoRgbLight);
    procedure UpdatePiezo(Sensor: TwclWeDoPiezo);
    procedure UpdateMotion(Sensor: TwclWeDoMotionSensor);
    procedure UpdateTilt(Sensor: TwclWeDoTiltSensor);
    procedure UpdateMotor(Sensor: TwclWeDoMotor);
    procedure UpdateTabs;
    procedure RemovePage(Page: TTabSheet);
    procedure AddPage(Page: TTabSheet);
    procedure ConnectEvents(Device: TwclWeDoIo);
    procedure UpdateAngle(Sensor: TwclWeDoTiltSensor);
    procedure UpdateCrash(Sensor: TwclWeDoTiltSensor);
    procedure UpdateDirection(Sensor: TwclWeDoTiltSensor);
    procedure UpdateTiltMode(Sensor: TwclWeDoTiltSensor);
    procedure ChangeMotionMode(Port: Byte);
    procedure ResetMotion(Port: Byte);
    procedure ChangeTiltMode(Port: Byte);
    procedure ResetTilt(Port: Byte);
    procedure StartMotor(Port: Byte; Direction: TComboBox; Power: TEdit);
    procedure BreakMotor(Port: Byte);
    procedure DriftMotor(Port: Byte);

    procedure FRobot_OnHubConnected(Sender: TObject; Hub: TwclWeDoHub;
      Error: Integer);
    procedure FRobot_OnHubDisconnected(Sender: TObject; Hub: TwclWeDoHub;
      Reason: Integer);
    procedure FRobot_OnHubFound(Sender: TObject; Address: Int64; Name: String;
      out Connect: Boolean);
    procedure FRobot_OnStopped(Sender: TObject);
    procedure FRobot_OnStarted(Sender: TObject);

    procedure Hub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
    procedure Hub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
    procedure Hub_OnButtonStateChanged(Sender: TObject; Pressed: Boolean);
    procedure Hub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
    procedure Hub_OnHighCurrentAlert(Sender: TObject; Alert: Boolean);
    procedure Hub_OnLowSignalAlert(Sender: TObject; Alert: Boolean);

    procedure Tilt_OnAngleChanged(Sender: TObject);
    procedure Tilt_OnCrashChanged(Sender: TObject);
    procedure Tilt_OnDirectionChanged(Sender: TObject);
    procedure Tilt_OnModeChanged(Sender: TObject);

    procedure FmMain_OnMotionModeChanged(Sender: TObject);
    procedure FmMain_OnDistanceChanged(Sender: TObject);
    procedure FmMain_OnCountChanged(Sender: TObject);
    procedure FmMain_OnModeChanged(Sender: TObject);
    procedure FmMain_OnColorChanged(Sender: TObject);
    procedure FmMain_OnVoltageChanged(Sender: TObject);
    procedure FmMain_OnCurrentChanged(Sender: TObject);

    procedure BatteryLevel_OnBatteryLevelChanged(Sender: TObject; Level: Byte);
  end;

var
  fmMain: TfmMain;

implementation

uses
  SysUtils, Dialogs, wclErrors, wclBluetoothErrors, Graphics,
  System.Generics.Collections, Windows;

{$R *.dfm}

{ TfmMain }

procedure TfmMain.AddPage(Page: TTabSheet);
begin
  Page.TabVisible := True;
end;

procedure TfmMain.BatteryLevel_OnBatteryLevelChanged(Sender: TObject;
  Level: Byte);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = TwclWeDoBatteryLevelService(Sender).Hub.Address) then
    UpdateBattLevel(Level);
end;

procedure TfmMain.BreakMotor(Port: Byte);
var
  Motor: TwclWeDoMotor;
  Res: Integer;
begin
  Motor := GetMotor(Port);
  if Motor <> nil then begin
    Res := Motor.Brake;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Brake failed; 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btBrake1Click(Sender: TObject);
begin
  BreakMotor(0);
end;

procedure TfmMain.btBrake2Click(Sender: TObject);
begin
  BreakMotor(1);
end;

procedure TfmMain.btChange1Click(Sender: TObject);
begin
  ChangeMotionMode(0);
end;

procedure TfmMain.btChange2Click(Sender: TObject);
begin
  ChangeMotionMode(1);
end;

procedure TfmMain.btChangeTilt1Click(Sender: TObject);
begin
  ChangeTiltMode(0);
end;

procedure TfmMain.btChangeTilt2Click(Sender: TObject);
begin
  ChangeTiltMode(1);
end;

procedure TfmMain.btClearClick(Sender: TObject);
begin
  lbLog.Items.Clear;
end;

procedure TfmMain.btDisconnectClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Res: Integer;
begin
  Hub := GetSelectedHub;
  if Hub <> nil then begin
    Res := Hub.TurnOff;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Disconnect failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btDrift1Click(Sender: TObject);
begin
  DriftMotor(0);
end;

procedure TfmMain.btDrift2Click(Sender: TObject);
begin
  DriftMotor(1);
end;

procedure TfmMain.btPlayClick(Sender: TObject);
const
  Notes: array of TwclWeDoPiezoNote = [ pnA, pnAis, pnB, pnC, pnCis, pnD, pnDis,
    pnE, pnF, pnFis, pnG, pnGis ];

var
  Hub: TwclWeDoHub;
  Piezo: TwclWeDoPiezo;
  Note: TwclWeDoPiezoNote;
  Res: Integer;
begin
  Hub := GetHub;
  Piezo := FRobot.GetPiezoDevice(Hub);
  if Piezo <> nil then begin
    Note := Notes[cbNote.ItemIndex];
    Res := Piezo.PlayNote(Note, cbOctave.ItemIndex + 1,
      StrToInt(edDuration.Text));
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Play failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btReset1Click(Sender: TObject);
begin
  ResetMotion(0);
end;

procedure TfmMain.btReset2Click(Sender: TObject);
begin
  ResetMotion(1);
end;

procedure TfmMain.btResetTilt1Click(Sender: TObject);
begin
  ResetTilt(0);
end;

procedure TfmMain.btResetTilt2Click(Sender: TObject);
begin
  ResetTilt(1);
end;

procedure TfmMain.btSetDefaultClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Rgb: TwclWeDoRgbLight;
  Res: Integer;
begin
  Hub := GetHub;
  Rgb := FRobot.GetRgbDevice(Hub);
  if Rgb <> nil then begin
    Res := Rgb.SwitchToDefaultColor();
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set default color: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btSetDeviceNameClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Res: Integer;
begin
  Hub := GetHub;
  if Hub <> nil then begin
    Res := Hub.WriteDeviceName(edDeviceName.Text);
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Write hub name failed with error: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btSetIndexClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Rgb: TwclWeDoRgbLight;
  Res: Integer;
begin
  Hub := GetHub;
  Rgb := FRobot.GetRgbDevice(Hub);
  if Rgb <> nil then begin
    Res := Rgb.SetColorIndex(TwclWeDoColor(cbColorIndex.ItemIndex));
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set color index: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btSetModeClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Rgb: TwclWeDoRgbLight;
  RgbEnabled: Boolean;
  IndexEnabled: Boolean;
  Res: Integer;
begin
  Hub := GetHub;
  Rgb := FRobot.GetRgbDevice(Hub);
  if Rgb <> nil then begin
    RgbEnabled := cbColorMode.ItemIndex = 1;
    IndexEnabled := cbColorMode.ItemIndex = 0;

    Res := WCL_E_SUCCESS;
    if RgbEnabled then
      Res := Rgb.SetMode(lmAbsolute)
    else begin
      if IndexEnabled then
        Res := Rgb.SetMode(lmDiscrete);
    end;

    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to change color mode: 0x' + IntToHex(Res, 8))
    else begin
      if RgbEnabled then
        UpdateRgbColors(Rgb)
      else begin
        if IndexEnabled then
          UpdateRgbIndex(Rgb);
      end;
    end;
  end;
end;

procedure TfmMain.btSetRgbClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  RgbLight: TwclWeDoRgbLight;
  c: TColor;
  Res: Integer;
begin
  Hub := GetHub;
  RgbLight := FRobot.GetRgbDevice(Hub);
  if RgbLight <> nil then begin
    c := Rgb(StrToInt(edR.Text), StrToInt(edG.Text), StrToInt(edB.Text));
    Res := RgbLight.SetColor(c);
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to set color: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btStart1Click(Sender: TObject);
begin
  StartMotor(0, cbMotorDirection1, edPower1);
end;

procedure TfmMain.btStart2Click(Sender: TObject);
begin
  StartMotor(1, cbMotorDirection2, edPower2);
end;

procedure TfmMain.btStartClick(Sender: TObject);
var
  Res: Integer;
begin
  Res := FRobot.Start;
  if Res <> WCL_E_SUCCESS then
    ShowMessage('Start failed: 0x' + IntToHex(Res, 8))
  else
    btStart.Enabled := False;
end;

procedure TfmMain.btStopClick(Sender: TObject);
var
  Res: Integer;
begin
  Res := FRobot.Stop;
  if Res <> WCL_E_SUCCESS then
    ShowMessage('Stop failed: 0x' + IntToHex(Res, 8))
  else
    btStop.Enabled := False;
end;

procedure TfmMain.btStopSoundClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Piezo: TwclWeDoPiezo;
  Res: Integer;
begin
  Hub := GetHub;
  Piezo := FRobot.GetPiezoDevice(Hub);
  if Piezo <> nil then begin
    Res := Piezo.StopPlaying;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Stop failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.btTurnOffClick(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Rgb: TwclWeDoRgbLight;
  Res: Integer;
begin
  Hub := GetHub;
  Rgb := FRobot.GetRgbDevice(Hub);
  if Rgb <> nil then begin
    Res := Rgb.SwitchOff();
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Unable to switch LED off: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.ChangeMotionMode(Port: Byte);
var
  Motion: TwclWeDoMotionSensor;
  Mode: TwclWeDoMotionSensorMode;
  Index: Integer;
  Res: Integer;
begin
  Motion := GetMotionSensor(Port);
  if Motion <> nil then begin
    if Port = 0 then
      Index := cbMode1.ItemIndex
    else
      Index := cbMode2.ItemIndex;
    case Index of
      0: Mode := mmDetect;
      1: Mode := mmCount;
      else Mode := mmUnknown;
    end;
    if Mode = mmUnknown then
      ShowMessage('Invalid mode.')
    else begin
      Res := Motion.SetMode(Mode);
      if Res <> WCL_E_SUCCESS then
        ShowMessage('Mode change failed: 0x' + IntToHex(Res, 8));
    end;
  end;
end;

procedure TfmMain.ChangeTiltMode(Port: Byte);
var
  Tilt: TwclWeDoTiltSensor;
  Mode: TwclWeDoTiltSensorMode;
  Index: Integer;
  Res: Integer;
begin
  Tilt := GetTiltSensor(Port);
  if Tilt <> nil then begin
    if Port = 0 then
      Index := cbTiltMode1.ItemIndex
    else
      Index := cbTiltMode2.ItemIndex;
    case Index of
      0: Mode := tmAngle;
      1: Mode := tmTilt;
      2: Mode := tmCrash;
      else Mode := tmUnknown;
    end;
    if Mode = tmUnknown then
      ShowMessage('Invalid mode.')
    else begin
      Res := Tilt.SetMode(Mode);
      if Res <> WCL_E_SUCCESS then
        ShowMessage('Mode change failed: 0x' + IntToHex(Res, 8));
    end;
  end;
end;

procedure TfmMain.ConnectEvents(Device: TwclWeDoIo);
var
  Rgb: TwclWeDoRgbLight;
  Motion: TwclWeDoMotionSensor;
  Tilt: TwclWeDoTiltSensor;
begin
  case Device.DeviceType of
    iodCurrentSensor:
      TwclWeDoCurrentSensor(Device).OnCurrentChanged := FmMain_OnCurrentChanged;
    iodVoltageSensor:
      TwclWeDoVoltageSensor(Device).OnVoltageChanged := FmMain_OnVoltageChanged;
    iodRgb:
      begin
        Rgb := TwclWeDoRgbLight(Device);
        Rgb.OnColorChanged := FmMain_OnColorChanged;
        Rgb.OnModeChanged := FmMain_OnModeChanged;
      end;
    iodMotionSensor:
      begin
        Motion := TwclWeDoMotionSensor(Device);
        Motion.OnCountChanged := FmMain_OnCountChanged;
        Motion.OnDistanceChanged := FmMain_OnDistanceChanged;
        Motion.OnModeChanged := FmMain_OnMotionModeChanged;
      end;
    iodTiltSensor:
      begin
        Tilt := TwclWeDoTiltSensor(Device);
        Tilt.OnModeChanged := Tilt_OnModeChanged;
        Tilt.OnDirectionChanged := Tilt_OnDirectionChanged;
        Tilt.OnCrashChanged := Tilt_OnCrashChanged;
        Tilt.OnAngleChanged := Tilt_OnAngleChanged;
      end;
  end;
end;

procedure TfmMain.DisplayDeviceInforValue(_label: TLabel; Value: String;
  Res: Integer);
begin
  // Helper function to show information value.
  if Res = WCL_E_SUCCESS then
    _label.Caption := Value
  else begin
    if Res = WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND then
      _label.Caption := '<unspecified>'
    else
      _label.Caption := 'Read error: 0x' + IntToHex(Res, 8);
  end;
end;

procedure TfmMain.DriftMotor(Port: Byte);
var
  Motor: TwclWeDoMotor;
  Res: Integer;
begin
  Motor := GetMotor(Port);
  if Motor <> nil then begin
    Res := Motor.Drift;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Drift failed; 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.FmMain_OnColorChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = TwclWeDoRgbLight(Sender).Hub.Address) then begin
    UpdateRgbColors(TwclWeDoRgbLight(Sender));
    UpdateRgbIndex(TwclWeDoRgbLight(Sender));
  end;
end;

procedure TfmMain.FmMain_OnCountChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoMotionSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoMotionSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then begin
    if Sensor.PortId = 0 then
      laCount1.Caption := Sensor.Count.ToString
    else
      laCount2.Caption := Sensor.Count.ToString;
  end;
end;

procedure TfmMain.FmMain_OnCurrentChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = TwclWeDoIo(Sender).Hub.Address) then
    UpdateCurrent(TwclWeDoCurrentSensor(Sender));
end;

procedure TfmMain.FmMain_OnDistanceChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoMotionSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoMotionSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then begin
    if Sensor.PortId = 0 then
      laDistance1.Caption := Sensor.Distance.ToString
    else
      laDistance2.Caption := Sensor.Distance.ToString;
  end;
end;

procedure TfmMain.FmMain_OnModeChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = TwclWeDoRgbLight(Sender).Hub.Address) then
    UpdateRgbMode(TwclWeDoRgbLight(sender));
end;

procedure TfmMain.FmMain_OnMotionModeChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoMotionSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoMotionSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then begin
    case Sensor.Mode of
      mmDetect:
        if Sensor.PortId = 0 then
          cbMode1.Itemindex := 0
        else
          cbMode2.Itemindex := 0;
      mmCount:
        if Sensor.PortId = 0 then
          cbMode1.Itemindex := 1
        else
          cbMode2.Itemindex := 1;
      else
        if Sensor.PortId = 0 then
          cbMode1.Itemindex := -1
        else
          cbMode2.Itemindex := -1;
    end;
  end;
end;

procedure TfmMain.FmMain_OnVoltageChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = TwclWeDoIo(Sender).Hub.Address) then
    UpdateVoltage(TwclWeDoVoltageSensor(Sender));
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  FRobot := TwclWeDoRobot.Create(nil);
  FRobot.OnStarted := FRobot_OnStarted;
  FRobot.OnStopped := FRobot_OnStopped;
  FRobot.OnHubFound := FRobot_OnHubFound;
  FRobot.OnHubConnected := FRobot_OnHubConnected;
  FRobot.OnHubDisconnected := FRobot_OnHubDisconnected;

  RemoveTabs;

  cbNote.ItemIndex := 0;
  cbOctave.ItemIndex := 0;
  cbMode1.ItemIndex := 0;
  cbMode2.ItemIndex := 0;
  cbTiltMode1.ItemIndex := 0;
  cbTiltMode2.ItemIndex := 0;
  cbMotorDirection1.ItemIndex := 0;
  cbMotorDirection2.ItemIndex := 0;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  FRobot.Stop;
  FRobot.Free;
end;

procedure TfmMain.FRobot_OnHubConnected(Sender: TObject; Hub: TwclWeDoHub;
  Error: Integer);
var
  Address: string;
  Item: TListItem;
begin
  Address := IntToHex(Hub.Address, 12);
  if Error = WCL_E_SUCCESS then begin
    lbLog.Items.Add('Hub ' + Address + ' connected.');
    Item := lvHubs.Items.Add;
    Item.Caption := Address;

    Hub.BatteryLevel.OnBatteryLevelChanged := BatteryLevel_OnBatteryLevelChanged;
    Hub.OnLowSignalAlert := Hub_OnLowSignalAlert;
    Hub.OnHighCurrentAlert := Hub_OnHighCurrentAlert;
    Hub.OnLowVoltageAlert := Hub_OnLowVoltageAlert;
    Hub.OnButtonStateChanged := Hub_OnButtonStateChanged;
    Hub.OnDeviceAttached := Hub_OnDeviceAttached;
    Hub.OnDeviceDetached := Hub_OnDeviceDetached;
  end else
    lbLog.Items.Add('Hub ' + Address + ' connect error: 0x' + IntToHex(Error, 8));
end;

procedure TfmMain.FRobot_OnHubDisconnected(Sender: TObject; Hub: TwclWeDoHub;
  Reason: Integer);
var
  Address: String;
  i: Integer;
begin
  Address := IntToHex(Hub.Address, 12);
  lbLog.Items.Add('Hub ' + Address + ' disconnected by reason: 0x' +
    IntToHex(Reason, 8));
  for i := 0 to lvHubs.Items.Count - 1 do begin
    if lvHubs.Items[i].Caption = Address then begin
      lvHubs.Items.Delete(i);
      Break;
    end;
  end;
end;

procedure TfmMain.FRobot_OnHubFound(Sender: TObject; Address: Int64;
  Name: String; out Connect: Boolean);
begin
  lbLog.Items.Add('Hub ' + IntToHex(Address, 12) + ' (' + Name + ') found');
  Connect := True;
end;

procedure TfmMain.FRobot_OnStarted(Sender: TObject);
begin
  lbLog.Items.Add('Robot started');
  btStop.Enabled := True;
  btStart.Enabled := False;
end;

procedure TfmMain.FRobot_OnStopped(Sender: TObject);
begin
  lbLog.Items.Add('Robot stopped');
  btStop.Enabled := False;
  btStart.Enabled := True;
end;

function TfmMain.GetHub: TwclWeDoHub;
var
  Address: Int64;
begin
  if lvHubs.Selected = nil then
    Result := nil
  else begin
    Address := StrToInt64('$' + lvHubs.Selected.Caption);
    Result := FRobot[Address];
  end;
end;

function TfmMain.GetMotionSensor(Port: Byte): TwclWeDoMotionSensor;
var
  Hub: TwclWeDoHub;
  Sensors: TList<TwclWeDoMotionSensor>;
  Sensor: TwclWeDoMotionSensor;
begin
  Result := nil;

  Hub := GetHub;
  Sensors := FRobot.GetMotionSensors(Hub);
  for Sensor in Sensors do begin
    if Sensor.PortId = Port then begin
      Result := Sensor;
    end;
  end;
end;

function TfmMain.GetMotor(Port: Byte): TwclWeDoMotor;
var
  Hub: TwclWeDoHub;
  Sensors: TList<TwclWeDoMotor>;
  Sensor: TwclWeDoMotor;
begin
  Result := nil;

  Hub := GetHub;
  Sensors := FRobot.GetMotors(Hub);
  for Sensor in Sensors do begin
    if Sensor.PortId = Port then begin
      Result := Sensor;
      Break;
    end;
  end;
end;

function TfmMain.GetSelectedHub: TwclWeDoHub;
begin
  Result := GetHub;
  if Result = nil then
    ShowMessage('Select Hub');
end;

function TfmMain.GetTiltSensor(Port: Byte): TwclWeDoTiltSensor;
var
  Hub: TwclWeDoHub;
  Sensors: TList<TwclWeDoTiltSensor>;
  Sensor: TwclWeDoTiltSensor;
begin
  Result := nil;

  Hub := GetHub;
  Sensors := FRobot.GetTiltSensors(Hub);
  for Sensor in Sensors do begin
    if Sensor.PortId = Port then begin
      Result := Sensor;
      Break;
    end;
  end;
end;

procedure TfmMain.Hub_OnButtonStateChanged(Sender: TObject; Pressed: Boolean);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (TwclWeDoHub(Sender).Address = Hub.Address) then
    laButtonPressed.Visible := Pressed;
end;

procedure TfmMain.Hub_OnDeviceAttached(Sender: TObject; Device: TwclWeDoIo);
var
  Hub: TwclWeDoHub;
begin
  ConnectEvents(Device);

  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = Device.Hub.Address) then begin
    case Device.DeviceType of
      iodCurrentSensor:
        begin
          AddPage(tsCurrent);
          UpdateCurrent(TwclWeDoCurrentSensor(Device));
        end;
      iodVoltageSensor:
        begin
          AddPage(tsVoltage);
          UpdateVoltage(TwclWeDoVoltageSensor(Device));
        end;
      iodRgb:
        begin
          AddPage(tsRgb);
          UpdateRgb(TwclWeDoRgbLight(Device));
        end;
      iodPiezo:
        begin
          AddPage(tsPiezo);
          UpdatePiezo(TwclWeDoPiezo(Device));
        end;
      iodMotionSensor:
        begin
          if Device.PortId = 0 then
            AddPage(tsMotion1)
          else
            AddPage(tsMotion2);
          UpdateMotion(TwclWeDoMotionSensor(Device));
        end;
      iodTiltSensor:
        begin
          if Device.PortId = 0 then
            AddPage(tsTilt1)
          else
            AddPage(tsTilt2);
          UpdateTilt(TwclWeDoTiltSensor(Device));
        end;
      iodMotor:
        begin
          if Device.PortId = 0 then
            AddPage(tsMotor1)
          else
            AddPage(tsMotor2);
          UpdateMotor(TwclWeDoMotor(Device));
        end;
    end;
  end;
end;

procedure TfmMain.Hub_OnDeviceDetached(Sender: TObject; Device: TwclWeDoIo);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (Hub.Address = Device.Hub.Address) then begin
    case Device.DeviceType of
      iodCurrentSensor:
        RemovePage(tsCurrent);
      iodVoltageSensor:
        RemovePage(tsVoltage);
      iodRgb:
        RemovePage(tsRgb);
      iodPiezo:
        RemovePage(tsPiezo);
      iodMotionSensor:
        if Device.PortId = 0 then
          RemovePage(tsMotion1)
        else
          RemovePage(tsMotion2);
      iodTiltSensor:
        if Device.PortId = 0 then
          RemovePage(tsTilt1)
        else
          RemovePage(tsTilt2);
      iodMotor:
        if Device.PortId = 0 then
          RemovePage(tsMotor1)
        else
          RemovePage(tsMotor2);
    end;
  end;
end;

procedure TfmMain.Hub_OnHighCurrentAlert(Sender: TObject; Alert: Boolean);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (TwclWeDoHub(Sender).Address = Hub.Address) then
    laHighCurrent.Visible := Alert;
end;

procedure TfmMain.Hub_OnLowSignalAlert(Sender: TObject; Alert: Boolean);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (TwclWeDoHub(Sender).Address = Hub.Address) then
    laLowSignal.Visible := Alert;
end;

procedure TfmMain.Hub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
var
  Hub: TwclWeDoHub;
begin
  Hub := GetHub;
  if (Hub <> nil) and (TwclWeDoHub(Sender).Address = Hub.Address) then
    laLowVoltage.Visible := Alert;
end;

procedure TfmMain.lvHubsSelectItem(Sender: TObject; Item: TListItem;
  Selected: Boolean);
begin
  btDisconnect.Enabled := lvHubs.Selected <> nil;
  UpdateTabs;
end;

procedure TfmMain.RemovePage(Page: TTabSheet);
begin
  Page.TabVisible := False;
end;

procedure TfmMain.RemoveTabs;
var
  i: Integer;
begin
  for i := 0 to pcHub.PageCount - 1 do
    pcHub.Pages[i].TabVisible := False;
end;

procedure TfmMain.ResetMotion(Port: Byte);
var
  Motion: TwclWeDoMotionSensor;
  Res: Integer;
begin
  Motion := GetMotionSensor(Port);
  if Motion <> nil then begin
    Res := Motion.Reset;
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Reset failed; 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.ResetTilt(Port: Byte);
var
  Tilt: TwclWeDoTiltSensor;
  Res: Integer;
begin
  Tilt := GetTiltSensor(Port);
  if Tilt <> nil then begin
    Res := Tilt.Reset();
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Reset failed; 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.StartMotor(Port: Byte; Direction: TComboBox; Power: TEdit);
var
  Motor: TwclWeDoMotor;
  Dir: TwclWeDoMotorDirection;
  Res: Integer;
begin
  Motor := GetMotor(Port);
  if Motor <> nil then begin
    case Direction.ItemIndex of
      0: Dir := mdRight;
      1: Dir := mdLeft;
      else Dir := mdUnknown;
    end;
    Res := Motor.Run(Dir, StrToInt(Power.Text));
    if Res <> WCL_E_SUCCESS then
      ShowMessage('Start motor failed: 0x' + IntToHex(Res, 8));
  end;
end;

procedure TfmMain.Tilt_OnAngleChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoTiltSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoTiltSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then
    UpdateAngle(Sensor);
end;

procedure TfmMain.Tilt_OnCrashChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoTiltSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoTiltSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then
    UpdateCrash(Sensor);
end;

procedure TfmMain.Tilt_OnDirectionChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoTiltSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoTiltSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then
    UpdateDirection(Sensor);
end;

procedure TfmMain.Tilt_OnModeChanged(Sender: TObject);
var
  Hub: TwclWeDoHub;
  Sensor: TwclWeDoTiltSensor;
begin
  Hub := GetHub;
  Sensor := TwclWeDoTiltSensor(Sender);
  if (Hub <> nil) and (Hub.Address = Sensor.Hub.Address) then
    UpdateTiltMode(Sensor);
end;

procedure TfmMain.UpdateAngle(Sensor: TwclWeDoTiltSensor);
var
  Angle: TwclWeDoTiltSensorAngle;
begin
  Angle := Sensor.Angle;
  if Sensor.PortId = 0 then begin
    laX1.Caption := Angle.X.ToString;
    laY1.Caption := Angle.Y.ToString;
    laZ1.Caption := '0';
    laDirection1.Caption := 'Unknown';
  end else begin
    laX2.Caption := Angle.X.ToString;
    laY2.Caption := Angle.Y.ToString;
    laZ2.Caption := '0';
    laDirection2.Caption := 'Unknown';
  end;
end;

procedure TfmMain.UpdateBattLevel(Level: Byte);
begin
  laBattLevel.Caption := Level.ToString + ' %';
end;

procedure TfmMain.UpdateCrash(Sensor: TwclWeDoTiltSensor);
var
  Crash: TwclWeDoTiltSensorCrash;
begin
  Crash := Sensor.Crash;
  if Sensor.PortId = 0 then begin
    laX1.Caption := Crash.X.ToString;
    laY1.Caption := Crash.Y.ToString;
    laZ1.Caption := Crash.Z.ToString;
    laDirection1.Caption := 'Unknown';
  end else begin
    laX2.Caption := Crash.X.ToString;
    laY2.Caption := Crash.Y.ToString;
    laZ2.Caption := Crash.Z.ToString;
    laDirection2.Caption := 'Unknown';
  end;
end;

procedure TfmMain.UpdateCurrent(Sensor: TwclWeDoCurrentSensor);
begin
  UpdateDeviceInfo(laCurrentVersion, laCurrentDeviceType, laCurrentConnectionId,
    Sensor);
  laCurrent.Caption := Sensor.Current.ToString + ' mA';
end;

procedure TfmMain.UpdateDeviceInfo(Version, Internal, ConnectionId: TLabel;
  Device: TwclWeDoIo);
begin
  Version.Caption := 'Firmware: ' +
    Device.FirmwareVersion.MajorVersion.ToString + '.' +
    Device.FirmwareVersion.MinorVersion.ToString + '.' +
    Device.FirmwareVersion.BuildNumber.ToString + '.' +
    Device.FirmwareVersion.BugFixVersion.ToString +
    '  Hardware: ' +
    Device.HardwareVersion.MajorVersion.ToString + '.' +
    Device.HardwareVersion.MinorVersion.ToString + '.' +
    Device.HardwareVersion.BuildNumber.ToString + '.' +
    Device.HardwareVersion.BugFixVersion.ToString;
  if Device.Internal then
    Internal.Caption := 'Internal'
  else
    Internal.Caption := 'External';
  ConnectionId.Caption := 'Connection ID: ' + Device.ConnectionId.ToString +
    '  Port ID: ' + Device.PortId.ToString;
end;

procedure TfmMain.UpdateDirection(Sensor: TwclWeDoTiltSensor);
begin
  if Sensor.PortId = 0 then begin
    case Sensor.Direction of
      tdBackward:
        laDirection1.Caption := 'Backward';
      tdForward:
        laDirection1.Caption := 'Forward';
      tdLeft:
        laDirection1.Caption := 'Left';
      tdNeutral:
        laDirection1.Caption := 'Neutral';
      tdRight:
        laDirection1.Caption := 'Right';
      else
        laDirection1.Caption := 'Unknown';
    end;
    laX1.Caption := '0';
    laY1.Caption := '0';
    laZ1.Caption := '0';

  end else begin
    case Sensor.Direction of
      tdBackward:
        laDirection2.Caption := 'Backward';
      tdForward:
        laDirection2.Caption := 'Forward';
      tdLeft:
        laDirection2.Caption := 'Left';
      tdNeutral:
        laDirection2.Caption := 'Neutral';
      tdRight:
        laDirection2.Caption := 'Right';
      else
        laDirection2.Caption := 'Unknown';
    end;
    laX2.Caption := '0';
    laY2.Caption := '0';
    laZ2.Caption := '0';
  end;
end;

procedure TfmMain.UpdateHubInfo(Hub: TwclWeDoHub);
var
  Value: string;
  Res: Integer;
  Name: string;
  Level: Byte;
  Pressed: Boolean;
begin
  Res := Hub.DeviceInformation.ReadFirmwareVersion(Value);
  DisplayDeviceInforValue(laFirmwareVersion, Value, Res);
  Res := Hub.DeviceInformation.ReadHardwareVersion(Value);
  DisplayDeviceInforValue(laHardwareVersion, Value, Res);
  Res := Hub.DeviceInformation.ReadSoftwareVersion(Value);
  DisplayDeviceInforValue(laSoftwareVersion, Value, Res);
  Res := Hub.DeviceInformation.ReadManufacturerName(Value);
  DisplayDeviceInforValue(laManufacturerName, Value, Res);

  case Hub.BatteryType of
    btRechargeable:
      laBatteryType.Caption := 'Rechargeable';
    btStandard:
      laBatteryType.Caption := 'Standard';
    btUnknown:
      laBatteryType.Caption := 'Unknown';
    else
      laBatteryType.Caption := 'Undefined';
  end;

  Res := Hub.ReadDeviceName(Name);
  if Res = WCL_E_SUCCESS then
    laDeviceName.Caption := Name
  else begin
    if Res = WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND then
      laDeviceName.Caption := '<unsupported>'
    else
      laDeviceName.Caption := '<error>';
  end;

  Res := Hub.BatteryLevel.ReadBatteryLevel(Level);
  if Res = WCL_E_SUCCESS then
    UpdateBattLevel(Level);

  Res := Hub.ReadButtonState(Pressed);
  if Res = WCL_E_SUCCESS then
    laButtonPressed.Visible := Pressed;

  laLowSignal.Visible := False;
  laLowVoltage.Visible := False;
  laHighCurrent.Visible := False;
end;

procedure TfmMain.UpdateMotion(Sensor: TwclWeDoMotionSensor);
begin
  if Sensor.PortId = 0 then begin
    UpdateDeviceInfo(laMotionVersion1, laMotionDeviceType1,
      laMotionConnectionId1, Sensor);
    laCount1.Caption := Sensor.Count.ToString;
    laDistance1.Caption := Sensor.Distance.ToString;
  end else begin
    UpdateDeviceInfo(laMotionVersion2, laMotionDeviceType2,
      laMotionConnectionId2, Sensor);
    laCount2.Caption := Sensor.Count.ToString;
    laDistance2.Caption := Sensor.Distance.ToString;
  end;
end;

procedure TfmMain.UpdateMotor(Sensor: TwclWeDoMotor);
begin
  if Sensor.PortId = 0 then begin
    UpdateDeviceInfo(laMotorVersion1, laMotorDeviceType1, laMotorConnectionId1,
      Sensor);
  end else begin
    UpdateDeviceInfo(laMotorVersion2, laMotorDeviceType2, laMotorConnectionId2,
      Sensor);
  end;
end;

procedure TfmMain.UpdatePiezo(Sensor: TwclWeDoPiezo);
begin
  UpdateDeviceInfo(laPiezoVersion, laPiezoDeviceType, laPiezoConnectionId,
    Sensor);
end;

procedure TfmMain.UpdateRgb(Sensor: TwclWeDoRgbLight);
begin
  UpdateDeviceInfo(laRgbVersion, laRgbDeviceType, laRgbConnectionId, Sensor);

  UpdateRgbColors(Sensor);
  UpdateRgbIndex(Sensor);
  UpdateRgbMode(Sensor);
end;

procedure TfmMain.UpdateRgbColors(Rgb: TwclWeDoRgbLight);
var
  c: TColor;
begin
  c := Rgb.Color;
  edR.Text := (c and $000000FF).ToString;
  edG.Text := ((c shr 8) and $000000FF).ToString;
  edB.Text := ((c shr 16) and $000000FF).ToString;
end;

procedure TfmMain.UpdateRgbIndex(Rgb: TwclWeDoRgbLight);
begin
  cbColorIndex.ItemIndex := Integer(Rgb.ColorIndex);
end;

procedure TfmMain.UpdateRgbMode(Rgb: TwclWeDoRgbLight);
begin
  case Rgb.Mode of
    lmDiscrete:
      cbColorMode.ItemIndex := 0;
    lmAbsolute:
      cbColorMode.ItemIndex := 1;
    else
      cbColorMode.ItemIndex := -1;
  end;
end;

procedure TfmMain.UpdateTabs;
var
  Hub: TwclWeDoHub;
  Current: TwclWeDoCurrentSensor;
  Voltage: TwclWeDoVoltageSensor;
  Rgb: TwclWeDoRgbLight;
  Piezo: TwclWeDoPiezo;
  Motion: TwclWeDoMotionSensor;
  Tilt: TwclWeDoTiltSensor;
  Motor: TwclWeDoMotor;
begin
  RemoveTabs;

  Hub := GetHub;
  if Hub <> nil then begin
    AddPage(tsHubInfo);
    UpdateHubInfo(Hub);

    Current := FRobot.GetCurrentSensor(Hub);
    if (Current <> nil) and Current.Attached then begin
      AddPage(tsCurrent);
      UpdateCurrent(Current);
    end;

    Voltage := FRobot.GetVoltageSensor(Hub);
    if (Voltage <> nil ) and Voltage.Attached then begin
      AddPage(tsVoltage);
      UpdateVoltage(Voltage);
    end;

    Rgb := FRobot.GetRgbDevice(Hub);
    if (Rgb <> nil) and Rgb.Attached then begin
      AddPage(tsRgb);
      UpdateRgb(Rgb);
    end;

    Piezo := FRobot.GetPiezoDevice(Hub);
    if (Piezo <> nil) and Piezo.Attached then begin
      AddPage(tsPiezo);
      UpdatePiezo(Piezo);
    end;

    Motion := GetMotionSensor(0);
    if (Motion <> nil) and Motion.Attached then begin
      AddPage(tsMotion1);
      UpdateMotion(Motion);
    end;

    Motion := GetMotionSensor(1);
    if (Motion <> nil) and Motion.Attached then begin
      AddPage(tsMotion2);
      UpdateMotion(Motion);
    end;

    Tilt := GetTiltSensor(0);
    if (Tilt <> nil) and Tilt.Attached then begin
      AddPage(tsTilt1);
      UpdateTilt(Tilt);
    end;

    Tilt := GetTiltSensor(1);
    if (Tilt <> nil) and Tilt.Attached then begin
      AddPage(tsTilt2);
      UpdateTilt(Tilt);
    end;

    Motor := GetMotor(0);
    if (Motor <> nil) and Motor.Attached then begin
      AddPage(tsMotor1);
      UpdateMotor(Motor);
    end;

    Motor := GetMotor(1);
    if (Motor <> nil) and Motor.Attached then begin
      AddPage(tsMotor2);
      UpdateMotor(Motor);
    end;
  end;
end;

procedure TfmMain.UpdateTilt(Sensor: TwclWeDoTiltSensor);
begin
  if Sensor.PortId = 0 then begin
    UpdateDeviceInfo(laTiltVersion1, laTiltDeviceType1, laTiltConnectionId1,
      Sensor);
  end else begin
    UpdateDeviceInfo(laTiltVersion2, laTiltDeviceType2, laTiltConnectionId2,
      Sensor);
  end;
  UpdateTiltMode(Sensor);
  UpdateDirection(Sensor);
  UpdateCrash(Sensor);
  UpdateAngle(Sensor);
end;

procedure TfmMain.UpdateTiltMode(Sensor: TwclWeDoTiltSensor);
begin
  case Sensor.Mode of
    tmAngle:
      if Sensor.PortId = 0 then
        cbTiltMode1.ItemIndex := 0
      else
        cbTiltMode2.ItemIndex := 0;
    tmTilt:
      if Sensor.PortId = 0 then
        cbTiltMode1.ItemIndex := 1
      else
        cbTiltMode2.ItemIndex := 1;
    tmCrash:
      if Sensor.PortId = 0 then
        cbTiltMode1.ItemIndex := 2
      else
        cbTiltMode2.ItemIndex := 2;
    else
      if Sensor.PortId = 0 then
        cbTiltMode1.ItemIndex := -1
      else
        cbTiltMode2.ItemIndex := -1;
  end;
end;

procedure TfmMain.UpdateVoltage(Sensor: TwclWeDoVoltageSensor);
begin
  UpdateDeviceInfo(laVoltageVersion, laVoltageDeviceType, laVoltageConnectionId,
    Sensor);
  laVoltage.Caption := Sensor.Voltage.ToString + ' mV';
end;

end.
