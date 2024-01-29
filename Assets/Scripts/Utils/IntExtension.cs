using System;
using System.Globalization;
using UnityEngine;

namespace Utils
{
    public static class IntExtension
    {
        public static bool ToBool(this int value)
        {
            return value > 0 ? true : false;
        }

        public static string ToFormattedBigNumber(this int number)
        {
            return GetFormattedInt(number);
        }

        public static string ToSpaceBetweenChars(this int number, bool isForced = false)
        {
            if (!isForced && Mathf.Abs(number) < 100)
            {
                return number.ToString();
            }
            
            return string.Format(CultureInfo.InvariantCulture, "{0:0,#}", number).Replace(",", " ");
        }

        private static string GetFormattedInt(int value)
        {
            var currency = value.ToString();

            if (currency.Length <= 3)
            {
                return currency;
            }

            var numberString = currency.Substring(0, Math.Min(currency.Length, 4));
            var number = double.Parse(numberString);

            if (numberString.Length == 4)
            {
                number = AddACommaToNumber(number, currency);
            }

            numberString = String.Format("{0:0.0}", number);

            return numberString + NumbersPostfix.GetPostfix((currency.Length - 1) / 3);
        }

        private static double AddACommaToNumber(double number, string currency)
        {
            return number / Math.Pow(10, 3 - (currency.Length - 1) % 3);
        }
        
        public static int GetDigitsCount(this int n)
        {
            if (n >= 0)
            {
                if (n < 10L) return 1;
                if (n < 100L) return 2;
                if (n < 1000L) return 3;
                if (n < 10000L) return 4;
                if (n < 100000L) return 5;
                if (n < 1000000L) return 6;
                if (n < 10000000L) return 7;
                if (n < 100000000L) return 8;
                if (n < 1000000000L) return 9;
                //if (n < 10000000000L) return 10;
                //if (n < 100000000000L) return 11;
                //if (n < 1000000000000L) return 12;
                //if (n < 10000000000000L) return 13;
                //if (n < 100000000000000L) return 14;
                //if (n < 1000000000000000L) return 15;
                //if (n < 10000000000000000L) return 16;
                //if (n < 100000000000000000L) return 17;
                //if (n < 1000000000000000000L) return 18;
                return 19;
            }
            else
            {
                if (n > -10L) return 2;
                if (n > -100L) return 3;
                if (n > -1000L) return 4;
                if (n > -10000L) return 5;
                if (n > -100000L) return 6;
                if (n > -1000000L) return 7;
                if (n > -10000000L) return 8;
                if (n > -100000000L) return 9;
                if (n > -1000000000L) return 10;
                //if (n > -10000000000L) return 11;
                //if (n > -100000000000L) return 12;
                //if (n > -1000000000000L) return 13;
                //if (n > -10000000000000L) return 14;
                //if (n > -100000000000000L) return 15;
                //if (n > -1000000000000000L) return 16;
                //if (n > -10000000000000000L) return 17;
                //if (n > -100000000000000000L) return 18;
                //if (n > -1000000000000000000L) return 19;
                return 20;
            }
        }
    }
}