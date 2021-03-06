using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Suyeong.Lib.Google.Shared;

namespace Suyeong.Lib.Google.Translate
{
    public static class GoogleTranslate
    {
        const string TRANSLATE_URI = "http://translate.google.com.tr/m?hl=en&sl={0}&tl={1}&ie=UTF-8&prev=_m&q={2}";

        public static string GetTranslate(GoogleLanguage fromLang, GoogleLanguage toLang, string text)
        {
            string translate = string.Empty;

            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                wc.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                wc.Encoding = Encoding.UTF8;

                string fromLangStr, toLangStr;

                if (LanguageDic.TryGetValue(fromLang, out fromLangStr) && LanguageDic.TryGetValue(toLang, out toLangStr))
                {
                    string url = string.Format(CultureInfo.InvariantCulture, TRANSLATE_URI, new string[] { fromLangStr, toLangStr, WebUtility.UrlEncode(text), });

                    string result = wc.DownloadString(url);

                    result = result.Remove(0, result.IndexOf(@"<div dir=""ltr"" class=""t0"">", StringComparison.InvariantCulture)).Replace(@"<div dir=""ltr"" class=""t0"">", "");
                    int last = result.IndexOf("</div>", StringComparison.InvariantCulture);
                    result = result.Remove(last, result.Length - last);
                    result = result.Replace("\x3cbr\x3e", "");
                    result = result.Replace("\x26amp;", "&");
                    result = result.Replace("\x26quot;", @"""");
                    result = result.Replace("\x26#39;", "'");
                    result = result.Replace("&#8217;", "'");
                    result = result.Replace("&#8225;", "\t");
                    result = result.Replace("\r", Environment.NewLine);

                    translate = WebUtility.HtmlDecode(result);
                }
            }

            return translate;
        }

        static Dictionary<GoogleLanguage, string> _languageDic;
        static Dictionary<GoogleLanguage, string> LanguageDic
        {
            get
            {
                if (_languageDic == null)
                {
                    _languageDic = new Dictionary<GoogleLanguage, string>();

                    _languageDic.Add(GoogleLanguage.Afrikaans, "af");
                    _languageDic.Add(GoogleLanguage.Albanian, "sq");
                    _languageDic.Add(GoogleLanguage.Amharic, "am");
                    _languageDic.Add(GoogleLanguage.Arabic, "ar");
                    _languageDic.Add(GoogleLanguage.Armenian, "hy");
                    _languageDic.Add(GoogleLanguage.Azerbaijani, "az");
                    _languageDic.Add(GoogleLanguage.Basque, "eu");
                    _languageDic.Add(GoogleLanguage.Belarusian, "be");
                    _languageDic.Add(GoogleLanguage.Bengali, "bn");
                    _languageDic.Add(GoogleLanguage.Bosnian, "bs");
                    _languageDic.Add(GoogleLanguage.Bulgarian, "bg");
                    _languageDic.Add(GoogleLanguage.Catalan, "ca");
                    _languageDic.Add(GoogleLanguage.Cebuano, "ceb");
                    _languageDic.Add(GoogleLanguage.Chichewa, "ny");
                    _languageDic.Add(GoogleLanguage.ChineseSimplified, "zh");
                    _languageDic.Add(GoogleLanguage.ChineseTraditional, "zh-TW");
                    _languageDic.Add(GoogleLanguage.Corsican, "co");
                    _languageDic.Add(GoogleLanguage.Croatian, "hr");
                    _languageDic.Add(GoogleLanguage.Czech, "cs");
                    _languageDic.Add(GoogleLanguage.Danish, "da");
                    _languageDic.Add(GoogleLanguage.Dutch, "nl");
                    _languageDic.Add(GoogleLanguage.English, "en");
                    _languageDic.Add(GoogleLanguage.Esperanto, "eo");
                    _languageDic.Add(GoogleLanguage.Estonian, "et");
                    _languageDic.Add(GoogleLanguage.Filipino, "tl");
                    _languageDic.Add(GoogleLanguage.Finnish, "fi");
                    _languageDic.Add(GoogleLanguage.French, "fr");
                    _languageDic.Add(GoogleLanguage.Frisian, "fy");
                    _languageDic.Add(GoogleLanguage.Galician, "gl");
                    _languageDic.Add(GoogleLanguage.Georgian, "ka");
                    _languageDic.Add(GoogleLanguage.German, "de");
                    _languageDic.Add(GoogleLanguage.Greek, "el");
                    _languageDic.Add(GoogleLanguage.Gujarati, "gu");
                    _languageDic.Add(GoogleLanguage.HaitianCreole, "ht");
                    _languageDic.Add(GoogleLanguage.Hausa, "ha");
                    _languageDic.Add(GoogleLanguage.Hawaiian, "haw");
                    _languageDic.Add(GoogleLanguage.Hebrew, "iw");
                    _languageDic.Add(GoogleLanguage.Hindi, "hi");
                    _languageDic.Add(GoogleLanguage.Hmong, "hmn");
                    _languageDic.Add(GoogleLanguage.Hungarian, "hu");
                    _languageDic.Add(GoogleLanguage.Icelandic, "is");
                    _languageDic.Add(GoogleLanguage.Igbo, "ig");
                    _languageDic.Add(GoogleLanguage.Indonesian, "id");
                    _languageDic.Add(GoogleLanguage.Irish, "ga");
                    _languageDic.Add(GoogleLanguage.Italian, "it");
                    _languageDic.Add(GoogleLanguage.Japanese, "ja");
                    _languageDic.Add(GoogleLanguage.Javanese, "jw");
                    _languageDic.Add(GoogleLanguage.Kannada, "kn");
                    _languageDic.Add(GoogleLanguage.Kazakh, "kk");
                    _languageDic.Add(GoogleLanguage.Khmer, "km");
                    _languageDic.Add(GoogleLanguage.Korean, "ko");
                    _languageDic.Add(GoogleLanguage.KurdishKurmanji, "ku");
                    _languageDic.Add(GoogleLanguage.Kyrgyz, "ky");
                    _languageDic.Add(GoogleLanguage.Lao, "lo");
                    _languageDic.Add(GoogleLanguage.Latin, "la");
                    _languageDic.Add(GoogleLanguage.Latvian, "lv");
                    _languageDic.Add(GoogleLanguage.Lithuanian, "lt");
                    _languageDic.Add(GoogleLanguage.Luxembourgish, "lb");
                    _languageDic.Add(GoogleLanguage.Macedonian, "mk");
                    _languageDic.Add(GoogleLanguage.Malagasy, "mg");
                    _languageDic.Add(GoogleLanguage.Malay, "ms");
                    _languageDic.Add(GoogleLanguage.Malayalam, "ml");
                    _languageDic.Add(GoogleLanguage.Maltese, "mt");
                    _languageDic.Add(GoogleLanguage.Maori, "mi");
                    _languageDic.Add(GoogleLanguage.Marathi, "mr");
                    _languageDic.Add(GoogleLanguage.Mongolian, "mn");
                    _languageDic.Add(GoogleLanguage.MyanmarBurmese, "my");
                    _languageDic.Add(GoogleLanguage.Nepali, "ne");
                    _languageDic.Add(GoogleLanguage.Norwegian, "no");
                    _languageDic.Add(GoogleLanguage.Pashto, "ps");
                    _languageDic.Add(GoogleLanguage.Persian, "fa");
                    _languageDic.Add(GoogleLanguage.Polish, "pl");
                    _languageDic.Add(GoogleLanguage.Portuguese, "pt");
                    _languageDic.Add(GoogleLanguage.Punjabi, "pa");
                    _languageDic.Add(GoogleLanguage.Romanian, "ro");
                    _languageDic.Add(GoogleLanguage.Russian, "ru");
                    _languageDic.Add(GoogleLanguage.Samoan, "sm");
                    _languageDic.Add(GoogleLanguage.ScotsGaelic, "gd");
                    _languageDic.Add(GoogleLanguage.Serbian, "sr");
                    _languageDic.Add(GoogleLanguage.Sesotho, "st");
                    _languageDic.Add(GoogleLanguage.Shona, "sn");
                    _languageDic.Add(GoogleLanguage.Sindhi, "sd");
                    _languageDic.Add(GoogleLanguage.Sinhala, "si");
                    _languageDic.Add(GoogleLanguage.Slovak, "sk");
                    _languageDic.Add(GoogleLanguage.Slovenian, "sl");
                    _languageDic.Add(GoogleLanguage.Somali, "so");
                    _languageDic.Add(GoogleLanguage.Spanish, "es");
                    _languageDic.Add(GoogleLanguage.Sundanese, "su");
                    _languageDic.Add(GoogleLanguage.Swahili, "sw");
                    _languageDic.Add(GoogleLanguage.Swedish, "sv");
                    _languageDic.Add(GoogleLanguage.Tajik, "tg");
                    _languageDic.Add(GoogleLanguage.Tamil, "ta");
                    _languageDic.Add(GoogleLanguage.Telugu, "te");
                    _languageDic.Add(GoogleLanguage.Thai, "th");
                    _languageDic.Add(GoogleLanguage.Turkish, "tr");
                    _languageDic.Add(GoogleLanguage.Ukrainian, "uk");
                    _languageDic.Add(GoogleLanguage.Urdu, "ur");
                    _languageDic.Add(GoogleLanguage.Uzbek, "uz");
                    _languageDic.Add(GoogleLanguage.Vietnamese, "vi");
                    _languageDic.Add(GoogleLanguage.Welsh, "cy");
                    _languageDic.Add(GoogleLanguage.Xhosa, "xh");
                    _languageDic.Add(GoogleLanguage.Yiddish, "yi");
                    _languageDic.Add(GoogleLanguage.Yoruba, "yo");
                    _languageDic.Add(GoogleLanguage.Zulu, "zu");
                }

                return _languageDic;
            }
        }
    }
}
