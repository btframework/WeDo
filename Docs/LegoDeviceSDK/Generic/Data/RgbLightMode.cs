namespace LegoDeviceSDK.Generic.Data {
    /// <summary>
    /// Mode of the RGB light
    /// </summary>
    public enum RgbLightMode {
        /// <summary>
        /// Discrete mode allows selecting a color index from a set of predefined colors
        /// </summary>
        Discrete = 0,
        /// <summary>
        /// Absolute mode allows selecting any color by specifying its RGB component values
        /// </summary>
        Absolute = 1,
        /// <summary>
        /// Unknown (unsupported) mode
        /// </summary>
        Unknown
    }
}
