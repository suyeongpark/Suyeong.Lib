﻿using System;
using System.Text;

namespace Suyeong.Lib.Util
{
    public static class TextUtil
    {
        public static string GetOrdinalIndicator(int num)
        {
            int rest = num % 100;

            if (rest == 11 || rest == 12 || rest == 13)
            {
                return "th";
            }

            switch (rest % 10)
            {
                case 1:
                    return "st";

                case 2:
                    return "nd";

                case 3:
                    return "rd";

                default:
                    return "th";
            }
        }

        public static string CutWordByByteCount(string text, int byteCount)
        {
            Encoding encoding = Encoding.GetEncoding("ks_c_5601-1987");
            byte[] buf = encoding.GetBytes(text);
            return encoding.GetString(buf, 0, byteCount);
        }

        public static bool IsContainTextByIgnoreCase(string source, string text)
        {
            return source.IndexOf(text, StringComparison.OrdinalIgnoreCase) > -1;
        }

        public static string ConvertTextToPascalCase(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] words = text.Split(new char[] { ' ', }, StringSplitOptions.RemoveEmptyEntries);
                string[] result = new string[words.Length];

                for (int i = 0; i < words.Length; i++)
                {
                    result[i] = words[i][0].ToString().ToUpper() + words[i].Substring(1);
                }

                return string.Join(" ", result);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
