using HunterPie.Core.Game.Rise.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests.Game.Rise.Crypto
{
    [TestClass]
    public class TestMHRFloat
    {
        struct DecodeHealthTestCase
        {
            public uint Value;
            public uint Key;
            public float Expect;
        }

        [TestMethod]
        public void DecodeHealth_ShouldDecodeHealthCorrectly()
        {
            
            DecodeHealthTestCase[] testCases = new DecodeHealthTestCase[]
            {
                new() {
                    Value = 0x39D89684u,
                    Key = 0x820281AAu,
                    Expect = 3690f
                },

                new()
                {
                    Value = 0xB96A0A0A,
                    Key = 0x36894949,
                    Expect = 10f
                }
            };
            
            foreach (var testCase in testCases)
            {
                float result = MHRFloat.DecodeHealth(testCase.Value, testCase.Key);

                Assert.AreEqual(testCase.Expect, result);
            }
        }
    }
}
