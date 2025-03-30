using PbSI;

namespace PbSITests
{
    [TestClass]
    public class GrapheTest
    {
        [TestMethod]
        public void TestDefaultConstructor()
        {
            Graphe graphe = new Graphe();

            Assert.IsNotNull(graphe.Noeuds);
            Assert.IsNotNull(graphe.MatriceAdjacence);
            Assert.IsNotNull(graphe.ListeAdjacence);
            Assert.AreEqual(0, graphe.Noeuds.Count);
            Assert.AreEqual(0, graphe.Taille);
        }

        [TestMethod]
        public void TestConstructorWithAdjacencyMatrix()
        {
            int[,] matriceAdjacence = new int[,]
            {
                { 0, 1 },
                { 1, 0 },
            };

            Graphe graphe = new Graphe(matriceAdjacence);

            Assert.AreEqual(2, graphe.Noeuds.Count);
            Assert.AreEqual(1, graphe.Noeuds[1].Id);
            Assert.AreEqual(2, graphe.Noeuds[2].Id);
            Assert.AreEqual(1, graphe.Taille);
        }

        [TestMethod]
        public void TestConstructorWithAdjacencyList()
        {
            var listeAdjacence = new Dictionary<int, List<int>>
            {
                {
                    1,
                    new List<int> { 2 }
                },
                {
                    2,
                    new List<int> { 1 }
                },
            };

            Graphe graphe = new Graphe(listeAdjacence);

            Assert.AreEqual(2, graphe.Noeuds.Count);
            Assert.AreEqual(1, graphe.Noeuds[1].Id);
            Assert.AreEqual(2, graphe.Noeuds[2].Id);
            Assert.AreEqual(1, graphe.Taille);
        }

        [TestMethod]
        public void TestAjouterMembre()
        {
            Graphe graphe = new Graphe();

            graphe.AjouterMembre(1);

            Assert.AreEqual(1, graphe.Noeuds.Count);
            Assert.AreEqual(1, graphe.Noeuds[1].Id);
        }

        [TestMethod]
        public void TestAjouterRelation()
        {
            Graphe graphe = new Graphe();

            graphe.AjouterRelation(1, 2);

            Assert.AreEqual(2, graphe.Noeuds.Count);
            Assert.AreEqual(1, graphe.Noeuds[1].Id);
            Assert.AreEqual(2, graphe.Noeuds[2].Id);
            Assert.AreEqual(1, graphe.Taille);
        }

        [TestMethod]
        public void TestGetMatriceAdjacence()
        {
            Graphe graphe = new Graphe();
            graphe.AjouterRelation(1, 2);

            int[,] matrice = graphe.MatriceAdjacence;

            Assert.AreEqual(1, matrice[0, 1]);
            Assert.AreEqual(1, matrice[1, 0]);
        }

        [TestMethod]
        public void TestGetListeAdjacence()
        {
            Graphe graphe = new Graphe();
            graphe.AjouterRelation(1, 2);

            var liste = graphe.ListeAdjacence;

            Assert.AreEqual(2, liste[1][0]);
            Assert.AreEqual(1, liste[2][0]);
        }

        [TestMethod]
        public void TestUpdateProprietes()
        {
            Graphe graphe = new Graphe();
            graphe.AjouterRelation(1, 2);

            graphe.UpdateProprietes();

            Assert.AreEqual(2, graphe.Ordre);
            Assert.AreEqual(1, graphe.Taille);
            Assert.IsFalse(graphe.EstOriente);
            Assert.IsFalse(graphe.EstPondere);
            Assert.AreEqual(1.0, graphe.Densite);
        }
    }
}
