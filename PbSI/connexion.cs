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

                string connexionString = "SERVER=localhost;PORT=3306;UID=root;PASSWORD=root;";
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
            foreach (var fichier in fichiers)
            {
                try
                {
                    if (!File.Exists(fichier))
                    {
                        Console.WriteLine($"Le fichier {fichier} n'existe pas.");
                        continue;
                    }

                    string script = File.ReadAllText(fichier);
                    string[] commandes = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string commande in commandes)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(commande, maConnexion))
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine($"Commande exécutée : {commande}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erreur lors de l'exécution du fichier {fichier}: " + e.Message);
                }
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
            if (requete == null)
            {
                Console.WriteLine("Aucune requête à exécuter.");
                return;
            }

            try
            {
                reader = requete.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("Pas de résultats pour cette requête.");
                    reader.Close();
                    return;
                }

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetValue(i).ToString() + ", ");
                    }
                    Console.WriteLine();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'affichage des résultats: " + e.Message);
            }
        }


        public void exporterResultatRequete(string nomFichier = "export")
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
