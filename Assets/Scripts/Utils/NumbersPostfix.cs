namespace Utils
{
    public static class NumbersPostfix
    {
        private static string[] _postfixes =
        {
            "",
            "K",
            "M",
            "B",
            "T",
            " aa",
            " ab",
            " ac",
            " ad",
            " ae",
            " af",
            " ag",
            " ah",
            " ai",
            " aj",
            " ak",
            " al",
            " am",
            " an",
            " ao",
            " ap",
            " aq",
            " ar",
            " as",
            " at",
            " au",
            " av",
            " aw",
            " ax",
            " ay",
            " az",
            " ba",
            " bb",
            " bc",
            " bd",
            " be",
            " bf",
            " bg",
            " bh",
            " bi",
            " bj",
            " bk",
            " bl",
            " bm",
            " bn",
            " bo",
            " bp",
            " bq"
        };

        public static string GetPostfix(int index)
        {
            return _postfixes[index];
        }
    }
}