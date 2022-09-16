using HunterPie.Core.Game.World.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests.Game.World.Crypto;

[TestClass]
public class MHWCryptoTest
{
    private ref struct QuestTimerTestCase
    {
        public ulong Value;
        public ulong Key;
        public float Expect;
    }

    [TestMethod]
    public void DecryptQuestTimer_ShouldDecryptCorrectly()
    {
        var testCase = new QuestTimerTestCase()
        {
            Value = 0xBEE46316u,
            Key = 0xAB4FD38DEB44C953u,
            Expect = 935.59f
        };

        float output = MHWCrypto.DecryptQuestTimer(testCase.Value, testCase.Key);

        Assert.AreEqual(output, testCase.Expect, 0.1);
    }
}
