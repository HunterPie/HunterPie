namespace HunterPie.Core.Extensions
{
    public static class UIntExtensions
    {
        public static uint ApproximateHigh(this uint self, uint[] values)
        {
            var closest = self;

            for (var i = 0; i < values.Length; i++)
            {
                if (values[i] < closest)
                    continue;

                if (values[i] >= closest)
                {
                    closest = values[i];
                    break;
                }
            }

            return closest;
        }
    }
}
