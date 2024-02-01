using System;

namespace Utils
{
    public static class StringExtension
    {
        public static string AddedUpper(this string text)
        {
            string result = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsUpper(text, i))
                    result += " " + text[i];
                else
                    result += text[i];
            }

            return result;
        }
    }
}