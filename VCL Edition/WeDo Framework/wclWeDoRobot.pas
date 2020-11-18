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

{$I wclWeDo.inc}

interface

uses
  System.Generics.Collections, Classes, wclWeDoHub, wclWeDoWatcher,
  wclBluetooth;

type
  /// <summary> The <c>OnHubConnected</c> event handler prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Hub"> The WeDo HUB object that has been connected. </param>
  /// <param name="Error"> The connection result. If the connection completed with
  ///   success the <c>Error</c> value is <c>WCL_E_SUCCESS</c>. </param>
  /// <seealso cref="TwclWeDoHub"/>
  TwclWeDoRobotHubConnectedEvent = procedure(Sender: TObject; Hub: TwclWeDoHub;
    Error: Integer) of object;
  /// <summary> The <c>OnHubDisconnected</c> event handler prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Hub"> The WeDo HUB object that has been disconnected. </param>
  /// <param name="Reason"> The disconnection reason code. </param>
  /// <seealso cref="TwclWeDoHub"/>
  TwclWeDoRobotHubDisconnectedEvent = procedure(Sender: TObject;
    Hub: TwclWeDoHub; Reason: Integer) of object;
  /// <summary> The <c>OnHubFound</c> event handler prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Address"> The HUD address. </param>
  /// <param name="Name"> The HUB name. </param>
  /// <param name="Connect"> An application sets this boolean value to
  ///   <c>true</c> to accept connection to this HUB. Set this parameter to
  ///   <c>false</c> to ignore HUB. </param>
  TwclWeDoRobotHubFoundEvent = procedure(Sender: TObject; Address: Int64;
    Name: String; out Connect: Boolean) of object;

  /// <summary> The class represents a WeDo Robot.  </summary>
  /// <remarks> The WeDo Robot is the class that combines all the WeDo
  ///   Framework features into a single place. It allows to work with more
  ///   then single Hub and hides all the Bluetooth Framework preparation
  ///   steps required to discover WeDo Hubs and to work with them.</remarks>
  TwclWeDoRobot = class(TComponent)
  private
    FHubs: TDictionary<Int64, TwclWeDoHub>;
    FManager: TwclBluetoothManager;
    FRadio: TwclBluetoothRadio;
    FWatcher: TwclWeDoWatcher;

    FOnHubConnected: TwclWeDoRobotHubConnectedEvent;
    FOnHubDisconnected: TwclWeDoRobotHubDisconnectedEvent;
    FOnHubFound: TwclWeDoRobotHubFoundEvent;
    FOnStarted: TNotifyEvent;
    FOnStopped: TNotifyEvent;

    function GetActive: Boolean;
    function GetHubs: TList<TwclWeDoHub>;
    function GetHub(Address: Int64): TwclWeDoHub;

    function GetDevice(Hub: TwclWeDoHub;
      IoType: TwclWeDoIoDeviceType): TwclWeDoIo;

    procedure HubConnected(Sender: TObject; Error: Integer);
    procedure HubDisconnected(Sender: TObject; Reason: Integer);

    procedure WatcherHubFound(Sender: TObject; Address: Int64; Name: String);
    procedure WatcherStarted(Sender: TObject);
    procedure WatcherStopped(Sender: TOBject);

  protected
    /// <summary> Fires the <c>OnHubConnected</c> event. </summary>
    /// <param name="Hub"> The WeDo HUB object that has been
    ///   connected. </param>
    /// <param name="Error"> The connection result code. If connection
    ///   completed with success the value is<c>WCL_E_SUCCESS</c>. </param>
    /// <seealso cref="TwclWeDoHub"/>
    procedure DoHubConnected(Hub: TwclWeDoHub; Error: Integer); virtual;
    /// <summary> Fires the <c>OnHubDisconnected</c> event. </summary>
    /// <param name="Hub"> The WeDo Hub that just disconnected. </param>
    /// <param name="Reason"> The disconnection reason code. </param>
    procedure DoHubDisconnected(Hub: TwclWeDoHub; Reason: Integer); virtual;
    /// <summary> Fires the <c>OnHubFound</c> event </summary>
    /// <param name="Address"> The HUD address. </param>
    /// <param name="Name"> The HUB name. </param>
    /// <param name="Connect"> An application sets this boolean value to
    ///   <c>true</c> to accept connection to this HUB. Set this parameter
    ///   to <c>false</c> to ignore HUB. </param>
    procedure DoHubFound(Address: Int64; Name: String;
      out Connect: Boolean); virtual;
    /// <summary> Fires the <c>OnStarted</c> event. </summary>
    procedure DoStarted; virtual;
    /// <summary> Fires the <c>OnStopped</c> event. </summary>
    procedure DoStopped; virtual;

  public
    /// <summary> Creates new instance of the WeDo Robot class. </summary>
    /// <param name="AOwner"> The component owner. </param>
    constructor Create(AOwner: TComponent); override;
    /// <summary> Frees the object. </summary>
    destructor Destroy; override;

    /// <summary> Starts connection to the WeDo Hubs. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <c>WCL_E_SUCCESS</c>. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <remarks> The method starts searching for WeDo Hubs and to connect to
    ///   each found. Once the Hub found the <c>OnHubFound</c> event fires. An
    ///   application may accept connection to this Hub by setting the
    ///   <c>Connect</c> parameter to <c>true</c>. </remarks>
    function Start: Integer;
    /// <summary> Stops the WeDo Robot. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <c>WCL_E_SUCCESS</c>. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Stop: Integer;

    /// <summary> Gets the voltage sensor object for the given Hub. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> The WeDo Voltage Sensor object or null if not
    ///   found. </returns>
    /// <seealso cref="TwclWeDoVoltageSensor"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetVoltageSensor(Hub: TwclWeDoHub): TwclWeDoVoltageSensor;
    /// <summary> Gets the current sensor object for the given Hub. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> The WeDo Current Sensor object or null if not
    ///   found. </returns>
    /// <seealso cref="TwclWeDoCurrentSensor"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetCurrentSensor(Hub: TwclWeDoHub): TwclWeDoCurrentSensor;
    /// <summary> Gets the piezo device object for the given Hub. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> The WeDo Piezo device object or null if not found. </returns>
    /// <seealso cref="TwclWeDoPiezo"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetPiezoDevice(Hub: TwclWeDoHub): TwclWeDoPiezo;
    /// <summary> Gets the RGB device object for the given Hub. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> The WeDo RGB device object or null if not found. </returns>
    /// <seealso cref="TwclWeDoRgbLight"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetRgbDevice(Hub: TwclWeDoHub): TwclWeDoRgbLight;
    /// <summary> Gets the Tilt sensor object for the given Hub. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> The WeDo Tilt sensor object or null if not found. </returns>
    /// <seealso cref="TwclWeDoTiltSensor"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetTiltSensors(Hub: TwclWeDoHub): TList<TwclWeDoTiltSensor>;
    /// <summary> Gets the Motion sensors list for the given Hub. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> The WeDo Tilt sensor object or null if not found. </returns>
    /// <seealso cref="TwclWeDoMotionSensor"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetMotionSensors(Hub: TwclWeDoHub): TList<TwclWeDoMotionSensor>;
    /// <summary> Gets the list of connected Motor devices. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <returns> Te list of connected motors. </returns>
    /// <seealso cref="TwclWeDoMotor"/>
    /// <seealso cref="TwclWeDoHub"/>
    function GetMotors(Hub: TwclWeDoHub): TList<TwclWeDoMotor>;
    /// <summary> Gets the IO device connected to the specified port. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <param name="Port"> The port ID. </param>
    /// <returns> The IO device or null. </returns>
    /// <seealso cref="TwclWeDoHub"/>
    /// <seealso cref="TwclWeDoIo"/>
    function GetIoDevice(Hub: TwclWeDoHub; Port: Byte): TwclWeDoIo;
    /// <summary> Gets the IO device by its type. </summary>
    /// <param name="Hub"> The WeDo Hub object. </param>
    /// <param name="IoType"> The device type. </param>
    /// <returns> The IO device or null. </returns>
    /// <seealso cref="TwclWeDoHub"/>
    /// <seealso cref="TwclWeDoIo"/>
    /// <seealso cref="TwclWeDoIoDeviceType"/>
    function GetIoDevices(Hub: TwclWeDoHub;
      IoType: TwclWeDoIoDeviceType): TList<TwclWeDoIo>;

    /// <summary> Gets the class state. </summary>
    /// <value> <c>True</c> if connection is running. <c>False</c>
    ///   otherwise. </value>
    property Active: Boolean read GetActive;
    /// <summary> Gets list of the connected WeDo Hubs. </summary>
    /// <value> The list of the WeDo Hubs. </value>
    /// <seealso cref="TwclWeDoHub"/>
    property Hubs: TList<TwclWeDoHub> read GetHubs;
    /// <summary> Gets the WeDo Hub object by its MAC address. </summary>
    /// <value> The WeDo Hubs object. </value>
    /// <seealso cref="TwclWeDoHub"/>
    property Hub[Address: Int64]: TwclWeDoHub read GetHub; default;

  published
    /// <summary> The event fires when connection operation has been
    ///   completed. </summary>
    /// <seealso cref="TwclWeDoRobotHubConnectedEvent"/>
    property OnHubConnected: TwclWeDoRobotHubConnectedEvent read FOnHubConnected
      write FOnHubConnected;
    /// <summary> The event fires when the WeDo Hub has been
    ///   disconnected. </summary>
    /// <seealso cref="TwclWeDoRobotHubDisconnectedEvent"/>
    property OnHubDisconnected: TwclWeDoRobotHubDisconnectedEvent
      read FOnHubDisconnected write FOnHubDisconnected;
    /// <summary> The event fires when new WeDo HUB found. </summary>
    /// <seealso cref="TwclWeDoRobotHubFoundEvent"/>
    property OnHubFound: TwclWeDoRobotHubFoundEvent read FOnHubFound
      write FOnHubFound;
    /// <summary> The event fires when search and connect to found WeDo Hubs
    ///   has been started. </summary>
    property OnStarted: TNotifyEvent read FOnStarted write FOnStarted;
    /// <summary> The event firs when search and connect to the WeDo Hubs
    ///   stopped. </summary>
    property OnStopped: TNotifyEvent read FOnStopped write FOnStopped;
  end;

implementation

uses
  wclErrors, wclConnectionErrors, wclBluetoothErrors;

{ TwclWeDoRobot }

constructor TwclWeDoRobot.Create(AOwner: TComponent);
begin
  inherited;

  FHubs := TDictionary<Int64, TwclWeDoHub>.Create();
  FManager := TwclBluetoothManager.Create(nil);
  FRadio := nil;
  FWatcher := TwclWeDoWatcher.Create(nil);

  FWatcher.OnHubFound := WatcherHubFound;
  FWatcher.OnStarted := WatcherStarted;
  FWatcher.OnStopped := WatcherStopped;

  FOnHubConnected := nil;
  FOnHubDisconnected := nil;
  FOnHubFound := nil;
  FOnStarted := nil;
  FOnStopped := nil;
end;

destructor TwclWeDoRobot.Destroy;
begin
  Stop;

  inherited;
end;

procedure TwclWeDoRobot.DoHubConnected(Hub: TwclWeDoHub; Error: Integer);
begin
  if Assigned(FOnHubConnected) then
    FOnHubConnected(Self, Hub, Error);
end;

procedure TwclWeDoRobot.DoHubDisconnected(Hub: TwclWeDoHub; Reason: Integer);
begin
  if Assigned(FOnHubDisconnected) then
    FOnHubDisconnected(Self, Hub, Reason);
end;

procedure TwclWeDoRobot.DoHubFound(Address: Int64; Name: String;
  out Connect: Boolean);
begin
  Connect := False;
  if Assigned(FOnHubFound) then
    FOnHubFound(Self, Address, Name, Connect);
end;

procedure TwclWeDoRobot.DoStarted;
begin
  if Assigned(FOnStarted) then
    FOnStarted(Self);
end;

procedure TwclWeDoRobot.DoStopped;
begin
  if Assigned(FOnStopped) then
    FOnStopped(Self);
end;

function TwclWeDoRobot.GetActive: Boolean;
begin
  Result := (FRadio <> nil);
end;

function TwclWeDoRobot.GetCurrentSensor(
  Hub: TwclWeDoHub): TwclWeDoCurrentSensor;
begin
  Result := TwclWeDoCurrentSensor(GetDevice(Hub, iodCurrentSensor))
end;

function TwclWeDoRobot.GetDevice(Hub: TwclWeDoHub;
  IoType: TwclWeDoIoDeviceType): TwclWeDoIo;
var
  Io: TwclWeDoIo;
begin
  Result := nil;
  if Hub <> nil then begin
    for Io in Hub.IoDevices do begin
      if Io.DeviceType = IoType then begin
        Result := Io;
        Break;
      end;
    end;
  end;
end;

function TwclWeDoRobot.GetHub(Address: Int64): TwclWeDoHub;
begin
  Result := FHubs[Address];
end;

function TwclWeDoRobot.GetHubs: TList<TwclWeDoHub>;
var
  Hub: TwclWeDoHub;
begin
  Result := TList<TwclWeDoHub>.Create;
  for Hub in FHubs.Values do
    Result.Add(Hub);
end;

function TwclWeDoRobot.GetIoDevice(Hub: TwclWeDoHub; Port: Byte): TwclWeDoIo;
var
  Io: TwclWeDoIo;
begin
  Result := nil;
  if Hub <> nil then begin
    for Io in Hub.IoDevices do begin
      if Io.PortId = Port then begin
        Result := Io;
        BReak;
      end;
    end;
  end;
end;

function TwclWeDoRobot.GetIoDevices(Hub: TwclWeDoHub;
  IoType: TwclWeDoIoDeviceType): TList<TwclWeDoIo>;
var
  Io: TwclWeDoIo;
begin
  if Hub = nil then
    Result := nil

  else begin
    Result := TList<TwclWeDoIo>.Create;
    for Io in Hub.IoDevices do begin
      if Io.DeviceType = IoType then
        Result.Add(Io);
    end;
  end;
end;

function TwclWeDoRobot.GetMotionSensors(
  Hub: TwclWeDoHub): TList<TwclWeDoMotionSensor>;
var
  Io: TwclWeDoIo;
begin
  Result := TList<TwclWeDoMotionSensor>.Create;
  if Hub <> nil then begin
    for Io in Hub.IoDevices do begin
      if Io.DeviceType = iodMotionSensor then
        Result.Add(TwclWeDoMotionSensor(Io));
    end;
  end;
end;

function TwclWeDoRobot.GetMotors(Hub: TwclWeDoHub): TList<TwclWeDoMotor>;
var
  Io: TwclWeDoIo;
begin
  Result := TList<TwclWeDoMotor>.Create;
  if Hub <> nil then begin
    for Io in Hub.IoDevices do begin
      if Io.DeviceType = iodMotor then
        Result.Add(TwclWeDoMotor(Io));
    end;
  end;
end;

function TwclWeDoRobot.GetPiezoDevice(Hub: TwclWeDoHub): TwclWeDoPiezo;
begin
  Result := TwclWeDoPiezo(GetDevice(Hub, iodPiezo));
end;

function TwclWeDoRobot.GetRgbDevice(Hub: TwclWeDoHub): TwclWeDoRgbLight;
begin
  Result := TwclWeDoRgbLight(GetDevice(Hub, iodRgb));
end;

function TwclWeDoRobot.GetTiltSensors(
  Hub: TwclWeDoHub): TList<TwclWeDoTiltSensor>;
var
  Io: TwclWeDoIo;
begin
  Result := TList<TwclWeDoTiltSensor>.Create;
  if Hub <> nil then begin
    for Io in Hub.IoDevices do begin
      if Io.DeviceType = iodTiltSensor then
        Result.Add(TwclWeDoTiltSensor(Io));
    end;
  end;
end;

function TwclWeDoRobot.GetVoltageSensor(
  Hub: TwclWeDoHub): TwclWeDoVoltageSensor;
begin
  Result := TwclWeDoVoltageSensor(GetDevice(Hub, iodVoltageSensor));
end;

procedure TwclWeDoRobot.HubConnected(Sender: TObject; Error: Integer);
var
  Hub: TwclWeDoHub;
begin
  Hub := TwclWeDoHub(Sender);
  DoHubConnected(Hub, Error);
  if Error <> WCL_E_SUCCESS then
    FHubs.Remove(Hub.Address);
end;

procedure TwclWeDoRobot.HubDisconnected(Sender: TObject; Reason: Integer);
var
  Hub: TwclWeDoHub;
begin
  Hub := TwclWeDoHub(Sender);
  if FHubs.ContainsKey(Hub.Address) then begin
    FHubs.Remove(Hub.Address);
    DoHubDisconnected(Hub, Reason);
  end;
end;

function TwclWeDoRobot.Start: Integer;
var
  i: Integer;
begin
  if FRadio <> nil then
    Result := WCL_E_CONNECTION_ACTIVE

  else begin
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

      // If something went wrong we must close Bluetooth Manager
      if Result <> WCL_E_SUCCESS then
        FManager.Close;
    end;
  end;
end;

function TwclWeDoRobot.Stop: Integer;
var
  Hub: TwclWeDoHub;
begin
  if FRadio = nil then
    Result := WCL_E_CONNECTION_NOT_ACTIVE

  else begin
    FWatcher.Stop;
    for Hub in FHubs.Values do
      Hub.Disconnect;
    FManager.Close;
    FRadio := nil;

    Result := WCL_E_SUCCESS;
  end;
end;

procedure TwclWeDoRobot.WatcherHubFound(Sender: TObject; Address: Int64;
  Name: String);
var
  Connect: Boolean;
  Hub: TwclWeDoHub;
begin
  // First, check that we are not connected to the HUB.
  if not FHubs.ContainsKey(Address) then begin
    // Query application about interest of this HUB.
    Connect := False;
    DoHubFound(Address, Name, Connect);
    if Connect then begin
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

procedure TwclWeDoRobot.WatcherStarted(Sender: TObject);
begin
  DoStarted;
end;

procedure TwclWeDoRobot.WatcherStopped(Sender: TOBject);
begin
  DoStopped;
end;

end.
