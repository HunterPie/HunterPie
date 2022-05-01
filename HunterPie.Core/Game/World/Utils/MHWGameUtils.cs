namespace HunterPie.Core.Game.World.Utils
{
    public static class MHWGameUtils
    {
        public static uint[] MaxQuestTimers = { 54000, 72000, 108000, 126000, 180000 };

        public static float ToSeconds(this uint self) => self / 60.0f;
    }
}
