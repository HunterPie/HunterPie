using System;

namespace HunterPie.Core.Domain
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ScannableMethod : Attribute
    {
        public readonly Type DtoType;

        public ScannableMethod(Type dtoType = null)
        {
            DtoType = dtoType;
        }
    }
}
