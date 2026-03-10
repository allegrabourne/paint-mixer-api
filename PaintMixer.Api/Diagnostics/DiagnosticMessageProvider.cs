namespace PaintMixer.Api.Diagnostics
{
    /// <summary>
    /// Observable provider of diagnostic messages for error responses.
    /// Localisation is supported through the optional culture parameter, 
    /// which allows retrieval of messages in different languages based on the 
    /// specified culture code (e.g., "en" for English, "fr" for French).
    /// </summary>
    public sealed class DiagnosticMessageProvider : IDiagnosticMessageProvider
    {
        private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Messages =
            new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["invalid_request"] = "The request was invalid.",
                    ["invalid_paint_mix"] = "The paint mix is invalid. The total dye amount must not exceed 100.",
                    ["job_submission_failed"] = "The paint mixing job could not be submitted.",
                    ["job_not_found"] = "The requested paint mixing job could not be found.",
                    ["job_status_failed"] = "The paint mixing job status could not be retrieved.",
                    ["job_cancellation_failed"] = "The paint mixing job could not be cancelled.",
                    ["unexpected_error"] = "An unexpected error occurred while processing the request."
                },
                ["fr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["invalid_request"] = "La requête est invalide.",
                    ["invalid_paint_mix"] = "Le mélange de peinture est invalide. La quantité totale de colorant ne doit pas dépasser 100.",
                    ["job_submission_failed"] = "La tâche de mélange de peinture n'a pas pu être soumise.",
                    ["job_not_found"] = "La tâche de mélange de peinture demandée est introuvable.",
                    ["job_status_failed"] = "Le statut de la tâche de mélange de peinture n'a pas pu être récupéré.",
                    ["job_cancellation_failed"] = "La tâche de mélange de peinture n'a pas pu être annulée.",
                    ["unexpected_error"] = "Une erreur inattendue s'est produite lors du traitement de la requête."
                }
            };

        public string GetMessage(string code, string? culture = null)
        {
            var language = string.IsNullOrWhiteSpace(culture) ? "en" : culture;

            if (!Messages.TryGetValue(language, out var languageMessages))
            {
                languageMessages = Messages["en"];
            }

            return languageMessages.TryGetValue(code, out var message)
                ? message
                : languageMessages["unexpected_error"];
        }
    }
}
