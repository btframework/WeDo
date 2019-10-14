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

using wclCommon;
using wclBluetooth;

namespace wclWeDoFramework
{
    /// <summary> Tones that can be played using the <see cref="wclWeDoPieazo"/> </summary>
	public enum wclWeDoPiezoNote
    {
        /// <summary> C </summary>
        pnC = 1,
        /// <summary> C# </summary>
        pnCis = 2,
        /// <summary> D </summary>
        pnD = 3,
        /// <summary> D# </summary>
        pnDis = 4,
        /// <summary> E </summary>
        pnE = 5,
        /// <summary> F </summary>
        pnF = 6,
        /// <summary> F# </summary>
        pnFis = 7,
        /// <summary> G </summary>
        pnG = 8,
        /// <summary> G# </summary>
        pnGis = 9,
        /// <summary> A </summary>
        pnA = 10,
        /// <summary> A# </summary>
        pnAis = 11,
        /// <summary> B </summary>
        pnB = 12
    };

    /// <summary> The class represents a Piezo tone player device. </summary>
    /// <seealso cref="wclWeDoIo"/>
    public class wclWeDoPieazo : wclWeDoIo
    {
        private const UInt16 PIEZO_MAX_FREQUENCY = 1500;
        private const UInt16 PIEZO_MAX_DURATION = 65535;

        /// <summary> Creates new Piezo device object. </summary>
        /// <param name="Hub"> The Hub object that owns the device. If this parameter is <c>null</c>
        ///   the <seealso cref="wclEInvalidArgument"/> exception raises. </param>
        /// <param name="ConnectionId"> The device's Connection ID. </param>
        /// <seealso cref="wclWeDoHub"/>
        /// <exception cref="wclEInvalidArgument"> The exception raises when the <c>Hub</c>
        ///   parameter is <c>null</c>. </exception>
        public wclWeDoPieazo(wclWeDoHub Hub, Byte ConnectionId)
            : base(Hub, ConnectionId)
        {
        }

        /// <summary> Plays a tone with a given frequency for the given duration in ms. </summary>
		/// <param name="Frequency"> The frequency to play (max allowed frequency is 1500). </param>
		/// <param name="Duration"> The duration to play (max supported is 65535 milli seconds). </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 PlayTone(UInt16 Frequency, UInt16 Duration)
        {
            if (Frequency > PIEZO_MAX_FREQUENCY || Duration > PIEZO_MAX_DURATION)
                return wclErrors.WCL_E_INVALID_ARGUMENT;
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return Hub.Io.PiezoPlayTone(Frequency, Duration, ConnectionId);
        }

        /// <summary> Plays a note. The highest supported node is F# in 6th octave. </summary>
        /// <param name="Note"> The note to play. </param>
        /// <param name="Octave"> The octave in which to play the node. </param>
        /// <param name="Duration"> The duration to play (max supported is 65535 milli seconds). </param>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        /// <seealso cref="wclWeDoPiezoNote"/>
        public Int32 PlayNote(wclWeDoPiezoNote Note, Byte Octave, UInt16 Duration)
        {
            if (Octave == 0 || Octave > 6 || (Octave == 6 && Note > wclWeDoPiezoNote.pnFis))
                return wclErrors.WCL_E_INVALID_ARGUMENT;
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            // The basic formula for the frequencies of the notes of the equal tempered scale is given by
            // fn = f0 * (a)n
            // where
            //   f0 - the frequency of one fixed note which must be defined.
            //        A common choice is setting the A above middle C (A4) at f0 = 440 Hz.
            //   n  - the number of half steps away from the fixed note you are. If you are at a higher note,
            //        n is positive. If you are on a lower note, n is negative.
            //   fn - the frequency of the note n half steps away.
            //   a  - (2)1/12 = the twelfth root of 2 = the number which when multiplied by itself 12 times
            //        equals 2 = 1.059463094359...
            Double BaseTone = 440.0;
            Int32 OctavesAboveMiddle = Octave - 4;
            float HalfStepsAwayFromBase = (float)Note - (float)wclWeDoPiezoNote.pnA + (OctavesAboveMiddle * 12);
            Double Frequency = BaseTone * Math.Pow(Math.Pow(2.0, 1.0 / 12), HalfStepsAwayFromBase);

            return Hub.Io.PiezoPlayTone((UInt16)Math.Round(Frequency), Duration, ConnectionId);
        }

        /// <summary> Stop playing any currently playing tone. </summary>
        /// <returns> If the method completed with success the returning value is
        ///   <see cref="wclErrors.WCL_E_SUCCESS" />. If the method failed the returning value is
        ///   one of the Bluetooth Framework error code. </returns>
        public Int32 StopPlaying()
        {
            if (!Attached)
                return wclBluetoothErrors.WCL_E_BLUETOOTH_DEVICE_NOT_INSTALLED;

            return Hub.Io.PiezoStopPlaying(ConnectionId);
        }
    };
}
