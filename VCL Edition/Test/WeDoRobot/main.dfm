object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'WeDo Robot'
  ClientHeight = 571
  ClientWidth = 504
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 13
  object btStart: TButton
    Left = 8
    Top = 8
    Width = 75
    Height = 25
    Caption = 'Start'
    TabOrder = 0
    OnClick = btStartClick
  end
  object btStop: TButton
    Left = 89
    Top = 8
    Width = 75
    Height = 25
    Caption = 'Stop'
    Enabled = False
    TabOrder = 1
    OnClick = btStopClick
  end
  object lvHubs: TListView
    Left = 8
    Top = 39
    Width = 397
    Height = 98
    Columns = <
      item
        Caption = 'Address'
        Width = 300
      end>
    GridLines = True
    HideSelection = False
    ReadOnly = True
    RowSelect = True
    TabOrder = 2
    ViewStyle = vsReport
    OnSelectItem = lvHubsSelectItem
  end
  object btDisconnect: TButton
    Left = 415
    Top = 39
    Width = 75
    Height = 25
    Caption = 'Disconnect'
    Enabled = False
    TabOrder = 3
    OnClick = btDisconnectClick
  end
  object pcHub: TPageControl
    Left = 8
    Top = 143
    Width = 401
    Height = 274
    ActivePage = tsMotor2
    TabOrder = 4
    object tsHubInfo: TTabSheet
      Caption = 'Hub'
      ExplicitWidth = 537
      ExplicitHeight = 406
      object laDeviceInformationTitle: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laFirmwareVersionTitle: TLabel
        Left = 32
        Top = 35
        Width = 86
        Height = 13
        Caption = 'Firmware version:'
      end
      object laHardwareVersionTitle: TLabel
        Left = 32
        Top = 54
        Width = 89
        Height = 13
        Caption = 'Hardware version:'
      end
      object laSoftwareVersionTitle: TLabel
        Left = 32
        Top = 73
        Width = 86
        Height = 13
        Caption = 'Software version:'
      end
      object laManufacturerNameTitle: TLabel
        Left = 32
        Top = 92
        Width = 98
        Height = 13
        Caption = 'Manufacturer name:'
      end
      object laFirmwareVersion: TLabel
        Left = 144
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laHardwareVersion: TLabel
        Left = 144
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laSoftwareVersion: TLabel
        Left = 144
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laManufacturerName: TLabel
        Left = 144
        Top = 92
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laBattTypeTitle: TLabel
        Left = 16
        Top = 120
        Width = 75
        Height = 13
        Caption = 'Battery type:'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laBatteryType: TLabel
        Left = 112
        Top = 120
        Width = 49
        Height = 13
        Caption = 'Undefined'
      end
      object laBattLevelTitle: TLabel
        Left = 15
        Top = 139
        Width = 76
        Height = 13
        Caption = 'Battery level:'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laBattLevel: TLabel
        Left = 112
        Top = 139
        Width = 20
        Height = 13
        Caption = '0 %'
      end
      object laDeviceNameCaption: TLabel
        Left = 15
        Top = 168
        Width = 76
        Height = 13
        Caption = 'Device name:'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laDeviceName: TLabel
        Left = 112
        Top = 168
        Width = 59
        Height = 13
        Caption = '<unknonw>'
      end
      object laNewName: TLabel
        Left = 16
        Top = 190
        Width = 61
        Height = 13
        Caption = 'New name:'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laLowVoltage: TLabel
        Left = 296
        Top = 35
        Width = 71
        Height = 13
        Caption = 'Low voltage!'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clRed
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
        Visible = False
      end
      object laLowSignal: TLabel
        Left = 296
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Low signal!'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clRed
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
        Visible = False
      end
      object laHighCurrent: TLabel
        Left = 296
        Top = 73
        Width = 73
        Height = 13
        Caption = 'High current!'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clRed
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
        Visible = False
      end
      object laButtonPressed: TLabel
        Left = 296
        Top = 120
        Width = 86
        Height = 13
        Caption = 'Button pressed'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
        Visible = False
      end
      object edDeviceName: TEdit
        Left = 112
        Top = 187
        Width = 185
        Height = 21
        TabOrder = 0
      end
      object btSetDeviceName: TButton
        Left = 303
        Top = 185
        Width = 75
        Height = 25
        Caption = 'Change'
        TabOrder = 1
        OnClick = btSetDeviceNameClick
      end
    end
    object tsCurrent: TTabSheet
      Caption = 'Current'
      ImageIndex = 1
      object laCurrentDeviceInfo: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laCurrentVersionCaption: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laCurrentDeviceTypeCaption: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laCurrentConnectionIdCaption: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laCurrentVersion: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laCurrentDeviceType: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laCurrentConnectionId: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laCurrentCaption: TLabel
        Left = 16
        Top = 120
        Width = 46
        Height = 13
        Caption = 'Current:'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laCurrent: TLabel
        Left = 92
        Top = 120
        Width = 24
        Height = 13
        Caption = '0 mA'
      end
    end
    object tsVoltage: TTabSheet
      Caption = 'Voltage'
      ImageIndex = 2
      object laVotageDeviceInformation: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laVoltageVersionCaption: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laVoltageDeviceTypeCaption: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laVoltageConnectionIdCaption: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laVoltageConnectionId: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laVoltageDeviceType: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laVoltageVersion: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laVoltageCaption: TLabel
        Left = 16
        Top = 120
        Width = 46
        Height = 13
        Caption = 'Voltage:'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laVoltage: TLabel
        Left = 77
        Top = 120
        Width = 23
        Height = 13
        Caption = '0 mV'
      end
    end
    object tsPiezo: TTabSheet
      Caption = 'Piezo'
      ImageIndex = 3
      ExplicitLeft = 8
      ExplicitTop = 28
      object laPiezoDeviceInformation: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laPiezoVersionCaption: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laPiezoDeviceTypeCaption: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laPiezoConnectionIdCaption: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laPiezoConnectionId: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laPiezoDeviceType: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laPiezoVersion: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laNote: TLabel
        Left = 16
        Top = 107
        Width = 23
        Height = 13
        Caption = 'Note'
      end
      object laOctave: TLabel
        Left = 110
        Top = 107
        Width = 35
        Height = 13
        Caption = 'Octave'
      end
      object laDuration: TLabel
        Left = 223
        Top = 107
        Width = 41
        Height = 13
        Caption = 'Duration'
      end
      object cbNote: TComboBox
        Left = 45
        Top = 104
        Width = 59
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'A'
          'A#'
          'B'
          'C'
          'C#'
          'D'
          'D#'
          'E'
          'F'
          'F#'
          'G'
          'G#')
      end
      object cbOctave: TComboBox
        Left = 151
        Top = 104
        Width = 66
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 1
        Items.Strings = (
          '1'
          '2'
          '3'
          '4'
          '5'
          '6')
      end
      object edDuration: TEdit
        Left = 270
        Top = 104
        Width = 73
        Height = 21
        TabOrder = 2
        Text = '500'
      end
      object btPlay: TButton
        Left = 45
        Top = 132
        Width = 75
        Height = 25
        Caption = 'Play'
        TabOrder = 3
        OnClick = btPlayClick
      end
      object btStopSound: TButton
        Left = 126
        Top = 132
        Width = 75
        Height = 25
        Caption = 'Stop'
        TabOrder = 4
        OnClick = btStopSoundClick
      end
    end
    object tsRgb: TTabSheet
      Caption = 'RGB'
      ImageIndex = 4
      object laRgbDeviceInformation: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laRgbVersionCaption: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laRgbDeviceTypeCaption: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laRgbConnectionIdCaption: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laRgbConnectionId: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laRgbDeviceType: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laRgbVersion: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laColorMode: TLabel
        Left = 16
        Top = 101
        Width = 58
        Height = 13
        Caption = 'Color mode:'
      end
      object laR: TLabel
        Left = 15
        Top = 129
        Width = 11
        Height = 13
        Caption = 'R:'
      end
      object laG: TLabel
        Left = 104
        Top = 129
        Width = 11
        Height = 13
        Caption = 'G:'
      end
      object laB: TLabel
        Left = 188
        Top = 129
        Width = 10
        Height = 13
        Caption = 'B:'
      end
      object laColorIndex: TLabel
        Left = 15
        Top = 156
        Width = 54
        Height = 13
        Caption = 'Color index'
      end
      object cbColorMode: TComboBox
        Left = 80
        Top = 98
        Width = 105
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Discrete'
          'Absolute')
      end
      object btSetMode: TButton
        Left = 191
        Top = 95
        Width = 75
        Height = 25
        Caption = 'Set mode'
        TabOrder = 1
        OnClick = btSetModeClick
      end
      object edR: TEdit
        Left = 32
        Top = 126
        Width = 61
        Height = 21
        TabOrder = 2
      end
      object edG: TEdit
        Left = 121
        Top = 126
        Width = 61
        Height = 21
        TabOrder = 3
      end
      object edB: TEdit
        Left = 205
        Top = 126
        Width = 61
        Height = 21
        TabOrder = 4
      end
      object btSetRgb: TButton
        Left = 272
        Top = 124
        Width = 75
        Height = 25
        Caption = 'Set RGB'
        TabOrder = 5
        OnClick = btSetRgbClick
      end
      object cbColorIndex: TComboBox
        Left = 80
        Top = 153
        Width = 87
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 6
        Items.Strings = (
          'Black'
          'Pink'
          'Purple'
          'Blue'
          'Sky blue'
          'Teal'
          'Green'
          'Yellow'
          'Orange'
          'Red'
          'White')
      end
      object btSetIndex: TButton
        Left = 173
        Top = 151
        Width = 75
        Height = 25
        Caption = 'Set Index'
        TabOrder = 7
        OnClick = btSetIndexClick
      end
      object btSetDefault: TButton
        Left = 80
        Top = 181
        Width = 75
        Height = 25
        Caption = 'Set default'
        TabOrder = 8
        OnClick = btSetDefaultClick
      end
      object btTurnOff: TButton
        Left = 161
        Top = 181
        Width = 75
        Height = 25
        Caption = 'Turn off'
        TabOrder = 9
        OnClick = btTurnOffClick
      end
    end
    object tsMotion1: TTabSheet
      Caption = 'Motion sensor 1'
      ImageIndex = 5
      ExplicitLeft = 8
      ExplicitTop = 28
      object laMotionDeviceInformation1: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laMotionVersionCaption1: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laMotionDeviceTypeCaption1: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laMotionConnectionIdCaption1: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laMotionConnectionId1: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotionDeviceType1: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotionVersion1: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMode1: TLabel
        Left = 16
        Top = 107
        Width = 30
        Height = 13
        Caption = 'Mode:'
      end
      object laCountTitle1: TLabel
        Left = 16
        Top = 176
        Width = 33
        Height = 13
        Caption = 'Count:'
      end
      object laDistanceTitle1: TLabel
        Left = 16
        Top = 208
        Width = 45
        Height = 13
        Caption = 'Distance:'
      end
      object laCount1: TLabel
        Left = 77
        Top = 176
        Width = 6
        Height = 13
        Caption = '0'
      end
      object laDistance1: TLabel
        Left = 77
        Top = 208
        Width = 6
        Height = 13
        Caption = '0'
      end
      object cbMode1: TComboBox
        Left = 52
        Top = 104
        Width = 100
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Detect'
          'Count')
      end
      object btChange1: TButton
        Left = 158
        Top = 102
        Width = 75
        Height = 25
        Caption = 'Change'
        TabOrder = 1
        OnClick = btChange1Click
      end
      object btReset1: TButton
        Left = 158
        Top = 133
        Width = 75
        Height = 25
        Caption = 'Reset'
        TabOrder = 2
        OnClick = btReset1Click
      end
    end
    object tsMotion2: TTabSheet
      Caption = 'Motion sensor 2'
      ImageIndex = 6
      object laMotionDeviceInformation2: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laMotionVersionCaption2: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laMotionDeviceTypeCaption2: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laMotionConnectionIdCaption2: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laMotionConnectionId2: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotionDeviceType2: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotionVersion2: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMode2: TLabel
        Left = 16
        Top = 107
        Width = 30
        Height = 13
        Caption = 'Mode:'
      end
      object laCountTitle2: TLabel
        Left = 16
        Top = 176
        Width = 33
        Height = 13
        Caption = 'Count:'
      end
      object laDistanceTitle2: TLabel
        Left = 16
        Top = 208
        Width = 45
        Height = 13
        Caption = 'Distance:'
      end
      object laDistance2: TLabel
        Left = 77
        Top = 208
        Width = 6
        Height = 13
        Caption = '0'
      end
      object laCount2: TLabel
        Left = 77
        Top = 176
        Width = 6
        Height = 13
        Caption = '0'
      end
      object cbMode2: TComboBox
        Left = 52
        Top = 104
        Width = 100
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Detect'
          'Count')
      end
      object btChange2: TButton
        Left = 158
        Top = 102
        Width = 75
        Height = 25
        Caption = 'Change'
        TabOrder = 1
        OnClick = btChange2Click
      end
      object btReset2: TButton
        Left = 158
        Top = 133
        Width = 75
        Height = 25
        Caption = 'Reset'
        TabOrder = 2
        OnClick = btReset2Click
      end
    end
    object tsTilt1: TTabSheet
      Caption = 'Tilt sensor 1'
      ImageIndex = 7
      object laTiltDeviceInformation1: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laTiltVersionCaption1: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laTiltDeviceTypeCaption1: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laTiltConnectionCaption1: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laTiltConnectionId1: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laTiltDeviceType1: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laTiltVersion1: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laTiltMode1: TLabel
        Left = 16
        Top = 104
        Width = 30
        Height = 13
        Caption = 'Mode:'
      end
      object laDirectionTitle1: TLabel
        Left = 16
        Top = 136
        Width = 46
        Height = 13
        Caption = 'Direction:'
      end
      object laDirection1: TLabel
        Left = 92
        Top = 136
        Width = 44
        Height = 13
        Caption = 'Unknown'
      end
      object laXTitle1: TLabel
        Left = 16
        Top = 155
        Width = 10
        Height = 13
        Caption = 'X:'
      end
      object laYTitle1: TLabel
        Left = 16
        Top = 174
        Width = 10
        Height = 13
        Caption = 'Y:'
      end
      object laZTitle1: TLabel
        Left = 16
        Top = 193
        Width = 10
        Height = 13
        Caption = 'Z:'
      end
      object laX1: TLabel
        Left = 92
        Top = 155
        Width = 6
        Height = 13
        Caption = '0'
      end
      object laY1: TLabel
        Left = 92
        Top = 174
        Width = 6
        Height = 13
        Caption = '0'
      end
      object laZ1: TLabel
        Left = 92
        Top = 193
        Width = 6
        Height = 13
        Caption = '0'
      end
      object cbTiltMode1: TComboBox
        Left = 52
        Top = 101
        Width = 115
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Angle'
          'Tilt'
          'Crash')
      end
      object btChangeTilt1: TButton
        Left = 173
        Top = 99
        Width = 75
        Height = 25
        Caption = 'Change'
        TabOrder = 1
        OnClick = btChangeTilt1Click
      end
      object btResetTilt1: TButton
        Left = 173
        Top = 130
        Width = 75
        Height = 25
        Caption = 'Reset'
        TabOrder = 2
        OnClick = btResetTilt1Click
      end
    end
    object tsTilt2: TTabSheet
      Caption = 'Tilt sensor 2'
      ImageIndex = 8
      object laTiltDeviceInformation2: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laTiltVersionCaption2: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laTiltDeviceTypeCaption2: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laTiltConnectionCaption2: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laTiltConnectionId2: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laTiltDeviceType2: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laTiltVersion2: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laTiltMode2: TLabel
        Left = 16
        Top = 104
        Width = 30
        Height = 13
        Caption = 'Mode:'
      end
      object laDirection2: TLabel
        Left = 92
        Top = 136
        Width = 44
        Height = 13
        Caption = 'Unknown'
      end
      object laDirectionTitle2: TLabel
        Left = 16
        Top = 136
        Width = 46
        Height = 13
        Caption = 'Direction:'
      end
      object laXTitle2: TLabel
        Left = 16
        Top = 155
        Width = 10
        Height = 13
        Caption = 'X:'
      end
      object laYTitle2: TLabel
        Left = 16
        Top = 174
        Width = 10
        Height = 13
        Caption = 'Y:'
      end
      object laZTitle2: TLabel
        Left = 16
        Top = 193
        Width = 10
        Height = 13
        Caption = 'Z:'
      end
      object laZ2: TLabel
        Left = 92
        Top = 193
        Width = 6
        Height = 13
        Caption = '0'
      end
      object laY2: TLabel
        Left = 92
        Top = 174
        Width = 6
        Height = 13
        Caption = '0'
      end
      object laX2: TLabel
        Left = 92
        Top = 155
        Width = 6
        Height = 13
        Caption = '0'
      end
      object cbTiltMode2: TComboBox
        Left = 52
        Top = 101
        Width = 115
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Angle'
          'Tilt'
          'Crash')
      end
      object btChangeTilt2: TButton
        Left = 173
        Top = 99
        Width = 75
        Height = 25
        Caption = 'Change'
        TabOrder = 1
        OnClick = btChangeTilt2Click
      end
      object btResetTilt2: TButton
        Left = 173
        Top = 130
        Width = 75
        Height = 25
        Caption = 'Reset'
        TabOrder = 2
        OnClick = btResetTilt2Click
      end
    end
    object tsMotor1: TTabSheet
      Caption = 'Motor 1'
      ImageIndex = 9
      object laMotorDeviceInformation1: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laMotorVersionCaption1: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laMotorDeviceTypeCaption1: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laMotorConnectionIdCaption1: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laMotorConnectionId1: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotorDeviceType1: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotorVersion1: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotorDirectionCaption1: TLabel
        Left = 16
        Top = 112
        Width = 46
        Height = 13
        Caption = 'Direction:'
      end
      object laPower1: TLabel
        Left = 16
        Top = 140
        Width = 30
        Height = 13
        Caption = 'Power'
      end
      object cbMotorDirection1: TComboBox
        Left = 68
        Top = 109
        Width = 103
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Right'
          'Left')
      end
      object edPower1: TEdit
        Left = 68
        Top = 137
        Width = 103
        Height = 21
        TabOrder = 1
        Text = '20'
      end
      object btStart1: TButton
        Left = 177
        Top = 135
        Width = 75
        Height = 25
        Caption = 'Start'
        TabOrder = 2
        OnClick = btStart1Click
      end
      object btBrake1: TButton
        Left = 68
        Top = 164
        Width = 75
        Height = 25
        Caption = 'Brake'
        TabOrder = 3
        OnClick = btBrake1Click
      end
      object btDrift1: TButton
        Left = 149
        Top = 164
        Width = 75
        Height = 25
        Caption = 'Drift'
        TabOrder = 4
        OnClick = btDrift1Click
      end
    end
    object tsMotor2: TTabSheet
      Caption = 'Motor 2'
      ImageIndex = 10
      object laMotorDeviceInformation2: TLabel
        Left = 16
        Top = 16
        Width = 107
        Height = 13
        Caption = 'Device information'
        Font.Charset = DEFAULT_CHARSET
        Font.Color = clWindowText
        Font.Height = -11
        Font.Name = 'Tahoma'
        Font.Style = [fsBold]
        ParentFont = False
      end
      object laMotorVersionCaption2: TLabel
        Left = 32
        Top = 35
        Width = 39
        Height = 13
        Caption = 'Version:'
      end
      object laMotorDeviceTypeCaption2: TLabel
        Left = 32
        Top = 54
        Width = 61
        Height = 13
        Caption = 'Device type:'
      end
      object laMotorConnectionIdCaption2: TLabel
        Left = 32
        Top = 73
        Width = 72
        Height = 13
        Caption = 'Connection ID:'
      end
      object laMotorConnectionId2: TLabel
        Left = 121
        Top = 73
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotorDeviceType2: TLabel
        Left = 121
        Top = 54
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotorVersion2: TLabel
        Left = 121
        Top = 35
        Width = 46
        Height = 13
        Caption = '<empty>'
      end
      object laMotorDirectionCaption2: TLabel
        Left = 16
        Top = 112
        Width = 46
        Height = 13
        Caption = 'Direction:'
      end
      object laPower2: TLabel
        Left = 16
        Top = 140
        Width = 30
        Height = 13
        Caption = 'Power'
      end
      object cbMotorDirection2: TComboBox
        Left = 68
        Top = 109
        Width = 103
        Height = 22
        Style = csOwnerDrawFixed
        TabOrder = 0
        Items.Strings = (
          'Right'
          'Left')
      end
      object edPower2: TEdit
        Left = 68
        Top = 137
        Width = 103
        Height = 21
        TabOrder = 1
        Text = '20'
      end
      object btStart2: TButton
        Left = 177
        Top = 135
        Width = 75
        Height = 25
        Caption = 'Start'
        TabOrder = 2
        OnClick = btStart2Click
      end
      object btDrift2: TButton
        Left = 149
        Top = 164
        Width = 75
        Height = 25
        Caption = 'Drift'
        TabOrder = 3
        OnClick = btDrift2Click
      end
      object btBrake2: TButton
        Left = 68
        Top = 164
        Width = 75
        Height = 25
        Caption = 'Brake'
        TabOrder = 4
        OnClick = btBrake2Click
      end
    end
  end
  object btClear: TButton
    Left = 415
    Top = 392
    Width = 75
    Height = 25
    Caption = 'Clear'
    TabOrder = 5
    OnClick = btClearClick
  end
  object lbLog: TListBox
    Left = 8
    Top = 423
    Width = 482
    Height = 140
    ItemHeight = 13
    TabOrder = 6
  end
end
