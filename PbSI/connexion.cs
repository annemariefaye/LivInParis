using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PbSI
{
    internal class Connexion
    {

        MySqlConnection maConnexion;
        MySqlDataReader reader;
        MySqlCommand requete;

        public Connexion()
        {
            MySqlConnection maConnexion = null;

            try
            {
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                        "DATABASE=LivInParis;" +
                                        "UID=root;PASSWORD=admin";
                maConnexion = new MySqlConnection(connexionString);
                maConnexion.Open();
                this.maConnexion = maConnexion;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Erreur Connexion:" + e.ToString);
                return;
            }
        }

        public void executerRequete(string stringRequete)
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


        public MySqlDataReader recupererResultatRequete()
        {
            return this.requete.ExecuteReader();
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
                // Vérifie si un DataReader est ouvert et le ferme avant d'utiliser un DataAdapter
                if (this.reader != null && !this.reader.IsClosed)
                    this.reader.Close();

                string nomCompletFichier = "../export/"+nomFichier + ".xml";
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(this.requete))
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
            if (this.reader != null && !this.reader.IsClosed)
                this.reader.Close();
            if (this.requete != null)
                this.requete.Dispose();
            if (this.maConnexion != null)
                this.maConnexion.Close();
        }
    }
}
 