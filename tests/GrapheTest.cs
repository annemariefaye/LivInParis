using PbSI;

namespace PbSITests
{
    [TestClass]
    public class GrapheTest
    {
        [TestMethod]
        public void TestDefaultConstructor()
        {
            Graphe<int> graphe = new Graphe<int>();

            Assert.IsNotNull(graphe.Noeuds);
            Assert.IsNotNull(graphe.MatriceAdjacence);
            Assert.IsNotNull(graphe.ListeAdjacence);
            Assert.AreEqual(0, graphe.Noeuds.Count);
            Assert.AreEqual(0, graphe.Taille);
        }


        [TestMethod]
        public void TestAjouterMembre()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> noeud = new Noeud<int>(1);

            graphe.AjouterMembre(noeud);

            Assert.AreEqual(1, graphe.Noeuds.Count);
            Assert.AreEqual(1, graphe.Noeuds[1].Id);
        }

        [TestMethod]
        public void TestGetMatriceAdjacence()
        {
            Graphe<int> graphe = new Graphe<int>();
            Noeud<int> noeud1 = new Noeud<int>(1);
            Noeud<int> noeud2 = new Noeud<int>(1);

            graphe.AjouterRelation(noeud1, noeud2);

            double[,] matrice = graphe.MatriceAdjacence;

            Assert.AreEqual(1, matrice[0, 1]);
            Assert.AreEqual(1, matrice[1, 0]);
        }
    }
}
