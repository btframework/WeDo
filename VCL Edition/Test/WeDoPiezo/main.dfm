object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'WeDo Piezo Test Application'
  ClientHeight = 156
  ClientWidth = 439
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
    Left = 8
    Top = 56
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
    Left = 200
    Top = 56
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
  object laNote: TLabel
    Left = 8
    Top = 88
    Width = 27
    Height = 13
    Caption = 'Note:'
  end
  object laOctave: TLabel
    Left = 159
    Top = 88
    Width = 35
    Height = 13
    Caption = 'Octave'
  end
  object laDuration: TLabel
    Left = 303
    Top = 88
    Width = 41
    Height = 13
    Caption = 'Duration'
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
    Enabled = False
    TabOrder = 1
    OnClick = btDisconnectClick
  end
  object cbNote: TComboBox
    Left = 41
    Top = 85
    Width = 96
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 2
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
    Left = 200
    Top = 85
    Width = 89
    Height = 21
    Style = csDropDownList
    Enabled = False
    TabOrder = 3
    Items.Strings = (
      '1'
      '2'
      '3'
      '4'
      '5'
      '6')
  end
  object edDuration: TEdit
    Left = 350
    Top = 85
    Width = 75
    Height = 21
    Enabled = False
    TabOrder = 4
    Text = '500'
  end
  object btPlay: TButton
    Left = 41
    Top = 123
    Width = 75
    Height = 25
    Caption = 'Play'
    Enabled = False
    TabOrder = 5
    OnClick = btPlayClick
  end
  object btStop: TButton
    Left = 122
    Top = 123
    Width = 75
    Height = 25
    Caption = 'Stop'
    Enabled = False
    TabOrder = 6
    OnClick = btStopClick
  end
end
