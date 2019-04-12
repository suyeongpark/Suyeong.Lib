using System;
using LemmaSharp;

namespace Suyeong.Lib.Text.Lemmatize
{
    public class Lemmatize
    {
        ILemmatizer lemmatizer;

        public Lemmatize(LanguagePrebuilt language = LanguagePrebuilt.English)
        {
            this.lemmatizer = new LemmatizerPrebuiltCompact(language);
        }

        public string GetLemmaWord(string lowerWord)
        {
            string result = string.Empty;

            try
            {
                result = this.lemmatizer.Lemmatize(lowerWord);
            }
            catch (Exception ex)
            {

            }

            return string.IsNullOrWhiteSpace(result) ? lowerWord : result;
        }
    }
}
