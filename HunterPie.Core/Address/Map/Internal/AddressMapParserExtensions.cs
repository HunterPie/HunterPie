using System;
using System.Linq;
using AddressMapType = HunterPie.Core.Address.Map.Internal.AddressMapKeyWords.AddressMapType;

namespace HunterPie.Core.Address.Map.Internal
{
    static class AddressMapParserExtensions
    {
        private static int[] ParseStringToVecInt32(string stringified)
        {
            return stringified.Split(",")
                    .Select(element => Convert.ToInt32(element, 16))
                    .ToArray();
        }

        public static void AddValueByType(this IAddressMapParser self, AddressMapType type, string key, string value)
        {
            switch (type)
            {
                case AddressMapType.Long:
                    self.Add(key, Convert.ToInt64(value, 16));
                    break;

                case AddressMapType.VecInt32:
                    self.Add(key, ParseStringToVecInt32(value));
                    break;

                default:
                    break;

            }
        }

    }
}
