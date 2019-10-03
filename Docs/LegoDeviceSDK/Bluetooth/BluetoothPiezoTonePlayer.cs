using System;
using LegoDeviceSDK.Generic;
using LegoDeviceSDK.Generic.Data;
using LegoDeviceSDK.Interfaces;
using LegoDeviceSDK.Interfaces.Services;

namespace LegoDeviceSDK.Bluetooth {

	/// <summary>
	/// This service allows for playing of tones at a given frequency
	/// </summary>
	public class BluetoothPiezoTonePlayer : BluetoothService, IPiezoTonePlayer {
		const string Tag = "BluetoothPiezoTonePlayer";
		/// <summary>
		/// Piezo tone player max Frequency
		/// </summary>
		public const int PiezoToneMaxFrequency = 1500;
		/// <summary>
		/// Piezo tone player max duration
		/// </summary>
    public const int PiezoToneMaxDuration = 65536;



		internal BluetoothPiezoTonePlayer(IDevice device, IIo io, ConnectInfo connectInfo)
			: base(device, io, connectInfo) {
				ServiceName = "Piezo Tone Player";

		}

		/// <summary>
		/// Play a tone a frequency for the given duration in ms
		/// </summary>
		/// <param name="frequency">The frequency to play (max allowed frequency is 1500)</param>
		/// <param name="duration">The duration to play (max supported is 65536 milli seconds).</param>
		public void PlayFrequency(int frequency, int duration) {
			if (frequency > PiezoToneMaxFrequency) {
				SdkLogger.W(Tag, string.Format("Cannot play frequenzy {0}, max supported frequency is {1}", frequency, PiezoToneMaxFrequency));
				frequency = PiezoToneMaxFrequency;
			}
			if (duration > PiezoToneMaxDuration) {
				SdkLogger.W(Tag, string.Format("Cannot play piezo tone with duration {0} ms, max supported frequency is {1} ms", duration, PiezoToneMaxDuration));
				duration = PiezoToneMaxDuration;
			}

			Io.WritePiezoToneFrequency(frequency, duration, ConnectInfo.ConnectId);
		}

		/// <summary>
		/// Play a note. The highest supported node is F# in 6th octave
		/// </summary>
		/// <param name="note">The note to play</param>
		/// <param name="octave">The octave in which to play the node</param>
		/// <param name="duration">The duration to play (max supported is 65536 milli seconds).</param>
		public void PlayNote(PiezoTonePlayerNote note, int octave, int duration) {
        if (octave > 6) {
					SdkLogger.W(Tag, string.Format("Highest supported note is F# in 6th octave - invalid octave: {0}", octave));
        }
        if (octave == 6 && (int)note > (int)PiezoTonePlayerNote.PiezoNoteFis) {
					SdkLogger.W(Tag, "Cannot play note. Highest supported note is F# in 6th octave");
        }

				// The basic formula for the frequencies of the notes of the equal tempered scale is given by
				// fn = f0 * (a)n
				// where
				// f0 = the frequency of one fixed note which must be defined. A common choice is setting the A above middle C (A4) at f0 = 440 Hz.
				// n = the number of half steps away from the fixed note you are. If you are at a higher note, n is positive. If you are on a lower note, n is negative.
				// fn = the frequency of the note n half steps away.
				// a = (2)1/12 = the twelfth root of 2 = the number which when multiplied by itself 12 times equals 2 = 1.059463094359...
        double baseTone = 440.0;
        int octavesAboveMiddle = octave - 4;
        float halfStepsAwayFromBase = (float) note - (float) PiezoTonePlayerNote.PiezoNoteA + (octavesAboveMiddle * 12);
				double frequency = baseTone * Math.Pow(Math.Pow(2.0, 1.0 / 12), halfStepsAwayFromBase);

        PlayFrequency((int) Math.Round(frequency), duration);
    }

		/// <summary>
		/// Stop playing any currently playing tone.
		/// </summary>
		public void StopPlaying() {
			Io.WritePiezoToneStop(ConnectInfo.ConnectId);
		}


	}
}
