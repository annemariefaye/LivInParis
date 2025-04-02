using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{
    internal class Menu
    {

        private string pageChoisie;
        private Connexion bdd;
        public Menu()
        {
            this.bdd = new Connexion();

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("====================================================");
                Console.WriteLine("                       LIVINPARIS");
                Console.WriteLine("====================================================\n\n");
                Console.ResetColor();
                Console.WriteLine(" Bienvenue sur l'interface de LivInParis ");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Interface SQL");
                Console.WriteLine("  2.  Exporter une requête SQL");
                Console.WriteLine("  3.  Quitter");
                Console.WriteLine("----------------------------------------------------");
                Console.Write(" Sélectionnez une option : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Chargement de l'exécution SQL...");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        MenuSQL();
                        this.pageChoisie = "Menu SQL";
                        break;
                    case "2":
                        //ExporterRequeteSQL();
                        break;
                    case "3":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Merci d'avoir utilisé LivInParis ! À bientôt !");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Option invalide. Appuyez sur une touche pour continuer...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
        public void MenuSQL()
        {
                
            Console.WriteLine("Bienvenue dans l'espace SQL !");    
            Console.WriteLine("Ecrire une commande SQL l'éxecutera");
            Console.WriteLine("\n\nDes mots clefs à rajouter avant la commande permettent différentes fonctionnalités:");            
            Console.WriteLine("AFFICHER: Executera le code SQL et affiche le résultat");             
            Console.WriteLine("EXPORTER: Executera le code SQL et exportera le résultat dans un fichier XML");                
            Console.Write("Entrez votre commande : \n");
                
            string mot_clef;

                
            while (true)
            {    
                string requete = Console.ReadLine();
 
                try
                {
                    this.bdd.executerRequete(requete.Substring(9));   

                    mot_clef = "";                        
                    if (requete.Length < 8) break;
                        
                    for(int i = 0; i < 8; i++) 
                    {     
                        mot_clef += requete[i];  
                    }
                        
                    if(mot_clef == "AFFICHER")   
                    {       
                        Console.WriteLine("Résultat de la commande:\n");
                        bdd.afficherResultatRequete();
                    }
                        
                    if(mot_clef == "EXPORTER")   
                    {
                        Console.WriteLine("Nom du fichier d'export:\n");
                        string nom_fichier = Console.ReadLine();
                        bdd.exporterResultatRequete(nom_fichier);
                    }
                }
                catch (Exception ex)
                {     
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erreur : " + ex.Message);
                    Console.ResetColor();    
                    break;    
                }
            }    
            Console.ReadKey();
        }

        
    }
}
