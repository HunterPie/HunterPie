namespace HunterPie.Core.Http
{
    public static class PoogieFactory
    {

        public static string[] Hosts =
        {
            "https://api.hunterpie.com",
            "https://mirror.hunterpie.com/mirror"
        };

        public static PoogieBuilder Default()
        {
            return new PoogieBuilder(Hosts);
        }
    }
}
