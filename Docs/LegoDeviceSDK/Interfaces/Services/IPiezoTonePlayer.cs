using System;

namespace LegoDeviceSDK.Interfaces.Services {
    /// <summary>
    /// This service allows for playing of tones at a given frequency
    /// </summary>
    public interface IPiezoTonePlayer : IService {
        /// <summary>
        /// Play a tone a frequency for the given duration in ms
        /// </summary>
        /// <param name="frequency">The frequency to play (max allowed frequency is 1500)</param>
        /// <param name="duration">The duration to play (max supported is 65536 milli seconds).</param>
        void PlayFrequency(int frequency, int duration);

        /// <summary>
        /// Play a note. The highest supported node is F# in 6th octave
        /// </summary>
        /// <param name="note">The note to play</param>
        /// <param name="octave">The octave in which to play the node</param>
        /// <param name="duration">The duration to play (max supported is 65536 milli seconds).</param>
        void PlayNote(LegoDeviceSDK.Generic.Data.PiezoTonePlayerNote note, int octave, int duration);

        /// <summary>
        /// Stop playing any currently playing tone.
        /// </summary>
        void StopPlaying();
    }
}
