using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Server.Localization
{
    /// <summary>
    /// Provides translation utilities for converting player speech into the server's base language.
    /// Currently this is a stub that returns the original text.
    /// </summary>
    public static class TranslationService
    {
        private const string TargetLanguage = "es";

        /// <summary>
        /// Translate text from the player's language to the server's default language.
        /// </summary>
        /// <param name="text">The player's original speech.</param>
        /// <returns>The translated text suitable for server processing.</returns>
        public static string TranslateToServerLanguage(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            try
            {
                var url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={0}&dt=t&q={1}", TargetLanguage, Uri.EscapeDataString(text));

                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0");

                    var response = client.DownloadString(url);

                    // Response format: [[[\"Hola Mundo\",\"Hello world\",null,null,10]],null,\"en\",...]
                    var translationMatch = Regex.Match(response, "\\[\\[\\[\"(?<translated>.+?)\"");
                    var languageMatch = Regex.Match(response, "\\],null,\"(?<lang>[^\"]+)\"");

                    if (translationMatch.Success)
                    {
                        var translated = translationMatch.Groups["translated"].Value;

                        if (languageMatch.Success && languageMatch.Groups["lang"].Value.StartsWith(TargetLanguage, StringComparison.OrdinalIgnoreCase))
                        {
                            return text; // Already in target language
                        }

                        return translated;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Translation error: " + e.Message);
            }

            return text;
        }
    }
}
