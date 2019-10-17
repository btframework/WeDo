object fmMain: TfmMain
  Left = 0
  Top = 0
  BorderStyle = bsSingle
  Caption = 'WeDo Search Application'
  ClientHeight = 228
  ClientWidth = 402
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
    Width = 386
    Height = 150
    Columns = <
      item
        Caption = 'Address'
        Width = 140
      end
      item
        Caption = 'Name'
        Width = 210
      end>
    ColumnClick = False
    FlatScrollBars = True
    GridLines = True
    HideSelection = False
    ReadOnly = True
    RowSelect = True
    TabOrder = 2
    ViewStyle = vsReport
    OnSelectItem = lvHubsSelectItem
  end
  object btInfo: TButton
    Left = 8
    Top = 195
    Width = 386
    Height = 25
    Caption = 'Get Hub Information'
    Enabled = False
    TabOrder = 3
    OnClick = btInfoClick
  end
end
