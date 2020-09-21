////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//   Wireless Communication Library 7                                         //
//                                                                            //
//   Copyright (C) 2006-2019 Mike Petrichenko                                 //
//                           Soft Service Company                             //
//                           All Rights Reserved                              //
//                                                                            //
//   http://www.btframework.com                                               //
//                                                                            //
//   support@btframework.com                                                  //
//   shop@btframework.com                                                     //
//                                                                            //
// -------------------------------------------------------------------------- //
//                                                                            //
//   WCL Bluetooth Framework: Lego WeDo 2.0 Education Extension.              //
//                                                                            //
//     https://github.com/btframework/WeDo                                    //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

unit wclWeDoWatcher;

{$I wclWeDo.inc}

interface

uses
  Classes, wclMessaging, System.Generics.Collections, Vcl.ExtCtrls,
  wclBluetooth;

type
  /// <summary> The <c>OnHubFound</c> and <c>OnHubLost</c> events handler
  ///   prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Address"> The WeDo Hub MAC address. </param>
  /// <param name="Name"> The WeDo Hub name. </param>
  TwclWeDoHubAppearanceEvent = procedure(Sender: TObject; Address: Int64;
    Name: string) of object;
  /// <summary> The <c>OnHunNameChanged</c> event handler prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Address"> The WeDo Hub MAC address. </param>
  /// <param name="OldName"> The old name of WeDo Hub. </param>
  /// <param name="NewName"> The new name of WeDo Hub. </param>
  TwclWeDoHubNameChangedEvent = procedure(Sender: TObject; Address: Int64;
    OldName: string; NewName: string) of object;

  /// <summary> The class used to search WeDo devices (Hubs). </summary>
  TwclWeDoWatcher = class(TComponent)
  private type
    // Structure used internally to monitor WeDo Hubs.
    TWeDoHub = record
      // Devices name.
      Name: string;
      // Last seen.
      Timestamp: TDateTime;
      // Indicates that value has been assigned.
      IsNull: Boolean;
    end;

    TWeDoLostTimerMessage = class(TwclMessage)
    public
      constructor Create;
    end;

  private const
    WEDO_LOST_TIMER_MESSAGE_ID = 1;

    // WeDo advertises this service UUID so we look for it to identify
    // WeDo device.
    WEDO_ADVERTISING_SERVICE: TGUID = '{00001523-1212-efde-1523-785feabcd123}';

  private
    FHubs: TDictionary<Int64, TWeDoHub>;
    FReceiver: TwclMessageReceiver;
    FTimer: TTimer;
    FWatcher: TwclBluetoothLeBeaconWatcher;

    FOnHubFound: TwclWeDoHubAppearanceEvent;
    FOnHubLost: TwclWeDoHubAppearanceEvent;
    FOnHubNameChanged: TwclWeDoHubNameChangedEvent;
    FOnStarted: TNotifyEvent;
    FOnStopped: TNotifyEvent;

    function GetActive: Boolean;
    function GetRadio: TwclBluetoothRadio;

    procedure WatcherAdvertisementUuidFrame(Sender: TObject;
      const Address: Int64; const Timestamp: Int64; const Rssi: ShortInt;
      const Uuid: TGUID);
    procedure WatcherAdvertisementFrameInformation(Sender: TObject;
      const Address: Int64; const Timestamp: Int64; const Rssi: ShortInt;
      const Name: string; const PacketType: TwclBluetoothLeAdvertisementType;
      const Flags: TwclBluetoothLeAdvertisementFlags);
    procedure WatcherStarted(Sender: TObject);
    procedure WatcherStopped(Sender: TObject);

    procedure TimerElapsed(Sender: TObject);

    procedure ReceiverMessage(const Message: TwclMessage);

  protected
    /// <summary> Fires the <c>OnHubFound</c> event. </summary>
    /// <param name="Address"> The Hub's MAC. </param>
    /// <param name="Name"> The WeDo Hub name. </param>
    procedure DoHubFound(Address: Int64; Name: string); virtual;
    /// <summary> Fires the <c>OnHubLost</c> event. </summary>
    /// <param name="Address"> The Hub's MAC. </param>
    /// <param name="Name"> The WeDo Hub name. </param>
    procedure DoHubLost(Address: Int64; Name: string); virtual;
    /// <summary> Fires the <c>OnNameChanged</c> event. </summary>
    /// <param name="Address"> The WeDo device's MAC address. </param>
    /// <param name="OldName"> The old name of WeDo Hub. </param>
    /// <param name="NewName"> The new name of WeDo Hub. </param>
    procedure DoHubNameChanged(Address: Int64; OldName: string;
      NewName: string); virtual;
    /// <summary> Fires the <c>OnStarte</c> event. </summary>
    procedure DoStarted; virtual;
    /// <summary> Fires the <c>OnStopped</c> event. </summary>
    procedure DoStopped; virtual;

  public
    /// <summary> Creates new WeDo Watcher object. </summary>
    /// <param name="Owner"> The component's owner. </param>
    constructor Create(AOwner: TComponent); override;

    /// <summary> Starts watching (discovering) for WeDo devices. </summary>
    /// <param name="Radio"> The <c>TwclBluetoothRadio</c> object that
    ///   should be used for executing Bluetooth LE discovering. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <c>WCL_E_SUCCESS</c>. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Start(Radio: TwclBluetoothRadio): Integer;
    /// <summary> Stops discovering WeDo devices. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <c>WCL_E_SUCCESS</c>. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Stop: Integer;

    /// <summary> Gets the WeDo Hub Watcher state. </summary>
    /// <value> Returns <c>true</c> if Watcher is running (searching for WeDo
    ///   Hubs). Returns <c>false</c> otherwise. </value>
    property Active: Boolean read GetActive;
    /// <summary> Gets the <c>TwclBluetoothRadio</c> object that is used
    ///   for searching WeDo hubs. </summary>
    /// <value> If Watcher is searching returns the
    ///   <c>TwclBluetoothRadio</c> object used for searching. If the
    ///   Watcher is not active returns <c>nil</c>. </value>
    property Radio: TwclBluetoothRadio read GetRadio;

  published
    /// <summary> The event fires when new WeDo Hub has been found. </summary>
    /// <seealso cref="TwclWeDoHubAppearanceEvent" />
    property OnHubFound: TwclWeDoHubAppearanceEvent read FOnHubFound
      write FOnHubFound;
    /// <summary> The event fires when new WeDo Hub has been lost. </summary>
    /// <seealso cref="TwclWeDoHubAppearanceEvent" />
    property OnHubLost: TwclWeDoHubAppearanceEvent read FOnHubLost
      write FOnHubLost;
    /// <summary> The event fires when name of a WeDo Hub has been
    ///   changed. </summary>
    /// <seealso cref="TwclWeDoHubNameChangedEvent"/>
    property OnHubNameChanged: TwclWeDoHubNameChangedEvent
      read FOnHubNameChanged write FOnHubNameChanged;
    /// <summary> The event fires when discovering has been started. </summary>
    property OnStarted: TNotifyEvent read FOnStarted write FOnStarted;
    /// <summary> The event fires when discovering has been stopped. </summary>
    property OnStopped: TNotifyEvent read FOnStopped write FOnStopped;
  end;

implementation

uses
  SysUtils, DateUtils, wclErrors;

{ TwclWeDoWatcher.TWeDoLostTimerMessage }

constructor TwclWeDoWatcher.TWeDoLostTimerMessage.Create;
begin
  inherited Create(WEDO_LOST_TIMER_MESSAGE_ID, WCL_MSG_CATEGORY_USER);
end;

{ TwclWeDoWatcher }

constructor TwclWeDoWatcher.Create(AOwner: TComponent);
begin
  inherited;

  // Create Disctionary that will be used to store found devices.
  FHubs := TDictionary<Int64, TWeDoHub>.Create;

  // Create beacon watcher to monitor WeDo Hubs.
  FWatcher := TwclBluetoothLeBeaconWatcher.Create(nil);
  FWatcher.OnAdvertisementUuidFrame := WatcherAdvertisementUuidFrame;
  FWatcher.OnAdvertisementFrameInformation := WatcherAdvertisementFrameInformation;
  FWatcher.OnStarted := WatcherStarted;
  FWatcher.OnStopped := WatcherStopped;

  // We also need timer that allows to checkif device is still available.
  FTimer := TTimer.Create(nil);
  // Check that devide is available during 3 seconds. If it did not update
  // information - it is disappeared.
  FTimer.Interval := 3000;
  FTimer.Enabled := False;
  FTimer.OnTimer := TimerElapsed;

  // We need message receiver to process messages from timer.
  FReceiver := TwclMessageReceiver.Create;
  FReceiver.OnMessage := ReceiverMessage;

  FOnHubFound := nil;
  FOnHubLost := nil;
  FOnHubNameChanged := nil;
  FOnStarted := nil;
  FOnStopped := nil;
end;

procedure TwclWeDoWatcher.DoHubFound(Address: Int64; Name: string);
begin
  if Assigned(FOnHubFound) then
    FOnHubFound(Self, Address, Name);
end;

procedure TwclWeDoWatcher.DoHubLost(Address: Int64; Name: string);
begin
  if Assigned(FOnHubLost) then
    FOnHubLost(Self, Address, Name);
end;

procedure TwclWeDoWatcher.DoHubNameChanged(Address: Int64;
  OldName: string; NewName: string);
begin
  if Assigned(FOnHubNameChanged) then
    FOnHubNameChanged(Self, Address, OldName, NewName);
end;

procedure TwclWeDoWatcher.DoStarted;
begin
  if Assigned(FOnStarted) then
    FOnStarted(Self);
end;

procedure TwclWeDoWatcher.DoStopped;
begin
  if Assigned(FOnStopped) then
    FOnStopped(Self);
end;

function TwclWeDoWatcher.GetActive: Boolean;
begin
  Result := FWatcher.Monitoring;
end;

function TwclWeDoWatcher.GetRadio: TwclBluetoothRadio;
begin
  if FWatcher.Monitoring then
    Result := FWatcher.Radio
  else
    Result := nil;
end;

procedure TwclWeDoWatcher.ReceiverMessage(const Message: TwclMessage);
var
  ToRemove: TList<Int64>;
  Hub: TPair<Int64, TWeDoHub>;
  Address: Int64;
  Name: string;
begin
  // If timer is still active.
  if FTimer.Enabled then begin
    // All we have to do here is check each device in list that it is
    // still available.
    ToRemove := TList<Int64>.Create;
    try
      for Hub in FHubs do begin
        if not Hub.Value.IsNull then begin
          if IncSecond(Hub.Value.Timestamp, 3) < Now then
            ToRemove.Add(Hub.Key);
        end;
      end;

      // Remove disappered devices.
      for Address in ToRemove do begin
        Name := FHubs[Address].Name;
        FHubs.Remove(Address);
        DoHubLost(Address, Name);
      end;

    finally
      ToRemove.Free;
    end;
  end;
end;

function TwclWeDoWatcher.Start(Radio: TwclBluetoothRadio): Integer;
begin
  // First, try to open message receiver.
  Result := FReceiver.Open();
  if Result = WCL_E_SUCCESS then begin
    // Now try to start watcher.
    Result := FWatcher.Start(Radio);
    // If failed we must close message receiver.
    if Result <> WCL_E_SUCCESS then
      FReceiver.Close();
  end;
end;

function TwclWeDoWatcher.Stop: Integer;
begin
  // Close message receiver.
  FReceiver.Close;
  // And stop watcher.
  Result := FWatcher.Stop;
end;

procedure TwclWeDoWatcher.TimerElapsed(Sender: TObject);
var
  Message: TWeDoLostTimerMessage;
begin
  Message := TWeDoLostTimerMessage.Create;
  try
    FReceiver.Post(Message);
  finally
    Message.Release;
  end;
end;

procedure TwclWeDoWatcher.WatcherAdvertisementFrameInformation(Sender: TObject;
  const Address: Int64; const Timestamp: Int64; const Rssi: ShortInt;
  const Name: string; const PacketType: TwclBluetoothLeAdvertisementType;
  const Flags: TwclBluetoothLeAdvertisementFlags);
var
  WeDo: TWeDoHub;
begin
  // If we get device's name in this advertisement.
  if Name <> '' then begin
    // Check that this is one of previously found WeDo HUB.
    if FHubs.ContainsKey(Address) then begin
      if FHubs[Address].IsNull then
        // If the device was just found, fire the OnFoudn event.
        DoHubFound(Address, Name)

      else begin
        // Have the name been changed?
        if FHubs[Address].Name <> Name then
          DoHubNameChanged(Address, FHubs[Address].Name, Name);
      end;

      // Now update its timestamp., Cause we use Distionary we have to update value with
      // new record!
      WeDo.Name := Name;
      WeDo.Timestamp := Now;
      WeDo.IsNull := False;
      FHubs.AddOrSetValue(Address, WeDo);
    end;
  end;
end;

procedure TwclWeDoWatcher.WatcherAdvertisementUuidFrame(Sender: TObject;
  const Address: Int64; const Timestamp: Int64; const Rssi: ShortInt;
  const Uuid: TGUID);
var
  Hub: TWeDoHub;
begin
  // WeDo advertises its service's UUID so we can check that this advertisement
  // from WeDo HUB.
  if Uuid = WEDO_ADVERTISING_SERVICE then begin
    // Make sure it was not found early.
    if not FHubs.ContainsKey(Address) then begin
      // If not - add new HUB to the list. But do not fire event yest because
      // we do not know device's name.
      Hub.Name := '';
      Hub.Timestamp := 0;
      Hub.IsNull := True;
      FHubs.Add(Address, Hub);
    end;
  end;
end;

procedure TwclWeDoWatcher.WatcherStarted(Sender: TObject);
begin
  // Fire the started event.
  DoStarted;
  // And start timer.
  FTimer.Enabled := True;
end;

procedure TwclWeDoWatcher.WatcherStopped(Sender: TObject);
begin
  // Stop timer.
  FTimer.Enabled := False;
  // Do not forget to clear the found devices list!
  FHubs.Clear;
  // And now fire the event.
  DoStopped();
end;

end.
