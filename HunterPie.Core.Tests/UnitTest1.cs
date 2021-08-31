using HunterPie.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Player test = new Player();
            
            test.RegisterMiddleware("health", ScannerMiddleware);

            test.ScannerHealth();

            Assert.AreEqual(20.0f, test.Health);

        }

        public void ScannerMiddleware(ref dynamic data)
        {
            data.health += 10.0f;
        }
    }
}
