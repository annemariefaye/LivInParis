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
            this.requete = this.maConnexion.CreateCommand();
            this. requete.CommandText = stringRequete;
        }

        public void afficherResultatRequete()
        {
            this.reader = this.requete.ExecuteReader();
            string[] valueString = new string[this.reader.FieldCount];
            while (this.reader.Read())
            {
                for (int i = 0; i < this.reader.FieldCount; i++)
                {
                    valueString[i] = this.reader.GetValue(i).ToString();
                    Console.Write(valueString[i] + ", ");
                }
                Console.WriteLine();
            }
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
 