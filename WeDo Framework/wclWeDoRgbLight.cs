using System;
using System.Windows.Media;

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> Mode of the RGB light device. </summary>
    public enum wclWeDoRgbLightMode
    {
        /// <summary> Discrete mode allows selecting a color index from a set of predefined colors. </summary>
        lmDiscrete = 0,
        /// <summary> Absolute mode allows selecting any color by specifying its RGB component values. </summary>
        lmAbsolute = 1,
        /// <summary> Unknown (unsupported) mode. </summary>
        lmUnknown = 255
    };

    /// <summary> A Lego WeDo color indexes. </summary>
    /// <remarks> This enumration is used in <c>Absolute</c> color mode. </remarks>
    /// <seealso cref="wclWeDoRgbLightMode"/>
    public enum wclWeDoColor
    {
        /// <summary> Black (none) color. </summary>
        clBlack = 0,
        /// <summary> Pink color. </summary>
        clPink = 1,
        /// <summary> Purple color. </summary>
        clPurple = 2,
        /// <summary> Blue color. </summary>
        clBlue = 3,
        /// <summary> Sky blue color. </summary>
        clSkyBlue = 4,
        /// <summary> Teal color. </summary>
        clTeal = 5,
        /// <summary> Green color. </summary>
        clGreen = 6,
        /// <summary> Yellow color. </summary>
        clYellow = 7,
        /// <summary> Orange color. </summary>
        clOrange = 8,
        /// <summary> Red color. </summary>
        clRed = 9,
        /// <summary> White color. </summary>
        clWhite = 10,
        /// <summary> Unknwon color index. </summary>
        clUnknown = 255
    };

    /// <summary> The class represents a HUB RGB light. </summary>
    public class wclWeDoRgbLight : wclWeDoIo
    {
        private Color FColor;
        private wclWeDoColor FColorIndex;

        /// <summary> The method called when Input Format has been changed. </summary>
        protected override void InputFormatChanged()
        {
            // Do nothign here.
        }

        /// <summary> The method called when data value has been changed. </summary>
        protected override void ValueChanged()
        {
            /*
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
		}*/

            /*bool GetColorFromByteArray(byte[] data, out Color newColor) {
			newColor = Colors.Black;
			if (data.Length != 3) {
				SdkLogger.E(Tag, string.Format("Cannot create color from data {0}", BitConverter.ToString(data)));
				return false;
			}
			newColor = Windows.UI.Color.FromArgb(255, data[0], data[1], data[2]);
			return true;
		}*/
        }

        /// <summary> Creates new RGB light device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoRgbLight(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoRgbLightMode.lmDiscrete, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(1, 1, (Byte)wclWeDoRgbLightMode.lmDiscrete, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(1, 4, (Byte)wclWeDoRgbLightMode.lmDiscrete, wclWeDoSensorDataUnit.suSi));

            AddValidDataFormat(new wclWeDoDataFormat(3, 1, (Byte)wclWeDoRgbLightMode.lmAbsolute, wclWeDoSensorDataUnit.suRaw));
            AddValidDataFormat(new wclWeDoDataFormat(3, 1, (Byte)wclWeDoRgbLightMode.lmAbsolute, wclWeDoSensorDataUnit.suPercentage));
            AddValidDataFormat(new wclWeDoDataFormat(3, 4, (Byte)wclWeDoRgbLightMode.lmAbsolute, wclWeDoSensorDataUnit.suSi));

            DefaultInputFormat = new wclWeDoInputFormat(ConnectionId, wclWeDoIoDeviceType.iodRgb, (Byte)wclWeDoRgbLightMode.lmDiscrete,
                1, wclWeDoSensorDataUnit.suRaw, true, 0, 1);

            FColor = DefaultColor;
            FColorIndex = DefaultColorIndex;
        }

        /// <summary> Switch off the RGB light on the device. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		public Int32 SwitchOff()
        {
            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
                return SetColor(Colors.Black);
            if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                return SetColorIndex(wclWeDoColor.clBlack);
            return wclErrors.WCL_E_INVALID_ARGUMENT;
        }

        /// <summary> Switches to the default Color (i.e. the same color as the device has right after a successful connection has
        ///   been established). </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 SwitchToDefaultColor()
        {
            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
                return SetColor(DefaultColor);
            if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                SetColorIndex(DefaultColorIndex);
            return wclErrors.WCL_E_INVALID_ARGUMENT;
        }

        /// <summary> Sets the RGB color. </summary>
        /// <param name="Rgb"> The RGB color. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="System.Windows.Media.Color"/>
        public Int32 SetColor(Color Rgb)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode != wclWeDoRgbLightMode.lmAbsolute)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            FColor = Rgb;
            return Hub.Io.WriteColor(Rgb.R, Rgb.G, Rgb.B, ConnectionId);
        }

        /// <summary> Sets the index of the currently selected color (discrete mode). </summary>
        /// <param name="Index"> The color index. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 SetColorIndex(wclWeDoColor Index)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode != wclWeDoRgbLightMode.lmDiscrete)
                return wclErrors.WCL_E_INVALID_ARGUMENT;

            FColorIndex = Index;
            return Hub.Io.WriteColorIndex((Byte)Index, ConnectionId);
        }

        /// <summary> Sets the mode of the RGB light. </summary>
        /// <param name="Mode"> The RGB lite mode. </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoRgbLightMode"/>
        public Int32 SetMode(wclWeDoRgbLightMode Mode)
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return SetInputFormatMode((Byte)Mode);
        }

        /// <summary> Gets the color of the RGB light on the device (absolute mode). </summary>
        /// <value> The RGB color. </value>
        /// <seealso cref="System.Windows.Media.Color"/>
        public Color Color { get { return FColor; } }
        /// <summary> Gets the index of the currently selected color (discrete mode). </summary>
        /// <value> The color index. </value>
        /// <seealso cref="wclWeDoColor"/>
        public wclWeDoColor ColorIndex { get { return FColorIndex; } }
        /// <summary> Gets the default color of the RGB light (absolute mode). </summary>
        /// <value> The default color. </value>
        /// <seealso cref="System.Windows.Media.Color"/>
        public Color DefaultColor { get { return Color.FromArgb(0xFF, 0x00, 0x00, 0xFF); } }
        /// <summary> Gets the default color index of the RGB, when in the discrete mode. </summary>
        /// <value> The default color index. </value>
        /// <seealso cref="wclWeDoColor"/>
        public wclWeDoColor DefaultColorIndex { get { return wclWeDoColor.clBlue; } }
        /// <summary> Gets the mode of the RGB light. </summary>
        /// <value> The RGB light device mode. </value>
        /// <seealso cref="wclWeDoRgbLightMode"/>
        public wclWeDoRgbLightMode Mode { get { return (wclWeDoRgbLightMode)InputFormatMode; } }
    };
}