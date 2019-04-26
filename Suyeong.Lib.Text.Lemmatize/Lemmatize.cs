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

        public string GetLemmaWord(string word)
        {
            string result = string.Empty;

            try
            {
                result = this.lemmatizer.Lemmatize(word.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.IsNullOrWhiteSpace(result) ? word : result;
        }

        public string GetLemmaWordWithLowerWord(string lowerWord)
        {
            string result = string.Empty;

            try
            {
                result = this.lemmatizer.Lemmatize(lowerWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return string.IsNullOrWhiteSpace(result) ? lowerWord : result;
        }
    }
}
