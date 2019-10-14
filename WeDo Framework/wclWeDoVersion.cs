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

namespace wclWeDoFramework
{
    /// <summary> The structure describes the device version number. </summary>
    public struct wclWeDoVersion
    {
        internal static wclWeDoVersion FromByteArray(Byte[] Data)
        {
            wclWeDoVersion Version = new wclWeDoVersion();
            Version.MajorVersion = Data[0];
            Version.MinorVersion = Data[1];
            Version.BugFixVersion = Data[2];
            Version.BuildNumber = Data[3];
            return Version;
        }

        /// <summary> The bug fix version number. </summary>
		public Byte BugFixVersion;
        /// <summary> The build number. </summary>
		public Byte BuildNumber;
        /// <summary> The build number. </summary>
		public Byte MajorVersion;
        /// <summary> The major version number. </summary>
		public int MinorVersion;

        /// <summary> Compares two versions. </summary>
        /// <param name="obj"> The other object to be compared with current. </param>
        /// <returns> <c>True</c> if this version is equal to <c>obj</c>.
        ///   <c>False</c> otherwise. </returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;

            wclWeDoVersion Version = (wclWeDoVersion)obj;
            return (Version.BugFixVersion == BugFixVersion && Version.BuildNumber == BuildNumber &&
                Version.MajorVersion == MajorVersion && Version.MinorVersion == MinorVersion);
        }

        /// <summary> Gets the hash code for the current version. </summary>
        /// <returns> The hash code. </returns>
        public override Int32 GetHashCode()
        {
            Int32 Hash = BugFixVersion.GetHashCode();
            Hash = (Hash * 397) ^ BuildNumber.GetHashCode();
            Hash = (Hash * 397) ^ MajorVersion.GetHashCode();
            Hash = (Hash * 397) ^ MinorVersion.GetHashCode();
            return Hash;
        }

        /// <summary> Override th <c>==</c> operator. </summary>
        /// <param name="a"> The first argument for comparison. </param>
        /// <param name="b"> The second argument for comparison. </param>
        /// <returns> <c>True</c> if <c>a==b</c>. <c>False</c> otherwise. </returns>
        /// <seealso cref="wclWeDoVersion"/>
        public static Boolean operator ==(wclWeDoVersion a, wclWeDoVersion b)
        {
            return Equals(a, b);
        }

        /// <summary> Override th <c>!=</c> operator. </summary>
        /// <param name="a"> The first argument for comparison. </param>
        /// <param name="b"> The second argument for comparison. </param>
        /// <returns> <c>True</c> if <c>a!=b</c>. <c>False</c> otherwise. </returns>
        /// <seealso cref="wclWeDoVersion"/>
        public static Boolean operator !=(wclWeDoVersion a, wclWeDoVersion b)
        {
            return (!(a==b));
        }

        /// <summary> A formatted string representation of the version. </summary>
		/// <returns> A formatted string representation of the version </returns>
		public override String ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", MajorVersion, MinorVersion, BugFixVersion, BuildNumber);
        }
    };
}
