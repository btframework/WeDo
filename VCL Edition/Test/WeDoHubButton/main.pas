unit main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants,
  System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs,
  wclBluetooth, wclWeDoWatcher, wclWeDoHub, Vcl.ComCtrls, Vcl.StdCtrls,
  System.generics.Collections;

type
  TfmMain = class(TForm)
    btStart: TButton;
    btStop: TButton;
    lvHubs: TListView;
    procedure FormCreate(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure btStopClick(Sender: TObject);
    procedure FormDestroy(Sender: TObject);

  private
    FManager: TwclBluetoothManager;
    FWatcher: TwclWeDoWatcher;
    FHubs: TList<TwclWeDoHub>;

    procedure UpdateButtons(Started: Boolean);

    procedure FWatcher_OnStopped(Sender: TObject);
    procedure FWatcher_OnStarted(Sender: TObject);
    procedure FWatcher_OnHubNameChanged(Sender: TObject; Address: Int64;
      OldName: string; NewName: string);
    procedure FWatcher_OnHubFound(Sender: TObject; Address: Int64;
      Name: string);

    procedure Hub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
    procedure Hub_OnButtonStateChanged(Sender: TObject; Pressed: Boolean);
    procedure Hub_OnDisconnected(Sender: TObject; Reason: Integer);
    procedure Hub_OnConnected(Sender: TObject; Error: Integer);

    procedure BatteryLevel_OnBatteryLevelChanged(Sender: TObject; Level: Byte);

    procedure Stop;
  end;

var
  fmMain: TfmMain;

implementation

uses
  wclErrors;

{$R *.dfm}

{ TfmMain }

procedure TfmMain.BatteryLevel_OnBatteryLevelChanged(Sender: TObject;
  Level: Byte);
var
  Address: Int64;
  Item: TListItem;
begin
  Address := TwclWeDoBatteryLevelService(Sender).Hub.Address;
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Address, 12) then
      Item.SubItems[2] := IntToStr(Level);
  end;
end;

procedure TfmMain.btStartClick(Sender: TObject);
var
  Res: Integer;
  Radio: TwclBluetoothRadio;
  i: Integer;
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

    // Check that at least one Bluetooth Radio exists (or at least Bluetooth
    // drivers installed).
    if FManager.Count = 0 then
      // No one, even drivers?

      ShowMessage('No Bluetooth Hardware installed')
    else begin
      // Ok, at least one Bluetooth Radio module should be available.
      for i := 0 to FManager.Count - 1 do begin
        // Check if current Radio module is available (plugged in and turned ON).
        if FManager[i].Available then begin
          // Looks like we have Bluetooth on this PC!
          Radio := FManager[i];
          // Terminate the loop.
          Break;
        end;
      end;

      // Check that we found the Bluetooth Radio module.
      if Radio = nil then
        // If not, let user know that he has no Bluetooth.
        ShowMessage('No available Bluetooth Radio found')

      else begin
        // If found, try to start discovering.
        Res := FWatcher.Start(Radio);
        if Res <> WCL_E_SUCCESS then begin
          // It is something wrong with discovering starting. Notify user about
          // the error.
          ShowMessage('Unable to start discovering: 0x' + IntToHex(Res, 8));
          // Also clean up found Radio variable so we can check it later.
          Radio := nil;

        end else begin
          btStart.Enabled := False;
          btStop.Enabled := True;
        end;
      end;
    end;

    // Again, check the found Radio.
    if Radio = nil then
      // And if it is null (not found or discovering was not started
      // close the Bluetooth Manager to release all the allocated resources.
      FManager.Close;
  end;
end;

procedure TfmMain.btStopClick(Sender: TObject);
begin
  // We need to execute stop operatrion in few places: when Stop button clicked
  // and when application closed. So use separate function to preven us from
  // code duplication (copy/patse is very bad practice).
  Stop;
end;

procedure TfmMain.FormCreate(Sender: TObject);
begin
  FManager := TwclBluetoothManager.Create(nil);

  FWatcher := TwclWeDoWatcher.Create(nil);
  FWatcher.OnHubFound := FWatcher_OnHubFound;
  FWatcher.OnHubNameChanged := FWatcher_OnHubNameChanged;
  FWatcher.OnStarted := FWatcher_OnStarted;
  FWatcher.OnStopped := FWatcher_OnStopped;

  FHubs := TList<TwclWeDoHub>.Create;
end;

procedure TfmMain.FormDestroy(Sender: TObject);
begin
  // Stop discovering.
  Stop;

  FManager.Free;
  FWatcher.Free;
  FHubs.Free;
end;

procedure TfmMain.FWatcher_OnHubFound(Sender: TObject; Address: Int64;
  Name: string);
var
  Hub: TwclWeDoHub;
  Res: Integer;
  Item: TListItem;
begin
  Hub := TwclWeDoHub.Create(nil);
  Hub.OnConnected := Hub_OnConnected;
  Hub.OnDisconnected := Hub_OnDisconnected;
  Hub.OnButtonStateChanged := Hub_OnButtonStateChanged;
  Hub.OnLowVoltageAlert := Hub_OnLowVoltageAlert;
  Hub.BatteryLevel.OnBatteryLevelChanged := BatteryLevel_OnBatteryLevelChanged;

  Res := Hub.Connect(FWatcher.Radio, Address);
  if Res = WCL_E_SUCCESS then begin
    Item := lvHubs.Items.Add();
    Item.Caption := IntToHex(Address, 12);
    Item.SubItems.Add(Name);
    Item.SubItems.Add('Connecting');
    Item.SubItems.Add('');
    Item.SubItems.Add('');
    Item.SubItems.Add('');
    FHubs.Add(Hub);
  end else
    Hub.Free;
end;

procedure TfmMain.FWatcher_OnHubNameChanged(Sender: TObject; Address: Int64;
  OldName: string; NewName: string);
var
  Item: TListItem;
begin
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Address, 12) then
      Item.SubItems[0] := Name;
  end;
end;

procedure TfmMain.FWatcher_OnStarted(Sender: TObject);
begin
  UpdateButtons(True);
end;

procedure TfmMain.FWatcher_OnStopped(Sender: TObject);
begin
  UpdateButtons(False);
end;

procedure TfmMain.Hub_OnButtonStateChanged(Sender: TObject; Pressed: Boolean);
var
  Address: Int64;
  Item: TListItem;
begin
  Address := TwclWeDoHub(Sender).Address;
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Address, 12) then begin
      if Pressed then
        Item.SubItems[4] := 'PRESSED'
      else
        Item.SubItems[4] := '';
    end;
  end;
end;

procedure TfmMain.Hub_OnConnected(Sender: TObject; Error: Integer);
var
  Hub: TwclWeDoHub;
  Item: TListItem;
  Level: Byte;
  Res: Integer;
begin
  Hub := TwclWeDoHub(Sender);
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Hub.Address, 12) then begin
      if Error <> WCL_E_SUCCESS then begin
        lvHubs.Items.Delete(Item.Indent);
        FHubs.Remove(Hub);
        Hub.Free;
        Break;
      end else begin
        Item.SubItems[1] := 'Connected';
        Res := Hub.BatteryLevel.ReadBatteryLevel(Level);
        if Res = WCL_E_SUCCESS then
          Item.SubItems[2] := IntToStr(Level);
      end;
    end;
  end;
end;

procedure TfmMain.Hub_OnDisconnected(Sender: TObject; Reason: Integer);
var
  Hub: TwclWeDoHub;
  Item: TListItem;
begin
  Hub := TwclWeDoHub(Sender);
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Hub.Address, 12) then begin
      lvHubs.Items.Delete(Item.Index);
      FHubs.Remove(Hub);
      Hub.Free;
      Break;
    end;
  end;
end;

procedure TfmMain.Hub_OnLowVoltageAlert(Sender: TObject; Alert: Boolean);
var
  Address: Int64;
  Item: TListItem;
begin
  Address := TwclWeDoHub(Sender).Address;
  for Item in lvHubs.Items do begin
    if Item.Caption = IntToHex(Address, 12) then begin
      if Alert then
        Item.SubItems[3] := 'ALERT'
      else
        Item.SubItems[3] := '';
    end;
  end;
end;

procedure TfmMain.Stop;
begin
  // Stop discovering.
  FWatcher.Stop;
  // Close Bluetooth Manager.
  FManager.Close;
end;

procedure TfmMain.UpdateButtons(Started: Boolean);
begin
  if Started then begin
    btStart.Enabled := False;
    btStop.Enabled := True;
  end else begin
    btStart.Enabled := True;
    btStop.Enabled := False;

    while FHubs.Count > 0 do
      FHubs[0].Disconnect;

    lvHubs.Items.Clear;
  end;
end;

end.
