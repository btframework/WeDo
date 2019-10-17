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

unit wclWeDoHub;

interface

uses
  wclBluetooth, System.Generics.Collections, Classes, wclConnections,
  Vcl.Graphics;

type
  /// <summary> Represents a type of an attached IO (motor, sensor,
  ///   etc). </summary>
  TwclWeDoIoDeviceType = (
    /// <summary> A Motor. </summary>
    iodMotor,
    /// <summary> A Voltage Sensor. </summary>
    iodVoltageSensor,
    /// <summary> A Current Sensor. </summary>
    iodCurrentSensor,
    /// <summary> A Piezo Tone player. </summary>
    iodPiezo,
    /// <summary> An RGB light. </summary>
    iodRgb,
    /// <summary> A Tilt Sensor. </summary>
    iodTiltSensor,
    /// <summary> A Motion Sensor (aka. Detect Sensor). </summary>
    iodMotionSensor,
    /// <summary> A type is unknown. </summary>
    iodUnknown
  );

  /// <summary> The sensor's data unit. </summary>
	TwclWeDoSensorDataUnit = (
    /// <summary> Raw. </summary>
    suRaw,
    /// <summary> Percentage. </summary>
    suPercentage,
    /// <summary> SI. </summary>
    suSi,
    /// <summary> Unknown. </summary>
    suUnknown
  );

  /// <summary> The structure describes the device version number. </summary>
  TwclWeDoVersion = record
  public
    /// <summary> The bug fix version number. </summary>
    BugFixVersion: Byte;
    /// <summary> The build number. </summary>
    BuildNumber: Byte;
    /// <summary> The build number. </summary>
    MajorVersion: Byte;
    /// <summary> The major version number. </summary>
    MinorVersion: Byte;
    /// <summary> A formatted string representation of the version. </summary>
    /// <returns> A formatted string representation of the version </returns>
    function ToString: string;
  end;

  /// <summary> This class contains info detailing how the data received for a
  ///   given service (typically a sensor of some kind) should be
  ///   interpreted. </summary>
  TwclWeDoDataFormat = class sealed
  private
    FDataSetCount: Byte;
    FDataSetSize: Byte;
    FMode: Byte;
    FUnit: TwclWeDoSensorDataUnit;

  public
    /// <summary> Creates a new instance of the Data Format class. </summary>
    /// <param name="DataSetCount"> The number of data sets. </param>
    /// <param name="DataSetSize"> The number of bytes in a data set. </param>
    /// <param name="Mode"> The sensor mode. </param>
    /// <param name="Unit_"> The sensor data unit. </param>
    /// <seealso cref="TwclWeDoSensorDataUnit"/>
    constructor Create(const DataSetCount: Byte; const DataSetSize: Byte;
      const Mode: Byte; const Unit_: TwclWeDoSensorDataUnit);

    /// <summary> Compares two Data Formats </summary>
		/// <param name="Obj"> The other object to be compared with
    ///   current. </param>
		/// <returns> <c>True</c> if this data format is equal to <c>obj</c>.
    ///   <c>False</c> otherwise. </returns>
    function Equals(Obj: TObject): Boolean; override;

    /// <summary> Gets the data set count. </summary>
    /// <value> The data set count. </value>
    property DataSetCount: Byte read FDataSetCount;
    /// <summary> Gets the data set size. </summary>
    /// <value> The data set size. </value>
    property DataSetSize: Byte read FDataSetSize;
    /// <summary> Gets the sensor mode. </summary>
    /// <value> The sensor mode. </value>
    property Mode: Byte read FMode;
    /// <summary> Gets the sensor data unit. </summary>
    /// <value> The sensor data unit. </value>
    /// <seealso cref="TwclWeDoSensorDataUnit"/>
    property Unit_: TwclWeDoSensorDataUnit read FUnit;
  end;

  /// <summary> This class describes a configuration of an Input (sensor). At
  ///   any time a sensor can be in just one mode, and the details of this mode
  ///   is captured by this structure. </summary>
  TwclWeDoInputFormat = class sealed
  private const
    INPUT_FORMAT_PACKET_SIZE = 11;

    WEDO_DEVICE_MOTOR = 1;
    WEDO_DEVICE_VOLTAGE_SENSOR = 20;
    WEDO_DEVICE_CURRENT_SENSOR = 21;
    WEDO_DEVICE_PIEZO = 22;
    WEDO_DEVICE_RGB = 23;
    WEDO_DEVICE_TILT_SENSOR = 34;
    WEDO_DEVICE_MOTION_SENSOR = 35;
    WEDO_DEVICE_UNKNOWN = 255;

    WEDO_DATA_UNIT_RAW = 0;
    WEDO_DATA_UNIT_PERCENTAGE = 1;
    WEDO_DATA_UNIT_SI = 2;
    WEDO_DATA_UNIT_UNKNOWN = 255;

  private
    FConnectionId: Byte;
    FDeviceType: TwclWeDoIoDeviceType;
    FInterval: Cardinal;
    FMode: Byte;
    FNotificationsEnabled: Boolean;
    FNumberOfBytes: Byte;
    FRevision: Byte;
    FUnit: TwclWeDoSensorDataUnit;

    class function FromBytesArray(
      const Data: TArray<Byte>): TwclWeDoInputFormat;

    function ToBytesArray: TArray<Byte>;
    function InputFormatBySettingMode(const Mode: Byte): TwclWeDoInputFormat;

  public
    /// <summary> Create a new instance of <c>TwclWeDoInputFormat</c>
    ///   class. </summary>
    /// <param name="ConnectionId"> The connection ID of the service.</param>
    /// <param name="DeviceType"> The type of the device. </param>
    /// <param name="Mode"> The mode of the device. </param>
    /// <param name="Interval"> The notifications interval. </param>
    /// <param name="Unit_"> The unit the sensor should return values
    ///   in. </param>
    /// <param name="NotificationsEnabled"> <c>True</c> if the device should
    ///   send updates when the value changes. </param>
    /// <param name="Revision"> The Input Format revision. </param>
    /// <param name="NumberOfBytes"> The number of bytes in device's data
    ///   packet. </param>
    /// <seealso cref="TwclWeDoIoDeviceType"/>
    /// <seealso cref="TwclWeDoSensorDataUnit"/>
    constructor Create(const ConnectionId: Byte;
      const DeviceType: TwclWeDoIoDeviceType; const Mode: Byte;
      const Interval: Cardinal; const Unit_: TwclWeDoSensorDataUnit;
      const NotificationsEnabled: Boolean; const Revision: Byte;
      const NumberOfBytes: Byte);

    /// <summary> Compares two Input Formats. </summary>
    /// <param name="Obj"> The other object to be compared with
    ///   current. </param>
    /// <returns> <c>True</c> if this input format is equal to <c>Obj</c>.
    ///   <c>False</c> otherwise. </returns>
    function Equals(Obj: TObject): Boolean; override;

    /// <summary> The Connect ID of the corresponding device. </summary>
    /// <value> The connect ID. </value>
    property ConnectionId: Byte read FConnectionId;
    /// <summary> Gets the device type of the Input Format. </summary>
    /// <value> The device type of the corresponding service. </value>
    /// <seealso cref="TwclWeDoIoDeviceType"/>
    property DeviceType: TwclWeDoIoDeviceType read FDeviceType;
    /// <summary> Gets The notifications interval. </summary>
    /// <value> The notifications interval. </value>
    /// <remarks> When notifications are enabled the device sends notifications
    ///   if the value has change. The interval indicates how fast/often updates
    ///   will be send </remarks>
    property Interval: Cardinal read FInterval;
    /// <summary> Gets the Input mode. </summary>
    /// <value> The mode of the Input. </value>
    property Mode: Byte read FMode;
    /// <summary> Gets the notifications state. </summary>
    /// <value> <c>True</c> if new values are send whenever the value of the
    ///   Input changes beyond delta interval. </value>
    property NotificationsEnabled: Boolean read FNotificationsEnabled;
    /// <summary> Gets the number of bytes to be expected in the Input data
    ///   payload (set by the Device). </summary>
    /// <value> The number of bytes. </value>
    property NumberOfBytes: Byte read FNumberOfBytes;
    /// <summary> Gets the Input Format revision. </summary>
    /// <value> The revision of the Input Format. </value>
    property Revision: Byte read FRevision;
    /// <summary> Gets the value unit. </summary>
    /// <value> The unit of the values. </value>
    /// <seealso cref="TwclWeDoSensorDataUnit"/>
    property Unit_: TwclWeDoSensorDataUnit read FUnit;
  end;

  // Forward declaration
  TwclWeDoHub = class;

  /// <summary> The base class for all WeDo services. </summary>
  TwclWeDoService = class abstract
  private
    FCharacteristics: TwclGattCharacteristics;
    FClient: TwclGattClient;
    FConnected: Boolean;
    FHub: TwclWeDoHub;
    FServices: TwclGattServices;

    function ReadCharacteristics(const Service: TwclGattService): Integer;

    function Connect(const Services: TwclGattServices): Integer;
    function Disconnect: Integer;

    /// <summary> This method called internally by the <see cref="TwclWeDoHub"/>
    ///   to notify about characteristic changes. A derived class may override
    ///   this method to check for required characteristic changes. </summary>
    /// <param name="Handle"> The characteristic handle. </param>
    /// <param name="Value"> The new characteristic value. </param>
    procedure CharacteristicChanged(const Handle: Word;
      const Value: TArray<Byte>); virtual;

    property Connected: Boolean read FConnected;

  protected
    /// <summary> Converts <see cref="TwclGattUuid"/> type to standard system
    ///   GUID. </summary>
    /// <param name="Uuid"> The <see cref="TwclGattUuid"/> that should be
    ///   converted. </param>
    /// <returns> The GUID composed from the <c>Uuid</c>. </returns>
    /// <seealso cref="TwclGattUuid"/>
    function ToGuid(const Uuid: TwclGattUuid): TGUID;
    /// <summary> Compares the attribute's <see cref="TwclGattUuid"/> with
    ///   given standard system GUID. </summary>
    /// <param name="GattUuid"> The attribute's
    ///   <see cref="TwclGattUuid"/>. </param>
    /// <param name="Uuid">The system GUID. </param>
    /// <returns> Returns <c>true</c> if the attribute's UUID is equals to the
    ///   GUID. Returns <c>false</c> otherwise. </returns>
    /// <seealso cref="TwclGattUuid"/>
    function CompareGuid(const GattUuid: TwclGattUuid;
      const Uuid: TGUID): Boolean;

    /// <summary> Finds the service with given UUID. </summary>
    /// <param name="Uuid"> The service's UUID. </param>
    /// <param name="Service"> If the method completed with success the
    ///   parameter contains found service. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclGattService"/>
    function FindService(const Uuid: TGUID;
      out Service: TwclGattService): Integer;
    /// <summary> Finds the characteristic with given UUID. </summary>
    /// <param name="Uuid"> The characteristic's UUID. </param>
    /// <param name="Service"> The GATT service that should contain the required
    ///   characteristic. </param>
    /// <param name="Characteristic"> If the method completed with success the
    ///   parameter contains the found characteristic. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclGattService"/>
    /// <seealso cref="TwclGattCharacteristic"/>
    function FindCharactersitc(const Uuid: TGUID;
      const Service: TwclGattService;
      out Characteristic: TwclGattCharacteristic): Integer;

    /// <summary> Subscribes to the changes notifications of the given
    ///   characteristic. </summary>
    /// <param name="Characteristic"> The characteristic. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclGattCharacteristic"/>
    function SubscribeForNotifications(
      Characteristic: TwclGattCharacteristic): Integer;
    /// <summary> Unsubscribes from the changes notifications of the given
    ///   characteristic. </summary>
    /// <param name="Characteristic"> The characteristic to unsubsribe. </param>
    /// <seealso cref="TwclGattCharacteristic"/>
    procedure UnsubscribeFromNotifications(
      Characteristic: TwclGattCharacteristic);

    /// <summary> Reads string value from the given characteristic. </summary>
    /// <param name="Characteristic"> The GATT characteristic. </param>
    /// <param name="Value"> If the method completed with success contains the
    ///   read value. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadStringValue(const Characteristic: TwclGattCharacteristic;
      out Value: string): Integer;
    /// <summary> Reads byte value from the given characteristic. </summary>
    /// <param name="Characteristic"> The GATT characteristic. </param>
    /// <param name="Value"> If the method completed with success contains the
    ///   read value. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadByteValue(const Characteristic: TwclGattCharacteristic;
      out Value: Byte): Integer;

    /// <summary> Initializes the WeDo service. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
     /// <remarks> A derived clases must override this method to initialize all required
     ///   parameters to work with WeDo service. </remarks>
     function Initialize: Integer; virtual; abstract;
     /// <summary> Uninitializes the WeDo service. </summary>
     /// <remarks> A derived clases must override this method to cleanup
     ///   allocated resources. </remarks>
     procedure Uninitialize; virtual; abstract;

     /// <summary> Gets the GATT client object. </summary>
     /// <value> The GATT client object. </value>
     /// <seealso cref="TwclGattClient"/>
     property Client: TwclGattClient read FClient;

  public
    /// <summary> Creates new WeDo Service Client object. </summary>
    /// <param name="Client"> The <see cref="TwclGattClient"/> object that
    ///   handles the connection to a WeDo device. </param>
    /// <param name="Hub"> The <see cref="TwclWeDoHub"/> object that owns the
    ///   service. </param>
    /// <exception cref="wclEInvalidArgument"> The exception raises if the
    ///   <c>Client</c> or <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Client: TwclGattClient;
      const Hub: TwclWeDoHub); virtual;

    /// <summary> Gets the <see cref="TwclWeDoHub"/> object that owns the
    ///   service. </summary>
    /// <value> The <see cref="TwclWeDoHub"/> object. </value>
    /// <seealso cref="TwclWeDoHub"/>
    property Hub: TwclWeDoHub read FHub;
  end;

  /// <summary> The class represents the WeDo Device Information
  ///   service. </summary>
  /// <seealso cref="TwclWeDoService"/>
  TwclWeDoDeviceInformationService = class(TwclWeDoService)
  private const
    // Standard Bluetooth LE Device Information Service.
    WEDO_SERVICE_DEVICE_INFORMATION: TGUID = '{0000180a-0000-1000-8000-00805f9b34fb}';

    // Firmware Revision characteristic. [Optional] [Readable]
    WEDO_CHARACTERISTIC_FIRMWARE_REVISION: TGUID = '{00002a26-0000-1000-8000-00805f9b34fb}';
    // Firmware Revision characteristic. [Optional] [Readable]
    WEDO_CHARACTERISTIC_HARDWARE_REVISION: TGUID = '{00002a27-0000-1000-8000-00805f9b34fb}';
    // Software Revision characteristic. [Optional] [Readable]
    WEDO_CHARACTERISTIC_SOFTWARE_REVISION: TGUID = '{00002a28-0000-1000-8000-00805f9b34fb}';
    // Manufacturer Name characteristic. [Optional] [Readable]
    WEDO_CHARACTERISTIC_MANUFACTURER_NAME: TGUID = '{00002a29-0000-1000-8000-00805f9b34fb}';

  private
    FFirmwareVersionChar: TwclGattCharacteristic;
    FHardwareVersionChar: TwclGattCharacteristic;
    FSoftwareVersionChar: TwclGattCharacteristic;
    FManufacturerNameChar: TwclGattCharacteristic;

  protected
    /// <summary> Initializes the WeDo service. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Initialize: Integer; override;
    /// <summary> Uninitializes the WeDo service. </summary>
    procedure Uninitialize; override;

  public
    /// <summary> Creates new Device Information service client. </summary>
    /// <param name="Client"> The <see cref="TwclGattClient"/> object that
    ///   handles the connection to a WeDo device. </param>
    /// <param name="Hub"> The <see cref="TwclWeDoHub"/> object that owns the
    ///   service. </param>
    /// <exception cref="wclEInvalidArgument"> The exception raises if the
    ///   <c>Client</c> or <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Client: TwclGattClient;
      const Hub: TwclWeDoHub); override;

    /// <summary> Reads the firmware version. </summary>
    /// <param name="Version"> If the method completed with success the
    ///   parameter contains the current device's firmware version. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadFirmwareVersion(out Version: string): Integer;
    /// <summary> Reads the hardware version. </summary>
    /// <param name="Version"> If the method completed with success the
    ///   parameter contains the current device's hardware version. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadHardwareVersion(out Version: string): Integer;
    /// <summary> Reads the software version. </summary>
    /// <param name="Version"> If the method completed with success the
    ///   parameter contains the current device's software version. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadSoftwareVersion(out Version: string): Integer;
    /// <summary> Reads the device's manufacturer name. </summary>
    /// <param name="Name"> If the method completed with success the parameter
    ///   contains the current device's manufacturer name. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadManufacturerName(out Name: string): Integer;
  end;

  /// <summary> The <c>OnBatteryLevelChanged</c> event handler
  ///   prototype. </summary>
  /// <param name="Sender"> The object that fired the event. </param>
  /// <param name="Level"> The current battery level in percents in
  ///   range 0-100. </param>
  TwclBatteryLevelChangedEvent = procedure(Sender: TObject;
    const Level: Byte) of object;

  /// <summary> The class represents the WeDo Battery Level service. </summary>
  /// <seealso cref="TwclWeDoService"/>
  TwclWeDoBatteryLevelService = class(TwclWeDoService)
  private const
    // Standard Bluetooth LE Battery Level Service.
    WEDO_SERVICE_BATTERY_LEVEL: TGUID = '{0000180f-0000-1000-8000-00805f9b34fb}';

    // Battery level characteristic. [Mandatory] [Readable, Notifiable]
    WEDO_CHARACTERISTIC_BATTERY_LEVEL: TGUID = '{00002a19-0000-1000-8000-00805f9b34fb}';

  private
    FBatteryLevelChar: TwclGattCharacteristic;

    FOnBatteryLevelChanged: TwclBatteryLevelChangedEvent;

    /// <summary> This method called internally by the <see cref="TwclWeDoHub"/>
    ///   to notify about characteristic changes. A derived class may override
    ///   this method to check for required characteristic changes. </summary>
    /// <param name="Handle"> The characteristic handle. </param>
    /// <param name="Value"> The new characteristic value. </param>
    procedure CharacteristicChanged(const Handle: Word;
      const Value: TArray<Byte>); override;

  protected
    /// <summary> Fires the <c>OnBatteryLevelChanged</c> event. </summary>
    /// <param name="Level"> The current battery level in percents in
    ///   range 0-100. </param>
    procedure DoBatteryLevelChanged(const Level: Byte); virtual;

    /// <summary> Initializes the WeDo service. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Initialize: Integer; override;
    /// <summary> Uninitializes the WeDo service. </summary>
    procedure Uninitialize; override;

  public
    /// <summary> Creates new Battery Level service client. </summary>
    /// <param name="Client"> The <see cref="TwclGattClient"/> object that
    ///   handles the connection to a WeDo device. </param>
    /// <param name="Hub"> The <see cref="TwclWeDoHub"/> object that owns the
    ///   service. </param>
    /// <exception cref="wclEInvalidArgument"> The exception raises if the
    ///   <c>Client</c> or <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Client: TwclGattClient;
      const Hub: TwclWeDoHub); override;

    /// <summary> Reads the device's battery level. </summary>
    /// <param name="Level"> the current battery level in percents. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadBatteryLevel(out Level: Byte): Integer;

    /// <summary> The event fires when the battery level has been
    ///   changed. </summary>
    /// <seealso cref="TwclBatteryLevelChangedEvent" />
    property OnBatteryLevelChanged: TwclBatteryLevelChangedEvent
      read FOnBatteryLevelChanged write FOnBatteryLevelChanged;
  end;

  // Forward declaration
  TwclWeDoIo = class;

  /// <summary> The class represents the WeDo IO service. </summary>
  /// <seealso cref="TwclWeDoService"/>
  TwclWeDoIoService = class(TwclWeDoService)
  private const
    // WeDo IO Service
    WEDO_SERVICE_IO: TGUID ='{00004f0e-1212-efde-1523-785feabcd123}';

    // Sensor Value characteristic. [Mandatory] [Readable, Notifiable]
    WEDO_CHARACTERISTIC_SENSOR_VALUE: TGUID ='{00001560-1212-efde-1523-785feabcd123}';
    // Sensor Value Format characteristic. [Mandatory] [Notifiable]
    WEDO_CHARACTERISTIC_SENSOR_VALUE_FORMAT: TGUID ='{00001561-1212-efde-1523-785feabcd123}';
    // Input command characteristic. [Mandatory] [Writable, Writable Without Response]
    WEDO_CHARACTERISTIC_INPUT_COMMAND: TGUID ='{00001563-1212-efde-1523-785feabcd123}';
    // Output command characteristic. [Mandatory] [Writable, Writable Without Response]
    WEDO_CHARACTERISTIC_OUTPUT_COMMAND: TGUID ='{00001565-1212-efde-1523-785feabcd123}';

    OUT_CMD_HDR_SIZE = 3;
    OUT_CMD_ID_MOTOR_POWER_CONTROL = 1;
    OUT_CMD_ID_PIEZO_PLAY_TONE = 2;
    OUT_CMD_ID_PIEZO_STOP = 3;
    OUT_CMD_ID_RGB_CONTROL = 4;
    OUT_CMD_ID_DIRECT_WRITE = 5;

    IN_CMD_HDR_SIZE = 3;
    IN_CMD_TYPE_READ = 1;
    IN_CMD_TYPE_WRITE = 2;
    IN_CMD_ID_INPUT_VALUE = 0;
    IN_CMD_ID_INPUT_FORMAT = 1;

  private
    // Local list of ALL input formats of the WeDo HUB.
    FInputFormats: TDictionary<Byte, TwclWeDoInputFormat>;
    // When an input format is missing (value received does not have a valid
    // input format) this dictionary sets it's value to true to signal that a
    // request for a new input format was received.
    FMissingInputFormats: TDictionary<Byte, Boolean>;

    FSensorValueChar: TwclGattCharacteristic;
    FSensorValueFormatChar: TwclGattCharacteristic;
    FInputCommandChar: TwclGattCharacteristic;
    FOutputCommandChar: TwclGattCharacteristic;

    procedure ClearInputFormats;

    function ComposeOutputCommand(const CommandId: Byte;
      const ConnectionId: Byte; const Data: TArray<Byte>): TArray<Byte>;
    function ComposeInputCommand(const CommandId: Byte; const CommandType: Byte;
      const ConnectionId: Byte; const Data: TArray<Byte>): TArray<Byte>;

    function WriteOutputCommand(const Command: TArray<Byte>): Integer;
    function WriteInputCommand(const Command: TArray<Byte>): Integer;

    procedure RequestMissingInputFormat(const ConnectionId: Byte);

    procedure InputValueChanged(const Value: TArray<Byte>);
    procedure InputFormatChanged(const Data: TArray<Byte>);

    function PiezoPlayTone(const Frequency: Word; const Duration: Word;
      const ConnectionId: Byte): Integer;
    function PiezoStopPlaying(const ConnectionId: Byte): Integer;

    function WriteData(const Data: TArray<Byte>;
      const ConnectionId: Byte): Integer;

    function WriteMotorPower(Power: ShortInt; const Offset: Byte;
      const ConnectionId: Byte): Integer; overload;
    function WriteMotorPower(const Power: ShortInt;
      const ConnectionId: Byte): Integer; overload;

    function WriteColor(const Red: Byte; const Green: Byte; const Blue: Byte;
      const ConnectionId: Byte): Integer;
    function WriteColorIndex(const Index: Byte;
      const ConnectionId: Byte): Integer;

    function ReadValue(const ConnectionId: Byte): Integer;

    function WriteInputFormat(const Format: TwclWeDoInputFormat;
      const ConnectionId: Byte): Integer;
    function ReadInputFormat(const ConnectionId: Byte): Integer;

    function ResetIo(const ConnectionId: Byte): Integer;

    /// <summary> This method called internally by the <see cref="TwclWeDoHub"/>
    ///   to notify about characteristic changes. A derived class may override
    ///   this method to check for required characteristic changes. </summary>
    /// <param name="Handle"> The characteristic handle. </param>
    /// <param name="Value"> The new characteristic value. </param>
    procedure CharacteristicChanged(const Handle: Word;
      const Value: TArray<Byte>); override;

  protected
    /// <summary> Initializes the WeDo service. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Initialize: Integer; override;
    /// <summary> Uninitializes the WeDo service. </summary>
    procedure Uninitialize; override;

  public
    /// <summary> Creates new IO service client. </summary>
    /// <param name="Client"> The <see cref="TwclGattClient"/> object that
    ///   handles the connection to a WeDo device. </param>
    /// <param name="Hub"> The <see cref="TwclWeDoHub"/> object that owns
    ///   the service. </param>
    /// <exception cref="wclEInvalidArgument"> The exception raises if the
    ///   <c>Client</c> or <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Client: TwclGattClient;
      const Hub: TwclWeDoHub); override;
    /// <summary. Frees the object. </summary>
    destructor Destroy; override;
  end;

  /// <summary> The <c>OnButtonStateChanged</c> event handler
  ///   prototype. </summary>
  /// <param name="Sender"> The object that fires the event. </param>
  /// <param name="Pressed"> The button's state. <c>True</c> if button has been
  ///   pressed. <c>False</c> if button has been released. </param>
  TwclWeDoHubButtonStateChangedEvent = procedure(Sender: TObject;
    const Pressed: Boolean) of object;
  /// <summary> The event handler prototype for alert events. </summary>
  /// <param name="Sender"> The object that fires the event. </param>
  /// <param name="Alert"> <c>True</c> if the alert is active. <c>False</c>
  ///   otherwise. </param>
  TwclWeDoHubAlertEvent = procedure(Sender: TObject;
    const Alert: Boolean) of object;
  /// <summary> The <c>OnDeviceAttached</c> and <c>OnDeviceDetached</c> events
  ///   handler prototype. </summary>
  /// <param name="Sender"> The object that fires the event. </param>
  /// <param name="Device"> The Input/Output device object. </param>
  /// <seealso cref="TwclWeDoIo"/>
  TwclWeDoDeviceStateChangedEvent = procedure(Sender: TObject;
    const Device: TwclWeDoIo) of object;

  /// <summary> The class represents the WeDo Hub service. </summary>
  /// <seealso cref="TwclWeDoService"/>
  TwclWeDoHubService = class(TwclWeDoService)
  private const
    // WeDo HUB service.
    WEDO_SERVICE_HUB: TGUID = '{00001523-1212-efde-1523-785feabcd123}';

    // Device name characteristic. [Mandatory] [Readable, Writable]
    WEDO_CHARACTERISTIC_DEVICE_NAME: TGUID = '{00001524-1212-efde-1523-785feabcd123}';
    // Buttons state characteristic. [Mandatory] [Readable, Notifiable]
    WEDO_CHARACTERISTIC_BUTTON_STATE: TGUID = '{00001526-1212-efde-1523-785feabcd123}';
    // IO attached charactrisitc. [Mandatory] [Notifiable]
    WEDO_CHARACTERISTIC_IO_ATTACHED: TGUID = '{00001527-1212-efde-1523-785feabcd123}';
    // Low Voltrage Alert characteristic. [Mandatory] [Readable, Notifiable]
    WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT: TGUID = '{00001528-1212-efde-1523-785feabcd123}';
    // Turn Off command characteristic. [Mandatory] [Writable]
    WEDO_CHARACTERISTIC_TURN_OFF: TGUID = '{0000152b-1212-efde-1523-785feabcd123}';

    // High Current Aleart characteristic. [Optional] [Readable, Notifiable]
    WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT: TGUID = '{00001529-1212-efde-1523-785feabcd123}';
    // Low Signal Aleart characteristic. [Optional] [Readable, Notifiable]
    WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT: TGUID = '{0000152a-1212-efde-1523-785feabcd123}';
    // VCC Port Control characteristic. [Optional] [Readable, Writable]
    WEDO_CHARACTERISTIC_VCC_PORT_CONTROL: TGUID = '{0000152c-1212-efde-1523-785feabcd123}';
    // Battery Type characteristic. [Optional] [Readable]
    WEDO_CHARACTERISTIC_BATTERY_TYPE: TGUID = '{0000152d-1212-efde-1523-785feabcd123}';
    // Device Disconnect Cpmmand characteristic. [Optional] [Writable]
    WEDO_CHARACTERISTIC_DEVICE_DISCONNECT: TGUID = '{0000152e-1212-efde-1523-785feabcd123}';

  private type
    TwclWeDoHubDeviceDetachedEvent = procedure(Sender: TObject;
      const ConnectionId: Byte) of object;

  private
    FDeviceNameChar: TwclGattCharacteristic;
    FButtonStateChar: TwclGattCharacteristic;
    FIoAttachedChar: TwclGattCharacteristic;
    FLowVoltageAlertChar: TwclGattCharacteristic;
    FTurnOffChar: TwclGattCharacteristic;

    FHighCurrentAleartChar: TwclGattCharacteristic;
    FLowSignalChar: TwclGattCharacteristic;
    FVccPortChar: TwclGattCharacteristic;
    FBatteryTypeChar: TwclGattCharacteristic;
    FDeviceDisconnectChar: TwclGattCharacteristic;

    FOnButtonStateChanged: TwclWeDoHubButtonStateChangedEvent;
    FOnDeviceAttached: TwclWeDoDeviceStateChangedEvent;
    FOnDeviceDetached: TwclWeDoHubDeviceDetachedEvent;
    FOnHighCurrentAlert: TwclWeDoHubAlertEvent;
    FOnLowVoltageAlert: TwclWeDoHubAlertEvent;
    FOnLowSignalAlert: TwclWeDoHubAlertEvent;

    /// <summary> This method called internally by the <see cref="TwclWeDoHub"/>
    ///   to notify about characteristic changes. A derived class may override
    ///   this method to check for required characteristic changes. </summary>
    /// <param name="Handle"> The characteristic handle. </param>
    /// <param name="Value"> The new characteristic value. </param>
    procedure CharacteristicChanged(const Handle: Word;
      const Value: TArray<Byte>); override;

    function ReadDeviceName(out Name: string): Integer;
    function WriteDeviceName(Name: string): Integer;

    function TurnOff: Integer;

    property OnButtonStateChanged: TwclWeDoHubButtonStateChangedEvent
      read FOnButtonStateChanged write FOnButtonStateChanged;
    property OnDeviceAttached: TwclWeDoDeviceStateChangedEvent
      read FOnDeviceAttached write FOnDeviceAttached;
    property OnDeviceDetached: TwclWeDoHubDeviceDetachedEvent
      read FOnDeviceDetached write FOnDeviceDetached;
    property OnHighCurrentAlert: TwclWeDoHubAlertEvent read FOnHighCurrentAlert
      write FOnHighCurrentAlert;
    property OnLowVoltageAlert: TwclWeDoHubAlertEvent read FOnLowVoltageAlert
      write FOnLowVoltageAlert;
    property OnLowSignalAlert: TwclWeDoHubAlertEvent read FOnLowSignalAlert
      write FOnLowSignalAlert;

  protected
    /// <summary> Initializes the WeDo service. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Initialize: Integer; override;
    /// <summary> Uninitializes the WeDo service. </summary>
    procedure Uninitialize; override;

    /// <summary> Fires the <c>OnButtonStateChanged</c> event. </summary>
    /// <param name="Pressed"> <c>True</c> if the button has been pressed.
    ///   <c>False</c> if the button has been released. </param>
    procedure DoButtonStateChanged(const Pressed: Boolean); virtual;
    /// <summary> Fires the <c>OnLowVoltageAlert</c> event. </summary>
    /// <param name="Alert"> <c>True</c> if device runs on low battery.
    ///   <c>False</c> otherwise. </param>
    procedure DoLowVoltageAlert(const Alert: Boolean); virtual;
    /// <summary> Fires the <c>OnDeviceAttached</c> event. </summary>
    /// <param name="Device"> The IO device object. </param>
    /// <seealso cref="TwclWeDoIo"/>
    procedure DoDeviceAttached(const Device: TwclWeDoIo); virtual;
    /// <summary> Fires the <c>OnDeviceDetached</c> event. </summary>
    /// <param name="ConnectionId"> The device connection ID. </param>
    procedure DoDeviceDetached(const ConnectionId: Byte); virtual;
    /// <summary> Fires then <c>OnHighCurrentAlert</c> event.</summary>
    /// <param name="Alert"> <c>True</c> if device runs on high current.
    ///   <c>False</c> otherwise. </param>
    procedure DoHightCurrentAlert(const Alert: Boolean); virtual;
    /// <summary> Fires then <c>OnLowSignalAlert</c> event.</summary>
    /// <param name="Alert"> <c>True</c> if the signal from radio has low RSSI.
    ///   <c>False</c> otherwise. </param>
    procedure DoLowSignalAlert(const Alert: Boolean); virtual;

  public
    /// <summary> Creates new IO service client. </summary>
    /// <param name="Client"> The <see cref="TwclGattClient"/> object that
    ///   handles the connection to a WeDo device. </param>
    /// <param name="Hub"> The <see cref="TwclWeDoHub"/> object that owns the
    ///   service. </param>
    /// <exception cref="wclEInvalidArgument"> The exception raises if the
    ///   <c>Client</c> or <c>Hub</c> parameter is <c>null</c>. </exception>
    constructor Create(const Client: TwclGattClient;
      const Hub: TwclWeDoHub); override;
  end;

  /// <summary> The class represents a WeDo Hub hardware. </summary>
  /// <seealso cref="TComponent" />
  TwclWeDoHub = class(TComponent)
  private
    FClient: TwclGattClient;
    FConnected: Boolean;
    FDevices: TList<TwclWeDoIo>;
    FHubConnected: Boolean;

    // Hub GATT services.
    FDeviceInformation: TwclWeDoDeviceInformationService;
    FBatteryLevel: TwclWeDoBatteryLevelService;
    FIo: TwclWeDoIoService;
    FHub: TwclWeDoHubService;

    FOnConnected: TwclClientConnectionConnectEvent;
    FOnDisconnected: TwclClientConnectionDisconnectEvent;
    FOnButtonStateChanged: TwclWeDoHubButtonStateChangedEvent;
    FOnDeviceAttached: TwclWeDoDeviceStateChangedEvent;
    FOnDeviceDetached: TwclWeDoDeviceStateChangedEvent;
    FOnLowVoltageAlert: TwclWeDoHubAlertEvent;
    FOnHighCurrentAlert: TwclWeDoHubAlertEvent;
    FOnLowSignalAlert: TwclWeDoHubAlertEvent;

    function GetAddress: Int64;
    function GetState: TwclClientState;

    // Detaches all devices when Hub disconnected.
    procedure DetachDevices;
    // Disconnect from all WeDo services.
    procedure DisconnectServices;
    procedure DisconnectHub;

    // GATT client connect event handler.
    procedure ClientConnect(Sender: TObject; const Error: Integer);
    // GATT client disconnect event handler.
    procedure ClientDisconnect(Sender: TObject; const Reason: Integer);
    // GATT client characteristi changed event handler.
    procedure ClientCharacteristicChanged(Sender: TObject; const Handle: Word;
      const Value: TwclGattCharacteristicValue);

    procedure HubButtonStateChanged(Sender: TObject; const Pressed: Boolean);
    procedure HubLowVoltageAlert(Sender: TObject; const Alert: Boolean);
    procedure HubHighCurrentAlert(Sender: TObject; const Alert: Boolean);
    procedure HubLowSignalAlert(Sender: TObject; const Alert: Boolean);
    procedure HubDeviceAttached(Sender: TObject; const Device: TwclWeDoIo);
    procedure HubDeviceDetached(Sender: TObject; const ConnectionId: Byte);

    property Io: TwclWeDoIoService read FIo;

  protected
    /// <summary> Fires the <c>OnConnected</c> event. </summary>
    /// <param name="Error"> If the connection has been established the
    ///   parameter is <see cref="WCL_E_SUCCESS" />. If connection has not been
    ///   established the parameter value is one of the Bluetooth error
    ///   codes. </param>
    procedure DoConnected(const Error: Integer); virtual;
    /// <summary> Fires the <c>OnDisconnected</c> event. </summary>
    /// <param name="Reason"> The disconnection reason code. </param>
    procedure DoDisconnected(const Reason: Integer); virtual;
    /// <summary> Fires the <c>OnButtonStateChanged</c> event. </summary>
    /// <param name="Pressed"> <c>True</c> if the button has been pressed.
    ///   <c>False</c> if the button has been released. </param>
    procedure DoButtonStateChanged(const Pressed: Boolean); virtual;
    /// <summary> Fires the <c>OnLowVoltageAlert</c> event. </summary>
    /// <param name="Alert"> <c>True</c> if device runs on low battery.
    ///   <c>False</c> otherwise. </param>
    procedure DoLowVoltageAlert(const Alert: Boolean); virtual;
    /// <summary> Fires the <c>OnHighCurrentAlert</c> event. </summary>
    /// <param name="Alert"> <c>True</c> if device runs on high current.
    ///  <c>False</c> otherwise. </param>
    procedure DoHighCurrentAlert(const Alert: Boolean); virtual;
    /// <summary> Fires the <c>OnLowSignalAlert</c> event. </summary>
    /// <param name="Alert"> <c>True</c> if the RSSI value is low. <c>False</c>
    ///   otherwise. </param>
    procedure DoLowSignalAlert(const Alert: Boolean); virtual;
    /// <summary> Fires the <c>OnDeviceAttached</c> event. </summary>
    /// /// <param name="Device"> The Input/Output device object. </param>
    /// <seealso cref="TwclWeDoIo"/>
    procedure DoDeviceAttached(const Device: TwclWeDoIo); virtual;
    /// <summary> Fires the <c>OnDeviceDetached</c> event. </summary>
    /// <param name="Device"> The Input/Output device object. </param>
    /// <seealso cref="TwclWeDoIo"/>
    procedure DoDeviceDetached(const Device: TwclWeDoIo); virtual;

  public
    /// <summary> Creates new WeDo Client. </summary>
    /// <param name="AOwner"> The component owner. </param>
    constructor Create(AOwner: TComponent); override;
    /// <summary> Frees the object. </summary>
    destructor Destroy; override;

    /// <summary> Connects to a selected WeDo Hub. </summary>
    /// <param name="Radio"> The <see cref="TwclBluetoothRadio" /> object that
    ///   should be used for executing Bluetooth LE connection. </param>
    /// <param name="Address"> The WeDo Hub MAC address. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclBluetoothRadio" />
    function Connect(const Radio: TwclBluetoothRadio;
      const Address: Int64): Integer;
    /// <summary> Disconnects from WeDo Hub. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Disconnect: Integer;

    /// <summary> Turns the Hub off. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <remarks> The method sends the Turn Off command to the connected
    ///   Hub. </remarks>
    function TurnOff: Integer;

    /// <summary> Reads the current device name. </summary>
    /// <param name="Name"> If the method completed with success the parameter
    ///   contains the current device name. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ReadDeviceName(out Name: string): Integer;
    /// <summary> Writes new device name. </summary>
    /// <param name="Name"> The new device name. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function WriteDeviceName(const Name: string): Integer;

    /// <summary> Gets the Hub device information service object. </summary>
    /// <value> The Hub device information service object. </value>
    /// <seealso cref="TwclWeDoDeviceInformationService"/>
    property DeviceInformation: TwclWeDoDeviceInformationService
      read FDeviceInformation;
    /// <summary> Gets the battery level service object. </summary>
    /// <value> The battery level service object. </value>
    /// <seealso cref="TwclWeDoBatteryLevelService"/>
    property BatteryLevel: TwclWeDoBatteryLevelService
      read FBatteryLevel;

    /// <summary> Gets the connected WeDo Hub Address. </summary>
    /// <value> The Hub MAC address. </value>
    property Address: Int64 read GetAddress;
    /// <summary> Gets connected status. </summary>
    /// <value> <c>true</c> if connected to WeDo Hub. </value>
    property Connected: Boolean read FConnected;
    /// <summary> Gets internal GATT client state. </summary>
    /// <value> The internal GATT client state. </value>
    /// <seealso cref="wclClientState" />
    property ClientState: TwclClientState read GetState;
    /// <summary> Gets the list of the attached IO devices. </summary>
    /// <value> The list of the attached IO devices. </value>
    /// <seealso cref="wclWeDoIo"/>
    property IoDevices: TList<TwclWeDoIo> read FDevices;

  published
    /// <summary> The event fires when connection to a WeDo Hub
    ///   has been established. </summary>
    /// <seealso cref="TwclClientConnectionConnectEvent" />
    property OnConnected: TwclClientConnectionConnectEvent read FOnConnected
      write FOnConnected;
    /// <summary> The event fires when WeDo Hub has been
    ///   disconnected. </summary>
    /// <seealso cref="TwclClientConnectionDisconnectEvent" />
    property OnDisconnected: TwclClientConnectionDisconnectEvent
      read FOnDisconnected write FOnDisconnected;
    /// <summary> The event fires when button state has been changed. </summary>
    /// <seealso cref="TwclWeDoHubButtonStateChangedEvent"/>
    property OnButtonStateChanged: TwclWeDoHubButtonStateChangedEvent
      read FOnButtonStateChanged write FOnButtonStateChanged;
    /// <summary> The event fires when new IO device has been
    ///   attached. </summary>
    /// <seealso cref="TwclWeDoDeviceStateChangedEvent"/>
    property OnDeviceAttached: TwclWeDoDeviceStateChangedEvent
      read FOnDeviceAttached write FOnDeviceAttached;
    /// <summary> The event fires when an existing IO device has been
    ///   detached. </summary>
    /// <seealso cref="TwclWeDoDeviceStateChangedEvent"/>
    property OnDeviceDetached: TwclWeDoDeviceStateChangedEvent
      read FOnDeviceDetached write FOnDeviceDetached;
    /// <summary> The event fires when device runs on low battery. </summary>
    /// <seealso cref="TwclWeDoHubAlertEvent"/>
    property OnLowVoltageAlert: TwclWeDoHubAlertEvent read FOnLowVoltageAlert
      write FOnLowVoltageAlert;
    /// <summary> The event fires when device runs on high current. </summary>
    /// <seealso cref="TwclWeDoHubAlertEvent"/>
    property OnHighCurrentAlert: TwclWeDoHubAlertEvent read FOnHighCurrentAlert
      write FOnHighCurrentAlert;
    /// <summary> The event fires when low RSSI value received from the
    ///   device. </summary>
    /// <seealso cref="TwclWeDoHubAlertEvent"/>
    property OnLowSignalAlert: TwclWeDoHubAlertEvent read FOnLowSignalAlert
      write FOnLowSignalAlert;
  end;

  /// <summary> The class represets an attached Input/Outpout device. </summary>
  TwclWeDoIo = class abstract
  private const
    WEDO_DEVICE_MOTOR = 1;
    WEDO_DEVICE_VOLTAGE_SENSOR = 20;
    WEDO_DEVICE_CURRENT_SENSOR = 21;
    WEDO_DEVICE_PIEZO = 22;
    WEDO_DEVICE_RGB = 23;
    WEDO_DEVICE_TILT_SENSOR = 34;
    WEDO_DEVICE_MOTION_SENSOR = 35;

  private
    FAttached: Boolean;
    FConnectionId: Byte;
    FDataFormats: TList<TwclWeDoDataFormat>;
    FDefaultInputFormat: TwclWeDoInputFormat;
    FDeviceType: TwclWeDoIoDeviceType;
    FFirmwareVersion: TwclWeDoVersion;
    FHardwareVersion: TwclWeDoVersion;
    FHub: TwclWeDoHub;
    FInputFormat: TwclWeDoInputFormat;
    FInternal: Boolean;
    FNumbersFromValueData: TList<TArray<Byte>>;
    FPortId: Byte;
    FValidDataFormats: TList<TwclWeDoDataFormat>;
    FValue: TArray<Byte>;

    procedure SetDefaultInputFormatProp(Format: TwclWeDoInputFormat);

    procedure SetValue(const Value: TArray<Byte>);

    function SetDefaultInputFormat(const Format: TwclWeDoInputFormat): Integer;

    function GetAsFloat: Single;
    function GetAsInteger: Integer;

    function GetInputFormatMode: Byte;

    function DataFormatForInputFormat(
      const InputFormat: TwclWeDoInputFormat): TwclWeDoDataFormat;

    function VerifyValue(const Value: TArray<Byte>): Boolean;

    // The method called by HUB when new device found (attached).
    class function Attach(const Hub: TwclWeDoHub;
      const RawInfo: TArray<Byte>): TwclWeDoIo;
    // The method called by HUB when device has been detached.
    procedure Detach;

    // The method called by IO Service when Input Format has been updated.
    procedure UpdateInputFormat(const Format: TwclWeDoInputFormat);
    // The method called by the IO Service when new value received.
    procedure UpdateValue(const Value: TArray<Byte>);

  protected
    /// <summary> Sends data to the IO service. </summary>
    /// <param name="Data"> The data to write. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function WriteData(const Data: TArray<Byte>): Integer;
    /// <summary> If the notifications is disabled for the service in the Input
    ///   Format you will have to use this method to request an updated value
    ///   for the service. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function SendReadValueRequest: Integer;
    /// <summary> Sends a reset command to the Device. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function ResetSensor: Integer;

    /// <summary> Adds a new valid data format. </summary>
    /// <param name="Format"> The data format to add. </param>
    /// <seealso cref="TwclWeDoDataFormat"/>
    procedure AddValidDataFormat(const Format: TwclWeDoDataFormat);
    /// <summary> Removes a valid data format. </summary>
    /// <param name="Format"> The data format to remove. </param>
    /// <seealso cref="TwclWeDoDataFormat"/>
    procedure RemoveValidDataFormat(const Format: TwclWeDoDataFormat);
    /// <summary> Send an updated input format for this service to the
    ///   device. </summary>
    /// <param name="Format"> New input format. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seelso cref="TwclWeDoInputFormat"/>
    function SendInputFormat(const Format: TwclWeDoInputFormat): Integer;
    /// <summary> Changes mode of the Input Format. </summary>
    /// <param name="Mode"> The Input Format mode. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function SetInputFormatMode(const Mode: Byte): Integer;

    /// <summary> The method called when Input Format has been
    ///   changed. </summary>
    /// <param name="OldFormat"> The old Input Format. </param>
    /// <remarks> A derived class must override this method to get notifications
    ///   about format changes. </remarks>
    procedure InputFormatChanged(const OldFormat: TwclWeDoInputFormat); virtual;
    /// <summary> The method called when data value has been changed. </summary>
    /// <remarks> A derived class must override this method to get notifications
    ///   about value changes. </remarks>
    procedure ValueChanged; virtual;

    /// <summary> Gets current sensor's value as <c>Float</c> number. </summary>
    /// <value> The float sensors value. </value>
    property AsFloat: Single read GetAsFloat;
    /// <summary> Gets the current sensor's value as <c>Integer</c>
    ///   number. </summary>
    /// <value> The integer sensor's value. </value>
    property AsInteger: Integer read GetAsInteger;
    /// <summary> Gets the list of supported Data Formats. </summary>
    /// <value> The list of supported Data Formats. </value>
    /// <seealso cref="TwclWeDoDataFormat"/>
    property DataFormats: TList<TwclWeDoDataFormat> read FDataFormats;
    /// <summary> Gets and sets the default input format. </summary>
    /// <value> The default input format. </value>
    /// <seealso cref="TwclWeDoInputFormat"/>
    property DefaultInputFormat: TwclWeDoInputFormat read FDefaultInputFormat
      write SetDefaultInputFormatProp;
    /// <summary> Gets the sensor Input Format. </summary>
    /// <value> The Input Format. </value>
    /// <seealso cref="TwclWeDoInputFormat"/>
    property InputFormat: TwclWeDoInputFormat read FInputFormat;
    /// <summary> Gets the Input Format mode. </summary>
    /// <value> The Input Format Mode. </value>
    property InputFormatMode: Byte read GetInputFormatMode;
    /// <summary> Gets alist with one byte[] per number received. </summary>
    /// <value> The list of bytes array. </value>
    property NumbersFromValueData: TList<TArray<Byte>>
      read FNumbersFromValueData;
    /// <summary> Gets the current sensors value. </summary>
    /// <value> The sensors value as raw bytes array. </value>
    property Value: TArray<Byte> read FValue write SetValue;

  public
    /// <summary> Creates new IO device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///  <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); virtual;
    /// <summary> Frees the object. </summary>
    destructor Destroy; override;

    /// <summary> Gets the IO device state. </summary>
    /// <value> <c>True</c> if the device is attached. <c>False</c> if the
    ///   device is detached. </value>
    property Attached: Boolean read FAttached;
    /// <summary> Gets the IO connection ID. </summary>
    /// <value> The IO connection ID. </value>
    /// <remarks> It is guarateed that the connection ID is unique. </remarks>
    property ConnectionId: Byte read FConnectionId;
    /// <summary> Gets the device represented by this object. </summary>
    /// <value> The IO device type. </value>
    /// <seealso cref="TwclWeDoIoDeviceType"/>
    property DeviceType: TwclWeDoIoDeviceType read FDeviceType;
    /// <summary> Gets the IO device firmware version. </summary>
    /// <value> The IO device firmware version. </value>
    /// <seealso cref="TwclWeDoVersion"/>
    property FirmwareVersion: TwclWeDoVersion read FFirmwareVersion;
    /// <summary> Gets the IO device hardware version. </summary>
    /// <value> The IO device hardware version. </value>
    /// <seealso cref="TwclWeDoVersion"/>
    property HardwareVersion: TwclWeDoVersion read FHardwareVersion;
    /// <summary> Gets the IO type represented by this object. </summary>
    /// <value> <c>True</c> if the IO device is internal. <c>False</c> if the
    ///   IO device is external. </value>
    property Internal: Boolean read FInternal;
    /// <summary> Gets the WeDo Hub object that owns the IO device. </summary>
    /// <value> The WeDo Hub object. </value>
    /// <seealso cref="TwclWeDoHub"/>
    property Hub: TwclWeDoHub read FHub;
    /// <summary> The index of the port on the Hub the IO is
    ///   attached to.  </summary>
    /// <value> The port ID. </value>
    property PortId: Byte read FPortId;
  end;

  /// <summary> Tones that can be played using the
  ///   <see cref="TwclWeDoPieazo"/> </summary>
  TwclWeDoPiezoNote = (
    /// <summary> C </summary>
    pnC = 1,
    /// <summary> C# </summary>
    pnCis = 2,
    /// <summary> D </summary>
    pnD = 3,
    /// <summary> D# </summary>
    pnDis = 4,
    /// <summary> E </summary>
    pnE = 5,
    /// <summary> F </summary>
    pnF = 6,
    /// <summary> F# </summary>
    pnFis = 7,
    /// <summary> G </summary>
    pnG = 8,
    /// <summary> G# </summary>
    pnGis = 9,
    /// <summary> A </summary>
    pnA = 10,
    /// <summary> A# </summary>
    pnAis = 11,
    /// <summary> B </summary>
    pnB = 12
  );

  /// <summary> The class represents a Piezo tone player device. </summary>
  /// <seealso cref="TwclWeDoIo"/>
  TwclWeDoPieazo = class(TwclWeDoIo)
  private const
    PIEZO_MAX_FREQUENCY = 1500;
    
  public
    /// <summary> Creates new Piezo device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///  <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Plays a tone with a given frequency for the given
    ///   duration in ms. </summary>
    /// <param name="Frequency"> The frequency to play (max allowed frequency
    ///   is 1500). </param>
    /// <param name="Duration"> The duration to play (max supported is 65535
    ///   milli seconds). </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function PlayTone(const Frequency: Word; const Duration: Word): Integer;
    /// <summary> Plays a note. The highest supported node is F# in 6th
    ///   octave. </summary>
    /// <param name="Note"> The note to play. </param>
    /// <param name="Octave"> The octave in which to play the node. </param>
    /// <param name="Duration"> The duration to play (max supported is 65535
    ///   milli seconds). </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclWeDoPiezoNote"/>
    function PlayNote(const Note: TwclWeDoPiezoNote; const Octave: Byte;
      const Duration: Word): Integer;
    /// <summary> Stop playing any currently playing tone. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function StopPlaying: Integer;
  end;

  /// <summary> Mode of the RGB light device. </summary>
  TwclWeDoRgbLightMode = (
    /// <summary> Discrete mode allows selecting a color index from a set of
    ///   predefined colors. </summary>
    lmDiscrete = 0,
    /// <summary> Absolute mode allows selecting any color by specifying its
    ///   RGB component values. </summary>
    lmAbsolute = 1,
    /// <summary> Unknown (unsupported) mode. </summary>
    lmUnknown = 255
  );

  /// <summary> A Lego WeDo color indexes. </summary>
  /// <remarks> This enumration is used in <c>Absolute</c> color mode. </remarks>
  /// <seealso cref="TwclWeDoRgbLightMode"/>
  TwclWeDoColor = (
    /// <summary> Black (none) color. </summary>
    wclBlack = 0,
    /// <summary> Pink color. </summary>
    wclPink = 1,
    /// <summary> Purple color. </summary>
    wclPurple = 2,
    /// <summary> Blue color. </summary>
    wclBlue = 3,
    /// <summary> Sky blue color. </summary>
    wclSkyBlue = 4,
    /// <summary> Teal color. </summary>
    wclTeal = 5,
    /// <summary> Green color. </summary>
    wclGreen = 6,
    /// <summary> Yellow color. </summary>
    wclYellow = 7,
    /// <summary> Orange color. </summary>
    wclOrange = 8,
    /// <summary> Red color. </summary>
    wclRed = 9,
    /// <summary> White color. </summary>
    wclWhite = 10,
    /// <summary> Unknwon color index. </summary>
    wclUnknown = 255
  );

  /// <summary> The class represents a HUB RGB light. </summary>
  /// <seealso cref="TwclWeDoIo"/>
  TwclWeDoRgbLight = class(TwclWeDoIo)
  private
    FColor: TColor;
    FColorIndex: TwclWeDoColor;

    FOnColorChanged: TNotifyEvent;
    FOnModeChanged: TNotifyEvent;

    function GetColorFromByteArray(const Data: TArray<Byte>;
      out Color: TColor): Boolean;

    function GetDefaultColor: TColor;
    function GetDefaultColorIndex: TwclWeDoColor;
    function GetMode: TwclWeDoRgbLightMode;

  protected
    /// <summary> The method called when Input Format has been
    ///   changed. </summary>
    /// <param name="OldFormat"> The old Input Format. </param>
    procedure InputFormatChanged(
      const OldFormat: TwclWeDoInputFormat); override;
    /// <summary> The method called when data value has been
    ///   changed. </summary>
    procedure ValueChanged; override;

    /// <summary> Fires the <c>OnColorChanged</c> event. </summary>
    procedure DoColorChanged; virtual;
    /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
    procedure DoModeChanged; virtual;

  public
    /// <summary> Creates new RGB light device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Switch off the RGB light on the device. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function SwitchOff: Integer;
    /// <summary> Switches to the default Color (i.e. the same color as the
    ///   device has right after a successful connection has
    ///   been established). </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function SwitchToDefaultColor: Integer;
    /// <summary> Sets the RGB color. </summary>
    /// <param name="Rgb"> The RGB color. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function SetColor(const Rgb: TColor): Integer;
    /// <summary> Sets the index of the currently selected color (discrete
    ///   mode). </summary>
    /// <param name="Index"> The color index. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function SetColorIndex(const Index: TwclWeDoColor): Integer;
    /// <summary> Sets the mode of the RGB light. </summary>
    /// <param name="Mode"> The RGB lite mode. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclWeDoRgbLightMode"/>
    function SetMode(const Mode: TwclWeDoRgbLightMode): Integer;

    /// <summary> Gets the color of the RGB light on the device
    ///   (absolute mode). </summary>
    /// <value> The RGB color. </value>
    property Color: TColor read FColor;
    /// <summary> Gets the index of the currently selected color (discrete
    ///   mode). </summary>
    /// <value> The color index. </value>
    /// <seealso cref="TwclWeDoColor"/>
    property ColorIndex: TwclWeDoColor read FColorIndex;
    /// <summary> Gets the default color of the RGB light (absolute
    ///   mode). </summary>
    /// <value> The default color. </value>
    property DefaultColor: TColor read GetDefaultColor;
    /// <summary> Gets the default color index of the RGB, when in the discrete
    ///   mode. </summary>
    /// <value> The default color index. </value>
    /// <seealso cref="TwclWeDoColor"/>
    property DefaultColorIndex: TwclWeDoColor read GetDefaultColorIndex;
    /// <summary> Gets the mode of the RGB light. </summary>
    /// <value> The RGB light device mode. </value>
    /// <seealso cref="TwclWeDoRgbLightMode"/>
    property Mode: TwclWeDoRgbLightMode read GetMode;

    /// <summary> The event fires when color has been changed. </summary>
    property OnColorChanged: TNotifyEvent read FOnColorChanged
      write FOnColorChanged;
    /// <summary> The event fired when the RGB LED mode has been changed. </summary>
    property OnModeChanged: TNotifyEvent read FOnModeChanged
      write FOnModeChanged;
  end;

  /// <summary> The class represents a WeDo Hub current sensor. </summary>
  /// <seealso cref="TwclWeDoIo"/>
  TwclWeDoCurrentSensor = class(TwclWeDoIo)
  private
    FOnCurrentChanged: TNotifyEvent;

    function GetCurrent: Single;

  protected
    /// <summary> The method called when data value has been changed. </summary>
    procedure ValueChanged; override;

    /// <summary> Fires the <c>OnCurrentChanged</c> event. </summary>
    procedure DoCurrentChanged; virtual;

  public
    /// <summary> Creates new current sensor device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Gets the battery current in mA. </summary>
    /// <value> The current in milli ampers. </value>
    property Current: Single read GetCurrent;

    /// <summary> The event fires when current has been changed. </summary>
    property OnCurrentChanged: TNotifyEvent read FOnCurrentChanged
      write FOnCurrentChanged;
  end;

  /// <summary> The class represents a WeDo Hub Voltage sensor. </summary>
  /// <seealso cref="TwclWeDoIo"/>
  TwclWeDoVoltageSensor = class(TwclWeDoIo)
  private
    FOnVoltageChanged: TNotifyEvent;

    function GetVoltage: Single;

  protected
    /// <summary> The method called when data value has been changed. </summary>
    procedure ValueChanged; override;

    /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
    procedure DoVoltageChanged; virtual;

  public
    /// <summary> Creates new voltage sensor device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Gets the current battery voltage in milli volts. </summary>
    /// <value> The battery voltage in milli volts. </value>
    property Voltage: Single read GetVoltage;

    /// <summary> The event fires when voltage has been changed. </summary>
    property OnVoltageChanged: TNotifyEvent read FOnVoltageChanged
      write FOnVoltageChanged;
  end;

  /// <summary> The motor's direction. </summary>
  TwclWeDoMotorDirection = (
    /// <summary> Drifting (Floating). </summary>
    mdDrifting = 0,
    /// <summary> Running left. </summary>
    mdLeft = 1,
    /// <summary> Running right. </summary>
    mdRight = 2,
    /// <summary> Brake. </summary>
    mdBraking = 3,
    /// <summary> Unknwon. </summary>
    mdUnknown = 255
  );

  /// <summary> The class represents a WeDo motor. </summary>
  /// <seealso cref="TwclWeDoIo"/>
  TwclWeDoMotor = class(TwclWeDoIo)
  private const
    MOTOR_POWER_DRIFT = 0;
    MOTOR_POWER_BRAKE = 127;

    MOTOR_MIN_SPEED = 1;
    MOTOR_MAX_SPEED = 100;

    // Only send values in the range 35-100.
    // An offset is needed as values below 35 is not enough power to actually
    // make the motor turn.
    MOTOR_POWER_OFFSET = 35;

  private
    FDirection: TwclWeDoMotorDirection;
    FPower: ShortInt;

    function GetIsBraking: Boolean;
    function GetIsDrifting: Boolean;
    function GetPower: Byte;

    function SendPower(const Power: ShortInt): Integer;

    function ConvertUnsignedMotorPowerToSigned(
      const Direction: TwclWeDoMotorDirection; const Power: Byte): ShortInt;

  public
    /// <summary> Creates new motor class object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c>
    ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Sends a command to stop (brake) the motor. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Brake: Integer;
    /// <summary> Sends a command to stop (drift/float) the motor. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Drift: Integer;
    /// <summary> Sends a command to run the motor at a given <c>Power</c> in
    ///   a given <c>Direction</c>. The minimum speed is 0 and the maximum
    ///   speed is 100. </summary>
    /// <param name="Direction"> The direction to run the motor. </param>
    /// <param name="Power"> The power to run the motor with. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclWeDoMotorDirection"/>
    function Run(const Direction: TwclWeDoMotorDirection;
      const Power: Byte): Integer;

    /// <summary> Gets the current running direction of the motor. </summary>
    /// <value> Teh motor direction. </value>
    /// <seealso cref="TwclWeDoMotorDirection"/>
    property Direction: TwclWeDoMotorDirection read FDirection;
    /// <summary> Gets the motor's braking state. </summary>
    /// <value> <c>True</c> if the motor is in brake state. <c>False</c>
    ///   otheerwise. </value>
    property IsBraking: Boolean read GetIsBraking;
    /// <summary> Gets the motor's drifting state. </summary>
    /// <value> <c>True</c> if the motor is currently drifting or floating.
    ///   When floating the motor axis can be turned without
    ///   resistance. </value>
    property IsDrifting: Boolean read GetIsDrifting;
    /// <summary> Gets the power the motor is currently running with (0 if
    ///   braking or drifting). </summary>
    /// <value> Teh current motor power. </value>
    property Power: Byte read GetPower;
  end;

  /// <summary> The base class for WeDo sensors that can be reset. </summary>
 /// <seealso cref="TwclWeDoIo"/>
 TwclWeDoResetableSensor = class abstract(TwclWeDoIo)
 public
   /// <summary> Creates new device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Resets the sensor. </summary>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    function Reset: Integer;
 end;

 /// <summary> Supported modes for the motion sensor. </summary>
 TwclWeDoMotionSensorMode = (
    /// <summary> Detect mode - produces value that reflect the relative
    ///   distance from the sensor to objects in front of it. </summary>
    mmDetect = 0,
    /// <summary> Count mode - produces values that reflect how many times the
    ///   sensor has been activated. </summary>
    mmCount = 1,
    /// <summary> Unknown (unsupported) mode. </summary>
    mmUnknown = 255
 );

 /// <summary> The class represents a WeDo Motion Sensor. </summary>
 /// <seealso cref="TwclWeDoResetableSensor"/>
 TwclWeDoMotionSensor = class(TwclWeDoResetableSensor)
 private
   FCount: Cardinal;
   FDistance: Single;

   FOnCountChanged: TNotifyEvent;
   FOnDistanceChanged: TNotifyEvent;
   FOnModeChanged: TNotifyEvent;

   function GetCount: Cardinal;
   function GetDistance: Single;
   function GetMode: TwclWeDoMotionSensorMode;

 protected
   /// <summary> The method called when Input Format has been
   ///   changed. </summary>
   /// <param name="OldFormat"> The old Input Format. </param>
   procedure InputFormatChanged(const OldFormat: TwclWeDoInputFormat); override;
   /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
   procedure ValueChanged; override;

   /// <summary> Fires the <c>OnCountChanged</c> event. </summary>
   procedure DoCountChanged; virtual;
   /// <summary> Fires the <c>OnDistanceChanged</c> event. </summary>
   procedure DoDistanceChanged; virtual;
   /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
   procedure DoModeChanged; virtual;

 public
   /// <summary> Creates new motion sensor device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c> the <seealso cref="wclEInvalidArgument"/>
    ///   exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Sets the motion sensor mode. </summary>
    /// <param name="Mode"> The motion sensor mode. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclWeDoMotionSensorMode"/>
    function SetMode(const Mode: TwclWeDoMotionSensorMode): Integer;

    /// <summary> Gets the most recent count reading from the sensor. </summary>
    /// <value> The detections count. </value>
    property Count: Cardinal read GetCount;
    /// <summary> Gets the most recent distance reading from the
    ///   sensor. </summary>
    /// <value> The distance. </value>
    property Distance: Single read GetDistance;
    /// <summary> Gets the current mode of the motion sensor. </summary>
    /// <value> The sensor mode. </value>
    /// <seealso cref="TwclWeDoMotionSensorMode"/>
    property Mode: TwclWeDoMotionSensorMode read GetMode;

    /// <summary> The event fires when the counter has been changed. </summary>
    property OnCountChanged: TNotifyEvent read FOnCountChanged
      write FOnCountChanged;
    /// <summary> The event fires when distance has been changed. </summary>
    property OnDistanceChanged: TNotifyEvent read FOnDistanceChanged
      write FOnDistanceChanged;
    /// <summary> The event fires when the mode has been changed. </summary>
    property OnModeChanged: TNotifyEvent read FOnModeChanged
      write FOnModeChanged;
 end;

  /// <summary> The enumeration describes the Tilt sensor modes. </summary>
  TwclWeDoTiltSensorMode = (
    /// <summary> Angle. </summary>
    tmAngle = 0,
    /// <summary> Tilt. </summary>
    tmTilt = 1,
    /// <summary> Crash. </summary>
    tmCrash = 2,
    /// <summary> tmUnkown. </summary>
    tmUnknown = 255
  );

  /// <summary> The enumeration describes th Tilt sensor directions. </summary>
  TwclWeDoTiltSensorDirection = (
    /// <summary> Neutral. </summary>
    tdNeutral = 0,
    /// <summary> Backward. </summary>
    tdBackward = 3,
    /// <summary> Right. </summary>
    tdRight = 5,
    /// <summary> Left. </summary>
    tdLeft = 7,
    /// <summary> Forward. </summary>
    tdForward = 9,
    /// <summary> Unknown. </summary>
    tdUnknown = 255
  );

  /// <summary> The record represents the tilt sensor angle values. </summary>
  TwclWeDoTiltSensorAngle = record
    /// <summary>  The X value of tilt angle. </summary>
    X: Single;
    /// <summary> The Y value of tilt angle. </summary>
    Y: Single;
  end;

  /// <summary> The structure represents the Tilt sensor crash
  ///   values. </summary>
  TwclWeDoTiltSensorCrash = record
    /// <summary> The X value of crash. </summary>
    X: Single;
    /// <summary> The Y value of crash. </summary>
    Y: Single;
    /// <summary> The Z value of crash. </summary>
    Z: Single;
  end;

  /// <summary> The class represents a WeDo Titl Sensor device. </summary>
  /// <seealso cref="TwclWeDoResetableSensor"/>
  TwclWeDoTiltSensor = class(TwclWeDoResetableSensor)
  private
    FOnAngleChanged: TNotifyEvent;
    FOnCrashChanged: TNotifyEvent;
    FOnDirectionChanged: TNotifyEvent;
    FOnModeChanged: TNotifyEvent;

    function ConvertToSigned(const b: Byte): Integer;
    function ConvertToUnsigned(const b: Byte): Integer;

    function GetAngle: TwclWeDoTiltSensorAngle;
    function GetCrash: TwclWeDoTiltSensorCrash;
    function GetDirection: TwclWeDoTiltSensorDirection;
    function GetMode: TwclWeDoTiltSensorMode;

  protected
    /// <summary> The method called when Input Format has been
    ///   changed. </summary>
    /// <param name="OldFormat"> The old Input Format. </param>
    procedure InputFormatChanged(
      const OldFormat: TwclWeDoInputFormat); override;
    /// <summary> Fires the <c>OnVoltageChanged</c> event. </summary>
    procedure ValueChanged; override;

    /// <summary> Fires the <c>OnAngleChanged</c> event. </summary>
    procedure DoAngleChanged; virtual;
    /// <summary> Fires the <c>OnCrashChanged</c> event. </summary>
    procedure DoCrashChanged; virtual;
    /// <summary> Fires the <c>OnDirectionChanged</c> event. </summary>
    procedure DoDirectionChanged; virtual;
    /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
    procedure DoModeChanged; virtual;

  public
    /// <summary> Creates new tilt sensor device object. </summary>
    /// <param name="Hub"> The Hub object that owns the device. If this
    ///   parameter is <c>nil</c>
    ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
    /// <param name="ConnectionId"> The device's Connection ID. </param>
    /// <seealso cref="TwclWeDoHub"/>
    /// <exception cref="wclEInvalidArgument"> The exception raises when the
    ///   <c>Hub</c> parameter is <c>nil</c>. </exception>
    constructor Create(const Hub: TwclWeDoHub;
      const ConnectionId: Byte); override;

    /// <summary> Sets the tilt sensor mode. </summary>
    /// <param name="Mode"> The tils sensor mode. </param>
    /// <returns> If the method completed with success the returning value is
    ///   <see cref="WCL_E_SUCCESS" />. If the method failed the returning value
    ///   is one of the Bluetooth Framework error code. </returns>
    /// <seealso cref="TwclWeDoTiltSensorMode"/>
    function SetMode(const Mode: TwclWeDoTiltSensorMode): Integer;

    /// <summary> Gets the most recent angle reading from the sensor. The angle
    ///   represents the angle the sensor is tilted in the x and y. </summary>
    /// <value> The Tilt sensor angle. </value>
    /// <seealso cref="TwclWeDoTiltSensorAngle"/>
    property Angle: TwclWeDoTiltSensorAngle read GetAngle;
    /// <summary> Gets the most recent crash reading from the sensor.
    ///   The value represents the number of times the sensor has been �bumped�
    ///   in the x, y, and z.</summary>
    /// <value> The Tilt sensor crash. </value>
    /// <seealso cref="TwclWeDoTiltSensorCrash"/>
    property Crash: TwclWeDoTiltSensorCrash read GetCrash;
    /// <summary> Gets the most recent direction reading from the
    ///   sensor. </summary>
    /// <value> The Tilt sensor direction. </value>
    /// <seealso cref="TwclWeDoTiltSensorDirection"/>
    property Direction: TwclWeDoTiltSensorDirection read GetDirection;
    /// <summary> Gets the current mode of the tilt sensor. </summary>
    /// <value> The Tilt sensor mode. </value>
    /// <seealso cref="TwclWeDoTiltSensorMode"/>
    property Mode: TwclWeDoTiltSensorMode read GetMode;

    /// <summary> The event fires when angle has been changed. </summary>
    property OnAngleChanged: TNotifyEvent read FOnAngleChanged
      write FOnAngleChanged;
    /// <summary> The event fires when crash changed. </summary>
    property OnCrashChanged: TNotifyEvent read FOnCrashChanged
      write FOnCrashChanged;
    /// <summary> The event fires when direction has been changed. </summary>
    property OnDirectionChanged: TNotifyEvent read FOnDirectionChanged
      write FOnDirectionChanged;
    /// <summary> The event fires when Mode has been changed. </summary>
    property OnModeChanged: TNotifyEvent read FOnModeChanged
      write FOnModeChanged;
  end;

implementation

uses
  SysUtils, wclErrors, wclBluetoothErrors, wclUUIDs, wclConnectionErrors, Math,
  Windows;

const
  WCL_WEDO_GUID_NULL: TGUID = '{00000000-0000-0000-0000-000000000000}';

procedure SetNull(var Uuid: TwclGattUuid);
begin
  Uuid.IsShortUuid := False;
  Uuid.LongUuid := WCL_WEDO_GUID_NULL;
end;

function IsNull(const Uuid: TwclGattUuid): Boolean;
begin
  Result := (not Uuid.IsShortUuid) and (Uuid.LongUuid = WCL_WEDO_GUID_NULL);
end;

function ToList(const Arr: TArray<Byte>): TList<Byte>;
var
  i: Integer;
begin
  Result := TList<Byte>.Create;
  for i := 0 to Length(Arr) - 1 do
    Result.Add(Arr[i]);
end;

function ToArray(const List: TList<Byte>; const Start: Integer;
  const Length: Integer): TArray<Byte>;
var
  i: Integer;
begin
  if (List = nil) or (Start + Length > List.Count) then
    Result := nil

  else begin
    SetLength(Result, Length);
    for i := 0 to Length - 1 do
      Result[i] := List[Start + i];
  end;
end;

function VersionFromByteArray(const Data: TArray<Byte>): TwclWeDoVersion;
begin
  Result.MajorVersion := Data[0];
  Result.MinorVersion := Data[1];
  Result.BugFixVersion := Data[2];
  Result.BuildNumber := Data[3];
end;

{ TwclWeDoVersion }

function TwclWeDoVersion.ToString: string;
begin
  Result := MajorVersion.ToString + '.' + MinorVersion.ToString + '.' +
    BugFixVersion.ToString + '.' + BuildNumber.ToString;
end;

{ TwclWeDoDataFormat }

constructor TwclWeDoDataFormat.Create(const DataSetCount: Byte;
  const DataSetSize: Byte; const Mode: Byte;
  const Unit_: TwclWeDoSensorDataUnit);
begin
  FDataSetCount := DataSetCount;
  FDataSetSize := DataSetSize;
  FMode := Mode;
  FUnit := Unit_;
end;

function TwclWeDoDataFormat.Equals(Obj: TObject): Boolean;
var
  Format: TwclWeDoDataFormat;
begin
  if Obj = nil then
    Result := False

  else begin
    if Obj = Self then
      Result := True

    else begin
      if not (Obj is TwclWeDoDataFormat) then
        Result := False

      else begin
        Format := TwclWeDoDataFormat(Obj);
        Result := (FDataSetCount = Format.DataSetCount) and
          (FDataSetSize = Format.DataSetSize) and (FMode = Format.Mode) and
          (FUnit = Format.Unit_);
      end;
    end;
  end;
end;

{ TwclWeDoInputFormat }

constructor TwclWeDoInputFormat.Create(const ConnectionId: Byte;
  const DeviceType: TwclWeDoIoDeviceType; const Mode: Byte;
  const Interval: Cardinal; const Unit_: TwclWeDoSensorDataUnit;
  const NotificationsEnabled: Boolean; const Revision: Byte;
  const NumberOfBytes: Byte);
begin
  FConnectionId := ConnectionId;
  FInterval := Interval;
  FMode := Mode;
  FNotificationsEnabled := NotificationsEnabled;
  FDeviceType := DeviceType;
  FUnit := Unit_;
  FRevision := Revision;
  FNumberOfBytes := NumberOfBytes;
end;

function TwclWeDoInputFormat.Equals(Obj: TObject): Boolean;
var
  Format: TwclWeDoInputFormat;
begin
  if Obj = nil then
    Result := False

  else begin
    if Obj = Self then
      Result := True

    else begin
      if not (Obj is TwclWeDoInputFormat) then
        Result := False

      else begin
        Format := TwclWeDoInputFormat(Obj);
        Result := (FConnectionId = Format.ConnectionId) and
          (FInterval = Format.Interval) and (FMode = Format.Mode) and
          (FNotificationsEnabled = Format.NotificationsEnabled) and
          (FNumberOfBytes = Format.NumberOfBytes) and
          (FRevision = Format.Revision) and
          (FDeviceType = Format.DeviceType) and (FUnit = Format.Unit_);
      end;
    end;
  end;
end;

class function TwclWeDoInputFormat.FromBytesArray(
  const Data: TArray<Byte>): TwclWeDoInputFormat;
var
  Revision: Byte;
  ConnectionId: Byte;
  Mode: Byte;
  Interval: Cardinal;
  NotificationsEnabled: Boolean;
  NumberOfBytes: Byte;
  DeviceType: TwclWeDoIoDeviceType;
  Unit_: TwclWeDoSensorDataUnit;
begin
  if Length(Data) <> INPUT_FORMAT_PACKET_SIZE then
    Result := nil

  else begin
    Revision := Data[0];
    ConnectionId := Data[1];
    Mode := Data[3];
    Interval := PCardinal(@Data[4])^;
    NotificationsEnabled := Data[9] = 1;
    NumberOfBytes := Data[10];

    case Data[2] of
      WEDO_DEVICE_MOTOR:
        DeviceType := iodMotor;
      WEDO_DEVICE_VOLTAGE_SENSOR:
        DeviceType := iodVoltageSensor;
      WEDO_DEVICE_CURRENT_SENSOR:
        DeviceType := iodCurrentSensor;
      WEDO_DEVICE_PIEZO:
        DeviceType := iodPiezo;
      WEDO_DEVICE_RGB:
        DeviceType := iodRgb;
      WEDO_DEVICE_TILT_SENSOR:
        DeviceType := iodTiltSensor;
      WEDO_DEVICE_MOTION_SENSOR:
        DeviceType := iodMotionSensor;
      else
        DeviceType := iodUnknown;
    end;

    case Data[8] of
      WEDO_DATA_UNIT_RAW:
        Unit_ := suRaw;
      WEDO_DATA_UNIT_PERCENTAGE:
        Unit_ := suPercentage;
      WEDO_DATA_UNIT_SI:
        Unit_ := suSi;
      else
        Unit_ := suUnknown;
    end;

    Result := TwclWeDoInputFormat.Create(ConnectionId, DeviceType, Mode,
      Interval, Unit_, NotificationsEnabled, Revision, NumberOfBytes);
  end;
end;

function TwclWeDoInputFormat.InputFormatBySettingMode(
  const Mode: Byte): TwclWeDoInputFormat;
begin
  Result := TwclWeDoInputFormat.Create(FConnectionId, FDeviceType, Mode,
    FInterval, FUnit, FNotificationsEnabled, FRevision, FNumberOfBytes);
end;

function TwclWeDoInputFormat.ToBytesArray: TArray<Byte>;
begin
  SetLength(Result, 8);
  case FDeviceType of
    iodMotor:
      Result[0] := WEDO_DEVICE_MOTOR;
    iodVoltageSensor:
      Result[0] := WEDO_DEVICE_VOLTAGE_SENSOR;
    iodCurrentSensor:
      Result[0] := WEDO_DEVICE_CURRENT_SENSOR;
    iodPiezo:
      Result[0] := WEDO_DEVICE_PIEZO;
    iodRgb:
      Result[0] := WEDO_DEVICE_RGB;
    iodTiltSensor:
      Result[0] := WEDO_DEVICE_TILT_SENSOR;
    iodMotionSensor:
      Result[0] := WEDO_DEVICE_MOTION_SENSOR;
    else
      Result[0] := WEDO_DEVICE_UNKNOWN;
  end;
  Result[1] := FMode;
  PCardinal(@Result[2])^ := FInterval;
  case FUnit of
    suRaw:
      Result[6] := WEDO_DATA_UNIT_RAW;
    suPercentage:
      Result[6] := WEDO_DATA_UNIT_PERCENTAGE;
    suSi:
      Result[6] := WEDO_DATA_UNIT_SI;
    else
      Result[6] := WEDO_DATA_UNIT_UNKNOWN;
  end;
  if FNotificationsEnabled then
    Result[7] := 1
  else
    Result[7] := 0;
end;

{ TwclWeDoService }

procedure TwclWeDoService.CharacteristicChanged(const Handle: Word;
  const Value: TArray<Byte>);
begin
  // Do nothing in default implementation.
end;

function TwclWeDoService.CompareGuid(const GattUuid: TwclGattUuid;
  const Uuid: TGUID): Boolean;
begin
  Result := ToGuid(GattUuid) = Uuid;
end;

function TwclWeDoService.Connect(const Services: TwclGattServices): Integer;
begin
  if FConnected then
    Result := WCL_E_CONNECTION_ACTIVE

  else begin
    if Length(Services) = 0 then
      Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

    else begin
      FServices := Services;
      Result := Initialize;
      if Result = WCL_E_SUCCESS then
        FConnected := True;
    end;
  end;
end;

constructor TwclWeDoService.Create(const Client: TwclGattClient;
  const Hub: TwclWeDoHub);
begin
  if (Client = nil) or (Hub = nil) then
    raise wclEInvalidArgument.Create('Client parameter can not be null.');

  FCharacteristics := nil;
  FClient := Client;
  FConnected := False;
  FHub := Hub;
  FServices := nil;
end;

function TwclWeDoService.Disconnect: Integer;
begin
  if not FConnected then
    Result := WCL_E_CONNECTION_NOT_ACTIVE

  else begin
    Uninitialize;

    FCharacteristics := nil;
    FConnected := False;
    FServices := nil;

    Result := WCL_E_SUCCESS;
  end;
end;

function TwclWeDoService.FindCharactersitc(const Uuid: TGUID;
  const Service: TwclGattService;
  out Characteristic: TwclGattCharacteristic): Integer;
var
  Chr: TwclGattCharacteristic;
begin
  SetNull(Characteristic.Uuid);

  if IsNull(Service.Uuid) then
    Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

  else begin
    Result := ReadCharacteristics(Service);
    if Result = WCL_E_SUCCESS then begin
      Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

      for Chr in FCharacteristics do begin
        if CompareGuid(Chr.Uuid, Uuid) then begin
          Characteristic := Chr;
          Result := WCL_E_SUCCESS;
          Break;
        end;
      end;
    end;
  end;
end;

function TwclWeDoService.FindService(const Uuid: TGUID;
  out Service: TwclGattService): Integer;
var
  Svc: TwclGattService;
begin
  SetNull(Service.Uuid);

  if Length(FServices) = 0 then
    Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

  else begin
    Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;

    for Svc in FServices do begin
      if CompareGuid(Svc.Uuid, Uuid) then begin
        Service := Svc;
        Result := WCL_E_SUCCESS;
        Break;
      end;
    end;
  end;
end;

function TwclWeDoService.ReadByteValue(
  const Characteristic: TwclGattCharacteristic; out Value: Byte): Integer;
var
  CharValue: TwclGattCharacteristicValue;
begin
  Value := 0;

  if not FConnected then
    Result := WCL_E_CONNECTION_NOT_ACTIVE

  else begin
    if IsNull(Characteristic.Uuid) then
      Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

    else begin
      Result := FClient.ReadCharacteristicValue(Characteristic, goNone,
        CharValue);
      if Result = WCL_E_SUCCESS then begin
        if Length(CharValue) > 0 then
          Value := CharValue[0];
      end;
    end;
  end;
end;

function TwclWeDoService.ReadCharacteristics(
  const Service: TwclGattService): Integer;
begin
  // Did we already read the characteristics for given service?
  if Length(FCharacteristics) <> 0 then
    Result := WCL_E_SUCCESS

  else begin
    Result := FClient.ReadCharacteristics(Service, goNone, FCharacteristics);
    if Result = WCL_E_SUCCESS then begin
      if Length(FCharacteristics) = 0 then
        Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND;
    end;
  end;
end;

function TwclWeDoService.ReadStringValue(
  const Characteristic: TwclGattCharacteristic; out Value: string): Integer;
var
  CharValue: TwclGattCharacteristicValue;
begin
  Value := '';

  if not FConnected then
    Result := WCL_E_CONNECTION_NOT_ACTIVE

  else begin
    if IsNull(Characteristic.Uuid) then
      Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

    else begin
      Result := FClient.ReadCharacteristicValue(Characteristic, goNone,
        CharValue);
      if Result = WCL_E_SUCCESS then begin
        if Length(CharValue) > 0 then
          Value := UTF8ToWideString(RawByteString(CharValue));
      end;
    end;
  end;
end;

function TwclWeDoService.SubscribeForNotifications(
  Characteristic: TwclGattCharacteristic): Integer;
begin
  if IsNull(Characteristic.Uuid) then
    Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

  else begin
    // Windows does not support dual indicatable and notifiable chars so select
    // one.
    if Characteristic.IsIndicatable and Characteristic.IsNotifiable then
      Characteristic.IsIndicatable := False;

    Result := FClient.Subscribe(Characteristic);
    if Result <> WCL_E_SUCCESS then begin
      Result := FClient.WriteClientConfiguration(Characteristic, True, goNone);
      if Result <> WCL_E_SUCCESS then
        FClient.Unsubscribe(Characteristic);
    end;
  end;
end;

function TwclWeDoService.ToGuid(const Uuid: TwclGattUuid): TGUID;
begin
  if not Uuid.IsShortUuid then
    Result := Uuid.LongUuid
  else begin
    Result := Bluetooth_Base_UUID;
    Result.D1 := Uuid.ShortUuid;
  end;
end;

procedure TwclWeDoService.UnsubscribeFromNotifications(
  Characteristic: TwclGattCharacteristic);
begin
  if (FClient.State = csConnected) and (not IsNull(Characteristic.Uuid)) then
  begin
    if Characteristic.IsIndicatable and Characteristic.IsNotifiable then
      Characteristic.IsIndicatable := False;

    FClient.WriteClientConfiguration(Characteristic, False, goNone);
    FClient.Unsubscribe(Characteristic);
  end;
end;

{ TwclWeDoDeviceInformationService }

constructor TwclWeDoDeviceInformationService.Create(
  const Client: TwclGattClient; const Hub: TwclWeDoHub);
begin
  inherited Create(Client, Hub);

  Uninitialize;
end;

function TwclWeDoDeviceInformationService.Initialize: Integer;
var
  Service: TwclGattService;
begin
  // Find Device Information service and its characteristics.
  Result := FindService(WEDO_SERVICE_DEVICE_INFORMATION, Service);
  if Result = WCL_E_SUCCESS then begin
    // These characteristics are not important so we can ignore errors if some was
    // not found.
    FindCharactersitc(WEDO_CHARACTERISTIC_FIRMWARE_REVISION, Service,
      FFirmwareVersionChar);
    FindCharactersitc(WEDO_CHARACTERISTIC_HARDWARE_REVISION, Service,
      FHardwareVersionChar);
    FindCharactersitc(WEDO_CHARACTERISTIC_SOFTWARE_REVISION, Service,
      FSoftwareVersionChar);
    FindCharactersitc(WEDO_CHARACTERISTIC_MANUFACTURER_NAME, Service,
      FManufacturerNameChar);
  end;
end;

function TwclWeDoDeviceInformationService.ReadFirmwareVersion(
  out Version: string): Integer;
begin
  Result := ReadStringValue(FFirmwareVersionChar, Version);
end;

function TwclWeDoDeviceInformationService.ReadHardwareVersion(
  out Version: string): Integer;
begin
  Result := ReadStringValue(FHardwareVersionChar, Version);
end;

function TwclWeDoDeviceInformationService.ReadManufacturerName(
  out Name: string): Integer;
begin
  Result := ReadStringValue(FManufacturerNameChar, Name);
end;

function TwclWeDoDeviceInformationService.ReadSoftwareVersion(
  out Version: string): Integer;
begin
  Result := ReadStringValue(FSoftwareVersionChar, Version);
end;

procedure TwclWeDoDeviceInformationService.Uninitialize;
begin
  SetNull(FFirmwareVersionChar.Uuid);
  SetNull(FHardwareVersionChar.Uuid);
  SetNull(FSoftwareVersionChar.Uuid);
  SetNull(FManufacturerNameChar.Uuid);
end;

{ TwclWeDoBatteryLevelService }

procedure TwclWeDoBatteryLevelService.CharacteristicChanged(const Handle: Word;
  const Value: TArray<Byte>);
begin
  inherited;

  // We have to process battery level changes notifications here.
  if Handle = FBatteryLevelChar.Handle then
    DoBatteryLevelChanged(Value[0]);
end;

constructor TwclWeDoBatteryLevelService.Create(const Client: TwclGattClient;
  const Hub: TwclWeDoHub);
begin
  inherited Create(Client, Hub);

  Uninitialize;

  FOnBatteryLevelChanged := nil;
end;

procedure TwclWeDoBatteryLevelService.DoBatteryLevelChanged(const Level: Byte);
begin
  if Assigned(FOnBatteryLevelChanged) then
    FOnBatteryLevelChanged(Self, Level);
end;

function TwclWeDoBatteryLevelService.Initialize: Integer;
var
  Service: TwclGattService;
begin
  // Find Battery Level service and its characteristics.
  Result := FindService(WEDO_SERVICE_BATTERY_LEVEL, Service);
  if Result = WCL_E_SUCCESS then begin
    Result := FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_LEVEL, Service,
      FBatteryLevelChar);
    if Result = WCL_E_SUCCESS then begin
      Result := SubscribeForNotifications(FBatteryLevelChar);
      if Result <> WCL_E_SUCCESS then
        Uninitialize;
    end;
  end;
end;

function TwclWeDoBatteryLevelService.ReadBatteryLevel(out Level: Byte): Integer;
begin
  Result := ReadByteValue(FBatteryLevelChar, Level);
end;

procedure TwclWeDoBatteryLevelService.Uninitialize;
begin
  if Connected then
    UnsubscribeFromNotifications(FBatteryLevelChar);

  SetNull(FBatteryLevelChar.Uuid);
end;

{ TwclWeDoIoService }

procedure TwclWeDoIoService.CharacteristicChanged(const Handle: Word;
  const Value: TArray<Byte>);
begin
  inherited;

  if (not IsNull(FSensorValueChar.Uuid)) and
     (FSensorValueChar.Handle = Handle)
  then
    InputValueChanged(Value);
  if (not IsNull(FSensorValueFormatChar.Uuid)) and
     (FSensorValueFormatChar.Handle = Handle)
  then
    InputFormatChanged(Value);
end;

procedure TwclWeDoIoService.ClearInputFormats;
var
  Format: TwclWeDoInputFormat;
begin
  for Format in FInputFormats.Values do
    Format.Free;
  FInputFormats.Clear;
end;

function TwclWeDoIoService.ComposeInputCommand(const CommandId: Byte;
  const CommandType: Byte; const ConnectionId: Byte;
  const Data: TArray<Byte>): TArray<Byte>;
var
  i: Integer;
begin
  if Length(Data) > 255 then
    Result := nil

  else begin
    SetLength(Result, IN_CMD_HDR_SIZE + Length(Data));
    Result[0] := CommandId;
    Result[1] := CommandType;
    Result[2] := ConnectionId;
    for i := 0 to Length(Data) - 1 do
      Result[IN_CMD_HDR_SIZE + i] := Data[i];
  end;
end;

function TwclWeDoIoService.ComposeOutputCommand(const CommandId: Byte;
  const ConnectionId: Byte; const Data: TArray<Byte>): TArray<Byte>;
var
  i: Integer;
begin
  if Length(Data) > 255 then
    Result := nil

  else begin
    SetLength(Result, OUT_CMD_HDR_SIZE + Length(Data));
    Result[0] := ConnectionId;
    Result[1] := CommandId;
    Result[2] := Length(Data);
    for i := 0 to Length(Data) - 1 do
      Result [OUT_CMD_HDR_SIZE + i] := Data[i];
  end;
end;

constructor TwclWeDoIoService.Create(const Client: TwclGattClient;
  const Hub: TwclWeDoHub);
begin
  inherited Create(Client, Hub);

  // Create input formats lists first.
  FInputFormats := TDictionary<Byte, TwclWeDoInputFormat>.Create;
  FMissingInputFormats := TDictionary<Byte, Boolean>.Create;

  Uninitialize;
end;

destructor TwclWeDoIoService.Destroy;
begin
  ClearInputFormats;

  FInputFormats.Free;
  FMissingInputFormats.Free;

  inherited;
end;

function TwclWeDoIoService.Initialize: Integer;
var
  Service: TwclGattService;
begin
  // Find Battery Level service and its characteristics.
  Result := FindService(WEDO_SERVICE_IO, Service);
  if Result = WCL_E_SUCCESS then begin
    // All the IO characteristics are important!
    Result := FindCharactersitc(WEDO_CHARACTERISTIC_SENSOR_VALUE, Service, FSensorValueChar);
    if Result = WCL_E_SUCCESS then
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_SENSOR_VALUE_FORMAT, Service, FSensorValueFormatChar);
    if Result = WCL_E_SUCCESS then
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_INPUT_COMMAND, Service, FInputCommandChar);
    if Result = WCL_E_SUCCESS then
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_OUTPUT_COMMAND, Service, FOutputCommandChar);
    if Result = WCL_E_SUCCESS then begin
      Result := SubscribeForNotifications(FSensorValueChar);
      if Result = WCL_E_SUCCESS then begin
        Result := SubscribeForNotifications(FSensorValueFormatChar);
        if Result <> WCL_E_SUCCESS then
          UnsubscribeFromNotifications(FSensorValueChar);
      end;
    end;

    if Result <> WCL_E_SUCCESS then
      Uninitialize;
  end;
end;

procedure TwclWeDoIoService.InputFormatChanged(const Data: TArray<Byte>);
var
  Format: TwclWeDoInputFormat;
  AnyFormat: TwclWeDoInputFormat;
  Io: TwclWeDoIo;
begin
  Format := TwclWeDoInputFormat.FromBytesArray(Data);
  if Format <> nil then begin
    // If we already have input format with an earlier revision, delete all
    // those as all known formats must have the same version.
    if FInputFormats.Values.Count > 0 then
      AnyFormat := FInputFormats[0]
    else
      AnyFormat := nil;
    // Clear if revisions are not equal.
    if (AnyFormat <> nil) and (AnyFormat.Revision <> Format.Revision) then
      ClearInputFormats;

    // Update input formats in local list.
    if FInputFormats.ContainsKey(Format.ConnectionId) then begin
      FInputFormats[Format.ConnectionId].Free;
      FInputFormats[Format.ConnectionId] := Format;
    end else
      FInputFormats.Add(Format.ConnectionId, Format);

    // Check for missing Input Formats.
    if FMissingInputFormats.ContainsKey(Format.ConnectionId) then
      FMissingInputFormats[Format.ConnectionId] := False
    else
      FMissingInputFormats.Add(Format.ConnectionId, False);

    // Update Input format for IOs.
    for Io in Hub.IoDevices do begin
      if Io.ConnectionId = Format.ConnectionId then
        Io.UpdateInputFormat(Format);
    end;
  end;
end;

procedure TwclWeDoIoService.InputValueChanged(const Value: TArray<Byte>);
var
  Revision: Byte;
  Index: Integer;
  IdToValue: TDictionary<Byte, TArray<Byte>>;
  List: TList<Byte>;
  ConnectionId: Byte;
  Format: TwclWeDoInputFormat;
  Data: TArray<Byte>;
  Io: TwclWeDoIo;
begin
  if Length(Value) > 0 then begin
    Revision := Value[0];
    Index := 1;
    IdToValue := TDictionary<Byte, TArray<Byte>>.Create;
    try
      List := ToList(Value);
      try
        // Iterate over values in byte array until byte array is empty.
        while Index < Length(Value) do begin
          ConnectionId := List[Index];

          // If value has Connection ID that is not known by the system -
          // ignore.
          if not FInputFormats.TryGetValue(ConnectionId, Format) then begin
            RequestMissingInputFormat(ConnectionId);
            Exit;
          end;

          // If no input format is available for this Connection ID - ignore.
          if Format = nil then
            Exit;

          // If the revision from the input value is different than the revision
          // from the input format - ignore.
          if Format.Revision <> Revision then
            Exit;

          // Read data value.
          Inc(Index);
          Data := ToArray(List, Index, Format.NumberOfBytes);
          Inc(Index, Format.NumberOfBytes);
          IdToValue.Add(ConnectionId, Data);
        end;

      finally
        List.Free;
      end;

      // Update value son devices.
      for Io in Hub.IoDevices do begin
        if IdToValue.TryGetValue(Io.ConnectionId, Data) then
          Io.UpdateValue(Data);
      end;

    finally
      IdToValue.Free;
    end;
  end;
end;

function TwclWeDoIoService.PiezoPlayTone(const Frequency: Word;
  const Duration: Word; const ConnectionId: Byte): Integer;
var
  Data: TArray<Byte>;
  Cmd: TArray<Byte>;
begin
  SetLength(Data, 4);
  PWord(@Data[0])^ := Frequency;
  PWord(@Data[2])^ := Duration;
  Cmd := ComposeOutputCommand(OUT_CMD_ID_PIEZO_PLAY_TONE, ConnectionId, Data);
  Result := WriteOutputCommand(Cmd);
end;

function TwclWeDoIoService.PiezoStopPlaying(const ConnectionId: Byte): Integer;
var
  Cmd: TArray<Byte>;
begin
  Cmd := ComposeOutputCommand(OUT_CMD_ID_PIEZO_STOP, ConnectionId, nil);
  Result := WriteOutputCommand(Cmd);
end;

function TwclWeDoIoService.ReadInputFormat(const ConnectionId: Byte): Integer;
var
  Cmd: TArray<Byte>;
begin
  Cmd := ComposeInputCommand(IN_CMD_ID_INPUT_FORMAT, IN_CMD_TYPE_READ,
    ConnectionId, nil);
  Result := WriteInputCommand(Cmd);
end;

function TwclWeDoIoService.ReadValue(const ConnectionId: Byte): Integer;
var
  Cmd: TArray<Byte>;
begin
  Cmd := ComposeInputCommand(IN_CMD_ID_INPUT_VALUE, IN_CMD_TYPE_READ,
    ConnectionId, nil);
  Result := WriteInputCommand(Cmd);
end;

procedure TwclWeDoIoService.RequestMissingInputFormat(const ConnectionId: Byte);
var
  InputFormatRequested: Boolean;
begin
  if not FMissingInputFormats.ContainsKey(ConnectionId) then
    FMissingInputFormats.Add(ConnectionId, False);

  // Have we already requested for missing Input Format?
  InputFormatRequested := FMissingInputFormats[ConnectionId];
  if not InputFormatRequested then begin
    // If no - do it right now.
    if ReadInputFormat(ConnectionId) = WCL_E_SUCCESS then
      InputFormatRequested := True;
  end;

  // Change Input Format request flag.
  FMissingInputFormats[ConnectionId] := InputFormatRequested;
end;

function TwclWeDoIoService.ResetIo(const ConnectionId: Byte): Integer;
var
  Cmd: TArray<Byte>;
begin
  SetLength(Cmd, 3);
  Cmd[0] := 68;
  Cmd[1] := 17;
  Cmd[2] := 170;
  Result := WriteData(Cmd, ConnectionId);
end;

procedure TwclWeDoIoService.Uninitialize;
begin
  if Connected then begin
    // Unsubscribe from all characteristics.
    UnsubscribeFromNotifications(FSensorValueChar);
    UnsubscribeFromNotifications(FSensorValueFormatChar);
  end;

  // Clear characteristics.
  SetNull(FSensorValueChar.Uuid);
  SetNull(FSensorValueFormatChar.Uuid);
  SetNull(FInputCommandChar.Uuid);
  SetNull(FOutputCommandChar.Uuid);

  // Clear input format lists.
  ClearInputFormats;
  FMissingInputFormats.Clear;
end;

function TwclWeDoIoService.WriteColor(const Red: Byte; const Green: Byte;
  const Blue: Byte; const ConnectionId: Byte): Integer;
var
  Data: TArray<Byte>;
  Cmd: TArray<Byte>;
begin
  SetLength(Data, 3);
  Data[0] := Red;
  Data[1] := Green;
  Data[2] := Blue;
  Cmd := ComposeOutputCommand(OUT_CMD_ID_RGB_CONTROL, ConnectionId, Data);
  Result := WriteOutputCommand(Cmd);
end;

function TwclWeDoIoService.WriteColorIndex(const Index: Byte;
  const ConnectionId: Byte): Integer;
var
  Data: TArray<Byte>;
  Cmd: TArray<Byte>;
begin
  if Index > 10 then
    Result := WCL_E_INVALID_ARGUMENT

  else begin
    SetLength(Data, 1);
    Data[0] := Index;
    Cmd := ComposeOutputCommand(OUT_CMD_ID_RGB_CONTROL, ConnectionId, Data);
    Result := WriteOutputCommand(Cmd);
  end;
end;

function TwclWeDoIoService.WriteData(const Data: TArray<Byte>;
  const ConnectionId: Byte): Integer;
var
  Cmd: TArray<Byte>;
begin
  Cmd := ComposeOutputCommand(OUT_CMD_ID_DIRECT_WRITE, ConnectionId, Data);
  Result := WriteOutputCommand(Cmd);
end;

function TwclWeDoIoService.WriteInputCommand(
  const Command: TArray<Byte>): Integer;
begin
  if IsNull(FInputCommandChar.Uuid) then
    Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND
  else begin
    Result := Client.WriteCharacteristicValue(FInputCommandChar,
      TwclGattCharacteristicValue(Command));
  end;
end;

function TwclWeDoIoService.WriteInputFormat(const Format: TwclWeDoInputFormat;
  const ConnectionId: Byte): Integer;
var
  Cmd: TArray<Byte>;
begin
  Cmd := ComposeInputCommand(IN_CMD_ID_INPUT_FORMAT, IN_CMD_TYPE_WRITE,
    ConnectionId, Format.ToBytesArray);
  Result := WriteInputCommand(Cmd);
end;

function TwclWeDoIoService.WriteMotorPower(const Power: ShortInt;
  const ConnectionId: Byte): Integer;
begin
  Result := WriteMotorPower(Power, 0, ConnectionId);
end;

function TwclWeDoIoService.WriteMotorPower(Power: ShortInt;
  const Offset: Byte; const ConnectionId: Byte): Integer;
var
  Positive: Boolean;
  ActualPower: Single;
  Value: Byte;
  Data: TArray<Byte>;
  Cmd: TArray<Byte>;
begin
  Positive := Power >= 0;
  Power := Abs(Power);

  ActualPower := ((100.0 - Offset) / 100.0) * Power + Offset;
  Value := Round(ActualPower);

  if not Positive then
    Value := ($FF - Value) + 1;

  SetLength(Data, 1);
  Data[0] := Value;
  Cmd := ComposeOutputCommand(OUT_CMD_ID_MOTOR_POWER_CONTROL, ConnectionId,
    Data);
  Result := WriteOutputCommand(Cmd);
end;

function TwclWeDoIoService.WriteOutputCommand(
  const Command: TArray<Byte>): Integer;
begin
  // Writes command to the IO service.
  if IsNull(FOutputCommandChar.Uuid) then
    Result := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND
  else begin
    Result := Client.WriteCharacteristicValue(FOutputCommandChar,
      TwclGattCharacteristicValue(Command));
  end;
end;

{ TwclWeDoHubService }

procedure TwclWeDoHubService.CharacteristicChanged(const Handle: Word;
  const Value: TArray<Byte>);
var
  Io: TwclWeDoIo;
begin
  inherited;

  // Process data only if it presents.
  if Length(Value) > 0 then begin
    // Button pressed?
    if (not IsNull(FButtonStateChar.Uuid)) and (Handle = FButtonStateChar.Handle) then
      DoButtonStateChanged(Value[0] = 1);
    // Low voltage?
    if (not IsNull(FLowVoltageAlertChar.Uuid)) and (Handle = FLowVoltageAlertChar.Handle) then
      DoLowVoltageAlert(Value[0] = 1);
    // High current!
    if (not IsNull(FHighCurrentAleartChar.Uuid)) and (Handle = FHighCurrentAleartChar.Handle) then
      DoHightCurrentAlert(Value[0] = 1);
    // Low signal
    if (not IsNull(FLowSignalChar.Uuid)) and (Handle = FLowSignalChar.Handle) then
      DoLowSignalAlert(Value[0] = 1);
    // IO attached/detached
    if (not IsNull(FIoAttachedChar.Uuid)) and (Handle = FIoAttachedChar.Handle) then begin
      if Length(Value) >= 2 then begin
        if Value[1] = 1 then begin
          // Attached
          Io := TwclWeDoIo.Attach(Hub, Value);
          if Io <> nil then
            DoDeviceAttached(Io);

        end else
          // Detached.
          DoDeviceDetached(Value[0]);
      end;
    end;
  end;
end;

constructor TwclWeDoHubService.Create(const Client: TwclGattClient;
  const Hub: TwclWeDoHub);
begin
  inherited Create(Client, Hub);

  FOnButtonStateChanged := nil;
  FOnLowVoltageAlert := nil;
  FOnDeviceAttached := nil;
  FOnDeviceDetached := nil;
  FOnHighCurrentAlert := nil;
  FOnLowSignalAlert := nil;

  Uninitialize;
end;

procedure TwclWeDoHubService.DoButtonStateChanged(const Pressed: Boolean);
begin
  if Assigned(FOnButtonStateChanged) then
    FOnButtonStateChanged(Self, Pressed);
end;

procedure TwclWeDoHubService.DoDeviceAttached(const Device: TwclWeDoIo);
begin
  if Assigned(FOnDeviceAttached) then
    FOnDeviceAttached(Self, Device);
end;

procedure TwclWeDoHubService.DoDeviceDetached(const ConnectionId: Byte);
begin
  if Assigned(FOnDeviceDetached) then
    FOnDeviceDetached(Self, ConnectionId);
end;

procedure TwclWeDoHubService.DoHightCurrentAlert(const Alert: Boolean);
begin
  if Assigned(FOnHighCurrentAlert) then
    FOnHighCurrentAlert(Self, Alert);
end;

procedure TwclWeDoHubService.DoLowSignalAlert(const Alert: Boolean);
begin
  if Assigned(FOnLowSignalAlert) then
    FOnLowSignalAlert(Self, Alert);
end;

procedure TwclWeDoHubService.DoLowVoltageAlert(const Alert: Boolean);
begin
  if Assigned(FOnLowVoltageAlert) then
    FOnLowVoltageAlert(Self, Alert);
end;

function TwclWeDoHubService.Initialize: Integer;
var
  Service: TwclGattService;
begin
  // Find Battery Level service and its characteristics.
  Result := FindService(WEDO_SERVICE_HUB, Service);
  if Result = WCL_E_SUCCESS then begin
    // The following characteristics are important so we have to check if they are present.
    Result := FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_NAME, Service,
      FDeviceNameChar);
    if Result = WCL_E_SUCCESS then begin
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_BUTTON_STATE, Service,
        FButtonStateChar);
    end;
    if Result = WCL_E_SUCCESS then begin
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_IO_ATTACHED, Service,
        FIoAttachedChar);
    end;
    if Result = WCL_E_SUCCESS then begin
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_LOW_VOLTAGE_ALERT,
        Service, FLowVoltageAlertChar);
    end;
    if Result = WCL_E_SUCCESS then begin
      Result := FindCharactersitc(WEDO_CHARACTERISTIC_TURN_OFF, Service,
        FTurnOffChar);
    end;

    // Subscribe for mandatory chars.
    if Result = WCL_E_SUCCESS then begin
      Result := SubscribeForNotifications(FButtonStateChar);
      if Result = WCL_E_SUCCESS then begin
        Result := SubscribeForNotifications(FIoAttachedChar);
        if Result = WCL_E_SUCCESS then begin
          Result := SubscribeForNotifications(FLowVoltageAlertChar);
          if Result <> WCL_E_SUCCESS then
            UnsubscribeFromNotifications(FIoAttachedChar);
        end;
        if Result <> WCL_E_SUCCESS then
          UnsubscribeFromNotifications(FButtonStateChar);
      end;
    end;

    // The following characterisitcs may not be available so we ignore any
    // errors. However we must check that previous characteristics (which are
    // important) have been found.
    if Result = WCL_E_SUCCESS then begin
      FindCharactersitc(WEDO_CHARACTERISTIC_HIGH_CURRENT_ALERT, Service,
        FHighCurrentAleartChar);
      FindCharactersitc(WEDO_CHARACTERISTIC_LOW_SIGNAL_ALERT, Service,
        FLowSignalChar);
      FindCharactersitc(WEDO_CHARACTERISTIC_VCC_PORT_CONTROL, Service,
        FVccPortChar);
      FindCharactersitc(WEDO_CHARACTERISTIC_BATTERY_TYPE, Service,
        FBatteryTypeChar);
      FindCharactersitc(WEDO_CHARACTERISTIC_DEVICE_DISCONNECT, Service,
        FDeviceDisconnectChar);

      // Subscribe for optional chars.
      if not Isnull(FHighCurrentAleartChar.Uuid) then begin
        Result := SubscribeForNotifications(FHighCurrentAleartChar);
        if (Result = WCL_E_SUCCESS) and (not IsNull(FLowSignalChar.Uuid)) then
        begin
          Result := SubscribeForNotifications(FLowSignalChar);
          if (Result <> WCL_E_SUCCESS) and (not IsNull(FHighCurrentAleartChar.Uuid)) then
            UnsubscribeFromNotifications(FHighCurrentAleartChar);
        end;
      end;

      if Result <> WCL_E_SUCCESS then begin
        UnsubscribeFromNotifications(FButtonStateChar);
        UnsubscribeFromNotifications(FIoAttachedChar);
        UnsubscribeFromNotifications(FLowVoltageAlertChar);

        Uninitialize;
      end;
    end;
  end;
end;

function TwclWeDoHubService.ReadDeviceName(out Name: string): Integer;
begin
  Result := ReadStringValue(FDeviceNameChar, Name);
end;

function TwclWeDoHubService.TurnOff: Integer;
var
  Value: TArray<Byte>;
begin
  if not Connected then
    Result := WCL_E_CONNECTION_NOT_ACTIVE

  else begin
    SetLength(Value, 1);
    Value[0] := $01;
    Result := Client.WriteCharacteristicValue(FTurnOffChar,
      TwclGattCharacteristicValue(Value));
  end;
end;

procedure TwclWeDoHubService.Uninitialize;
begin
  if Connected then begin
    // Mandatory characteristics.
    UnsubscribeFromNotifications(FButtonStateChar);
    UnsubscribeFromNotifications(FIoAttachedChar);
    UnsubscribeFromNotifications(FLowVoltageAlertChar);

    // Optional characteristics.
    if not IsNull(FHighCurrentAleartChar.Uuid) then
      UnsubscribeFromNotifications(FHighCurrentAleartChar);
    if not IsNull(FLowSignalChar.Uuid) then
      UnsubscribeFromNotifications(FLowSignalChar);
  end;

  SetNull(FDeviceNameChar.Uuid);
  SetNull(FButtonStateChar.Uuid);
  SetNull(FIoAttachedChar.Uuid);
  SetNull(FLowVoltageAlertChar.Uuid);
  SetNull(FHighCurrentAleartChar.Uuid);
  SetNull(FLowSignalChar.Uuid);
  SetNull(FTurnOffChar.Uuid);
  SetNull(FVccPortChar.Uuid);
  SetNull(FBatteryTypeChar.Uuid);
  SetNull(FDeviceDisconnectChar.Uuid);
end;

function TwclWeDoHubService.WriteDeviceName(Name: string): Integer;
var
  Bytes: RawByteString;
  CharVal: TArray<Byte>;
  i: Integer;
begin
  if Name = '' then
    Result := WCL_E_INVALID_ARGUMENT

  else begin
    if not Connected then
      Result := WCL_E_CONNECTION_NOT_ACTIVE

    else begin
      if Length(Name) > 20 then
        Name := Copy(Name, 1, 20);
      Bytes := UTF8Encode(Name);
      if Length(Bytes) < 20 then begin
        SetLength(CharVal, 20);
        for i := 0 to Length(Bytes) - 1 do
          CharVal[i] := Byte(Bytes[i]);
        for i := Length(Bytes) to 19 do
          CharVal[i] := 0;
      end else
        CharVal := TArray<Byte>(Bytes);

      Result := Client.WriteCharacteristicValue(FDeviceNameChar,
        TwclGattCharacteristicValue(CharVal));
    end;
  end;
end;

{ TwclWeDoHub }

procedure TwclWeDoHub.ClientCharacteristicChanged(Sender: TObject;
  const Handle: Word; const Value: TwclGattCharacteristicValue);
begin
  // Notify all services about characteristic changes. So each one can select
  // owm updated data.
  if FDeviceInformation.Connected then
    FDeviceInformation.CharacteristicChanged(Handle, TArray<Byte>(Value));
  if FBatteryLevel.Connected then
    FBatteryLevel.CharacteristicChanged(Handle, TArray<Byte>(Value));
  if FIo.Connected then
    FIo.CharacteristicChanged(Handle, TArray<Byte>(Value));
  if FHub.Connected then
    FHub.CharacteristicChanged(Handle, TArray<Byte>(Value));
end;

procedure TwclWeDoHub.ClientConnect(Sender: TObject; const Error: Integer);
var
  Services: TwclGattServices;
  Res: Integer;
begin
  // If connection was not established simple fire the event with error code.
  if Error <> WCL_E_SUCCESS then
    DoConnected(Error)

  else begin
    FHubConnected := True;

    // Read services. We do it only once to save connection time.
    Res := FClient.ReadServices(goNone, Services);
    if Res = WCL_E_SUCCESS then begin
      if Length(Services) = 0 then
        Res := WCL_E_BLUETOOTH_LE_ATTRIBUTE_NOT_FOUND

      else begin
        // Try to connect to WeDo services.
        Res := FDeviceInformation.Connect(Services);
        if Res = WCL_E_SUCCESS then
          Res := FBatteryLevel.Connect(Services);
        if Res = WCL_E_SUCCESS then
          Res := FIo.Connect(Services);
        if Res = WCL_E_SUCCESS then
          Res := FHub.Connect(Services);

        Services := nil;
      end;
    end;

    // If all the operations were success (services found, characteristics are
    // found too) we have to set connection flag.
    if Res = WCL_E_SUCCESS then
      FConnected := True
    else
      // Otherwise we have to disconnect!
      FClient.Disconnect;

    // Now we can fire the OnConnected event.
    DoConnected(Res);
  end;
end;

procedure TwclWeDoHub.ClientDisconnect(Sender: TObject; const Reason: Integer);
begin
  DisconnectHub;

  // We have to fire the event only if connection was really established and
  // all the services and characteristics were read.
  if FConnected then begin
    // Do not forget to reset connection state.
    FConnected := False;
    DoDisconnected(Reason);
  end;
end;

function TwclWeDoHub.Connect(const Radio: TwclBluetoothRadio;
  const Address: Int64): Integer;
begin
  if (Radio = nil) or (Address = 0) then
    Result := WCL_E_INVALID_ARGUMENT

  else begin
    // This check prevent exception raising when you try to change
    // connection MAC address on already connected client.
    if FClient.State <> csDisconnected then
      Result := WCL_E_CONNECTION_ACTIVE

    else begin
      FClient.Address := Address;
      Result := FClient.Connect(Radio);
    end;
  end;
end;

constructor TwclWeDoHub.Create(AOwner: TComponent);
begin
  inherited;

  FConnected := False;
  FHubConnected := False;

  FClient := TwclGattClient.Create(nil);
  FClient.OnCharacteristicChanged := ClientCharacteristicChanged;
  FClient.OnConnect := ClientConnect;
  FClient.OnDisconnect := ClientDisconnect;

  FDeviceInformation := TwclWeDoDeviceInformationService.Create(FClient, Self);
  FBatteryLevel := TwclWeDoBatteryLevelService.Create(FClient, Self);
  FIo := TwclWeDoIoService.Create(FClient, Self);

  // This service precessed by special way because we need to delegate all
  // methods and events to the Hub object.
  FHub := TwclWeDoHubService.Create(FClient, Self);
  FHub.OnButtonStateChanged := HubButtonStateChanged;
  FHub.OnDeviceAttached := HubDeviceAttached;
  FHub.OnDeviceDetached := HubDeviceDetached;
  FHub.OnLowVoltageAlert := HubLowVoltageAlert;
  FHub.OnHighCurrentAlert := HubHighCurrentAlert;
  FHub.OnLowSignalAlert := HubLowSignalAlert;

  // Create attached devices list.
  FDevices := TList<TwclWeDoIo>.Create;

  FOnConnected := nil;
  FOnDisconnected := nil;
  FOnButtonStateChanged := nil;
  FOnLowVoltageAlert := nil;
  FOnDeviceAttached := nil;
  FOnDeviceDetached := nil;
  FOnHighCurrentAlert := nil;
  FOnLowSignalAlert := nil;
end;

destructor TwclWeDoHub.Destroy;
begin
  Disconnect;

  FClient.Free;
  FDeviceInformation.Free;
  FHub.Free;
  FDevices.Free;

  inherited;
end;

procedure TwclWeDoHub.DetachDevices;
var
  Device: TwclWeDoIo;
begin
  for Device in FDevices do begin
    Device.Detach();
    DoDeviceDetached(Device);
    Device.Free;
  end;
  FDevices.Clear;
end;

function TwclWeDoHub.Disconnect: Integer;
begin
  DisconnectHub;

  Result := FClient.Disconnect;
end;

procedure TwclWeDoHub.DisconnectHub;
begin
  if FHubConnected then begin
    // Detach all attached devices.
    DetachDevices();

    // We have to release all services.
    DisconnectServices;

    FHubConnected := False;
  end;
end;

procedure TwclWeDoHub.DisconnectServices;
begin
  if FDeviceInformation.Connected then
    FDeviceInformation.Disconnect;
  if FBatteryLevel.Connected then
    FBatteryLevel.Disconnect;
  if FIo.Connected then
    FIo.Disconnect;
  if FHub.Connected then
    FHub.Disconnect;
end;

procedure TwclWeDoHub.DoButtonStateChanged(const Pressed: Boolean);
begin
  if Assigned(FOnButtonStateChanged) then
    FOnButtonStateChanged(Self, Pressed);
end;

procedure TwclWeDoHub.DoConnected(const Error: Integer);
begin
  if Assigned(FOnConnected) then
    FOnConnected(Self, Error);
end;

procedure TwclWeDoHub.DoDeviceAttached(const Device: TwclWeDoIo);
begin
  if Assigned(FOnDeviceAttached) then
    FOnDeviceAttached(Self, Device);
end;

procedure TwclWeDoHub.DoDeviceDetached(const Device: TwclWeDoIo);
begin
  if Assigned(FOnDeviceDetached) then
    FOnDeviceDetached(Self, Device);
end;

procedure TwclWeDoHub.DoDisconnected(const Reason: Integer);
begin
  if Assigned(FOnDisconnected) then
    FOnDisconnected(Self, Reason);
end;

procedure TwclWeDoHub.DoHighCurrentAlert(const Alert: Boolean);
begin
  if Assigned(FOnHighCurrentAlert) then
    FOnHighCurrentAlert(Self, Alert);
end;

procedure TwclWeDoHub.DoLowSignalAlert(const Alert: Boolean);
begin
  if Assigned(FOnLowSignalAlert) then
    FOnLowSignalAlert(Self, Alert);
end;

procedure TwclWeDoHub.DoLowVoltageAlert(const Alert: Boolean);
begin
  if Assigned(FOnLowVoltageAlert) then
    FOnLowVoltageAlert(Self, Alert);
end;

function TwclWeDoHub.GetAddress: Int64;
begin
  Result := FClient.Address;
end;

function TwclWeDoHub.GetState: TwclClientState;
begin
  Result := FClient.State;
end;

procedure TwclWeDoHub.HubButtonStateChanged(Sender: TObject;
  const Pressed: Boolean);
begin
  DoButtonStateChanged(Pressed);
end;

procedure TwclWeDoHub.HubDeviceAttached(Sender: TObject;
  const Device: TwclWeDoIo);
var
  Io: TwclWeDoIo;
begin
  // Make sure device is not attached yet.
  for Io in FDevices do begin
    if Io.ConnectionId = Device.ConnectionId then
      Exit;
  end;

  FDevices.Add(Device);
  DoDeviceAttached(Device);
end;

procedure TwclWeDoHub.HubDeviceDetached(Sender: TObject;
  const ConnectionId: Byte);
var
  Io: TwclWeDoIo;
  i: Integer;
begin
  Io := nil;
  // Make sure device was attached.
  for i := 0 to FDevices.Count - 1 do begin
    if FDevices[i].ConnectionId = ConnectionId then begin
      Io := FDevices[i];
      Break;
    end;
  end;

  if Io <> nil then begin
    Io.Detach;
    DoDeviceDetached(Io);
    FDevices.Remove(Io);
    Io.Free;
  end;
end;

procedure TwclWeDoHub.HubHighCurrentAlert(Sender: TObject;
  const Alert: Boolean);
begin
  DoHighCurrentAlert(Alert);
end;

procedure TwclWeDoHub.HubLowSignalAlert(Sender: TObject; const Alert: Boolean);
begin
  DoLowSignalAlert(Alert);
end;

procedure TwclWeDoHub.HubLowVoltageAlert(Sender: TObject; const Alert: Boolean);
begin
  DoLowVoltageAlert(Alert);
end;

function TwclWeDoHub.ReadDeviceName(out Name: string): Integer;
begin
  Result := FHub.ReadDeviceName(Name);
end;

function TwclWeDoHub.TurnOff: Integer;
begin
  Result := FHub.TurnOff;
end;

function TwclWeDoHub.WriteDeviceName(const Name: string): Integer;
begin
  Result := FHub.WriteDeviceName(Name);
end;

{ TwclWeDoIo }

procedure TwclWeDoIo.AddValidDataFormat(const Format: TwclWeDoDataFormat);
begin
  FValidDataFormats.Add(Format);
end;

class function TwclWeDoIo.Attach(const Hub: TwclWeDoHub;
  const RawInfo: TArray<Byte>): TwclWeDoIo;
var
  ConnectionId: Byte;
  Tmp: TArray<Byte>;
begin
  if Length(RawInfo) < 2 then
    Result := nil

  else begin
    if ((RawInfo[1] = 1) and (Length(RawInfo) <> 12)) or
       ((RawInfo[1] = 0) and (Length(RawInfo) <> 2))
    then
      Result := nil

    else begin
      if RawInfo[1] = 0 then
        // Detached???
        Result := nil

      else begin
        ConnectionId := RawInfo[0];
        case RawInfo[3] of
          WEDO_DEVICE_MOTOR:
            begin
              Result := TwclWeDoMotor.Create(Hub, ConnectionId);
              Result.FDeviceType := iodMotor;
            end;
          WEDO_DEVICE_VOLTAGE_SENSOR:
            begin
              Result := TwclWeDoVoltageSensor.Create(Hub, ConnectionId);
              Result.FDeviceType := iodVoltageSensor;
            end;
          WEDO_DEVICE_CURRENT_SENSOR:
            begin
              Result := TwclWeDoCurrentSensor.Create(Hub, ConnectionId);
              Result.FDeviceType := iodCurrentSensor;
            end;
          WEDO_DEVICE_PIEZO:
            begin
              Result := TwclWeDoPieazo.Create(Hub, ConnectionId);
              Result.FDeviceType := iodPiezo;
            end;
          WEDO_DEVICE_RGB:
            begin
              Result := TwclWeDoRgbLight.Create(Hub, ConnectionId);
              Result.FDeviceType := iodRgb;
            end;
          WEDO_DEVICE_TILT_SENSOR:
            begin
              Result := TwclWeDoTiltSensor.Create(Hub, ConnectionId);
              Result.FDeviceType := iodTiltSensor;
            end;
          WEDO_DEVICE_MOTION_SENSOR:
            begin
              Result := TwclWeDoMotionSensor.Create(Hub, ConnectionId);
              Result.FDeviceType := iodMotionSensor;
            end;
          else
            Result := nil;
        end;

        if Result <> nil then begin
          Tmp := Copy(RawInfo, 8, 4);
          Result.FFirmwareVersion := VersionFromByteArray(Tmp);
          Tmp := Copy(RawInfo, 4, 4);
          Result.FHardwareVersion := VersionFromByteArray(Tmp);
          Result.FPortId := RawInfo[2];
          Result.FInternal := Result.PortId > 50;
        end;
      end;
    end;
  end;
end;

constructor TwclWeDoIo.Create(const Hub: TwclWeDoHub; const ConnectionId: Byte);
begin
  if Hub = nil then
    raise wclEInvalidArgument.Create('Hub parameter can not be null.');

  FAttached := True; // It is always attached on creation!
  FConnectionId := ConnectionId;
  FDataFormats := TList<TwclWeDoDataFormat>.Create;
  FDefaultInputFormat := nil;
  FDeviceType := iodUnknown;
  FHub := Hub;
  FInputFormat := nil;
  FInternal := True;
  FPortId := 0;
  FNumbersFromValueData := TList<TArray<Byte>>.Create;
  FValidDataFormats := TList<TwclWeDoDataFormat>.Create;
  FValue := nil;
end;

function TwclWeDoIo.DataFormatForInputFormat(
  const InputFormat: TwclWeDoInputFormat): TwclWeDoDataFormat;
var
  DataFormat: TwclWeDoDataFormat;
begin
  Result := nil;

  for DataFormat in FValidDataFormats do begin
    if (DataFormat.Mode = InputFormat.Mode) and (DataFormat.Unit_ = InputFormat.Unit_) then begin
      if (DataFormat.DataSetCount * DataFormat.DataSetSize) = InputFormat.NumberOfBytes then
        Result := DataFormat;
      Break;
    end;
  end;
end;

destructor TwclWeDoIo.Destroy;
var
  Format: TwclWeDoDataFormat;
begin
  FDataFormats.Free;
  if FDefaultInputFormat <> nil then
    FDefaultInputFormat.Free;
  FNumbersFromValueData.Free;
  for Format in FValidDataFormats do
    Format.Free;
  FValidDataFormats.Free;

  inherited;
end;

procedure TwclWeDoIo.Detach;
begin
  FAttached := False;
  FNumbersFromValueData.Clear;
end;

function TwclWeDoIo.GetAsFloat: Single;
begin
  if Length(FValue) = 0 then
    Result := 0
  else begin
    if Length(FValue) = 4 then
      Result := PSingle(FValue)^
    else
      Result := 0;
  end;
end;

function TwclWeDoIo.GetAsInteger: Integer;
var
  Signed: Integer;
begin
  if Length(FValue) = 0 then
    Result := 0
  else begin
    if Length(FValue) = 1 then begin
      Signed := FValue[0];
      if Signed > 127 then
        Signed := 0 - (256 - Signed);
      Result := Signed;

    end else begin
      if Length(FValue) = 2 then
        Result := PWord(FValue)^

      else begin
        if Length(FValue) = 4 then
          Result := PInteger(FValue)^

        else
          Result := 0;
      end;
    end;
  end;
end;

function TwclWeDoIo.GetInputFormatMode: Byte;
begin
  if FInputFormat <> nil then
    Result := FInputFormat.Mode
  else begin
    if FDefaultInputFormat <> nil then
      Result := FDefaultInputFormat.Mode
    else
      Result := 0;
  end;
end;

procedure TwclWeDoIo.InputFormatChanged(const OldFormat: TwclWeDoInputFormat);
begin
  // Do nothing
end;

procedure TwclWeDoIo.RemoveValidDataFormat(const Format: TwclWeDoDataFormat);
begin
  FValidDataFormats.Remove(Format);
end;

function TwclWeDoIo.ResetSensor: Integer;
begin
  Result := FHub.Io.ResetIo(FConnectionId);
end;

function TwclWeDoIo.SendInputFormat(const Format: TwclWeDoInputFormat): Integer;
begin
  Result := FHub.Io.WriteInputFormat(Format, FConnectionId);
end;

function TwclWeDoIo.SendReadValueRequest: Integer;
begin
  Result := FHub.Io.ReadValue(FConnectionId);
end;

function TwclWeDoIo.SetDefaultInputFormat(const
  Format: TwclWeDoInputFormat): Integer;
begin
  if FDefaultInputFormat <> nil then
    FDefaultInputFormat.Free;
  FDefaultInputFormat := Format;
  Result := SendInputFormat(Format);
end;

procedure TwclWeDoIo.SetDefaultInputFormatProp(Format: TwclWeDoInputFormat);
begin
  SetDefaultInputFormat(Format);
end;

function TwclWeDoIo.SetInputFormatMode(const Mode: Byte): Integer;
var
  Format: TwclWeDoInputFormat;
begin
  if FInputFormat <> nil then begin
    Format := FInputFormat.InputFormatBySettingMode(Mode);
    try
      Result := SendInputFormat(Format);
    finally
      Format.Free;
    end;

  end else begin
    if FDefaultInputFormat <> nil then begin
      Format := FDefaultInputFormat.InputFormatBySettingMode(Mode);
      try
        Result := SendInputFormat(Format);
      finally
        Format.Free;
      end;

    end else
      Result := WCL_E_INVALID_ARGUMENT;
  end;
end;

procedure TwclWeDoIo.SetValue(const Value: TArray<Byte>);
begin
  FValue := Value;
  ValueChanged;
end;

procedure TwclWeDoIo.UpdateInputFormat(const Format: TwclWeDoInputFormat);
var
  OldFormat: TwclWeDoInputFormat;
begin
  if (FInputFormat = nil) or ((not Format.Equals(FInputFormat)) and (FConnectionId = Format.ConnectionId)) then begin
    OldFormat := FInputFormat;
    FInputFormat := Format;
    InputFormatChanged(OldFormat);
    SendReadValueRequest;
  end;
end;

procedure TwclWeDoIo.UpdateValue(const Value: TArray<Byte>);
begin
  if (FValue <> nil) and (FValue = Value) then
    Exit;

  if VerifyValue(Value) then begin
    FValue := Value;
    ValueChanged;
  end;
end;

procedure TwclWeDoIo.ValueChanged;
begin
  // Do nothing.
end;

function TwclWeDoIo.VerifyValue(const Value: TArray<Byte>): Boolean;
var
  DataFormat: TwclWeDoDataFormat;
  ValueCorrect: Boolean;
  DataList: TList<Byte>;
  i: Integer;
begin
  if Length(Value) = 0 then
    Result := True

  else begin
    if FValidDataFormats.Count = 0 then
      Result := True

    else begin
      // If one or more InputDataFormats are defined, we look at the latest
      // received InputFormat from the device for a received value to be
      // accepted, there:
      //   1. Must exists an DataFormat that matches the latest received
      //      InputFormat from device.
      //   2. The received valueData length must match this DataFormat.
      DataFormat := DataFormatForInputFormat(FInputFormat);
      if DataFormat = nil then
        Result := False

      else begin
        ValueCorrect := Length(Value) = (DataFormat.DataSetSize *
          DataFormat.DataSetCount);
        if not ValueCorrect then begin
          FNumbersFromValueData.Clear;
          Result := False;

        end else begin
          // If the Data Format has a value fill the NumbersFromValueData array with
          // all received numbers
          if DataFormat.DataSetCount > 0 then begin
            DataList := ToList(Value);
            if DataList <> nil then begin
              try
                FNumbersFromValueData.Clear;
                i := 0;
                while i < DataList.Count do begin
                  FNumbersFromValueData.Add(ToArray(DataList, i, DataFormat.DataSetSize));
                  Inc(i, DataFormat.DataSetSize);
                end;

              finally
                DataList.Free;
              end;
            end;
          end;

          Result := ValueCorrect;
        end;
      end;
    end;
  end;
end;

function TwclWeDoIo.WriteData(const Data: TArray<Byte>): Integer;
begin
  Result := FHub.Io.WriteData(Data, FConnectionId);
end;

{ TwclWeDoPieazo }

constructor TwclWeDoPieazo.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);
end;

function TwclWeDoPieazo.PlayNote(const Note: TwclWeDoPiezoNote;
  const Octave: Byte; const Duration: Word): Integer;
var
  BaseTone: Double;
  OctavesAboveMiddle: Integer;
  HalfStepsAwayFromBase: Real;
  Frequency: Double;
begin
  if (Octave = 0) or (Octave > 6) or ((Octave = 6) and (Note > pnFis)) then
    Result := WCL_E_INVALID_ARGUMENT
  else begin
    if not Attached then
      Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
    else begin
      // The basic formula for the frequencies of the notes of the equal
      //  tempered scale is given by fn = f0 * (a)n where
      //   f0 - the frequency of one fixed note which must be defined.
      //        A common choice is setting the A above middle C (A4)
      //        at f0 = 440 Hz.
      //   n  - the number of half steps away from the fixed note you are. If
      //        you are at a higher note,
      //        n is positive. If you are on a lower note, n is negative.
      //   fn - the frequency of the note n half steps away.
      //   a  - (2)1/12 = the twelfth root of 2 = the number which when
      //        multiplied by itself 12 times equals 2 = 1.059463094359...
      BaseTone := 440.0;
      OctavesAboveMiddle := Octave - 4;
      HalfStepsAwayFromBase := Byte(Note) - Byte(pnA) +
        (OctavesAboveMiddle * 12);
      Frequency := BaseTone * Power(Power(2.0, 1.0 / 12),
        HalfStepsAwayFromBase);
      Result := Hub.Io.PiezoPlayTone(Round(Frequency), Duration, ConnectionId);
    end;
  end;
end;

function TwclWeDoPieazo.PlayTone(const Frequency: Word;
  const Duration: Word): Integer;
begin
  if Frequency > PIEZO_MAX_FREQUENCY then
    Result := WCL_E_INVALID_ARGUMENT
  else begin
    if not Attached then
      Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
    else
      Result := Hub.Io.PiezoPlayTone(Frequency, Duration, ConnectionId);
  end;
end;

function TwclWeDoPieazo.StopPlaying: Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else
    Result := Hub.Io.PiezoStopPlaying(ConnectionId);
end;

{ TwclWeDoRgbLight }

constructor TwclWeDoRgbLight.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);

  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(lmDiscrete), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(lmDiscrete), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 4, Byte(lmDiscrete), suSi));

  AddValidDataFormat(TwclWeDoDataFormat.Create(3, 1, Byte(lmAbsolute), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(3, 1, Byte(lmAbsolute), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(3, 4, Byte(lmAbsolute), suSi));

  DefaultInputFormat := TwclWeDoInputFormat.Create(ConnectionId, iodRgb,
    Byte(lmDiscrete), 1, suRaw, True, 0, 1);

  FColor := DefaultColor;
  FColorIndex := DefaultColorIndex;

  FOnColorChanged := nil;
  FOnModeChanged := nil;
end;

procedure TwclWeDoRgbLight.DoColorChanged;
begin
  if Assigned(FOnColorChanged) then
    FOnColorChanged(Self);
end;

procedure TwclWeDoRgbLight.DoModeChanged;
begin
  if Assigned(FOnModeChanged) then
    FOnModeChanged(Self);
end;

function TwclWeDoRgbLight.GetColorFromByteArray(const Data: TArray<Byte>;
  out Color: TColor): Boolean;
begin
  Color := clBlack;
  if Length(Data) <> 3 then
    Result := False

  else begin
    Color := RGB(Data[0], Data[1], Data[2]);
    Result := True;
  end;
end;

function TwclWeDoRgbLight.GetDefaultColor: TColor;
begin
  Result := RGB($00, $00, $FF);
end;

function TwclWeDoRgbLight.GetDefaultColorIndex: TwclWeDoColor;
begin
  Result := wclBlue;
end;

function TwclWeDoRgbLight.GetMode: TwclWeDoRgbLightMode;
begin
  Result := TwclWeDoRgbLightMode(InputFormatMode);
end;

procedure TwclWeDoRgbLight.InputFormatChanged(
  const OldFormat: TwclWeDoInputFormat);
begin
  inherited;

  if InputFormat <> nil then begin
    if OldFormat = nil then
      DoModeChanged
    else begin
      if InputFormat.Mode <> OldFormat.Mode then
        DoModeChanged;
    end;
  end;
end;

function TwclWeDoRgbLight.SetColor(const Rgb: TColor): Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else begin
    if Mode <> lmAbsolute then
      Result := WCL_E_INVALID_ARGUMENT
    else begin
      FColor := Rgb;
      Result := Hub.Io.WriteColor(Rgb and $000000FF, (Rgb shr 8) and $000000FF,
        (Rgb shr 16) and $000000FF, ConnectionId);
    end;
  end;
end;

function TwclWeDoRgbLight.SetColorIndex(const Index: TwclWeDoColor): Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else begin
    if Mode <> lmDiscrete then
      Result := WCL_E_INVALID_ARGUMENT
    else begin
      FColorIndex := Index;
      Result := Hub.Io.WriteColorIndex(Byte(Index), ConnectionId);
    end;
  end;
end;

function TwclWeDoRgbLight.SetMode(const Mode: TwclWeDoRgbLightMode): Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else
    Result := SetInputFormatMode(Byte(Mode));
end;

function TwclWeDoRgbLight.SwitchOff: Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else begin
    if Mode = lmAbsolute then
      Result := SetColor(clBlack)
    else begin
      if Mode = lmDiscrete then
        Result := SetColorIndex(wclBlack)
      else
        Result := WCL_E_INVALID_ARGUMENT;
    end;
  end;
end;

function TwclWeDoRgbLight.SwitchToDefaultColor: Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else begin
    if Mode = lmAbsolute then
      Result := SetColor(DefaultColor)
    else begin
      if Mode = lmDiscrete then
        Result := SetColorIndex(DefaultColorIndex)
      else
        Result := WCL_E_INVALID_ARGUMENT;
    end;
  end;
end;

procedure TwclWeDoRgbLight.ValueChanged;
var
  NewColor: TColor;
  NewColorIndex: TwclWeDoColor;
begin
  inherited;

  if Mode = lmAbsolute then begin
    if GetColorFromByteArray(Value, NewColor) then begin
      if NewColor <> FColor then begin
        FColor := NewColor;
        DoColorChanged;
      end;
    end;
  end else begin
    if Mode = lmDiscrete then begin
      NewColorIndex := TwclWeDoColor(AsInteger);
      if NewColorIndex <> FColorIndex then begin
        FColorIndex := NewColorIndex;
        DoColorChanged;
      end;
    end;
  end;
end;

{ TwclWeDoCurrentSensor }

constructor TwclWeDoCurrentSensor.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);

  DefaultInputFormat := TwclWeDoInputFormat.Create(ConnectionId,
    iodCurrentSensor, 0, 30, suSi, True, 0, 1);

  FOnCurrentChanged := nil;
end;

procedure TwclWeDoCurrentSensor.DoCurrentChanged;
begin
  if Assigned(FOnCurrentChanged) then
    FOnCurrentChanged(Self);
end;

function TwclWeDoCurrentSensor.GetCurrent: Single;
begin
  if (InputFormat = nil) or (InputFormat.Mode <> 0) or (InputFormat.Unit_ <> suSi) then
    Result := 0
  else
    Result := AsFloat;
end;

procedure TwclWeDoCurrentSensor.ValueChanged;
begin
  inherited;

  DoCurrentChanged;
end;

{ TwclWeDoVoltageSensor }

constructor TwclWeDoVoltageSensor.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);

  DefaultInputFormat := TwclWeDoInputFormat.Create(ConnectionId,
    iodVoltageSensor, 0, 30, suSi, True, 0, 1);

  FOnVoltageChanged := nil;
end;

procedure TwclWeDoVoltageSensor.DoVoltageChanged;
begin
  if Assigned(FOnVoltageChanged) then
    FOnVoltageChanged(Self);
end;

function TwclWeDoVoltageSensor.GetVoltage: Single;
begin
  if (InputFormat = nil) or (InputFormat.Mode <> 0) or (InputFormat.Unit_ <> suSi) then
    Result := 0
  else
    Result := AsFloat;
end;

procedure TwclWeDoVoltageSensor.ValueChanged;
begin
  inherited;

  DoVoltageChanged;
end;

{ TwclWeDoMotor }

function TwclWeDoMotor.Brake: Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED

  else begin
    Result := SendPower(MOTOR_POWER_BRAKE);
    if Result = WCL_E_SUCCESS then
      FDirection := mdBraking;
  end;
end;

function TwclWeDoMotor.ConvertUnsignedMotorPowerToSigned(
  const Direction: TwclWeDoMotorDirection; const Power: Byte): ShortInt;
begin
  Result := Abs(Min(Max(Power, MOTOR_MIN_SPEED), MOTOR_MAX_SPEED));
  if Direction = mdLeft then
    Result := -Result;
end;

constructor TwclWeDoMotor.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);

  FDirection := mdBraking;
  FPower := 0;
end;

function TwclWeDoMotor.Drift: Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED

  else begin
    Result := SendPower(MOTOR_POWER_DRIFT);
    if Result = WCL_E_SUCCESS then
      FDirection := mdDrifting;
  end;
end;

function TwclWeDoMotor.GetIsBraking: Boolean;
begin
  Result := FPower = MOTOR_POWER_BRAKE;
end;

function TwclWeDoMotor.GetIsDrifting: Boolean;
begin
  Result := FPower = MOTOR_POWER_DRIFT;
end;

function TwclWeDoMotor.GetPower: Byte;
begin
  if (FDirection = mdBraking) or (FDirection = mdDrifting) then
    Result := 0
  else
    Result := Abs(FPower);
end;

function TwclWeDoMotor.Run(const Direction: TwclWeDoMotorDirection;
  const Power: Byte): Integer;
begin
  if (Direction = mdUnknown) or (Power > 100) then
    Result := WCL_E_INVALID_ARGUMENT
  else begin
    if not Attached then
      Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
    else begin
      if (Direction = mdDrifting) or (Power = 0) then
        Result := Drift
      else begin
        if Direction = mdBraking then
          Result := Brake
        else begin
          Result := SendPower(ConvertUnsignedMotorPowerToSigned(Direction,
            Power));
          if Result = WCL_E_SUCCESS then
            FDirection := Direction;
        end;
      end;
    end;
  end;
end;

function TwclWeDoMotor.SendPower(const Power: ShortInt): Integer;
var
  Offset: Byte;
begin
  if (Power = MOTOR_POWER_BRAKE) or (Power = MOTOR_POWER_DRIFT) then
    Result := Hub.Io.WriteMotorPower(Power, ConnectionId)

  else begin
    Offset := MOTOR_POWER_OFFSET;
    if FirmwareVersion.MajorVersion = 0 then
      // On version 0.x of the firmware, PVM offset is handled in the firmware
      Offset := 0;
    Result := Hub.Io.WriteMotorPower(Power, Offset, ConnectionId);
  end;

  if Result = WCL_E_SUCCESS then
    FPower := Power;
end;

{ TwclWeDoResetableSensor }

constructor TwclWeDoResetableSensor.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);
end;

function TwclWeDoResetableSensor.Reset: Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else
    Result := inherited ResetSensor;
end;

{ TwclWeDoMotionSensor }

constructor TwclWeDoMotionSensor.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);

  FCount := 0;
  FDistance := 0;

  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(mmDetect), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(mmDetect), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 4, Byte(mmDetect), suSi));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 4, Byte(mmCount), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(mmCount), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 4, Byte(mmCount), suSi));

  DefaultInputFormat := TwclWeDoInputFormat.Create(ConnectionId,
    iodMotionSensor, 0, 1, suRaw, True, 0, 4);

  FOnCountChanged := nil;
  FOnDistanceChanged := nil;
  FOnModeChanged := nil;
end;

procedure TwclWeDoMotionSensor.DoCountChanged;
begin
  if Assigned(FOnCountChanged) then
    FOnCountChanged(Self);
end;

procedure TwclWeDoMotionSensor.DoDistanceChanged;
begin
  if Assigned(FOnDistanceChanged) then
    FOnDistanceChanged(Self);
end;

procedure TwclWeDoMotionSensor.DoModeChanged;
begin
  if Assigned(FOnModeChanged) then
    FOnModeChanged(Self);
end;

function TwclWeDoMotionSensor.GetCount: Cardinal;
begin
  if Mode <> mmCount then
    Result := 0
  else
    Result := FCount;
end;

function TwclWeDoMotionSensor.GetDistance: Single;
begin
  if Mode <> mmDetect then
    Result := 0
  else
    Result := FDistance;
end;

function TwclWeDoMotionSensor.GetMode: TwclWeDoMotionSensorMode;
begin
  Result := TwclWeDoMotionSensorMode(InputFormatMode);
end;

procedure TwclWeDoMotionSensor.InputFormatChanged(
  const OldFormat: TwclWeDoInputFormat);
begin
  inherited;

  if InputFormat <> nil then begin
    if OldFormat = nil then
      DoModeChanged
    else begin
      if InputFormat.Mode <> OldFormat.Mode then
        DoModeChanged;
    end;
  end;
end;

function TwclWeDoMotionSensor.SetMode(
  const Mode: TwclWeDoMotionSensorMode): Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else
    Result := SetInputFormatMode(Byte(Mode));
end;

procedure TwclWeDoMotionSensor.ValueChanged;
begin
  inherited;

  if Mode = mmDetect then begin
    if Length(Value) = 4 then
      FDistance := AsFloat
    else
      FDistance := AsInteger;
    DoDistanceChanged;

  end else begin
    if Mode = mmCount then begin
      if Length(Value) = 4 then
        FCount := AsInteger
      else
        FCount := AsInteger;
      DoCountChanged;
    end;
  end;
end;

{ TwclWeDoTiltSensor }

function TwclWeDoTiltSensor.ConvertToSigned(const b: Byte): Integer;
var
  Signed: Integer;
begin
  Signed := Integer(b);
  if Signed > 127 then
    Signed := 0 - (256 - Signed);
  Result := Signed;
end;

function TwclWeDoTiltSensor.ConvertToUnsigned(const b: Byte): Integer;
begin
  Result := b;
end;

constructor TwclWeDoTiltSensor.Create(const Hub: TwclWeDoHub;
  const ConnectionId: Byte);
begin
  inherited Create(Hub, ConnectionId);

  AddValidDataFormat(TwclWeDoDataFormat.Create(2, 1, Byte(tmAngle), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(2, 1, Byte(tmAngle), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(2, 4, Byte(tmAngle), suSi));

  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(tmTilt), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 1, Byte(tmTilt), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(1, 4, Byte(tmTilt), suSi));

  AddValidDataFormat(TwclWeDoDataFormat.Create(3, 1, Byte(tmCrash), suRaw));
  AddValidDataFormat(TwclWeDoDataFormat.Create(3, 1, Byte(tmCrash), suPercentage));
  AddValidDataFormat(TwclWeDoDataFormat.Create(3, 4, Byte(tmCrash), suSi));

  DefaultInputFormat := TwclWeDoInputFormat.Create(ConnectionId, iodTiltSensor,
    Byte(tmTilt), 1, suRaw, True, 0, 1);

  FOnAngleChanged := nil;
  FOnCrashChanged := nil;
  FOnDirectionChanged := nil;
  FOnModeChanged := nil;
end;

procedure TwclWeDoTiltSensor.DoAngleChanged;
begin
  if Assigned(FOnAngleChanged) then
    FOnAngleChanged(Self);
end;

procedure TwclWeDoTiltSensor.DoCrashChanged;
begin
  if Assigned(FOnCrashChanged) then
    FOnCrashChanged(Self);
end;

procedure TwclWeDoTiltSensor.DoDirectionChanged;
begin
  if Assigned(FOnDirectionChanged) then
    FOnDirectionChanged(Self);
end;

procedure TwclWeDoTiltSensor.DoModeChanged;
begin
  if Assigned(FOnModeChanged) then
    FOnModeChanged(Self);
end;

function TwclWeDoTiltSensor.GetAngle: TwclWeDoTiltSensorAngle;
begin
  if (Mode <> tmAngle) or (NumbersFromValueData.Count <> 2) then begin
    Result.X := 0;
    Result.Y := 0;

  end else begin
    if (InputFormat.Unit_ = suSi) and (Length(NumbersFromValueData[0]) = 4) then begin
      Result.X := PSingle(NumbersFromValueData[0])^;
      Result.Y := PSingle(NumbersFromValueData[1])^;

    end else begin
      Result.X := ConvertToSigned(NumbersFromValueData[0][0]);
      Result.Y := ConvertToSigned(NumbersFromValueData[1][0]);
    end;
  end;
end;

function TwclWeDoTiltSensor.GetCrash: TwclWeDoTiltSensorCrash;
begin
  if (Mode <> tmCrash) or (NumbersFromValueData.Count <> 3) then begin
    Result.X := 0;
    Result.Y := 0;
    Result.Z := 0;

  end else begin
    if (InputFormat.Unit_ = suSi) and (Length(NumbersFromValueData[0]) = 4) then begin
      Result.X := PSingle(NumbersFromValueData[0])^;
      Result.Y := PSingle(NumbersFromValueData[1])^;
      Result.Z := PSingle(NumbersFromValueData[2])^;

    end else begin
      Result.X := ConvertToUnsigned(NumbersFromValueData[0][0]);
      Result.Y := ConvertToUnsigned(NumbersFromValueData[1][0]);
      Result.Z := ConvertToUnsigned(NumbersFromValueData[2][0]);
    end;
  end;
end;

function TwclWeDoTiltSensor.GetDirection: TwclWeDoTiltSensorDirection;
begin
  if Mode <> tmTilt then
    Result := tdUnknown
  else
    Result := TwclWeDoTiltSensorDirection(AsInteger);
end;

function TwclWeDoTiltSensor.GetMode: TwclWeDoTiltSensorMode;
begin
  Result := TwclWeDoTiltSensorMode(InputFormatMode);
end;

procedure TwclWeDoTiltSensor.InputFormatChanged(
  const OldFormat: TwclWeDoInputFormat);
begin
  inherited;

  if InputFormat <> nil then begin
    if OldFormat = nil then
      DoModeChanged
    else begin
      if InputFormat.Mode <> OldFormat.Mode then
        DoModeChanged;
    end;
  end;
end;

function TwclWeDoTiltSensor.SetMode(
  const Mode: TwclWeDoTiltSensorMode): Integer;
begin
  if not Attached then
    Result := WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED
  else
    Result := SetInputFormatMode(Byte(Mode));
end;

procedure TwclWeDoTiltSensor.ValueChanged;
begin
  inherited;

  if Mode = tmAngle then
    DoAngleChanged
  else begin
    if Mode = tmCrash then
      DoCrashChanged
    else begin
      if Mode = tmTilt then
        DoDirectionChanged;
    end;
  end;
end;

end.