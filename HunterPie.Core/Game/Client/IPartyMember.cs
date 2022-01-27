namespace HunterPie.Core.Game.Client
{
    public interface IPartyMember
    {

        public string Name { get; }
        public int Damage { get; }
        public bool IsPlayer { get; }
    }
}
