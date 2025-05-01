using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{
    public class relationsClientsCuisiniers
    {
        private readonly Graphe<string> graphe;
        private readonly Connexion connexion;

        public relationsClientsCuisiniers()
        {
            this.graphe = new Graphe<string>();
            this.connexion = new Connexion(); // utilise ta classe Connexion existante
            ConstruireGraphe();
        }

        public Graphe<string> Graphe => this.graphe;

        private void ConstruireGraphe()
        {
            Dictionary<int, string> clients = new Dictionary<int, string>();
            Dictionary<int, string> cuisiniers = new Dictionary<int, string>();

            // 1. Charger Clients
            connexion.executerRequete("SELECT Id, Nom FROM Utilisateur WHERE IdClient IS NOT NULL;");
            using (var reader = connexion.recupererResultatRequete())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string nom = reader.GetString("Nom");
                    clients[id] = nom;
                    graphe.AjouterMembre(new Noeud<string>(id, $"Client: {nom}"));
                }
            }

            // 2. Charger Cuisiniers
            connexion.executerRequete("SELECT Id, Nom FROM Utilisateur WHERE IdCuisinier IS NOT NULL;");
            using (var reader = connexion.recupererResultatRequete())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string nom = reader.GetString("Nom");
                    cuisiniers[id] = nom;
                    graphe.AjouterMembre(new Noeud<string>(id, $"Cuisinier: {nom}"));
                }
            }

            // 3. Créer les relations : Client -> Cuisinier (par commandes)
            string requeteRelations = @"
                SELECT co.IdClient AS IdClient, p.IdCuisinier AS IdCuisinier
                FROM Commande co
                INNER JOIN LigneDeCommande ldc ON ldc.IdCommande = co.IdCommande
                INNER JOIN Plat p ON p.IdPlat = ldc.IdPlat;
            ";

            connexion.executerRequete(requeteRelations);
            using (var reader = connexion.recupererResultatRequete())
            {
                while (reader.Read())
                {
                    int idClient = reader.GetInt32("IdClient");
                    int idCuisinier = reader.GetInt32("IdCuisinier");

                    var noeudClient = graphe.TrouverNoeudParId(idClient);
                    var noeudCuisinier = graphe.TrouverNoeudParId(idCuisinier);

                    if (noeudClient != null && noeudCuisinier != null)
                    {
                        graphe.AjouterRelation(noeudClient, noeudCuisinier, 1); // poids 1 par défaut
                    }
                }
            }
        }
    }
}
