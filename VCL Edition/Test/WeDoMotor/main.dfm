object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'WeDo Motor Test Application'
  ClientHeight = 201
  ClientWidth = 424
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
  object laStatus: TLabel
    Left = 24
    Top = 48
    Width = 76
    Height = 13
    Caption = 'Disconnected'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object laIoState: TLabel
    Left = 216
    Top = 48
    Width = 54
    Height = 13
    Caption = 'Detached'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object laCurrentTitle: TLabel
    Left = 24
    Top = 88
    Width = 41
    Height = 13
    Caption = 'Current:'
    Enabled = False
  end
  object laCurrent: TLabel
    Left = 71
    Top = 88
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laMA: TLabel
    Left = 128
    Top = 88
    Width = 15
    Height = 13
    Caption = 'mA'
    Enabled = False
  end
  object laVoltageTitle: TLabel
    Left = 192
    Top = 88
    Width = 40
    Height = 13
    Caption = 'Voltage:'
    Enabled = False
  end
  object laVoltage: TLabel
    Left = 239
    Top = 88
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laMV: TLabel
    Left = 288
    Top = 88
    Width = 14
    Height = 13
    Caption = 'mV'
    Enabled = False
  end
  object laDirection: TLabel
    Left = 24
    Top = 136
    Width = 46
    Height = 13
    Caption = 'Direction:'
    Enabled = False
  end
  object laPower: TLabel
    Left = 199
    Top = 136
    Width = 34
    Height = 13
    Caption = 'Power:'
    Enabled = False
  end
  object laHighCurrent: TLabel
    Left = 202
    Top = 173
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
  object laLowVoltage: TLabel
    Left = 304
    Top = 173
    Width = 71
    Height = 13
    Caption = 'Low Voltage!'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clRed
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = [fsBold]
    ParentFont = False
    Visible = False
  end
  object btConnect: TButton
    Left = 8
    Top = 8
    Width = 75
    Height = 25
    Caption = 'Connect'
    TabOrder = 0
    OnClick = btConnectClick
  end
  object btDisconnect: TButton
    Left = 89
    Top = 8
    Width = 75
    Height = 25
    Caption = 'Disconnect'
    TabOrder = 1
    OnClick = btDisconnectClick
  end
  object cbDirection: TComboBox
    Left = 76
    Top = 133
    Width = 117
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 2
    Items.Strings = (
      'Right'
      'Left')
  end
  object edPower: TEdit
    Left = 248
    Top = 133
    Width = 81
    Height = 21
    Enabled = False
    TabOrder = 3
    Text = '20'
  end
  object btStart: TButton
    Left = 335
    Top = 131
    Width = 75
    Height = 25
    Caption = 'Start'
    Enabled = False
    TabOrder = 4
    OnClick = btStartClick
  end
  object btBrake: TButton
    Left = 24
    Top = 168
    Width = 75
    Height = 25
    Caption = 'Brake'
    Enabled = False
    TabOrder = 5
    OnClick = btBrakeClick
  end
  object btDrift: TButton
    Left = 105
    Top = 168
    Width = 75
    Height = 25
    Caption = 'Drift'
    Enabled = False
    TabOrder = 6
    OnClick = btDriftClick
  end
end
