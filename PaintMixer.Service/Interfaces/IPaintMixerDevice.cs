namespace PaintMixer.Service.Interfaces
{
    public interface IPaintMixerDevice
    {
        int SubmitJob(
            int red = 0,
            int black = 0,
            int white = 0,
            int yellow = 0,
            int blue = 0,
            int green = 0);

        int CancelJob(int jobCode);

        int QueryJobState(int jobCode);
    }
}
