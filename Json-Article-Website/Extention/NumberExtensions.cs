namespace Json_Article_Website.Extention
{
    public static class NumberExtensions
    {
        public static string ToAbbreviated(this int number)
        {
            if (number >= 1_000_000_000)
                return (number / 1_000_000_000D).ToString("0.#") + "B";
            if (number >= 1_000_000)
                return (number / 1_000_000D).ToString("0.#") + "M";
            if (number >= 1_000)
                return (number / 1_000D).ToString("0.#") + "K";

            return number.ToString();
        }
    }

}
