using HunterPie.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HunterPie.Core.Tests
{
    [TestClass]
    public class ScannableEntityTest
    {
        
        private struct TestDTO
        {
            public string TestValue;
        }

        private class TestEntity : ScannableEntity
        {
            public string TestValue { get; private set; }

            public TestEntity()
            {
                AddScanner(typeof(TestDTO), ScanTestData);
            }

            public void ScanData() => Scan();

            public void ScanTestData()
            {
                TestDTO dto = new()
                {
                    TestValue = "DTO Value!"
                };

                Next(ref dto);
                TestValue = dto.TestValue;
            }

        }

        [TestMethod]
        public void TestMiddleware()
        {
            TestEntity test = new();
            
            test.MiddlewareFor<TestDTO>(ScannerMiddleware);

            // This is called internally by the scannable entity, this is here just for testing purposes
            test.ScanData();

            Assert.AreEqual("Modified by middleware!", test.TestValue);

        }

        private void ScannerMiddleware(ref TestDTO data)
        {
            data.TestValue = "Modified by middleware!";
        }
    }
}
