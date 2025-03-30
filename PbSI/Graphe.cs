using System;
using System.Drawing;

namespace PbSI
{
    public class Graphe<T> where T : notnull
    {
        #region Attributs

        /// <summary>
        /// Liste des noeuds du graphe
        /// </summary>
        private readonly List<Noeud<T>> noeuds;      

        /// <summary>
        /// Liste des liens du graphe
        /// </summary>
        private readonly HashSet<Lien<T>> liens;

        /// <summary>
        /// Matrice d'adjacence du graphe
        /// </summary>
        private double[,]? matriceAdjacence;

        /// <summary>
        /// Liste d'adjacence du graphe
        /// </summary>
        private Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>>? listeAdjacence;

        /// <summary>
        /// Ordre du graphe (nombre de noeuds)
        /// </summary>
        private int ordre;

        /// <summary>
        /// Taille du graphe (nombre de liens)
        /// </summary>
        private int taille;

        /// <summary>
        /// Indique si le graphe est orienté
        /// </summary>
        private bool estOriente;

        /// <summary>
        /// Indique si le graphe est pondéré
        /// </summary>
        private bool estPondere;

        /// <summary>
        /// Densité du graphe
        /// </summary>
        private double densite;

        /// <summary>
        /// Indique si les propriétés du graphe ont été calculées
        /// </summary>
        private bool proprietesCalculees = false;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Graphe()
        {
            noeuds = new List<Noeud<T>>();
            liens = new HashSet<Lien<T>>();  
        }

        /// <summary>
        /// Constructeur avec matrice d'adjacence
        /// </summary>
        /// <param name="matriceAdjacence">Matrice d'adjacence du graphe</param>
        public Graphe(double[,] matriceAdjacence)
        {
            noeuds = new List<Noeud<T>>();
            liens = new HashSet<Lien<T>>();
            this.matriceAdjacence = matriceAdjacence;


            for (int i = 0; i < matriceAdjacence.GetLength(0); i++)
            {
                noeuds[i] = new Noeud<T>(i);

                for (int j = 0; j < matriceAdjacence.GetLength(1); j++)
                {
                    if (this.matriceAdjacence[i, j] != 0)
                    {
                        AjouterRelation(noeuds[i], noeuds[j], this.matriceAdjacence[i, j]);
                    }
                }
            }

            this.listeAdjacence = GetListeAdjacence();
            UpdateProprietes();
        }

        /// <summary>
        /// Constructeur avec liste d'adjacence
        /// </summary>
        /// <param name="listeAdjacence">Liste d'adjacence du graphe</param>
        public Graphe(Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> listeAdjacence)
        {
            noeuds = new List<Noeud<T>>();
            liens = new HashSet<Lien<T>>();

            this.listeAdjacence = listeAdjacence;

            foreach (var noeud in this.listeAdjacence)
            {
                Noeud<T> source = noeud.Key; 
                int sourceIndex = noeuds.Count;
                noeuds[sourceIndex] = source; 

                foreach (var voisin in noeud.Value)
                {
                    Noeud<T> destination = voisin.Item1; 
                    double poids = voisin.Item2; 

                    AjouterRelation(source, destination, poids);
                }
            }

            this.matriceAdjacence = GetMatriceAdjacence();
            UpdateProprietes();
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne la liste des noeuds du graphe
        /// </summary>
        public List<Noeud<T>> Noeuds
        {
            get { return noeuds; }
        }

        /// <summary>
        /// Retourne la matrice d'adjacence du graphe
        /// </summary>
        public double[,] MatriceAdjacence
        {
            get
            {
                if (this.matriceAdjacence == null || !this.proprietesCalculees)
                {
                    this.matriceAdjacence = GetMatriceAdjacence();
                }
                return this.matriceAdjacence;
            }
        }

        /// <summary>
        /// Retourne la liste d'adjacence du graphe
        /// </summary>
        public Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> ListeAdjacence
        {
            get
            {
                if (this.listeAdjacence == null || !this.proprietesCalculees)
                {
                    this.listeAdjacence = GetListeAdjacence();
                }
                return this.listeAdjacence;
            }
        }


        /// <summary>
        /// Retourne l'ordre du graphe (nombre de noeuds)
        /// </summary>
        public int Ordre
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.ordre;
            }
        }

        /// <summary>
        /// Retourne la taille du graphe (nombre de liens)
        /// </summary>
        public int Taille
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.taille;
            }
        }

        /// <summary>
        /// Retourne si le graphe est orienté
        /// </summary>
        public bool EstOriente
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.estOriente;
            }
        }

        /// <summary>
        /// Retourne si le graphe est pondéré
        /// </summary>
        public bool EstPondere
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.estPondere;
            }
        }

        /// <summary>
        /// Retourne la densité du graphe
        /// </summary>
        public double Densite
        {
            get
            {
                if (!this.proprietesCalculees)
                {
                    UpdateProprietes();
                }
                return this.densite;
            }
        }

        public HashSet<Lien<T>> Liens
        {
            get { return liens; }
        }


        #endregion

        #region Méthodes

        /// <summary>
        /// Ajoute un noeud au graphe
        /// </summary>
        /// <param name="id">Identifiant du noeud à ajouter</param>
        public void AjouterMembre(Noeud<T> noeud)
        {
            if (!noeuds.Contains(noeud))
            {
                noeuds.Add(noeud);
                this.proprietesCalculees = false;
            }

        }

        


        /// <summary>
        /// Ajoute une relation entre deux noeuds
        /// </summary>
        /// <param name="id1">Identifiant du premier noeud</param>
        /// <param name="id2">Identifiant du second noeud</param>
        public void AjouterRelation(Noeud<T> source,Noeud<T> destination, double poids=1)
        {
            AjouterMembre(source);
            AjouterMembre(destination);

            var lien = new Lien<T>(source, destination, poids);

            if (liens.Add(lien))
            {
                this.proprietesCalculees = false;
            }
        }

        /// <summary>
        /// Construit et retourne la matrice d'adjacence du graphe
        /// </summary>
        /// <returns>Matrice d'adjacence du graphe</returns>
        private double[,] GetMatriceAdjacence()
        {
            int taille = noeuds.Count;
            double[,] matrice = new double[taille, taille];

            foreach (Lien<T> lien in liens)
            {
                matrice[lien.Source.Id, lien.Destination.Id] = lien.Poids;
            }

            return matrice;
        }

        /// <summary>
        /// Construit et retourne la liste d'adjacence du graphe
        /// </summary>
        /// <returns>Liste d'adjacence du graphe</returns>
     
        private Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> GetListeAdjacence()
        {
            var listeAdjacence = new Dictionary<Noeud<T>, List<(Noeud<T>, double)>>();

            foreach (var noeud in noeuds)
            {
                listeAdjacence[noeud] = new List<(Noeud<T>, double)>();
            }

            foreach (var lien in liens)
            {
                listeAdjacence[lien.Source].Add((lien.Destination, lien.Poids));
            }

            return listeAdjacence;
        }


        /// <summary>
        /// Met à jour les propriétés du graphe
        /// </summary>
        public void UpdateProprietes()
        {
            this.ordre = this.noeuds.Count;
            this.taille = this.liens.Count;
            this.estOriente = GetEstOriente();
            this.estPondere = GetEstPondere();
            this.densite = GetDensite();
            this.proprietesCalculees = true;
        }

        /// <summary>
        /// Détermine si le graphe est pondéré
        /// </summary>
        /// <returns>true si le graphe est pondéré, false sinon</returns>
        private bool GetEstPondere()
        {
            foreach (Lien<T> lien in this.liens)
            {
                if (lien.Poids != 0 && lien.Poids != 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Détermine si le graphe est orienté
        /// </summary>
        /// <returns>true si le graphe est orienté, false sinon</returns>
        private bool GetEstOriente()
        {
            if (this.matriceAdjacence == null)
            {
                //Console.WriteLine("La matrice n'a pas encore été initialisée");
                return false;
            }

            for (int i = 0; i < this.matriceAdjacence.GetLength(0); i++)
            {
                for (int j = 0; j < this.matriceAdjacence.GetLength(1); j++)
                {
                    if (this.matriceAdjacence[i, j] != this.matriceAdjacence[j, i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Calcule la densité du graphe
        /// </summary>
        /// <returns>Densité du graphe</returns>
        private double GetDensite()
        {
            if (this.ordre < 2)
            {
                return 0.0d;
            }

            if (!this.estOriente)
            {
                return (double)(2 * this.taille) / (this.ordre * (this.ordre - 1));
            }
            else
            {
                return (double)(this.taille) / (this.ordre * (this.ordre - 1));
            }
        }

        /// <summary>
        /// Affiche les propriétés du graphe
        /// </summary>
        public void AfficherProprietes()
        {
            UpdateProprietes();
            Console.WriteLine($"Ordre du graphe : {this.ordre}");
            Console.WriteLine($"Taille du graphe : {this.taille}");
            Console.WriteLine($"Type : {(this.estOriente ? "Orienté" : "Non orienté")}");
            Console.WriteLine($"Type : {(this.estPondere ? "Pondéré" : "Non pondéré")}");
            Console.WriteLine($"Densité : {GetDensite():F2}");
        }

        /// <summary>
        /// Affiche la liste d'adjacence du graphe
        /// </summary>
        public void AfficherListeAdjacence()
        {
            if(this.listeAdjacence != null)
            {
                foreach (var kvp in this.listeAdjacence)
                {
                    Console.Write($"Noeud {kvp.Key.Id} -> ");
                    foreach (var (destination, poids) in kvp.Value)
                    {
                        Console.Write($"(Noeud {destination.Id}, Poids : {poids}) ");
                    }
                    Console.WriteLine();
                }
            }

            else
            {
                Console.WriteLine("Liste d'adjacence null");
            }

        }

        /// <summary>
        /// Affiche la matrice d'adjacence du graphe
        /// </summary>
        public void AfficherMatriceAdjacence()
        {
            if (this.matriceAdjacence != null)
            {
                int taille = this.matriceAdjacence.GetLength(0);


                Console.Write("      ");
                foreach (var noeud in noeuds)
                {
                    Console.Write($"Noeud {noeud.Id}  ");
                }
                Console.WriteLine();

                for (int i = 0; i < taille; i++)
                {
                    Console.Write($"{noeuds[i]}  ");

                    for (int j = 0; j < taille; j++)
                    {
                        Console.Write($"Poids : {this.matriceAdjacence[i, j]:0.##}  ");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Liste d'adjacence null");
            }
        }


        #endregion
    }
}
