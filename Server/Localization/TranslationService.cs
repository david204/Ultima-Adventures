using System;

namespace Server.Localization
{
    /// <summary>
    /// Provides translation utilities for converting player speech into the server's base language.
    /// Currently this is a stub that returns the original text.
    /// </summary>
    public static class TranslationService
    {
        /// <summary>
        /// Translate text from the player's language to the server's default language.
        /// </summary>
        /// <param name="text">The player's original speech.</param>
        /// <returns>The translated text suitable for server processing.</returns>
        public static string TranslateToServerLanguage(string text)
        {
            return text;
        }
    }
}
