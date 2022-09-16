namespace HunterPie.Core.Extensions;

public static class UIntExtensions
{
    public static uint ApproximateHigh(this uint self, uint[] values)
    {
        uint closest = self;

        for (int i = 0; i < values.Length; i++)
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
