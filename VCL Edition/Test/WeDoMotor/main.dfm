object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'WeDo Motor Test Application'
  ClientHeight = 275
  ClientWidth = 461
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
  object laCurrentTitle: TLabel
    Left = 26
    Top = 72
    Width = 41
    Height = 13
    Caption = 'Current:'
    Enabled = False
  end
  object laCurrent: TLabel
    Left = 73
    Top = 72
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laMA: TLabel
    Left = 130
    Top = 72
    Width = 15
    Height = 13
    Caption = 'mA'
    Enabled = False
  end
  object laVoltageTitle: TLabel
    Left = 194
    Top = 72
    Width = 40
    Height = 13
    Caption = 'Voltage:'
    Enabled = False
  end
  object laVoltage: TLabel
    Left = 241
    Top = 72
    Width = 6
    Height = 13
    Caption = '0'
    Enabled = False
  end
  object laMV: TLabel
    Left = 290
    Top = 72
    Width = 14
    Height = 13
    Caption = 'mV'
    Enabled = False
  end
  object laHighCurrent: TLabel
    Left = 26
    Top = 91
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
    Left = 194
    Top = 91
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
  object PageControl: TPageControl
    Left = 8
    Top = 120
    Width = 441
    Height = 145
    ActivePage = tsMotor1
    TabOrder = 2
    object tsMotor1: TTabSheet
      Caption = 'Motor 1'
      ExplicitHeight = 121
      object laIoState1: TLabel
        Left = 14
        Top = 13
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
      object laDirection1: TLabel
        Left = 14
        Top = 40
        Width = 46
        Height = 13
        Caption = 'Direction:'
        Enabled = False
      end
      object laPower1: TLabel
        Left = 197
        Top = 40
        Width = 34
        Height = 13
        Caption = 'Power:'
        Enabled = False
      end
      object cbDirection1: TComboBox
        Left = 74
        Top = 32
        Width = 117
        Height = 21
        Style = csDropDownList
        Enabled = False
        TabOrder = 0
        Items.Strings = (
          'Right'
          'Left')
      end
      object edPower1: TEdit
        Left = 239
        Top = 32
        Width = 81
        Height = 21
        Enabled = False
        TabOrder = 1
        Text = '20'
      end
      object btStart1: TButton
        Left = 334
        Top = 28
        Width = 75
        Height = 25
        Caption = 'Start'
        Enabled = False
        TabOrder = 2
        OnClick = btStart1Click
      end
      object btBrake1: TButton
        Left = 13
        Top = 76
        Width = 75
        Height = 25
        Caption = 'Brake'
        Enabled = False
        TabOrder = 3
        OnClick = btBrake1Click
      end
      object btDrift1: TButton
        Left = 94
        Top = 75
        Width = 75
        Height = 25
        Caption = 'Drift'
        Enabled = False
        TabOrder = 4
        OnClick = btDrift1Click
      end
    end
    object tsMotor2: TTabSheet
      Caption = 'Motor 2'
      ImageIndex = 1
      ExplicitLeft = 0
      object laIoState2: TLabel
        Left = 22
        Top = 21
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
      object laDirection2: TLabel
        Left = 28
        Top = 40
        Width = 46
        Height = 13
        Caption = 'Direction:'
        Enabled = False
      end
      object laPower2: TLabel
        Left = 207
        Top = 48
        Width = 34
        Height = 13
        Caption = 'Power:'
        Enabled = False
      end
      object cbDirection2: TComboBox
        Left = 80
        Top = 40
        Width = 117
        Height = 21
        Style = csDropDownList
        Enabled = False
        TabOrder = 0
        Items.Strings = (
          'Right'
          'Left')
      end
      object edPower2: TEdit
        Left = 247
        Top = 40
        Width = 81
        Height = 21
        Enabled = False
        TabOrder = 1
        Text = '20'
      end
      object btStart2: TButton
        Left = 342
        Top = 36
        Width = 75
        Height = 25
        Caption = 'Start'
        Enabled = False
        TabOrder = 2
        OnClick = btStart2Click
      end
      object btDrift2: TButton
        Left = 102
        Top = 83
        Width = 75
        Height = 25
        Caption = 'Drift'
        Enabled = False
        TabOrder = 3
        OnClick = btDrift2Click
      end
      object btBrake2: TButton
        Left = 21
        Top = 83
        Width = 75
        Height = 25
        Caption = 'Brake'
        Enabled = False
        TabOrder = 4
        OnClick = btBrake2Click
      end
    end
  end
end
