using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PbSI
{
    public class Noeud<T> : IEquatable<Noeud<T>>
        where T : notnull
    {
        #region Attributs

        /// <summary>
        /// Identifiant du noeud
        /// </summary>
        private readonly int id;

        private readonly T contenu;

        private string? couleur = "";

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="id">Identifiant du noeud</param>
        public Noeud(int id, T contenu)
        {
            this.id = id;
            this.contenu = contenu;
        }

        public Noeud(int id)
        {
            this.id = id;
        }

        public Noeud(int id, T contenu, string couleur)
        {
            this.id = id;
            this.contenu = contenu;
            this.couleur = couleur;
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne l'identifiant du noeud
        /// </summary>
        public int Id
        {
            get { return this.id; }
        }

        public T? Contenu
        {
            get { return this.contenu; }
        }

        public string? Couleur
        {
            get { return this.couleur; }
            set { this.couleur = value; }
        }

        /// <summary>
        /// Retourne la liste des noeuds voisins
        /// </summary>

        #endregion

        #region Méthodes

        /// <summary>
        /// Retourne une chaine de caractères représentant le noeud
        /// </summary>
        /// <returns>Chaine de caractères représentant le noeud</returns>
        public override string ToString()
        {
            return $"Membre {Id}";
        }

        #endregion

        public bool Equals(Noeud<T> other)
        {
            return this.id.Equals(other.id);
        }
    }
}
