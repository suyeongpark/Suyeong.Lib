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
            return this.lemmatizer.Lemmatize(lowerWord);
        }
    }
}
