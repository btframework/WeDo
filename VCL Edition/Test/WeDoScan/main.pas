unit main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  Vcl.StdCtrls, Vcl.ComCtrls, wclWeDoWatcher, wclBluetooth;

type
  TfmMain = class(TForm)
    btStart: TButton;
    btStop: TButton;
    lvHubs: TListView;
    btInfo: TButton;
    procedure FormCreate(Sender: TObject);
    procedure btStopClick(Sender: TObject);
    procedure FormDestroy(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure lvHubsSelectItem(Sender: TObject; Item: TListItem;
      Selected: Boolean);
    procedure btInfoClick(Sender: TObject);

  private
    // The Bluetooth Manager object (from Bluetooth Framework) allows to
    // control Bluetooth hardware. It is required to work with Bluetooth.
    FManager: TwclBluetoothManager;
    // The WeDo Watcher allows to scan for available WeDo Hubs.
    FWatcher: TwclWeDoWatcher;

    procedure FWatcher_OnHubNameChanged(Sender: TObject; Address: Int64;
      OldName: string; NewName: string);
    procedure FWatcher_OnHubLost(Sender: TObject; Address: Int64; Name: string);
    procedure FWatcher_OnHubFound(Sender: TObject; Address: Int64;
      Name: string);

    procedure FWatcher_OnStopped(SenderO: TObject);
    procedure FWatcher_OnStarted(Sender: TObject);

    procedure EnableButtons(const Started: Boolean);

    procedure Stop;
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors, devinfo;

{$R *.dfm}

procedure TfmMain.btInfoClick(Sender: TObject);
begin
  // We use separate form to show selected Hub information.
  fmDevInfo := TfmDevInfo.Create(Self, FWatcher.Radio,
    StrToInt64('$' + lvHubs.Selected.Caption));
  fmDevInfo.ShowModal;
  fmDevInfo.Free;
end;

procedure TfmMain.btStartClick(Sender: TObject);
var
  Res: Integer;
  Radio: TwclBluetoothRadio;
begin
  // The very first thing we have to do is to open Bluetooth Manager.
  // That initializes the underlying drivers and allows us to work with
  // Bluetooth.

  // Always check result!
  Res := FManager.Open;
  if Res <> WCL_E_SUCCESS then
    // It should never happen but if it does notify user.
    ShowMessage('Unable to open Bluetooth Manager: 0x' + IntToHex(Res, 8))

  else begin
    // Assume that no one Bluetooth Radio available.
    Radio := nil;
    Res := FManager.GetLeRadio(Radio);
    if Res <> WCL_E_SUCCESS then
      // If not, let user know that he has no Bluetooth.
      ShowMessage('No available Bluetooth Radio found')

    else begin
      // If found, try to start discovering.
      Res := FWatcher.Start(Radio);
      if Res <> WCL_E_SUCCESS then
        // It is something wrong with discovering starting. Notify user about
        // the error.
        ShowMessage('Unable to start discovering: 0x' + IntToHex(Res, 8))
    end;

    // Again, check the found Radio.
    if Res <> WCL_E_SUCCESS then begin
      // And if it is null (not found or discovering was not started
      // close the Bluetooth Manager to release all the allocated resources.
      FManager.Close;
      // Also clean up found Radio variable so we can check it later.
      Radio := nil;
    end;
  end;
end;

procedure TfmMain.btStopClick(Sender: TObject);
begin
  // We need to execute stop operatrion in few places: when Stop button clicked
  // and when application closed. So use separate function to preven us from
  // code duplication (copy/patse is very bad practice).
  Stop;
end;

procedure TfmMain.EnableButtons(const Started: Boolean);
begin
  // Started flag indicates how we should update buttons.
  if Started then begin
    // If Started is true then we have to disable Start button and enable
    // Stop button.
    btStart.Enabled := False;
    btStop.Enabled := True;
  end else begin
    // If Started is false disable Stop button and enable Start button.
    btStart.Enabled := True;
    btStop.Enabled := False;
    btInfo.Enabled := False;
  end;

  // Also clear found devices (hubs) list.
  lvHubs.Items.Clear;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  // Create Bluetooth Manager. We do not need any events from it so simple
  // create the object.
  FManager := TwclBluetoothManager.Create(nil);

  // Now create WeDo Watcher. We need some events from it so also assign event
  // handler.
  FWatcher := TwclWeDoWatcher.Create(nil);
  // The event fires when we started scanning.
  FWatcher.OnStarted := FWatcher_OnStarted;
  // The event fires when we stopped scanning.
  FWatcher.OnStopped := FWatcher_OnStopped;
  // The event fires when new WeDo Hub found.
  FWatcher.OnHubFound := FWatcher_OnHubFound;
  // The event fires when WeDo hub lost.
  FWatcher.OnHubLost := FWatcher_OnHubLost;
  // The event fires when WeDo Hub name changed.
  FWatcher.OnHubNameChanged := FWatcher_OnHubNameChanged;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  // Stop discovering.
  Stop;

  FWatcher.Free;
  FManager.Free;
end;

procedure TfmMain.FWatcher_OnHubFound(Sender: TObject; Address: Int64;
  Name: string);
var
  Item: TLIstItem;
begin
  // One new Hub found simple add it to the list.
  // In Bluetooth world the device's address is always shown as hexadecimal.
  Item := lvHubs.Items.Add;
  Item.Caption := IntToHex(Address, 12);
  // Add name.
  Item.SubItems.Add(Name);
end;

procedure TfmMain.FWatcher_OnHubLost(Sender: TObject; Address: Int64;
  Name: string);
var
  Item: TListItem;
begin
  // Remove Hub from the list.
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Address, 12) then begin
      lvHubs.Items.Delete(lvHubs.Items.IndexOf(Item));
      Break;
    end;
  end;
end;

procedure TfmMain.FWatcher_OnHubNameChanged(Sender: TObject; Address: Int64;
  OldName: string; NewName: string);
var
  Item: TListItem;
begin
  // Update Hub name in the list.
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Address, 12) then begin
      Item.SubItems[0] := NewName;
      Break;
    end;
  end;
end;

procedure TfmMain.FWatcher_OnStarted(Sender: TObject);
begin
  // Here is nothing important to do. Just disable/enable buttons to update UI.
  // Call separate function. This prevents us from writting the same code few
  // times.
  EnableButtons(True);
end;

procedure TfmMain.FWatcher_OnStopped(SenderO: TObject);
begin
  // Here is nothing important to do. Just disable/enable buttons to update UI.
  // Call separate function. This prevents us from writting the same code few
  // times.
  EnableButtons(False);
end;

procedure TfmMain.lvHubsSelectItem(Sender: TObject; Item: TListItem;
  Selected: Boolean);
begin
  // We should enable Get Information button only when device is selected
  // in the list.
  btInfo.Enabled := lvHubs.Selected <> nil;
end;

procedure TfmMain.Stop;
begin
  // Stop discovering.
  FWatcher.Stop;
  // Close Bluetooth Manager.
  FManager.Close;
end;

end.
