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

unit wclWeDoRobot;

{$I ..\..\..\..\..\WCL7\VCL\Source\wcl.inc}

interface

uses
  System.Generics.Collections, Classes, wclWeDoHub, wclWeDoWatcher,
  wclBluetooth;

type
  /// <summary> The <c>OnHubConnected</c> event handler prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Hub"> The WeDo HUB object that has been connected. </param>
  /// <seealso cref="TwclWeDoHub"/>
  TwclWeDoRobotHubConnectedEvent = procedure(Sender: TObject;
    Hub: TwclWeDoHub) of object;
  /// <summary> The <c>OnHubDisconnected</c> event handler prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Hub"> The WeDo HUB object that has been disconnected. </param>
  /// <param name="Reason"> The disconnection reason code. </param>
  /// <seealso cref="TwclWeDoHub"/>
  TwclWeDoRobotHubDisconnectedEvent = procedure(Sender: TObject;
    Hub: TwclWeDoHub; Reason: Integer) of object;

  /// <summary> The class represents a WeDo Robot.  </summary>
  /// <remarks> The WeDo Robot is the class that combines all the WeDo Framework
  ///   features into a single place. It allows to work with more thean single
  ///   Hub and hides all the Bluetooth Framework prepartion steps required
  ///   todiscover WeDo Hubs and to work with them.</remarks>
  TwclWeDoRobot = class(TComponent)
  private
    FAddresses: TList<Int64>;
    FHubs: TDictionary<Int64, TwclWeDoHub>;
    FManager: TwclBluetoothManager;
    FRadio: TwclBluetoothRadio;
    FWatcher: TwclWeDoWatcher;

    FOnHubConnected: TwclWeDoRobotHubConnectedEvent;
    FOnHubDisconnected: TwclWeDoRobotHubDisconnectedEvent;

    function GetConnected: Boolean;

    procedure WatcherHubFound(Sender: TObject; Address: Int64; Name: string);

    procedure HubDisconnected(Sender: TObject; Reason: Integer);
    procedure HubConnected(Sender: TObject; Error: Integer);

  public
    /// <summary> Creates new instance of the WeDo Robot class. </summary>
    /// <param name="Owner"> The component's owner. </param>
    constructor Create(AOwner: TComponent); override;

    /// <summary> Connects to specified HUBs. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    //    is one of the Bluetooth Framework error code. </returns>
    function Connect: Integer;
    /// <summary> Disconnects from  all connected HUBs. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Disconnect: Integer;

    /// <summary> Gets the list of required addresses. </summary>
    /// <value> The list of HUBs MACs. </value>
    property Addresses: TList<Int64> read FAddresses;
    /// <summary> Gets the class state. </summary>
    /// <value> <c>True</c> if connection is running. <c>False</c>
    ///   otherwise. </value>
    property Connected: Boolean read GetConnected;
    /// <summary> Gets the list of connected HUBs. </summary>
    /// <value> The list of connected HUBs. </value>
    property Hubs: TDictionary<Int64, TwclWeDoHub> read FHubs;

    /// <summary> The event fires when new found WeDo HUB has just been
    ///   connected. </summary>
    /// <seealso cref="TwclWeDoRobotHubConnectedEvent"/>
    property OnHubConnected: TwclWeDoRobotHubConnectedEvent
      read FOnHubConnected write FOnHubConnected;
    /// <summary> The event fires when HUB has been disconnected. </summary>
    /// <seealso cref="TwclWeDoRobotHubDisconnectedEvent"/>
    property OnHubDisconnected: TwclWeDoRobotHubDisconnectedEvent
      read FOnHubDisconnected write FOnHubDisconnected;
  end;

implementation

uses
  wclErrors, wclConnectionErrors, wclBluetoothErrors;

{ TwclWeDoRobot }

function TwclWeDoRobot.Connect: Integer;
var
  i: Integer;
begin
  if FRadio <> nil then
    Result := WCL_E_CONNECTION_ACTIVE

  else begin
    // Now try to open Bluetooth Manager.
    Result := FManager.Open;
    if Result = WCL_E_SUCCESS then begin
      if FManager.Count = 0 then
        Result := WCL_E_BLUETOOTH_API_NOT_FOUND

      else begin
        // Look for first available radio.
        for i := 0 to FManager.Count - 1 do begin
          if FManager[i].Available then begin
            FRadio := FManager[i];
            Break;
          end;
        end;

        if FRadio = nil then
          Result := WCL_E_BLUETOOTH_RADIO_UNAVAILABLE

        else begin
          // Try to start watching for HUBs.
          Result := FWatcher.Start(FRadio);

          // If something went wrong we must clear the working radio objecy.
          if Result <> WCL_E_SUCCESS then
            FRadio := nil;
        end;
      end;
    end;

    // If something went wrong we must close Bluetooth Manager
    if Result <> WCL_E_SUCCESS then
      FManager.Close;
  end;
end;

constructor TwclWeDoRobot.Create(AOwner: TComponent);
begin
  inherited;

  FAddresses := TList<Int64>.Create;
  FHubs := TDictionary<Int64, TwclWeDoHub>.Create;
  FManager := TwclBluetoothManager.Create(nil);
  FRadio := nil;
  FWatcher := TwclWeDoWatcher.Create(nil);

  // WeDoWatcher events
  FWatcher.OnHubFound := WatcherHubFound;

  FOnHubConnected := nil;
  FOnHubDisconnected := nil;
end;

function TwclWeDoRobot.Disconnect: Integer;
begin
  if FRadio = nil then
    Result := WCL_E_CONNECTION_NOT_ACTIVE

  else begin
    // First, stop watcher to prevent from connecting other devices.
    FWatcher.Stop;
    // Disconnect all HUBs.
    while FHubs.Count > 0 do
      FHubs[0].Disconnect;
    // Close Bluetooth Manager.
    FManager.Close;
    // And cleanup radio.
    FRadio := nil;

    Result := WCL_E_SUCCESS;
  end;
end;

function TwclWeDoRobot.GetConnected: Boolean;
begin
  Result := FRadio <> nil;
end;

procedure TwclWeDoRobot.HubConnected(Sender: TObject; Error: Integer);
begin
  // If connection was success we can add HUB into the list and fire the event.
  if Error = WCL_E_SUCCESS then begin
    FHubs.Add(TwclWeDoHub(Sender).Address, TwclWeDoHub(Sender));
    if Assigned(FOnHubConnected) then
      FOnHubConnected(Self, TwclWeDoHub(Sender));
  end;
end;

procedure TwclWeDoRobot.HubDisconnected(Sender: TObject; Reason: Integer);
var
  Address: Int64;
begin
  // Simple remove HUb from the list and fire the event.
  Address := TwclWeDoHub(Sender).Address;
  if FHubs.ContainsKey(Address) then begin
    FHubs.Remove(Address);
    if Assigned(FOnHubDisconnected) then
      FOnHubDisconnected(Self, TwclWeDoHub(Sender), Reason);
  end;
end;

procedure TwclWeDoRobot.WatcherHubFound(Sender: TObject; Address: Int64;
  Name: string);
var
  Hub: TwclWeDoHub;
begin
  // First, check that we are not connected to the HUB.
  if not FHubs.ContainsKey(Address) then begin
    // Now make sure we are interested in this HUB.
    if (FAddresses.Count = 0) or (FAddresses.Contains(Address)) then begin
      // Prepare HUB object.
      Hub := TwclWeDoHub.Create(nil);
      // Setup HUB events.
      Hub.OnConnected := HubConnected;
      Hub.OnDisconnected := HubDisconnected;

      // Try to connect to the given HUB.
      if Hub.Connect(FRadio, Address) = WCL_E_SUCCESS then
        // If operation started with success add HUB to the list to prevent
        // From adding it one more time.
        FHubs.Add(Address, Hub);
    end;
  end;
end;

end.
