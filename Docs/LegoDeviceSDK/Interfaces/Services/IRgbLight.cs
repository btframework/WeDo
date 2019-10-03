using LegoDeviceSDK.Generic.Data;
using System;
using Windows.UI;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    /// This service allows for setting the colour of the RGB light on the device
    /// </summary>
    public interface IRgbLight : IService {
        /// <summary>
        /// The current mode of the RGB light
        /// </summary>
        RgbLightMode RGBMode { get; set; }

        /// <summary>
        /// The color of the RGB light on the device.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// The default color of the RGB light (absolute mode)
        /// </summary>
        Color DefaultColor { get; }

        /// <summary>
        /// Index of the currently selected color (discrete mode)
        /// </summary>
        int ColorIndex { get; set; }

        /// <summary>
        /// The default color index of the RGB, when in the discrete mode
        /// </summary>
        int DefaultColorIndex { get; }

        /// <summary>
        /// Switch off the RGB light on the device
        /// </summary>
        void SwitchOff();

        /// <summary>
        /// Switch to the default Color (i.e. the same color as the device has right after a successful connection has been established)
        /// </summary>
        void SwitchToDefaultColor();
    }
}
