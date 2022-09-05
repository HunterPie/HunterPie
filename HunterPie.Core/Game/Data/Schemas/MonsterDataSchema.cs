using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Data.Schemas
{
    public struct MonsterDataSchema
    {
        public int Id;
        public int Capture;
        public MonsterSizeSchema Size;
        public MonsterPartSchema[] Parts;
        public Element[] Weaknesses; 
    }
}
