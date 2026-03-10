using PaintMixer.Service.Interfaces;

using static PaintMixer.Service.PaintMixerEmulator;

namespace PaintMixer.Service
{
    /// <summary>
    /// Internal adapter class that allows the PaintMixerService to interact with the PaintMixerDeviceEmulator
    /// </summary>
    public sealed class PaintMixerDeviceAdapter : IPaintMixerDevice
    {
        private readonly PaintMixerDeviceEmulator _emulator;

        public PaintMixerDeviceAdapter()
        {
            _emulator = new PaintMixerDeviceEmulator();
        }

        public int SubmitJob(int red, int black, int white, int yellow, int blue, int green)
        {
            return _emulator.SubmitJob(red, black, white, yellow, blue, green);
        }

        public int CancelJob(int jobCode)
        {
            return _emulator.CancelJob(jobCode);
        }

        public int QueryJobState(int jobCode)
        {
            return _emulator.QueryJobState(jobCode);
        }
    }
}