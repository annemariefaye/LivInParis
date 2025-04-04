using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NAudio.Wave;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PbSI
{
    internal class Menu
    {
        private string pageChoisie;
        private Connexion connexion;
        ReseauMetro reseau;
        Graphe<StationMetro> graphe;
        private static WaveOutEvent _dispositifSortie;
        private static AudioFileReader _fichierAudio;
        private static bool _isPlaying;
        private string maroc;

        public Menu()
        {
            // Initialisation des membres
            this.connexion = new Connexion();
            this.reseau = new ReseauMetro("MetroParis.xlsx");
            this.graphe = reseau.Graphe;
            this.maroc = "maroc.mp3";
        }

        public async Task InitialiserAsync()
        {

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
                        await MenuSimplifie();
                        this.pageChoisie = "Menu simplifié";
                        break;

                    case '3':
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nMerci d'avoir utilisé LivInParis ! À bientôt !");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOption invalide. Appuyez sur une touche pour continuer...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region Menus
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
                if (requete.ToUpper() == "RETOUR") return;

                try
                {
                    this.connexion.executerRequete(requete.Substring(9));

                    mot_clef = "";
                    if (requete.Length < 8) break;

                    for (int i = 0; i < 8; i++)
                    {
                        mot_clef += requete[i];
                    }

                    if (mot_clef.ToUpper() == "AFFICHER")
                    {
                        Console.WriteLine("Résultat de la commande:\n");
                        connexion.afficherResultatRequete();
                    }

                    if (mot_clef.ToUpper() == "EXPORTER")
                    {
                        if (requete.Substring(9).Substring(0, 6) != "SELECT")
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
        public async Task MenuSimplifie()
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
                Console.WriteLine("  5.  Retour");
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
                        await ModuleCuisinier();
                        break;
                    case ('3'):
                        Console.Clear();
                        ModuleCommande();
                        break;
                    case ('4'):
                        Statistiques statistiques = new Statistiques(this.connexion);
                        Console.Clear();
                        break;
                    case ('5'):
                        Console.Clear();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOption invalide. Appuyez sur une touche pour continuer...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
        #endregion

        #region Client
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
                        supprimerClient();
                        break;
                    case ('4'):
                        Console.Clear();
                        modifierClient();
                        break;
                    case ('5'):
                        Console.Clear();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOption invalide. Appuyez sur une touche pour continuer...");
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

        public void supprimerClient()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Moyen d'identification du client à supprimer: \n");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Identifiant client");
                Console.WriteLine("  2.  Retour");
                Console.WriteLine("----------------------------------------------------\n\n");
                Console.WriteLine("Menu choisi:");
                Console.ResetColor();
                char menu_choisi = (char)Console.ReadKey(false).Key;

                string requete_client = "DELETE FROM Client WHERE";//pour pouvoir par la suite rajouter d'autres moyens de suppression de clients
                switch (menu_choisi)
                {
                    case ('1'):
                        Console.Clear();
                        Console.WriteLine("Identifiant client:");
                        string idClient = Console.ReadLine();
                        requete_client += " idClient = "+idClient;
                        this.connexion.executerRequete(requete_client);
                        Console.Clear();
                        Console.WriteLine("Client supprimé !\n");
                        break;
                    case ('2'):
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

        public void modifierClient()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Interface de modification des clients\n");
                Console.WriteLine("Tapez 'X' pour revenir au menu précédent\n\n");
                Console.WriteLine("Identifiant du client à modifier: \n");
                Console.ResetColor();
                string identifiant_client = Console.ReadLine();

                switch (identifiant_client)
                {
                    case ("X"):
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("informations du client:");
                        string requete = "SELECT * FROM Utilisateur JOIN Client ON Client.IdClient = Utilisateur.IdClient WHERE Utilisateur.IdClient ="+identifiant_client;
                        this.connexion.executerRequete(requete);
                        this.connexion.afficherResultatRequete();
                        Console.WriteLine("Quelle colonne souhaitez-vous modifier:");
                        string colonne = Console.ReadLine();
                        string table = (colonne == "NomEntreprise" || colonne == "MotDePasse") ? "Client" : "Utilisateur";
                        Console.WriteLine("Quelle est la nouvelle valeur de la colonne " + colonne);
                        string nouvelleValeur=Console.ReadLine();
                        requete = "UPDATE "+table+" SET "+colonne+" = '"+nouvelleValeur+"' WHERE idClient = "+identifiant_client;
                        this.connexion.executerRequete(requete);
                        Console.Clear();
                        Console.WriteLine("La requete a été modifiée avec succès");
                        break;
                }
            }
        }
        #endregion

        #region Cuisinier
        public async Task ModuleCuisinier()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Module Cuisinier !\n");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Afficher les cuisiniers");
                Console.WriteLine("  2.  Ajouter un cuisinier");
                Console.WriteLine("  3.  Supprimer un cuisinier");
                Console.WriteLine("  4.  Modifier un cuisinier");
                Console.WriteLine("  5.  Afficher les clients servis par un cuisinier");
                Console.WriteLine("  6.  Afficher les plats réalisés par un cuisinier");
                Console.WriteLine("  7.  Afficher le plat du jour");
                Console.WriteLine("  8.  Retour");
                Console.WriteLine("----------------------------------------------------\n\n");
                Console.WriteLine("Menu choisi:");
                Console.ResetColor();
                char menu_choisi = (char)Console.ReadKey(false).Key;

                switch (menu_choisi)
                {
                    case ('1'):
                        Console.Clear();
                        afficherCuisiniers();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('2'):
                        Console.Clear();
                        await ajouterCuisinier(this.graphe); 
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('3'):
                        Console.Clear();
                        supprimerCuisinier();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('4'):
                        Console.Clear();
                        modifierCuisinier();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('5'):
                        Console.Clear();
                        afficherClientsServisParCuisinier();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('6'):
                        Console.Clear();
                        afficherPlatsRealisesParCuisinier();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('7'):
                        Console.Clear();
                        afficherPlatDuJour();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('8'):
                        Console.Clear();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOption invalide. Appuyez sur une touche pour continuer...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        public void afficherCuisiniers()
        {
            string requete = "SELECT Utilisateur.Nom, Utilisateur.Prenom, Cuisinier.PlatDuJour FROM Cuisinier JOIN Utilisateur ON Cuisinier.IdCuisinier = Utilisateur.IdCuisinier";
            this.connexion.executerRequete(requete);
            this.connexion.afficherResultatRequete();
        }

        public async Task ajouterCuisinier(Graphe<StationMetro> graphe)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ajout de cuisinier\n");

            string nom;
            do
            {
                Console.WriteLine("Nom du cuisinier :");
                nom = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(nom) || !Regex.IsMatch(nom, @"^[A-Za-zÀ-ÖØ-öø-ÿ\-]+$"));

            string prenom;
            do
            {
                Console.WriteLine("Prénom du cuisinier :");
                prenom = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(prenom) || !Regex.IsMatch(prenom, @"^[A-Za-zÀ-ÖØ-öø-ÿ\-]+$"));

            string adresse;
            do
            {
                Console.WriteLine("Adresse du cuisinier :");
                adresse = Console.ReadLine();

                Console.WriteLine("Vérification de l'adresse...");
                bool adresseValide = await VerifierAdresseAsync(adresse);
                Console.WriteLine($"Adresse valide : {adresseValide}");

                if (!adresseValide)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Adresse invalide. Veuillez entrer une adresse réelle.");
                    Console.ResetColor();
                    adresse = null;
                }
            } while (string.IsNullOrWhiteSpace(adresse));

            string telephone;
            do
            {
                Console.WriteLine("Téléphone du cuisinier (10 chiffres) :");
                telephone = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(telephone) || !Regex.IsMatch(telephone, @"^\d{10}$"));

            string email;
            do
            {
                Console.WriteLine("Email du cuisinier :");
                email = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));

            Console.WriteLine("Mot de passe du cuisinier :");
            string motDePasse = Console.ReadLine();

            Console.WriteLine("Plat du jour du cuisinier :");
            string platDuJour = Console.ReadLine();

            // Ajout du cuisinier
            string requeteCuisinier = $"INSERT INTO Cuisinier (MotDePasse, PlatDuJour) VALUES ('{motDePasse}', '{platDuJour}')";
            this.connexion.executerRequete(requeteCuisinier);

            // Récupération ID cuisinier
            string requeteIdCuisinier = "SELECT IdCuisinier FROM Cuisinier ORDER BY IdCuisinier DESC LIMIT 1";
            this.connexion.executerRequete(requeteIdCuisinier);
            var reader = this.connexion.recupererResultatRequete();
            int idCuisinier = 0;
            if (reader.Read())
            {
                idCuisinier = reader.GetInt32(0);
            }
            reader.Close();

            // Ajout utilisateur
            string requeteUtilisateur = $"INSERT INTO Utilisateur (Nom, Prenom, Adresse, Telephone, Email, IdCuisinier) " +
                                        $"VALUES ('{nom}', '{prenom}', '{adresse}', '{telephone}', '{email}', {idCuisinier})";
            this.connexion.executerRequete(requeteUtilisateur);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cuisinier ajouté avec succès !");
            Console.ResetColor();
        }

        public void supprimerCuisinier()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            string idCuisinierStr;
            int idCuisinier;
            bool cuisinierExistant = false;

            while (!cuisinierExistant)
            {
                Console.WriteLine("Identifiant du cuisinier à supprimer :");
                idCuisinierStr = Console.ReadLine();

                if (!int.TryParse(idCuisinierStr, out idCuisinier))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("L'identifiant doit être un nombre valide. Veuillez réessayer.");
                    continue; 
                }

                string requeteVerifCuisinier = $"SELECT COUNT(*) FROM Cuisinier WHERE IdCuisinier = {idCuisinier};";
                this.connexion.executerRequete(requeteVerifCuisinier);

                var reader = this.connexion.recupererResultatRequete();
                if (reader.Read() && reader.GetInt32(0) > 0)
                {
                    cuisinierExistant = true; 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Aucun cuisinier trouvé avec cet identifiant. Veuillez réessayer.");
                }
                reader.Close();

                string requeteSupprimerCuisinier = $"DELETE FROM Cuisinier WHERE IdCuisinier = {idCuisinier};";
                this.connexion.executerRequete(requeteSupprimerCuisinier);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cuisinier supprimé avec succès !");
            Console.ResetColor();
        }


        public void modifierCuisinier()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            int idCuisinier = -1;
            bool identifiantValide = false;

            while (!identifiantValide)
            {
                Console.WriteLine("Identifiant du cuisinier à modifier :");
                string idCuisinierStr = Console.ReadLine();

                if (!int.TryParse(idCuisinierStr, out idCuisinier))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("L'identifiant doit être un nombre valide. Veuillez réessayer.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    continue;
                }

                string requeteVerifCuisinier = "SELECT COUNT(*) FROM Cuisinier WHERE IdCuisinier = " + idCuisinier + ";";
                this.connexion.executerRequete(requeteVerifCuisinier);

                var reader = this.connexion.recupererResultatRequete();
                if (reader.Read() && reader.GetInt32(0) > 0)
                {
                    identifiantValide = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Aucun cuisinier trouvé avec cet identifiant. Veuillez réessayer.");
                }
                reader.Close();
            }

            string nouveauNom = "";
            bool nomValide = false;

            while (!nomValide)
            {
                Console.WriteLine("Nouvelle valeur pour le nom :");
                nouveauNom = Console.ReadLine();

                if (nouveauNom.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Le nom ne doit pas contenir de chiffres. Veuillez réessayer.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    continue;
                }

                nomValide = true;
            }

            string nouveauPrenom = "";
            bool prenomValide = false;

            while (!prenomValide)
            {
                Console.WriteLine("Nouveau prénom :");
                nouveauPrenom = Console.ReadLine();

                if (nouveauPrenom.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Le prénom ne doit pas contenir de chiffres. Veuillez réessayer.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    continue;
                }

                prenomValide = true;
            }

            string requete = "UPDATE Utilisateur SET Nom = '" + nouveauNom + "', Prenom = '" + nouveauPrenom + "' WHERE IdCuisinier = " + idCuisinier + ";";
            this.connexion.executerRequete(requete);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cuisinier modifié avec succès !");
            Console.ResetColor();
        }


        public void afficherClientsServisParCuisinier()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            int idCuisinier = -1;
            bool identifiantValide = false;

            while (!identifiantValide)
            {
                Console.WriteLine("Identifiant du cuisinier :");
                string idCuisinierStr = Console.ReadLine();

                if (!int.TryParse(idCuisinierStr, out idCuisinier))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("L'identifiant doit être un nombre valide. Veuillez réessayer.");
                    continue;
                }

                string requeteVerifCuisinier = "SELECT COUNT(*) FROM Cuisinier WHERE IdCuisinier = " + idCuisinier + ";";
                this.connexion.executerRequete(requeteVerifCuisinier);

                var reader = this.connexion.recupererResultatRequete();
                if (reader.Read() && reader.GetInt32(0) > 0)
                {
                    identifiantValide = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Aucun cuisinier trouvé avec cet identifiant. Veuillez réessayer.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                reader.Close();
            }
            string requete = $"SELECT DISTINCT Utilisateur.Nom, Utilisateur.Prenom FROM Commande JOIN Utilisateur ON Commande.IdClient = Utilisateur.IdClient JOIN LigneDeCommande ON Commande.IdCommande = LigneDeCommande.IdCommande JOIN Plat ON LigneDeCommande.IdPlat = Plat.IdPlat WHERE Plat.IdCuisinier = {idCuisinier};";
            this.connexion.executerRequete(requete);
            this.connexion.afficherResultatRequete();
        }


        public void afficherPlatsRealisesParCuisinier()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            int idCuisinier = -1;
            bool identifiantValide = false;

            while (!identifiantValide)
            {
                Console.WriteLine("Identifiant du cuisinier :");
                string idCuisinierStr = Console.ReadLine();

                if (!int.TryParse(idCuisinierStr, out idCuisinier))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("L'identifiant doit être un nombre valide. Veuillez réessayer.");
                    continue;
                }

                string requeteVerifCuisinier = "SELECT COUNT(*) FROM Cuisinier WHERE IdCuisinier = " + idCuisinier + ";";
                this.connexion.executerRequete(requeteVerifCuisinier);

                var reader = this.connexion.recupererResultatRequete();
                if (reader.Read() && reader.GetInt32(0) > 0)
                {
                    identifiantValide = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Aucun cuisinier trouvé avec cet identifiant. Veuillez réessayer.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                reader.Close();
            }
            string requete = "SELECT Plat.Nom, COUNT(*) as Frequence FROM Commande JOIN LigneDeCommande ON Commande.IdCommande = LigneDeCommande.IdCommande JOIN Plat ON LigneDeCommande.IdPlat = Plat.IdPlat WHERE Plat.IdCuisinier = " + idCuisinier + " GROUP BY Plat.Nom;";
            this.connexion.executerRequete(requete);
            this.connexion.afficherResultatRequete();
        }

        public void afficherPlatDuJour()
        {
            Console.Clear();
            string requete = "SELECT Utilisateur.Nom, Utilisateur.Prenom, Cuisinier.PlatDuJour FROM Cuisinier JOIN Utilisateur ON Cuisinier.IdCuisinier = Utilisateur.IdCuisinier";
            this.connexion.executerRequete(requete);
            this.connexion.afficherResultatRequete();
        }
        #endregion

        #region Commande
        public void ModuleCommande()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Module Commande !\n");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("  1.  Créer une nouvelle commande");
                Console.WriteLine("  2.  Modifier une commande");
                Console.WriteLine("  3.  Afficher le prix d'une commande");
                Console.WriteLine("  4.  Afficher les commandes");
                Console.WriteLine("  5.  Retour");
                Console.WriteLine("----------------------------------------------------\n\n");
                Console.WriteLine("Menu choisi:");
                Console.ResetColor();
                char menu_choisi = (char)Console.ReadKey(false).Key;

                switch (menu_choisi)
                {
                    case ('1'):
                        Console.Clear();
                        creerCommande();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('2'):
                        Console.Clear();
                        modifierCommande();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('3'):
                        Console.Clear();
                        afficherPrixCommande();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('4'):
                        Console.Clear();
                        afficherCommandes();
                        Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case ('5'):
                        Console.Clear();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nOption invalide. Appuyez sur une touche pour continuer...");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        public async Task creerCommande()
        {
            Console.WriteLine("Identifiant du client :");
            string idClient = Console.ReadLine();

            string requeteAdresseClient = "SELECT Adresse FROM Utilisateur WHERE Id = " + idClient;
            this.connexion.executerRequete(requeteAdresseClient);
            var reader = this.connexion.recupererResultatRequete();

            string adresseDepart;
            if (reader.Read())
            {
                adresseDepart = reader.GetString(0);
                Console.WriteLine("Adresse du client : " + adresseDepart);
            }
            else
            {
                Console.WriteLine("Client introuvable.");
                return;
            }
            reader.Close();

            string requetePlats = "SELECT IdPlat, Nom FROM Plat WHERE IdCuisinier IS NOT NULL";
            this.connexion.executerRequete(requetePlats);
            var readerPlats = this.connexion.recupererResultatRequete();

            Console.Clear();


            Console.WriteLine("Plats disponibles :");
            while (readerPlats.Read())
            {
                Console.WriteLine($"{readerPlats.GetInt32(0)}: {readerPlats.GetString(1)}");
            }
            readerPlats.Close();


            Console.WriteLine("Choisissez un plat en entrant son identifiant :");
            string idPlat = Console.ReadLine();

            // Musique
            PlayAudioAsync(maroc);

            string requeteAdresseCuisinier = "SELECT Utilisateur.Adresse FROM Utilisateur JOIN Plat ON Utilisateur.Id = Plat.IdCuisinier WHERE Plat.IdPlat = " + idPlat;
            this.connexion.executerRequete(requeteAdresseCuisinier);
            var readerCuisinier = this.connexion.recupererResultatRequete();

            string adresseCuisinier;
            if (readerCuisinier.Read())
            {
                adresseCuisinier = readerCuisinier.GetString(0);
            }
            else
            {
                Console.WriteLine("Aucun cuisinier trouvé pour ce plat.");
                return;
            }
            readerCuisinier.Close();

            Console.Clear();

            RechercheStationProche rechercheDepart = new RechercheStationProche(adresseDepart, graphe);
            RechercheStationProche rechercheArrivee = new RechercheStationProche(adresseCuisinier, graphe);
            await rechercheDepart.InitialiserAsync();
            Console.WriteLine("\n\n");
            await rechercheArrivee.InitialiserAsync();
            Console.WriteLine("\n\n");


            List<int> depart = rechercheDepart.IdStationsProches;
            List<int> arrivee = rechercheArrivee.IdStationsProches;

            float tempsDeplacementDepart = rechercheDepart.TempsDeplacement;
            float tempsDeplacementArrivee = rechercheArrivee.TempsDeplacement;

            var resultat = RechercheChemin<StationMetro>.DijkstraListe(graphe, depart, arrivee);

            if (resultat != null)
            {
                double tempsTotal = tempsDeplacementDepart + tempsDeplacementArrivee + resultat.PoidsTotal;
                Console.WriteLine("Temps total de déplacement : " + (int)tempsTotal + " minutes.");
            }
            else
            {
                Console.WriteLine("Aucun chemin trouvé.");
                return;
            }

            Console.WriteLine("\n\n");


            string requeteCommande = "INSERT INTO Commande (IdClient, Statut) VALUES (" + idClient + ", 'En attente')";
            this.connexion.executerRequete(requeteCommande);

            string requeteIdCommande = "SELECT MAX(IdCommande) FROM Commande";
            this.connexion.executerRequete(requeteIdCommande);
            var readerCommande = this.connexion.recupererResultatRequete();
            int idCommande = 0;

            if (readerCommande.Read())
            {
                idCommande = readerCommande.GetInt32(0);
            }
            readerCommande.Close();

            string requeteLigneDeCommande = "INSERT INTO LigneDeCommande (IdCommande, IdPlat, Quantite, DateLivraison, LieuLivraison) VALUES (" + idCommande + ", " + idPlat + ", 1, CURDATE(), '" + adresseCuisinier + "')";
            this.connexion.executerRequete(requeteLigneDeCommande);

            Console.WriteLine("Commande créée avec succès !");
            StopAudio();
        }


        public void modifierCommande()
        {
            Console.WriteLine("Identifiant de la commande à modifier :");
            string idCommande = Console.ReadLine();

            Console.WriteLine("Nouveau statut de la commande :");
            string nouveauStatut = Console.ReadLine();

            string requete = "UPDATE Commande SET Statut = '" + nouveauStatut + "' WHERE IdCommande = " + idCommande;
            this.connexion.executerRequete(requete);
            Console.WriteLine("Commande modifiée avec succès !");
        }

        public void afficherPrixCommande()
        {
            Console.WriteLine("Identifiant de la commande :");
            string idCommande = Console.ReadLine();

            string requete = "SELECT SUM(Plat.Prix * LigneDeCommande.Quantite) AS PrixTotal FROM LigneDeCommande JOIN Plat ON LigneDeCommande.IdPlat = Plat.IdPlat WHERE LigneDeCommande.IdCommande = " + idCommande;
            this.connexion.executerRequete(requete);
            this.connexion.afficherResultatRequete();
        }

        public void afficherCommandes()
        {
            string requete = "SELECT * FROM Commande";
            this.connexion.executerRequete(requete);
            this.connexion.afficherResultatRequete();
        }

        #endregion

        #region Audio
        /// Fonction asynchrone pour jouer de la musique en même temps que de jouer au jeu
        static async Task PlayAudioAsync(string chemin)
        {
            Task.Run(() =>
            {
                using (var fichierAudio = new AudioFileReader(chemin))
                using (var dispositifSortie = new WaveOutEvent())
                {
                    dispositifSortie.Init(fichierAudio);
                    dispositifSortie.Play();

                    /// On attend que la musique finisse
                    while (dispositifSortie.PlaybackState == PlaybackState.Playing)
                    {
                        Task.Delay(100).Wait(); /// On évite de bloquer le thread principal
                    }
                }
            });
        }

        /// Fonction asynchrone pour jouer de la musique en boucle en même temps que de jouer au jeu

        static void PlayAudioLoop(string chemin)
        {
            _fichierAudio = new AudioFileReader(chemin);
            _dispositifSortie = new WaveOutEvent();
            _dispositifSortie.Init(_fichierAudio);
            _dispositifSortie.Play();
            _isPlaying = true; /// Indique que la musique est en lecture

            /// Événement pour jouer la musique en boucle
            _dispositifSortie.PlaybackStopped += (sender, args) =>
            {
                if (_isPlaying)
                {
                    _fichierAudio.Position = 0; /// Remet le fichier audio au début
                    _dispositifSortie.Play(); /// Rejoue la musique
                }
            };

            /// Attendre que la musique finisse ou qu'elle soit arrêtée
            while (_isPlaying)
            {
                /// Vérifie si la musique est toujours en lecture
                if (_dispositifSortie.PlaybackState != PlaybackState.Playing)
                {
                    break; /// Sort de la boucle si la musique s'arrête
                }
                Task.Delay(100).Wait(); /// Petite pause pour éviter de bloquer le thread principal
            }
        }

        static void StopAudio()
        {
            /// Vérifie si le dispositif de sortie est initialisé avant d'arrêter
            if (_dispositifSortie != null)
            {
                _isPlaying = false; /// Indique que la musique ne doit plus jouer
                _dispositifSortie.Stop();
                _dispositifSortie.Dispose(); /// Libère les ressources
                _fichierAudio.Dispose(); /// Libère les ressources
                _dispositifSortie = null; /// Réinitialise la référence
                _fichierAudio = null; /// Réinitialise la référence
            }
        }

        #endregion


        public static async Task<bool> VerifierAdresseAsync(string adresse)
        {
            if (string.IsNullOrWhiteSpace(adresse))
                return false;

            string url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(adresse)}";

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return false;

                string json = await response.Content.ReadAsStringAsync();
                JArray data = JArray.Parse(json);

                return data.Count > 0;
            }
            catch
            {
                return false;
            }
        }

    }
}
