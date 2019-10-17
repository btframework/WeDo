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
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoRgbLight : wclWeDoIo
    {
        private Color FColor;
        private wclWeDoColor FColorIndex;

        private Boolean GetColorFromByteArray(Byte[] Data, out Color Color)
        {
            Color = Colors.Black;
            if (Data == null || Data.Length != 3)
                return false;

            Color = Color.FromRgb(Data[0], Data[1], Data[2]);
            return true;
        }

        /// <summary> The method called when Input Format has been changed. </summary>
        /// <param name="OldFormat"> The old Input Format. </param>
        protected override void InputFormatChanged(wclWeDoInputFormat OldFormat)
        {
            if (InputFormat != null)
            {

                if (OldFormat == null)
                    DoModeChanged();
                else
                {
                    if (InputFormat.Mode != OldFormat.Mode)
                        DoModeChanged();
                }
            }
        }

        /// <summary> The method called when data value has been changed. </summary>
        protected override void ValueChanged()
        {
            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
            {
                Color NewColor;
                if (!GetColorFromByteArray(Value, out NewColor))
                    return;
                if (NewColor.Equals(FColor))
                    return;
                FColor = NewColor;
                DoColorChanged();
            }
            else
            {
                if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                {
                    wclWeDoColor NewColorIndex = (wclWeDoColor)AsInteger;
                    if (NewColorIndex != FColorIndex)
                    {
                        FColorIndex = NewColorIndex;
                        DoColorChanged();
                    }
                }
            }
        }

        /// <summary> Fires the <c>OnColorChanged</c> event. </summary>
        protected virtual void DoColorChanged()
        {
            if (OnColorChanged != null)
                OnColorChanged(this, EventArgs.Empty);
        }

        /// <summary> Fires the <c>OnModeChanged</c> event. </summary>
        protected virtual void DoModeChanged()
        {
            if (OnModeChanged != null)
                OnModeChanged(this, EventArgs.Empty);
        }

        /// <summary> Creates new RGB light device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
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

            OnColorChanged = null;
            OnModeChanged = null;
        }

        /// <summary> Switch off the RGB light on the device. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
		public Int32 SwitchOff()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

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
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            if (Mode == wclWeDoRgbLightMode.lmAbsolute)
                return SetColor(DefaultColor);
            if (Mode == wclWeDoRgbLightMode.lmDiscrete)
                return SetColorIndex(DefaultColorIndex);
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
        /// <summary> The event fires when color has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnColorChanged;

        /// <summary> The event fired when the RGB LED mode has been changed. </summary>
        /// <seealso cref="EventHandler"/>
        public event EventHandler OnModeChanged;
    };
}
