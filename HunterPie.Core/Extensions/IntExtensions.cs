namespace HunterPie.Core.Extensions
{
    public static class IntExtensions
    {
        const int PET_ID_START = 5;
        public static int ToPetId(this int self) => self + PET_ID_START;
    }
}
