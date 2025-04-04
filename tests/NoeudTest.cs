using PbSI;

namespace PbSITests
{
    [TestClass]
    public class NoeudTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            int expectedId = 1;

            Noeud<int> noeud = new Noeud<int>(expectedId);
                
            Assert.AreEqual(expectedId, noeud.Id);
        }

        [TestMethod]
        public void TestToString()
        {
            Noeud<int> noeud = new Noeud<int>(1);
            string expectedString = "Membre 1";

            string result = noeud.ToString();

            Assert.AreEqual(expectedString, result);
        }
    }
}
