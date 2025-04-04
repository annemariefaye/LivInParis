using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PbSI
{
    internal class Connexion
    {
        private MySqlConnection maConnexion;
        private MySqlDataReader reader;
        private MySqlCommand requete;

        public Connexion()
        {
            try
            {

                string connexionString = "SERVER=localhost;PORT=3306;UID=root;PASSWORD=admin;";
                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();

                if (maConnexion.State != ConnectionState.Open)
                {
                    throw new Exception("Connexion MySQL échouée.");
                }

                Console.WriteLine("Connexion à MySQL réussie !");
                CreerBaseSiNonExiste();
                maConnexion.ChangeDatabase("livinparics");

                Console.WriteLine("Base de données prête !");
                ExecuterFichiersSQL(new string[] { "01_create_tables.sql", "02_insert_date.sql", "03_select.sql" });
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur MySQL: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur: " + e.Message);
            }
        }

        public MySqlDataReader recupererResultatRequete()
        {
            return this.requete.ExecuteReader();
        }

        private void CreerBaseSiNonExiste()
        {
            if (maConnexion == null || maConnexion.State != ConnectionState.Open)
            {
                Console.WriteLine("Connexion non disponible pour créer la base.");
                return;
            }

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS livinparics;", maConnexion))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Base de données vérifiée/créée.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la création de la base: " + e.Message);
            }
        }

        private void ExecuterFichiersSQL(string[] fichiers)
        {
            try
            {
                this.requete = this.maConnexion.CreateCommand();
                this.requete.CommandText = stringRequete;
                this.requete.ExecuteNonQuery(); 
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur SQL : " + e.Message);
            }
        }

        public void executerRequete(string stringRequete)
        {
            if (maConnexion == null || maConnexion.State != ConnectionState.Open)
            {
                Console.WriteLine("Impossible d'exécuter la requête : connexion fermée.");
                return;
            }

            try
            {
                requete = maConnexion.CreateCommand();
                requete.CommandText = stringRequete;
                requete.ExecuteNonQuery();
                //Console.WriteLine("Requête exécutée avec succès !");
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur d'exécution de la requête: " + e.Message);
            }
        }

        public void afficherResultatRequete()
        {
            this.reader = this.requete.ExecuteReader();

            int nombreColonnes = this.reader.FieldCount;
            string[] nomsColonnes = new string[nombreColonnes];
            int[] taillesColonnes = new int[nombreColonnes];

            // Récupération des noms de colonnes et des tailles maximales
            for (int i = 0; i < nombreColonnes; i++)
            {
                nomsColonnes[i] = this.reader.GetName(i);
                taillesColonnes[i] = nomsColonnes[i].Length;
            }

            List<string[]> lignes = new List<string[]>();

            // Lecture des données et mise à jour des tailles de colonnes
            while (this.reader.Read())
            {
                string[] valeurs = new string[nombreColonnes];
                for (int i = 0; i < nombreColonnes; i++)
                {
                    valeurs[i] = this.reader.GetValue(i).ToString();
                    if (valeurs[i].Length > taillesColonnes[i])
                        taillesColonnes[i] = valeurs[i].Length;
                }
                lignes.Add(valeurs);
            }

            // Affichage de l'en-tête
            for (int i = 0; i < nombreColonnes; i++)
            {
                Console.Write(nomsColonnes[i].PadRight(taillesColonnes[i] + 2));
            }
            Console.WriteLine();

            // Trait de séparation
            for (int i = 0; i < nombreColonnes; i++)
            {
                Console.Write(new string('-', taillesColonnes[i]) + "  ");
            }
            Console.WriteLine();

            // Affichage des lignes
            foreach (var ligne in lignes)
            {
                for (int i = 0; i < nombreColonnes; i++)
                {
                    Console.Write(ligne[i].PadRight(taillesColonnes[i] + 2));
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            this.reader.Close();
        }



        public void exporterResultatRequete(string nomFichier="export")
        {
            try
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                if (requete == null)
                {
                    Console.WriteLine("Aucune requête à exporter.");
                    return;
                }

                string nomCompletFichier = $"../export/{nomFichier}.xml";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(requete))
                {
                    DataTable table = new DataTable(nomFichier);
                    adapter.Fill(table);
                    table.WriteXml(nomCompletFichier, XmlWriteMode.WriteSchema);
                    Console.WriteLine($"Export terminé : {nomCompletFichier}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur Export XML: " + e.Message);
            }
        }

        public void fermerConnexion()
        {
            if (maConnexion != null && maConnexion.State == ConnectionState.Open)
            {
                maConnexion.Close();
                Console.WriteLine("Connexion fermée proprement.");
            }
        }
    }
}
