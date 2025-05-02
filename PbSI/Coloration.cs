using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{

    public class Coloration<T>
    {
        #region Attributs
        private Graphe<T> graphe;
        private int nombreCouleurs;

        #endregion
        #region Constructeurs
        public Coloration(Graphe<T> graphe)
        {
            this.graphe = graphe;
            this.nombreCouleurs = 0;
        }
        #endregion
        #region Methodes
        public void WelshPowell()
        {
            List<Noeud<T>> noeudsTries = new List<Noeud<T>>(graphe.Noeuds);
            noeudsTries.Sort(
                delegate (Noeud<T> a, Noeud<T> b)
                {
                    int degreA = graphe.GetVoisins(a).Count;
                    int degreB = graphe.GetVoisins(b).Count;
                    return degreB.CompareTo(degreA);
                });
            int couleurActuelle = 1;
            List<string> couleurs = new List<string>();
            foreach (Noeud<T> noeud in noeudsTries)
            {
                if (noeud.Couleur != "")
                {
                    continue;
                }
                string couleur = "Couleur" + couleurActuelle;
                noeud.Couleur = couleur;
                for (int i = 0; i < noeudsTries.Count; i++)
                {
                    Noeud<T> autre = noeudsTries[i];
                    if (autre.Couleur == "")
                    {
                        bool peutColorier = true;
                        List<Noeud<T>> voisins = graphe.GetVoisins(autre);
                        for (int j = 0; j < voisins.Count; j++)
                        {
                            if (voisins[j].Couleur == couleur)
                            {
                                peutColorier = false;
                                break;
                            }
                        }
                        if (peutColorier)
                        {
                            autre.Couleur = couleur;
                        }
                    }
                }

                couleurActuelle++;
            }


            this.nombreCouleurs = couleurActuelle - 1;
        }
            public int GetNombreDeCouleurs()
            {
                return this.nombreCouleurs;
            }

        public void AfficherColoration()
        {
            foreach(Noeud<T> noeud in graphe.Noeuds)
            {
                Console.WriteLine("Noeud" + noeud.Id + " : " + noeud.Couleur);
            }
        }
            #endregion
        public bool EstBiparti()
        {
            Dictionary<Noeud<T>, int> couleurs = new Dictionary<Noeud<T>, int>();
            foreach(Noeud<T> noeud in graphe.Noeuds)
            {
                if (!couleurs.ContainsKey(noeud))
                {
                    Queue<Noeud<T>> file = new Queue<Noeud<T>>();
                    file.Enqueue(noeud);
                    couleurs[noeud] = 0;
                    while (file.Count > 0)
                    {
                        Noeud<T> courant = file.Dequeue();
                        foreach(Noeud<T> voisin in graphe.GetVoisins(courant))
                        {
                            if (!couleurs.ContainsKey(voisin))
                            {
                                couleurs[voisin] = 1 - couleurs[courant];
                                file.Enqueue(voisin);
                            }
                            else if (couleurs[voisin]== couleurs[courant])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool EstPlanaire()
        {
            int n = graphe.Ordre;
            int m = graphe.Taille;
            if (!graphe.EstOriente && m<=3*n-6)
            {
                return true;
            }
            return false;
        }
        public List<List<Noeud<T>>> GetGroupesIndependants()
        {
            List<List<Noeud<T>>> groupes = new List<List<Noeud<T>>>();
            HashSet<Noeud<T>> dejaPris = new HashSet<Noeud<T>>();
            foreach(Noeud<T> noeud in graphe.Noeuds)
            {
                if (dejaPris.Contains(noeud))
                {
                    continue;
                }
                List<Noeud<T>> groupe = new List<Noeud<T>> { noeud};
                dejaPris.Add(noeud);
                foreach(Noeud<T> autre in graphe.Noeuds)
                {
                    if(!dejaPris.Contains(autre)&& !graphe.GetVoisins(noeud).Contains(autre))
                    {
                        bool connecte = false;
                        foreach (var membre in groupe)
                        {
                            if (graphe.GetVoisins(membre).Contains(autre))
                            {
                                connecte = true;
                                break;
                            }
                        }
                        if (!connecte)
                        {
                            groupe.Add(autre);
                            dejaPris.Add(autre);
                        }
                    }
                }
                groupes.Add(groupe);
            }
            return groupes;
        }
    }
}   