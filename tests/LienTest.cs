using PbSI;

namespace PbSITests
{
    [TestClass]
    public class LienTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            Noeud source = new Noeud(1);
            Noeud destination = new Noeud(2);
            double expectedPoids = 1;

            Lien lien = new Lien(source, destination, expectedPoids);

            Assert.AreEqual(source, lien.Source);
            Assert.AreEqual(destination, lien.Destination);
            Assert.AreEqual(expectedPoids, lien.Poids);
        }

        [TestMethod]
        public void TestPoidsProperty()
        {
            Noeud source = new Noeud(1);
            Noeud destination = new Noeud(2);
            double expectedPoids = 2.5;

            Lien lien = new Lien(source, destination, expectedPoids);

            Assert.AreEqual(expectedPoids, lien.Poids);
        }

        [TestMethod]
        public void TestSourceProperty()
        {
            Noeud source = new Noeud(1);
            Noeud destination = new Noeud(2);

            Lien lien = new Lien(source, destination);

            Assert.AreEqual(source, lien.Source);
        }

        [TestMethod]
        public void TestDestinationProperty()
        {
            Noeud source = new Noeud(1);
            Noeud destination = new Noeud(2);

            Lien lien = new Lien(source, destination);

            Assert.AreEqual(destination, lien.Destination);
        }

        [TestMethod]
        public void TestToString()
        {
            Noeud source = new Noeud(1);
            Noeud destination = new Noeud(2);
            Lien lien = new Lien(source, destination);
            string expectedString = "1 est connecte a 2";

            string result = lien.ToString();

            Assert.AreEqual(expectedString, result);
        }
    }
}
