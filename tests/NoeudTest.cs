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

            Noeud noeud = new Noeud(expectedId);

            Assert.AreEqual(expectedId, noeud.Id);
            Assert.IsNotNull(noeud.Voisins);
            Assert.AreEqual(0, noeud.Voisins.Count);
        }

        [TestMethod]
        public void TestAjouterVoisin()
        {
            Noeud noeud1 = new Noeud(1);
            Noeud noeud2 = new Noeud(2);

            noeud1.AjouterVoisin(noeud2);

            Assert.IsTrue(noeud1.Voisins.Contains(noeud2));
            Assert.IsTrue(noeud2.Voisins.Contains(noeud1));
        }

        [TestMethod]
        public void TestToString()
        {
            Noeud noeud = new Noeud(1);
            string expectedString = "Membre 1";

            string result = noeud.ToString();

            Assert.AreEqual(expectedString, result);
        }
    }
}
