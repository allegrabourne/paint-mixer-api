using PaintMixer.Service.Interfaces;

using static PaintMixer.Service.PaintMixerEmulator;

namespace PaintMixer.Service
{
    /// <summary>
    /// Internal adapter class that allows the PaintMixerService to interact with the PaintMixerDeviceEmulator
    /// </summary>
    internal sealed class PaintMixerDeviceAdapter(PaintMixerEmulator.PaintMixerDeviceEmulator emulator) : IPaintMixerDevice
    {
        private readonly PaintMixerDeviceEmulator _emulator = emulator ?? throw new ArgumentNullException(nameof(emulator));

        public int SubmitJob(int red = 0, int black = 0, int white = 0, int yellow = 0, int blue = 0, int green = 0)
            => _emulator.SubmitJob(red, black, white, yellow, blue, green);

        public int CancelJob(int jobCode)
            => _emulator.CancelJob(jobCode);

        public int QueryJobState(int jobCode)
            => _emulator.QueryJobState(jobCode);
    }
}
