object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'WeDo Motion Sensor Test'
  ClientHeight = 229
  ClientWidth = 290
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
  object laMode: TLabel
    Left = 8
    Top = 88
    Width = 30
    Height = 13
    Caption = 'Mode:'
    Enabled = False
  end
  object laCountTitle: TLabel
    Left = 8
    Top = 168
    Width = 33
    Height = 13
    Caption = 'Count:'
    Enabled = False
  end
  object laDistanceTitle: TLabel
    Left = 8
    Top = 200
    Width = 41
    Height = 13
    Caption = 'Distance'
    Enabled = False
  end
  object laCount: TLabel
    Left = 69
    Top = 168
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laDistance: TLabel
    Left = 69
    Top = 200
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
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
  object cbMode: TComboBox
    Left = 44
    Top = 85
    Width = 120
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 2
    Items.Strings = (
      'Detect'
      'Count')
  end
  object btChange: TButton
    Left = 170
    Top = 83
    Width = 75
    Height = 25
    Caption = 'Change'
    Enabled = False
    TabOrder = 3
    OnClick = btChangeClick
  end
  object btReset: TButton
    Left = 170
    Top = 114
    Width = 75
    Height = 25
    Caption = 'Reset'
    Enabled = False
    TabOrder = 4
    OnClick = btResetClick
  end
end
