using LemmaSharp;

namespace Suyeong.Lib.Text.Lemmatize
{
    public class Lemmatize
    {
        ILemmatizer lmtz;

        public Lemmatize(LanguagePrebuilt language = LanguagePrebuilt.English)
        {
            this.lmtz = new LemmatizerPrebuiltCompact(language);
        }

        public string GetLemmaWord(string lowerWord)
        {
            return this.lmtz.Lemmatize(lowerWord);
        }
    }
}
