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
        private Connexion connexion;
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" Bienvenue sur l'interface de LivInParis ");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Interface SQL");
                Console.WriteLine("  2.  Menu simplifié");
                Console.WriteLine("  3.  Quitter");
                Console.WriteLine("----------------------------------------------------");
                Console.ResetColor();
                Console.Write(" Sélectionnez une option : ");

                char choix = (char)Console.ReadKey(false).Key;

                switch (choix)
                {
                    case '1':
                        Console.Clear();
                        MenuSQL();
                        this.pageChoisie = "Menu SQL";
                        break;
                    case '2':
                        Console.Clear();
                        MenuSimplifie();
                        this.pageChoisie = "Menu simplifié";
                        break;
                    case '3':
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bienvenue dans l'espace SQL !");
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Ecrire une commande SQL l'éxecutera");
            Console.WriteLine("\n\nDes mots clefs à rajouter avant la commande permettent différentes fonctionnalités:");            
            Console.WriteLine("AFFICHER: Executera le code SQL et affiche le résultat");             
            Console.WriteLine("EXPORTER: Executera le code SQL et exportera le résultat dans un fichier XML");                
            Console.Write("Entrez votre commande : \n");
            Console.ResetColor();

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
                        if(requete.Substring(9).Substring(0,6)!="SELECT")
                        {
                            Console.WriteLine("erreur: la commande n'est pas une requête de de retrait de données (SELECT)");
                        }
                        else
                        {
                            Console.WriteLine("Nom du fichier d'export:\n");
                            string nom_fichier = Console.ReadLine();
                            bdd.exporterResultatRequete(nom_fichier);
                        }
                    }
                }
                catch (Exception ex)
                {     
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erreur : " + ex.Message);
                    Console.ResetColor();    
                }
            }    
            Console.ReadKey();
        }

        public void MenuSimplifie()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Bienvenue dans le mode simplifié de l'interface !\n");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Gestion des clients");
                Console.WriteLine("  2.  Gestion des cuisines");
                Console.WriteLine("  3.  Gestion des commandes");
                Console.WriteLine("  4.  Statistiques");
                Console.WriteLine("  5.  Autre...");
                Console.WriteLine("  6.  Retour");
                Console.WriteLine("----------------------------------------------------\n\n");
                Console.WriteLine("Menu choisi:");
                Console.ResetColor();
                char menu_choisi = (char)Console.ReadKey(false).Key;

                switch (menu_choisi)
                {
                    case ('1'):
                        Console.Clear();
                        ModuleClient();
                        break;
                    case ('2'):
                        Console.Clear();
                        break;
                    case ('3'):
                        Console.Clear();
                        break;
                    case ('4'):
                        Console.Clear();
                        break;
                    case ('5'):
                        Console.Clear();
                        break;
                    case ('6'):
                        Console.Clear();
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

        public void ModuleClient()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Module Client !\n");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Afficher les clients");
                Console.WriteLine("  2.  Ajouter un client");
                Console.WriteLine("  3.  Supprimer un client");
                Console.WriteLine("  4.  Modifier un client");
                Console.WriteLine("  5.  Retour");
                Console.WriteLine("----------------------------------------------------\n\n");
                Console.WriteLine("Menu choisi:");
                Console.ResetColor();
                char menu_choisi = (char)Console.ReadKey(false).Key;

                switch (menu_choisi)
                {
                    case ('1'):
                        Console.Clear();
                        this.afficherClients();
                        break;
                    case ('2'):
                        Console.Clear();
                        break;
                    case ('3'):
                        Console.Clear();
                        break;
                    case ('4'):
                        Console.Clear();
                        break;
                    case ('5'):
                        Console.Clear();
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

        public void afficherClients()
        {
            while (true)
            {
                Console.WriteLine("Liste des clients:\n");
                Console.WriteLine("Tri possible: A: Ordre Alphabétique; R: Rue; M: Montant des achats cumulés; C: combinaisons de plusieurs tris");
                Console.WriteLine("X: Retour");
                char choix = (char)Console.ReadKey(false).Key;
                switch (choix)
                {
                    case 'A':
                        this.connexion.executerRequete("SELECT * FROM Utilisateur ORDER BY nom ASC;");
                        break;
                    case 'R':
                        break;
                    case 'M':
                        break;
                    case 'C':
                        break;
                    case 'X':
                        Console.Clear();
                        return;
                }
            }
        }


    }
}
