namespace HunterPie.Core.Extensions
{
    public static class LongExtensions
    {
        public static string FormatBytes(this long value)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixId = 0;
            while (value / 1024 != 0 && suffixId < suffixes.Length)
            {
                suffixId++;
                value /= 1024;
            }

            return $"{value}{suffixes[suffixId]}";
        }
    }
}
