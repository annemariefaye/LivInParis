using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PbSI
{
    internal class Menu
    {

        private string pageChoisie;
        private Connexion connexion;
        public Menu()
        {
            this.connexion = new Connexion();

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
            Console.WriteLine("\n\nRETOUR: Retourner au menu principal");
            Console.Write("Entrez votre commande : \n");
            Console.ResetColor();

            string mot_clef;

                
            while (true)
            {    
                string requete = Console.ReadLine();
                if (requete == "RETOUR") return;
 
                try
                {
                    this.connexion.executerRequete(requete.Substring(9));   

                    mot_clef = "";                        
                    if (requete.Length < 8) break;
                        
                    for(int i = 0; i < 8; i++) 
                    {     
                        mot_clef += requete[i];  
                    }
                        
                    if(mot_clef == "AFFICHER")   
                    {       
                        Console.WriteLine("Résultat de la commande:\n");
                        connexion.afficherResultatRequete();
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
                            connexion.exporterResultatRequete(nom_fichier);
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
                        ajouterClient();
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
            string base_requete = "SELECT * FROM Utilisateur WHERE idClient is not NULL";
            char choix=' ';
            string derniere_requete=base_requete;//on note la dernière requete exécutée sans indiquer l'ordre de tri(utile pour changer l'ordre avec - et +)
            string requete="";

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Liste des clients:\n");
                Console.WriteLine("Tri possible: A: Ordre Alphabétique; R: Rue; T: Total du montant des achats; C: combinaisons de plusieurs tris");
                Console.WriteLine("Ordre de tri: P: du plus petit au plus grand; M:du plus grand au plus petit (défaut: + )");
                Console.WriteLine("X: Retour\n");
                Console.ResetColor();
                switch (choix)
                {
                    case 'A':
                        requete = base_requete + " ORDER BY Nom ";
                        derniere_requete = requete;
                        this.connexion.executerRequete(requete);
                        this.connexion.afficherResultatRequete();
                        break;
                    case 'R':
                        requete = base_requete + " ORDER BY Adresse ";
                        derniere_requete = requete;
                        this.connexion.executerRequete(requete);
                        this.connexion.afficherResultatRequete();
                        break;
                    case 'T':
                        requete = "SELECT Utilisateur.*, m.total AS total_commandes\r\nFROM Utilisateur\r\nLEFT JOIN (\r\n    SELECT Commande.idClient, SUM(Transaction.Montant) AS total\r\n    FROM Transaction\r\n    JOIN Commande ON Commande.idCommande = Transaction.idCommande\r\n    WHERE Transaction.Reussie = 1\r\n    GROUP BY Commande.idClient\r\n) AS m ON m.idClient = Utilisateur.idClient ORDER BY m.total\r\n";
                        derniere_requete = requete;
                        this.connexion.executerRequete(requete);
                        this.connexion.afficherResultatRequete();
                        break;
                    case 'C':
                        Console.WriteLine("\nTri multiple par ordre du plus important au moins important (e.g. APCM). L'option de tri par total dépensé n'est pas disponible:");
                        string tri_multiple = Console.ReadLine();
                        string requete_finale = base_requete+" ORDER BY ";
                        for(int i=0; i < tri_multiple.Length; i++)
                        {
                            switch (tri_multiple[i])
                            {
                                case 'A':
                                    if (requete_finale[requete_finale.Length-1]!=',' && i!=0) requete_finale+=',';
                                    requete_finale += "Nom ";
                                    break;
                                case 'R':
                                    if (requete_finale[requete_finale.Length-1]!=',' && i!=0) requete_finale+=',';
                                    requete_finale += "Adresse ";
                                    break;
                                case 'M':
                                    requete_finale += "DESC";
                                    break;
                            }
                        }
                        Console.WriteLine(requete_finale);
                        this.connexion.executerRequete(requete_finale);
                        this.connexion.afficherResultatRequete();
                        break;
                    case 'P':
                        requete = derniere_requete;
                        this.connexion.executerRequete(requete);
                        this.connexion.afficherResultatRequete();
                        break;
                    case 'M':
                        if(derniere_requete!="")
                        {
                            if(derniere_requete == base_requete)//dès qu'on l'entre dans l'interface et qu'on appui sur -
                            {
                                derniere_requete += " ORDER BY id";
                            }
                            requete = derniere_requete + " DESC";
                            this.connexion.executerRequete(requete);
                            this.connexion.afficherResultatRequete();
                        }
                        break;
                    case ' '://Cas par défaut, affichage lorsqu'on arrive sur la page
                        this.connexion.executerRequete(base_requete);
                        this.connexion.afficherResultatRequete();
                        break;
                    case 'X':
                        Console.Clear();
                        return;
                }
                choix = (char)Console.ReadKey(false).Key;
            }
        }

        public void ajouterClient()
        {
            while (true)    
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ajout de client\n");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Ajout manuel");
                Console.WriteLine("  2.  Importer depuis un fichier");
                Console.WriteLine("  3.  Retour");
                Console.WriteLine("----------------------------------------------------\n\n");
                Console.WriteLine("Menu choisi:");
                Console.ResetColor();
                char menu_choisi = (char)Console.ReadKey(false).Key;

                switch (menu_choisi)
                {
                    case ('1'):
                        Console.Clear();
                        string[] champs = { "Nom :", "Prénom :", "Adresse :", "Numéro de téléphone :", "Email :", "Nom de l'entreprise (facultatif) :", "Mot de passe :", "id de la station la plus proche :" };
                        string[] valeurs = new string[champs.Length];
                        for(int i=0; i < champs.Length; i++)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(champs[i]+"\n");
                            Console.ResetColor();
                            valeurs[i] = Console.ReadLine();
                            Console.WriteLine("\n"); 
                        }
                        string requete = "INSERT INTO Client (NomEntreprise, MotDePasse) VALUES ('" + valeurs[5] + "', '" + valeurs[6] + "');";
                        this.connexion.executerRequete(requete);
                        requete = "SELECT idClient FROM Client ORDER BY idClient DESC LIMIT 1;";
                        this.connexion.executerRequete(requete);
                        MySqlDataReader reader = this.connexion.recupererResultatRequete();
                        int idClient = 0;
                        if (reader.Read()) //ouvre le reader
                        {
                            idClient = reader.GetInt32(0); //colonne 0
                        }

                        reader.Close();
                        valeurs[2] = valeurs[2].Replace("'", "''"); // Échappe les apostrophes dans l'adresse
                        requete = "INSERT INTO Utilisateur (Nom, Prenom, Adresse, Telephone, Email, IdCuisinier, IdClient, IdStationProche, EstBanni) " +
                            "VALUES('" + valeurs[0] +"', '" + valeurs[1] + "', '" + valeurs[2] + "', '" + valeurs[3] + "', '" + valeurs[4] +"', NULL, "+idClient+", " + valeurs[7] +", 0)";
                        this.connexion.executerRequete(requete);
                        Console.Clear();

                        break;
                    case ('2'):
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Import par fichier:" +
                            "Le fichier doit être au format xml et doit être obtenu en exécutant la requête:\n " +
                            "EXPORTER SELECT * FROM Utilisateur Join Client ON Client.idClient = Utilisateur.idClient WHERE Client.idClient=?;\n" +
                            "Depuis l'espace SQL dédié de l'interface");
                        Console.ResetColor();
                        Console.WriteLine("Chemin d'accès du fichier:");
                        string cheminFichier = Console.ReadLine();

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(cheminFichier);

                        foreach (XmlNode node in xmlDoc.SelectNodes("//export_client_1"))
                        {

                            requete = "INSERT INTO Client (NomEntreprise, MotDePasse) VALUES ('" + node["NomEntreprise"]?.InnerText + "', '" + node["MotDePasse"]?.InnerText + "');";
                            this.connexion.executerRequete(requete);

                            requete = "INSERT INTO Utilisateur (Nom, Prenom, Adresse, Telephone, Email, IdClient, IdStationProche, EstBanni) " +
                                             "VALUES ('" + node["Nom"]?.InnerText + "', '" + node["Prenom"]?.InnerText + "', '" +
                                             node["Adresse"]?.InnerText + "', '" + node["Telephone"]?.InnerText + "', '" +
                                             node["Email"]?.InnerText + "', " + node["IdClient"]?.InnerText + ", " +
                                             node["IdStationProche"]?.InnerText + ", " + (node["EstBanni"]?.InnerText == "true" ? "1" : "0") + ");";

                            this.connexion.executerRequete(requete);
                        }

                        Console.WriteLine("Importation terminée !");

                        break;
                    case ('3'):
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


    }
}
