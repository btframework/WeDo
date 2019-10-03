using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoDeviceSDK.Helpers {
	internal static class ValueHelper {

		/// <summary>
		/// Convert a byte to an unsigned int
		/// </summary>
		/// <param name="b">The unsigned int in byte form</param>
		/// <returns>An unsigned int</returns>
		internal static int ConvertToUnsigned(byte b) {
			return BitConverter.ToInt16(new byte[] { b, 0 }, 0);
		}

		/// <summary>
		/// Convert a byte to a signed int
		/// </summary>
		/// <param name="b">The signed int in byte form</param>
		/// <returns>A signed int</returns>
		internal static int ConvertToSigned(byte b) {
			var signed = (int) b;
			if (signed > 127) {
				signed = 0 - (256 - signed);
			}
			return signed;
		}
	}
}
