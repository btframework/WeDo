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
    /// <summary> The sensor's data unit. </summary>
	public enum wclWeDoSensorDataUnit
    {
        /// <summary> Raw. </summary>
        suRaw,
        /// <summary> Percentage. </summary>
        suPercentage,
        /// <summary> SI. </summary>
        suSi,
        /// <summary> Unknown. </summary>
        suUnknown
    };

    /// <summary> This class contains info detailing how the data received for a given service (typically
    ///   a sensor of some kind) should be interpreted. </summary>
    public sealed class wclWeDoDataFormat
    {
        private Byte FDataSetCount;
        private Byte FDataSetSize;
        private Byte FMode;
        private wclWeDoSensorDataUnit FUnit;

        /// <summary> Creates a new instance of the Data Format class. </summary>
        /// <param name="DataSetCount"> The number of data sets. </param>
        /// <param name="DataSetSize"> The number of bytes in a data set. </param>
        /// <param name="Mode"> The sensor mode. </param>
        /// <param name="Unit"> The sensor data unit. </param>
        /// <seealso cref="wclWeDoSensorDataUnit"/>
        public wclWeDoDataFormat(Byte DataSetCount, Byte DataSetSize, Byte Mode,
            wclWeDoSensorDataUnit Unit)
        {
            FDataSetCount = DataSetCount;
            FDataSetSize = DataSetSize;
            FMode = Mode;
            FUnit = Unit;
        }

        /// <summary> Compares two Data Formats </summary>
		/// <param name="obj"> The object to be compared to the current one. </param>
		/// <returns> <c>True</c> if this data format is equal to <c>obj</c>. <c>False</c> otherwise. </returns>
        public override Boolean Equals(Object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (GetType() != obj.GetType())
                return false;

            wclWeDoDataFormat Format = (wclWeDoDataFormat)obj;
            return (FDataSetCount == Format.DataSetCount && FDataSetSize == Format.DataSetSize &&
                FMode == Format.Mode && FUnit == Format.Unit);
        }

        /// <summary> Gets the object hash. </summary>
        /// <returns> The objects hash. </returns>
        public override Int32 GetHashCode()
        {
            return ((FDataSetCount << 24) | (FDataSetSize << 16) | (FMode << 8) | (Byte)FUnit).GetHashCode();
        }

        /// <summary> Override the <c>==</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a == b. <c>False</c> otherwise. </returns>
        public static Boolean operator ==(wclWeDoDataFormat a, wclWeDoDataFormat b)
        {
            if (Object.ReferenceEquals(a, null))
                return Object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary> Override the <c>!=</c> operator. </summary>
        /// <param name="a"> First argument. </param>
        /// <param name="b"> Second argument. </param>
        /// <returns> <c>True</c> if a != b. <c>False</c> otherwise. </returns>
        public static Boolean operator !=(wclWeDoDataFormat a, wclWeDoDataFormat b)
        {
            return !(a == b);
        }

        /// <summary> Gets the data set count. </summary>
        /// <value> The data set count. </value>
		public Byte DataSetCount { get { return FDataSetCount; } }
        /// <summary> Gets the data set size. </summary>
        /// <value> The data set size. </value>
		public Byte DataSetSize { get { return FDataSetSize; } }
        /// <summary> Gets the sensor mode. </summary>
        /// <value> The sensor mode. </value>
        public Byte Mode { get { return FMode; } }
        /// <summary> Gets the sensor data unit. </summary>
        /// <value> The sensor data unit. </value>
        /// <seealso cref="wclWeDoSensorDataUnit"/>
		public wclWeDoSensorDataUnit Unit { get { return FUnit; } }
    };
}
