using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Authentication.OnlineId;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Helpers;
using LegoDeviceSDK.Interfaces;

namespace LegoDeviceSDK.Bluetooth {
	/// <summary>
	/// An BluetoothService represent an IO of some kind, for example a motor or sensor. It could also be an internal IO, such as Voltage sensor build into the device.
	/// The LESerBluetoothServicevice has a number of sub-classes for known IO types. This includes <see cref="BluetoothMotor"/>, <see cref="BluetoothTiltSensor"/> and <see cref="BluetoothMotionSensor"/>just to mention a few.
	/// Add a instance of a <see cref="IServiceDelegate"/> using addDelegate to be notified when a service receives an updated value.
	/// </summary>
	public abstract class BluetoothService : IService {
		const string Tag = "BluetoothService";

		internal delegate void CharacteristicsValueChangeCompletedHandler(byte[] bytes, IService service);

		/// <summary>
		/// General info about the connected service
		/// </summary>
		public ConnectInfo ConnectInfo { get; internal set; }

		/// <summary>
		/// The default input format that will be uploaded to the device for this service upon discovery of the service. Only the known service types (<see cref="BluetoothMotor"/>, <see cref="BluetoothTiltSensor"/>, etc.) has a default input format.
		/// </summary>
		public InputFormat DefaultInputFormat {
			get { return defaultInputFormat; }
			internal set {
				defaultInputFormat = value;
				UpdateInputFormat(value);
			}
		}

		InputFormat defaultInputFormat;
		/// <summary>
		/// The <see cref="IDevice"/> this service is related to
		/// </summary>
		public IDevice Device { get; private set; }
		internal IIo Io { get; set; }
		InputFormat inputFormat;
		/// <summary>
		/// The current <see cref="InputFormat"/> for this service.
		/// </summary>
		public InputFormat InputFormat {
			get { return inputFormat; }
			internal set {
				var oldInputFormat = inputFormat;
				inputFormat = value;
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						serviceDelegate.DidUpdateInputFormat(this, oldInputFormat, inputFormat);
					}
				}
			}
		}
		/// <summary>
		/// Is YES if this service represents an internal IO, such as an <see cref="BluetoothVoltageSensor"/>
		/// </summary>
		public bool IsInternalService { get; private set; }
		/// <summary>
		/// The name of the service
		/// </summary>
		public string ServiceName { get; protected set; }
		/// <summary>
		/// The data formats that this service may use to parse received data.
		/// </summary>
		/// <remarks>
		/// The data formats that this service may use to parse received data.
		/// When a new value for a service is received from the device, the SDK will look for a <see cref="DataFormat"/> among the validDataFormats that matches the <see cref="Generic.InputFormat.Unit"/> and <see cref="Generic.InputFormat.Mode"/>. If a match is found the <see cref="DataFormat"/> is used to parse the received data correctly (as a float, integer).
		/// The known service types such as <see cref="BluetoothTiltSensor"/> and <see cref="BluetoothMotionSensor"/> etc. comes with a set of predefined validDataFormat and you do not need to add valid data formats yourself. However, if you wish to use the LEService with a service type unknown to the SDK you must add the valid data formats using the addValidDataFormat: method.
		/// </remarks>
		public List<DataFormat> ValidDataFormats { get; private set; }

        /// <summary>
        /// The value data representation from the service as a floating-point number.
        /// If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
        /// </summary>
        /// <param name="data">The data to parse</param>
        /// <returns>Passed data parsed as a floating-point number</returns>
        public float FloatFromData(byte[] data) {
            if (data == null || data.Length == 0)
                return 0f;
            if (data.Length == 4)
                return BitConverter.ToSingle(data, 0);
            return 0f;
        }

        /// <summary>
        /// The value data representation from the service as an integer.
        /// If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
        /// </summary>
        /// <param name="data">The data to parse</param>
        /// <returns>Passed data parsed as a integer</returns>
        public int IntegerFromData(byte[] data) {
            if (data == null || data.Length == 0)
                return 0;
            if (data.Length == 1) {
                return ValueHelper.ConvertToSigned(data[0]);
            }
            if (data.Length == 2)
                return BitConverter.ToInt16(data, 0);
            if (data.Length == 4)
                return BitConverter.ToInt32(data, 0);
            return 0;
        }

		/// <summary>
		/// The latest received value from the service as an integer. If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
		/// </summary>
		public float ValueAsFloat {
            get { return FloatFromData(ValueData); }
		}

		/// <summary>
		/// The latest received value from the service as an integer. If no valid data format is found to parse the data zero is returned, see <see cref="ValidDataFormats"/>.
		/// </summary>
		public int ValueAsInteger {
            get { return IntegerFromData(ValueData); }
		}

		byte[] valueData;
		/// <summary>
		/// The latest received value from the service as raw data
		/// </summary>
		public byte[] ValueData {
			get { return valueData; }
			internal set {
				var oldValueData = valueData;
				valueData = value;
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						serviceDelegate.DidUpdateValueData(this, oldValueData, value);
					}
				}
			}
		}

		/// <summary>
		/// A list with one byte[] per number received
		/// </summary>
		internal List<byte[]> NumbersFromValueData { get; private set; }

		/// <summary>
		/// Protected list of Service delegates
		/// </summary>
		protected List<IServiceDelegate> serviceDelegates = new List<IServiceDelegate>();

		internal BluetoothService(IDevice device, IIo io, ConnectInfo connectInfo) {
			Device = device;
			Io = io;
			if(Io != null)
				Io.AddDelegate(this);
			ConnectInfo = connectInfo;
			if (ConnectInfo.HubIndex > 50)
				IsInternalService = true;
			else
				IsInternalService = false;

			ValidDataFormats = new List<DataFormat>();
		}

		/// <summary>
		/// Used to free the resource. GC would not collect, because BluetoothIo still had a reference to the service
		/// </summary>
		internal void UnsubscribeFromBluetoothIo() {
			if (Io != null)
				Io.RemoveDelegate(this);
		}
		/// <summary>
		/// Add a delegate to receive service updates.
		/// </summary>
		/// <param name="serviceDelegate">The delegate to add</param>
		public void AddDelegate(IServiceDelegate serviceDelegate) {
			lock (serviceDelegates) {
				serviceDelegates.Add(serviceDelegate);
			}
		}

		/// <summary>
		/// Remove a delegate
		/// </summary>
		/// <param name="serviceDelegate">The delegate to remove</param>
		public void RemoveDelegate(IServiceDelegate serviceDelegate) {
			lock (serviceDelegates) {
				serviceDelegates.Remove(serviceDelegate);
			}
		}

		#region InputFormat
		/// <summary>
		/// Send an updated input format for this service to the device. If successful this will trigger an invocation of the delegate callback method <see cref="IServiceDelegate.DidUpdateInputFormat"/>.
		/// </summary>
		/// <param name="inputFormat"></param>
		public void UpdateInputFormat(InputFormat inputFormat) {
			Io.WriteInputFormat(inputFormat, ConnectInfo.ConnectId);
		}

		/// <summary>
		/// Add a new valid data format.
		/// </summary>
		/// <param name="dataFormat">The data format to add.</param>
		public void AddValidDataFormat(DataFormat dataFormat) {
			ValidDataFormats.Add(dataFormat);
		}

		/// <summary>
		/// Remove a valid data format
		/// </summary>
		/// <param name="dataFormat">The data format to remove</param>
		public void RemoveValidDataFormat(DataFormat dataFormat) {
			ValidDataFormats.Remove(dataFormat);
		}

        /// <summary>
        /// Convenience property that provides access to the mode of the current input format.
        /// It uses InputFormat's mode, or DefaultInputFormat's if the former is not defined.
        /// </summary>
        public int InputFormatMode {
            get {
                if (InputFormat != null)
                    return InputFormat.Mode;
                else if (DefaultInputFormat != null)
                    return DefaultInputFormat.Mode;
                SdkLogger.D(Tag, "No input format set, returning mode 0");
                return 0;
            }
            set {
                if (InputFormat != null)
                    UpdateInputFormat(InputFormat.InputFormatBySettingMode(value));
                else if (DefaultInputFormat != null)
                    UpdateInputFormat(DefaultInputFormat.InputFormatBySettingMode(value));
                else
                    SdkLogger.E(Tag, "Tried to update input format with new mode, but no current inputFormat or defaultInputFormat is set");
            }
        }

		#endregion

		#region InputValue
		/// <summary>
		/// If the notifications is disabled for the service in the inputFormat through <see cref="Generic.InputFormat.NotificationsEnabled"/> you will have to use this method to request an updated value for the service. The updated value will be delivered through the delegate method <see cref="IServiceDelegate.DidUpdateInputFormat"/>.
		/// </summary>
		public void SendReadValueRequest() {
			Io.ReadValueForConnectId(ConnectInfo.ConnectId);
		}

		/// <summary>
		/// This will send a reset command to the Device for this service. Some services has state, such as a bump-count for a tilt sensor that you may wish to reset.
		/// </summary>
		public void SendResetStateRequest() {
			Io.ResetStateForConnectId(ConnectInfo.ConnectId);
		}

		#endregion

		#region Output

		/// <summary>
		/// Will send data to the IO backed by this service. Useful to write data to an output unknown to the SDK.
		/// </summary>
		/// <param name="data">The data to write.</param>
		public void WriteData(byte[] data) {
			Io.WriteData(data, ConnectInfo.ConnectId);
		}

		#endregion

		/// <summary>
		/// Compare to services
		/// </summary>
		/// <param name="otherService">The other service to be compared to</param>
		/// <returns>True if services are equal</returns>
		public bool Equals(IService otherService) {
			return ConnectInfo.Equals(otherService.ConnectInfo) &&
			       Device.Equals(otherService.Device) &&
			       IsInternalService.Equals(otherService.IsInternalService) &&
			       ServiceName.Equals(otherService.ServiceName);
		}

		/// <summary>
		/// static function that returns a new Service from a <see cref="ConnectInfo"/>
		/// </summary>
		/// <param name="device">The device that discovered the new service</param>
		/// <param name="connectinfo">The <see cref="ConnectInfo"/> describing the new Service</param>
		/// <param name="io"><see cref="IIo"/> implementation for communication</param>
		/// <returns>a new service based on the <paramref name="connectinfo"/></returns>
		internal static IService GetServiceFromConnectInfo(IDevice device, ConnectInfo connectinfo, IIo io = null) {
			var leDevice = device as BluetoothDevice;
			if (leDevice == null || connectinfo == null) {
				SdkLogger.E(Tag, "Cannot instantiate service with no ConnectInfo or valid device");
				return new BluetoothGenericService(device, null, connectinfo);
			}
			if (io == null)
				io = leDevice.BluetoothIo;
			switch (connectinfo.TypeEnum) {
				case IoType.IoTypeMotor:
					return new BluetoothMotor(device, io, connectinfo);
				case IoType.IoTypeVoltage:
					return new BluetoothVoltageSensor(device, io, connectinfo);
				case IoType.IoTypeCurrent:
					return new BluetoothCurrentSensor(device, io, connectinfo);
				case IoType.IoTypePiezoTone:
					return new BluetoothPiezoTonePlayer(device, io, connectinfo);
				case IoType.IoTypeRgbLight:
					return new BluetoothRgbLight(device, io, connectinfo);
				case IoType.IoTypeTiltSensor:
					return new BluetoothTiltSensor(device, io, connectinfo);
				case IoType.IoTypeMotionSensor:
					return new BluetoothMotionSensor(device, io, connectinfo);
				case IoType.IoTypeGeneric:
					return new BluetoothGenericService(device, io, connectinfo);
				default:
					SdkLogger.E(Tag, string.Format("Tried to instantiate unknown Service with type {0}", (int)connectinfo.TypeEnum));
					return new BluetoothGenericService(device, io, connectinfo);
			}
		}

		internal void DidReceiveInputFormat(IIo io, InputFormat newInputFormat) {
			if (InputFormat == null || !newInputFormat.Equals(InputFormat) && ConnectInfo.ConnectId == newInputFormat.ConnectId) {
				HandleUpdatedInputFormat(newInputFormat);
			}
		}

		internal virtual void HandleUpdatedInputFormat(InputFormat newInputFormat) {
			InputFormat = newInputFormat;
			SendReadValueRequest();
		}

		internal void DidReceiveValueData(IIo io, byte[] newValueData) {
			if (!newValueData.Equals(ValueData)) {
				SdkError error;
				HandleUpdatedValueData(newValueData, out error);
				if (error != null) {
					SdkLogger.E(Tag, error.GetErrorMessage());
				}
			}
		}

		/// <summary>
		/// Verifies the Data and calls all delegates with the new data
		/// </summary>
		/// <param name="data">The new Data</param>
		/// <param name="error">The <see cref="SdkError"/></param>
		/// <returns>True if the data is correct</returns>
		internal virtual bool HandleUpdatedValueData(byte[] data, out SdkError error) {
			var dataDidVerify = VerifyData(data, out error);
			if (dataDidVerify) {
				var oldData = ValueData;
				ValueData = data;
				lock (serviceDelegates) {
					foreach (var serviceDelegate in serviceDelegates) {
						serviceDelegate.DidUpdateValueData(this, oldData, ValueData);
					}
				}
			}
			return dataDidVerify;
		}

		bool VerifyData(byte[] newValueData, out SdkError error) {
			error = null;
			if (newValueData == null || newValueData.Length == 0)
				return true;

			if (ValidDataFormats.Count == 0)
				return true;

			//If one or more InputDataFormats are defined, we look at the latest received InputFormat from the device
			//For a received value to be accepted, there:
			//1. Must exists an DataFormat that matches the latest received InputFormat from device
			//2. The received valueData length must match this DataFormat
			var dataFormat = DataFormatForInputFormat(inputFormat);
			if (dataFormat == null) {
				error = new SdkError(SdkError.LeDomain.DEVICE_ERROR_DOMAIN, SdkError.LeErrorCode.INTERNAL_ERROR, string.Format("Did not find a valid input data format.\nThe input format received from device is: {0}.\nSupported formats: {1}", InputFormat, ValidDataFormats));
				return false;
			}

			return VerifyDataToDataFormat(newValueData, dataFormat, out error);
		}


		DataFormat DataFormatForInputFormat(InputFormat newInputFormat) {
			foreach (var validDataFormat in ValidDataFormats) {
				if (validDataFormat.Mode == newInputFormat.Mode && validDataFormat.Unit == newInputFormat.Unit) {
					if ((validDataFormat.DataSetCount*validDataFormat.DataSetSize) != newInputFormat.NumberOfBytes) {
						SdkLogger.E(Tag, string.Format("{0} in mode {1} ({2}): expected data length is {3} data sets of {4} bytes input format received from device says {5} number of bytes",
																 ServiceName, validDataFormat.ModeName, validDataFormat.UnitName, validDataFormat.DataSetCount, validDataFormat.DataSetSize, inputFormat.NumberOfBytes));
						return null;
					}
					return validDataFormat;
				}
			}
			return null;
		}

		/// <summary>
		/// Verifies that the data is in accordance with the specified dataformats for this service
		/// </summary>
		/// <param name="newValueData">The new Value</param>
		/// <param name="dataFormat">The dataformat to check against</param>
		/// <param name="error">The error struct</param>
		/// <returns>True if data is according to the DataFromat specs</returns>
		bool VerifyDataToDataFormat(byte[] newValueData, DataFormat dataFormat, out SdkError error) {
			error = null;
			var didVerify = newValueData.Length == (dataFormat.DataSetSize * dataFormat.DataSetCount);
			if (!didVerify) {
				NumbersFromValueData = new List<byte[]>();
				SdkLogger.E(Tag, string.Format("Value for service {0} in mode {1} unit {2} is expected to have {3} data sets each with size {4} bytes, but did receive package with length {5}",
																 ServiceName, dataFormat.ModeName, dataFormat.UnitName, dataFormat.DataSetCount, dataFormat.DataSetSize, newValueData.Length));
				error = new SdkError(SdkError.LeDomain.DEVICE_ERROR_DOMAIN, SdkError.LeErrorCode.INTERNAL_ERROR,
					string.Format("Value for service {0} in mode {1} unit {2} is expected to have {3} data sets each with size {4} bytes, but did receive package with length {5}",
                                 ServiceName, dataFormat.ModeName, dataFormat.UnitName, dataFormat.DataSetCount, dataFormat.DataSetSize, newValueData.Length));
				return false;
			}
			// if the dataformat has a value fill the NumbersFromValueData array with all received numbers
			if (dataFormat.DataSetCount > 0) {
				var dataList = newValueData.ToList();
				var numbersList = new List<byte[]>();
				for (int i = 0; i < dataList.Count; i+=(int)dataFormat.DataSetSize) {
					numbersList.Add(dataList.GetRange(i, (int)dataFormat.DataSetSize).ToArray());
				}
				NumbersFromValueData = numbersList;
			}

			return didVerify;
		}

		internal ConnectInfo DidRequestConnectInfo(IIo io) {
			return ConnectInfo;
		}

	}
}
