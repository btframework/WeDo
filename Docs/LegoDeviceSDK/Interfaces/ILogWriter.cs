using LegoDeviceSDK.Generic;

namespace LegoDeviceSDK.Interfaces {
	/// <summary>
	/// You may provide an implementation of this protocol to the <see cref="SdkLogger"/> to have all log from the LEGO Device SDK written to a custom destination (for instance a remote logging server).
	/// </summary>
	public interface ILogWriter {

		/// <summary>
		/// Writes a message from the LEGO Device SDK to a custom logging destination.
		/// </summary>
		/// <param name="message">The message that will be written to the logging destination</param>
		/// <param name="level">The log level</param>
		void WriteMessage(string message, LoggerLevel level);

	}
}
