using System;
using LemmaSharp;

namespace Suyeong.Lib.Text.Lemmatize
{
    public static class Lemmatize
    {
        static ILemmatizer _lemmatizer;
        static ILemmatizer Lemmatizer
        {
            get
            {
                if (_lemmatizer == null)
                {
                    _lemmatizer = new LemmatizerPrebuiltCompact(LanguagePrebuilt.English);
                }

                return _lemmatizer;
            }
        }

        public static void Init(LanguagePrebuilt langage)
        {
            _lemmatizer = new LemmatizerPrebuiltCompact(langage);
        }

        public static string GetLemmaWord(string word)
        {
            string result = string.Empty;

            try
            {
                result = Lemmatizer.Lemmatize(word.ToLower());
            }
            catch (Exception)
            {
                throw;
            }

            return string.IsNullOrWhiteSpace(result) ? word : result;
        }

        public static string GetLemmaWordWithLowerWord(string lowerWord)
        {
            string result = string.Empty;

            try
            {
                result = Lemmatizer.Lemmatize(lowerWord);
            }
            catch (Exception)
            {
                throw;
            }

            return string.IsNullOrWhiteSpace(result) ? lowerWord : result;
        }
    }
}
