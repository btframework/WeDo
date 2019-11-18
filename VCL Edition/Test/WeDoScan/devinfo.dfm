object fmDevInfo: TfmDevInfo
  Left = 0
  Top = 0
  BorderStyle = bsDialog
  Caption = 'WeDo Device Information'
  ClientHeight = 271
  ClientWidth = 862
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poOwnerFormCenter
  OnClose = FormClose
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 13
  object laDeviceInformationTitle: TLabel
    Left = 8
    Top = 8
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
    Left = 20
    Top = 35
    Width = 86
    Height = 13
    Caption = 'Firmware version:'
  end
  object laHardwareVersionTitle: TLabel
    Left = 20
    Top = 64
    Width = 89
    Height = 13
    Caption = 'Hardware version:'
  end
  object laSoftwareVersionTitle: TLabel
    Left = 20
    Top = 94
    Width = 86
    Height = 13
    Caption = 'Software version:'
  end
  object laManufacturerNameTitle: TLabel
    Left = 20
    Top = 124
    Width = 98
    Height = 13
    Caption = 'Manufacturer name:'
  end
  object laFirmwareVersion: TLabel
    Left = 136
    Top = 35
    Width = 46
    Height = 13
    Caption = '<empty>'
  end
  object laHardwareVersion: TLabel
    Left = 136
    Top = 64
    Width = 46
    Height = 13
    Caption = '<empty>'
  end
  object laSoftwareVersion: TLabel
    Left = 136
    Top = 94
    Width = 46
    Height = 13
    Caption = '<empty>'
  end
  object laManufacturerName: TLabel
    Left = 136
    Top = 124
    Width = 46
    Height = 13
    Caption = '<empty>'
  end
  object laBattLevelTitle: TLabel
    Left = 8
    Top = 168
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
    Left = 136
    Top = 168
    Width = 20
    Height = 13
    Caption = '0 %'
  end
  object laDeviceName: TLabel
    Left = 208
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
  object laLowVoltage: TLabel
    Left = 20
    Top = 227
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
    Left = 224
    Top = 227
    Width = 62
    Height = 13
    Caption = 'Low Signal!'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clRed
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    Visible = False
  end
  object laBattTypeTitle: TLabel
    Left = 480
    Top = 168
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
  object laBattType: TLabel
    Left = 568
    Top = 168
    Width = 49
    Height = 13
    Caption = 'Undefined'
  end
  object lvAttachedDevices: TListView
    Left = 272
    Top = 35
    Width = 577
    Height = 102
    Columns = <
      item
        Caption = 'Connection ID'
        Width = 100
      end
      item
        Caption = 'Device type'
        Width = 100
      end
      item
        Caption = 'Firmware version'
        Width = 100
      end
      item
        Caption = 'Hardware version'
        Width = 100
      end
      item
        Caption = 'Is Internal'
        Width = 80
      end
      item
        Caption = 'Port ID'
        Width = 60
      end>
    ColumnClick = False
    GridLines = True
    HideSelection = False
    ReadOnly = True
    RowSelect = True
    TabOrder = 0
    ViewStyle = vsReport
  end
  object pbBattLevel: TProgressBar
    Left = 20
    Top = 200
    Width = 162
    Height = 21
    TabOrder = 1
  end
  object edDeviceName: TEdit
    Left = 224
    Top = 200
    Width = 153
    Height = 21
    TabOrder = 2
  end
  object btSetDeviceName: TButton
    Left = 383
    Top = 198
    Width = 75
    Height = 25
    Caption = 'Set'
    TabOrder = 3
    OnClick = btSetDeviceNameClick
  end
  object btTurnOff: TButton
    Left = 774
    Top = 198
    Width = 75
    Height = 25
    Caption = 'Turn Off'
    TabOrder = 4
    OnClick = btTurnOffClick
  end
  object btClose: TButton
    Left = 774
    Top = 229
    Width = 75
    Height = 25
    Cancel = True
    Caption = 'Close'
    Default = True
    ModalResult = 1
    TabOrder = 5
    OnClick = btCloseClick
  end
end
