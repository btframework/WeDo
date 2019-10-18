object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsDialog
  Caption = 'fmMain'
  ClientHeight = 249
  ClientWidth = 382
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
  object laColorMode: TLabel
    Left = 24
    Top = 80
    Width = 58
    Height = 13
    Caption = 'Color mode:'
    Enabled = False
  end
  object laR: TLabel
    Left = 24
    Top = 120
    Width = 11
    Height = 13
    Caption = 'R:'
    Enabled = False
  end
  object laG: TLabel
    Left = 112
    Top = 120
    Width = 11
    Height = 13
    Caption = 'G:'
    Enabled = False
  end
  object laB: TLabel
    Left = 207
    Top = 120
    Width = 10
    Height = 13
    Caption = 'B:'
    Enabled = False
  end
  object laColorIndex: TLabel
    Left = 24
    Top = 176
    Width = 58
    Height = 13
    Caption = 'Color index:'
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
  object cbColorMode: TComboBox
    Left = 88
    Top = 77
    Width = 145
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 2
    Items.Strings = (
      'Discrete'
      'Absolute')
  end
  object edR: TEdit
    Left = 41
    Top = 117
    Width = 59
    Height = 21
    Enabled = False
    TabOrder = 3
  end
  object edG: TEdit
    Left = 129
    Top = 117
    Width = 59
    Height = 21
    Enabled = False
    TabOrder = 4
  end
  object edB: TEdit
    Left = 223
    Top = 117
    Width = 59
    Height = 21
    Enabled = False
    TabOrder = 5
  end
  object btSetRgb: TButton
    Left = 288
    Top = 115
    Width = 75
    Height = 25
    Caption = 'Set RGB'
    Enabled = False
    TabOrder = 6
    OnClick = btSetRgbClick
  end
  object cbColorIndex: TComboBox
    Left = 88
    Top = 173
    Width = 113
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 7
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
    Left = 207
    Top = 171
    Width = 75
    Height = 25
    Caption = 'Set index'
    Enabled = False
    TabOrder = 8
    OnClick = btSetIndexClick
  end
  object btSetDefault: TButton
    Left = 24
    Top = 208
    Width = 75
    Height = 25
    Caption = 'Set default'
    Enabled = False
    TabOrder = 9
    OnClick = btSetDefaultClick
  end
  object btTurnOff: TButton
    Left = 105
    Top = 208
    Width = 75
    Height = 25
    Caption = 'Turn Off'
    Enabled = False
    TabOrder = 10
    OnClick = btTurnOffClick
  end
  object btSetMode: TButton
    Left = 288
    Top = 75
    Width = 75
    Height = 25
    Caption = 'Set mode'
    Enabled = False
    TabOrder = 11
    OnClick = btSetModeClick
  end
end
