using Windows.UI;
using LegoDeviceSDK.Bluetooth;

namespace LegoDeviceSDK.Interfaces.Services {
	/// <summary>
    /// Implement this protocol to be notified when the <see cref="IRgbLight"/> updates its value
	/// </summary>
	public interface IRgbLightDelegate : IServiceDelegate {

		/// <summary>
        /// Invoked when the <see cref="IRgbLight"/> service receives an updated value
		/// </summary>
		/// <param name="rgbLight">The RGB light</param>
		/// <param name="oldColor">The previous color</param>
		/// <param name="newColor">The new color</param>
        void DidUpdateColor(IRgbLight rgbLight, Color oldColor, Color newColor);

        /// <summary>
        /// Invoked when the <see cref="IRgbLight"/> service receives an updated color index
        /// </summary>
        /// <param name="rgbLight">The RGB light</param>
        /// <param name="oldColorIndex">The previous color index</param>
        /// <param name="newColorIndex">The new color index</param>
        void DidUpdateColorIndex(IRgbLight rgbLight, int oldColorIndex, int newColorIndex);
	}
}
