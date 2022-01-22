namespace HunterPie.Core.Extensions
{
    public static class LongExtensions
    {
        public static string FormatBytes(this long value)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixId = 0;
            float decimalValue = value;
            while (value / 1024 != 0 && suffixId < suffixes.Length)
            {
                suffixId++;
                value /= 1024;
                decimalValue /= 1024;
            }

            return $"{decimalValue:0.0}{suffixes[suffixId]}";
        }
    }
}
