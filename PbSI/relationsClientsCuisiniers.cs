using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{
    public class relationsClientsCuisiniers
    {
        private Graphe<string> graphe;
        private Connexion connexion;

        public relationsClientsCuisiniers()
        {
            this.graphe = new Graphe<string>();
            this.connexion = new Connexion(); 
            //Console.Clear();
            ConstruireGraphe();
            Console.WriteLine("Liens créés:");
            foreach (var lien in this.graphe.Liens)
            {
                Console.WriteLine($"{lien.Source.Id} ({lien.Source.Contenu}) -> {lien.Destination.Id} ({lien.Destination.Contenu})");
            }
            ///List<Noeud<T>> noeuds = graphe.Noeuds;
            /// double[,] ajdacence = this.graphe.MatriceAdjacence;
            /*for(int i = 0; i < ajdacence.GetLength(0); i++)
            {
                for(int j = 0; j < ajdacence.GetLength(1); j++) {
                    Console.Write(ajdacence[i,j]+" ");
                }
                Console.WriteLine();
            }*/

            /*HashSet<Lien<string>> liens = this.graphe.Liens;

            foreach(Lien<string> l in liens)
            {
                Console.WriteLine(l.Source+" "+l.Destination);
            }*/

        }

        public Graphe<string> Graphe => this.graphe;

        private void ConstruireGraphe()
        {


            /// 1. Charger Clients
            connexion.executerRequete("SELECT IdClient, Nom FROM Utilisateur WHERE IdClient IS NOT NULL;");
            using (var reader = connexion.recupererResultatRequete())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("IdClient");
                    string nom = reader.GetString("Nom");
                    graphe.AjouterMembre(new Noeud<string>(id, $"Client: {nom}"));
                }
            }

            /// 2. Charger Cuisiniers
            connexion.executerRequete("SELECT IdCuisinier, Nom FROM Utilisateur WHERE IdCuisinier IS NOT NULL;");
            using (var reader = connexion.recupererResultatRequete())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("IdCuisinier");
                    string nom = reader.GetString("Nom");
                    ///Console.WriteLine(-id);
                    graphe.AjouterMembre(new Noeud<string>(-id, $"Cuisinier: {nom}"));///si cuisinier id negatif, si client: positif
                }
            }

            /// 3. Créer les relations : Client vers Cuisinier
            string requeteRelations = @"
                SELECT DISTINCT
                cli.IdClient   AS IdClient,
                cuis.IdCuisinier AS IdCuisinier
                FROM Commande co
                  JOIN Utilisateur ucli   ON co.IdClient       = ucli.Id
                  JOIN Client     cli     ON ucli.IdClient     = cli.IdClient
                  JOIN LigneDeCommande ldc ON co.IdCommande    = ldc.IdCommande
                  JOIN Plat        p      ON ldc.IdPlat        = p.IdPlat
                  JOIN Utilisateur ucui   ON p.IdCuisinier     = ucui.Id
                  JOIN Cuisinier   cuis   ON ucui.IdCuisinier  = cuis.IdCuisinier;
            ";

            connexion.executerRequete(requeteRelations);
            using (var reader = connexion.recupererResultatRequete())
            {
                while (reader.Read())
                {
                    int idClient = reader.GetInt32("IdClient");
                    int idCuisinier = reader.GetInt32("IdCuisinier");
                    Console.WriteLine("idclient:" + idClient + " idCuisinier:" + -idCuisinier);

                    var noeudClient = graphe.TrouverNoeudParId(idClient);
                    var noeudCuisinier = graphe.TrouverNoeudParId(-idCuisinier);

                    if (noeudClient != null && noeudCuisinier != null)
                    {
                        graphe.AjouterRelation(noeudClient, noeudCuisinier, 1); /// poids 1 par défaut car toutes les relations ont le même poids
                    }
                }
            }
        }
    }
}
