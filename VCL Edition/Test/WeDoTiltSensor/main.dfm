object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'fmMain'
  ClientHeight = 258
  ClientWidth = 291
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
    Left = 24
    Top = 80
    Width = 30
    Height = 13
    Caption = 'Mode:'
    Enabled = False
  end
  object laDirectionTitle: TLabel
    Left = 24
    Top = 160
    Width = 46
    Height = 13
    Caption = 'Direction:'
    Enabled = False
  end
  object laDirection: TLabel
    Left = 96
    Top = 160
    Width = 44
    Height = 13
    Caption = 'Unknown'
    Enabled = False
  end
  object laXTitle: TLabel
    Left = 23
    Top = 192
    Width = 10
    Height = 13
    Caption = 'X:'
    Enabled = False
  end
  object laYTitle: TLabel
    Left = 23
    Top = 211
    Width = 10
    Height = 13
    Caption = 'Y:'
    Enabled = False
  end
  object laZTitle: TLabel
    Left = 24
    Top = 230
    Width = 10
    Height = 13
    Caption = 'Z:'
    Enabled = False
  end
  object laX: TLabel
    Left = 96
    Top = 192
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laY: TLabel
    Left = 96
    Top = 211
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laZ: TLabel
    Left = 96
    Top = 230
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
    Left = 60
    Top = 77
    Width = 117
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 2
    Items.Strings = (
      'Angle'
      'Tilt'
      'Crash')
  end
  object btChange: TButton
    Left = 183
    Top = 75
    Width = 75
    Height = 25
    Caption = 'Change'
    Enabled = False
    TabOrder = 3
    OnClick = btChangeClick
  end
  object btReset: TButton
    Left = 183
    Top = 106
    Width = 75
    Height = 25
    Caption = 'Reset'
    Enabled = False
    TabOrder = 4
    OnClick = btResetClick
  end
end
