using System;
using Windows.UI;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;
using LegoDeviceSDK.Generic.Data;

namespace LegoDeviceSDK.Bluetooth {
	/// <summary>
	/// This service allows for setting the colour of the RGB light on the device
	/// </summary>
	public class BluetoothRgbLight : BluetoothService, IRgbLight {
		const string Tag = "BluetoothRgbLight";

		internal BluetoothRgbLight(IDevice device, IIo io, ConnectInfo connectInfo)
			: base(device, io, connectInfo) {
			ServiceName = "RGB Light";
            AddValidDataFormats();
			DefaultInputFormat = new InputFormat() { ConnectId = ConnectInfo.ConnectId, DeltaInterval = 1, Mode = (int)RgbLightMode.Discrete, NumberOfBytes = 1, NotificationsEnabled = true, Unit = InputFormatUnit.InputFormatUnitRaw };
            color = DefaultColor;
		}

        /// <summary>
        /// The current mode of the RGB light
        /// </summary>
        public RgbLightMode RGBMode {
            get { return (RgbLightMode)InputFormatMode; }
            set { InputFormatMode = (int)value; }
        }

        Color color;
		/// <summary>
		/// The color of the RGB light on the device (absolute mode)
		/// </summary>
		public Color Color {
			get { return color; }
			set {
                if (RGBMode == RgbLightMode.Absolute) {
                    color = value;
                    Io.WriteColor(value.R, value.G, value.B, ConnectInfo.ConnectId);
                }
                else {
                    SdkLogger.W(Tag, "Ignoring attempt to set RGB color. It is only supported when RGB is in mode Absolute.");
                }
			}
		}

        /// <summary>
        /// The default color of the RGB light (absolute mode)
        /// </summary>
        public Color DefaultColor {
            get {
                // We have no reliable way of reading the default color of the Hub, so it is hardcoded here
                return Color.FromArgb(0xFF, 0x00, 0x00, 0xFF);
            }
        }

        int colorIndex;
        /// <summary>
        /// Index of the currently selected color (discrete mode)
        /// </summary>
        public int ColorIndex {
            get { return colorIndex; }
            set {
                if (RGBMode == RgbLightMode.Discrete) {
                    colorIndex = value;
                    Io.WriteColorIndex(value, ConnectInfo.ConnectId);
                }
                else {
                    SdkLogger.W(Tag, "Ignoring attempt to set RGB color index. It is only supported when RGB is in mode Discrete.");
                }
            }
        }

        /// <summary>
        /// The default color index of the RGB, when in the discrete mode
        /// </summary>
        public int DefaultColorIndex {
            get {
                // We have no reliable way of reading the default color of the Hub, so it is hardcoded here
                return 3;
            }
        }

		/// <summary>
		/// Switch off the RGB light on the device
		/// </summary>
		public void SwitchOff() {
            if (RGBMode == RgbLightMode.Absolute) {
                Color = Colors.Black;
            }
            else if (RGBMode == RgbLightMode.Discrete) {
                ColorIndex = 0;
            }
            else {
                SdkLogger.W(Tag, String.Format("Cannot switch off RGB - unknown mode: {0}", InputFormatMode));
            }
		}

        /// <summary>
        /// Switch to the default Color (i.e. the same color as the device has right after a successful connection has been established)
        /// </summary>
        public void SwitchToDefaultColor() {
            if (RGBMode == RgbLightMode.Absolute) {
                Color = DefaultColor;
            }
            else if (RGBMode == RgbLightMode.Discrete) {
                ColorIndex = DefaultColorIndex;
            }
            else {
                SdkLogger.W(Tag, String.Format("Cannot switch to default color - unknown mode: {0}", InputFormatMode));
            }
        }

		internal override bool HandleUpdatedValueData(byte[] data, out SdkError error) {
            if (RGBMode == RgbLightMode.Absolute) {
                Color oldColor = color;
                // check if data is a valid color
                if (!GetColorFromByteArray(data, out color)) {
                    error = new SdkError(SdkError.LeDomain.DEVICE_ERROR_DOMAIN, SdkError.LeErrorCode.INTERNAL_ERROR, string.Format("Cannot create color from data {0})", data));
                    return false;
                }

                if (!base.HandleUpdatedValueData(data, out error))
                    return false;
                lock (serviceDelegates) {
                    foreach (var serviceDelegate in serviceDelegates) {
                        var rgbDelegate = serviceDelegate as IRgbLightDelegate;
                        if (rgbDelegate != null)
                            rgbDelegate.DidUpdateColor(this, oldColor, color);
                    }
                }
            }
            else if (RGBMode == RgbLightMode.Discrete) {
                int oldColorIndex = colorIndex;
                colorIndex = IntegerFromData(data);

                if (!base.HandleUpdatedValueData(data, out error))
                    return false;
                lock (serviceDelegates) {
                    foreach (var serviceDelegate in serviceDelegates) {
                        var rgbDelegate = serviceDelegate as IRgbLightDelegate;
                        if (rgbDelegate != null)
                            rgbDelegate.DidUpdateColorIndex(this, oldColorIndex, colorIndex);
                    }
                }
            }
            else {
                error = new SdkError(SdkError.LeDomain.DEVICE_ERROR_DOMAIN, SdkError.LeErrorCode.INTERNAL_ERROR, String.Format("Cannot handle response for RGB in unknown mode {0}", InputFormatMode));
                SdkLogger.E(Tag, error.GetErrorMessage());
                return false;
            }

            error = null;
			return true;
		}

		bool GetColorFromByteArray(byte[] data, out Color newColor) {
			newColor = Colors.Black;
			if (data.Length != 3) {
				SdkLogger.E(Tag, string.Format("Cannot create color from data {0}", BitConverter.ToString(data)));
				return false;
			}
			newColor = Windows.UI.Color.FromArgb(255, data[0], data[1], data[2]);
			return true;
		}

        private void AddValidDataFormats() {
            AddValidDataFormat(DataFormat.FormatWithModeName("Discrete", (uint)RgbLightMode.Discrete, InputFormatUnit.InputFormatUnitRaw, 1, 1));
            AddValidDataFormat(DataFormat.FormatWithModeName("Discrete", (uint)RgbLightMode.Discrete, InputFormatUnit.InputFormatUnitPercentage, 1, 1));
            AddValidDataFormat(DataFormat.FormatWithModeName("Discrete", (uint)RgbLightMode.Discrete, InputFormatUnit.InputFormatUnitSi, 4, 1));

            AddValidDataFormat(DataFormat.FormatWithModeName("Absolute", (uint)RgbLightMode.Absolute, InputFormatUnit.InputFormatUnitRaw, 1, 3));
            AddValidDataFormat(DataFormat.FormatWithModeName("Absolute", (uint)RgbLightMode.Absolute, InputFormatUnit.InputFormatUnitPercentage, 1, 3));
            AddValidDataFormat(DataFormat.FormatWithModeName("Absolute", (uint)RgbLightMode.Absolute, InputFormatUnit.InputFormatUnitSi, 4, 3));
        }

	}
}
