namespace LegoDeviceSDK.Generic {
	/// <summary>
	/// Holds revision info to represent for example the hardware and firmware revisions of a device and attached IOs (services).
	/// </summary>
	public class Revision {
		/// <summary>
		/// The bug fix version number
		/// </summary>
		public int BugFixVersion { get; private set; }
		/// <summary>
		/// The build number
		/// </summary>
		public int BuildNumber { get; private set; }
		/// <summary>
		/// The build number
		/// </summary>
		public int MajorVersion { get; private set; }
		/// <summary>
		/// The major version number
		/// </summary>
		public int MinorVersion { get; private set; }
		/// <summary>
		/// The minor version number
		/// </summary>
		public string StringRepresentation { get { return this.ToString(); }}

		/// <summary>
		/// Return YES if this revision is equal to otherRevision
		/// </summary>
		/// <param name="otherRevision">The object to compare to</param>
		/// <returns>True if objects are equal</returns>
		public bool Equals(Revision otherRevision) {
			return BugFixVersion == otherRevision.BugFixVersion &&
						 BuildNumber == otherRevision.BuildNumber &&
						 MajorVersion == otherRevision.MajorVersion &&
						 MinorVersion == otherRevision.MinorVersion;
		}

		/// <summary>
		/// A formatted string representation of the revision
		/// </summary>
		/// <returns>A formatted string representation of the revision</returns>
		public override string ToString() {
			return string.Format("{0}.{1}.{2}.{3}", MajorVersion, MinorVersion, BugFixVersion, BuildNumber);
		}

		/// <summary>
		/// Get revision from byte array. used for Lego specific services
		/// </summary>
		/// <param name="data">The Revision as byte array</param>
		/// <returns>The Revision from the byte array</returns>
		internal static Revision FromByteArray(byte[] data) {
			var newRevision = new Revision();
			newRevision.MajorVersion = data[0];
			newRevision.MinorVersion = data[1];
			newRevision.BugFixVersion = data[2];
			newRevision.BuildNumber = data[3];
			return newRevision;
		}

		/// <summary>
		/// Get revision from string in byte array. Used for generic services
		/// </summary>
		/// <param name="data">The Revision as string in a byte array</param>
		/// <returns>The Revision from the string</returns>
		internal static Revision FromString(byte[] data) {
			var stringRep = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
			var tokens = stringRep.Split('.');
			var newRevision = new Revision();
			if (tokens.Length >= 1)
				newRevision.MajorVersion = int.Parse(tokens[0]);
			if (tokens.Length >= 2)
				newRevision.MinorVersion = int.Parse(tokens[1]);
			if (tokens.Length >= 3)
				newRevision.BugFixVersion = int.Parse(tokens[2]);
			if (tokens.Length >= 4)
				newRevision.BuildNumber = int.Parse(tokens[3]);
			return newRevision;
		}
	}
}
