using Library.API;
using Library.API.Controllers;

namespace Library.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        [DataRow(1,0,-1)]
        public void TestOczekiwanPracyW_HC_WORKS(int oczekiwania_z_HC)
        {
            bool isOnGoodLevel = oczekiwania_z_HC < 0;

            Assert.IsTrue(isOnGoodLevel);
        }
    }
}