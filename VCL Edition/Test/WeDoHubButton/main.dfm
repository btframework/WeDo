object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'fmMain'
  ClientHeight = 202
  ClientWidth = 662
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
    Width = 646
    Height = 154
    Columns = <
      item
        Caption = 'Address'
        Width = 120
      end
      item
        Caption = 'Name'
        Width = 200
      end
      item
        Caption = 'Connection state'
        Width = 100
      end
      item
        Caption = 'Battery'
        Width = 60
      end
      item
        Caption = 'Low Batt'
        Width = 60
      end
      item
        Caption = 'Button state'
        Width = 80
      end>
    ColumnClick = False
    GridLines = True
    HideSelection = False
    ReadOnly = True
    RowSelect = True
    TabOrder = 2
    ViewStyle = vsReport
  end
end
