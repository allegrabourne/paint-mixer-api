namespace PaintMixer.Api.Diagnostics
{
    public interface IDiagnosticMessageProvider
    {
        string GetMessage(string code, string? culture = null);
    }
}
