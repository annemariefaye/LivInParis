using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PbSI
{

    /// <summary>
    /// Classe représentant un noeud dans un graphe.
    /// </summary>
    public class Noeud<T> : IEquatable<Noeud<T>>
        where T : notnull
    {
        #region Attributs

        /// <summary>
        /// Identifiant du noeud
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Contenu du noeud
        /// </summary>
        private readonly T contenu;

        /// <summary>
        /// Couleur du noeud (pour l'affichage)
        /// </summary>
        private string? couleur = "";

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="id">Identifiant du noeud</param>
        /// <param name="contenu">Contenu du noeud</param>
        public Noeud(int id, T contenu)
        {
            this.id = id;
            this.contenu = contenu;
        }

        /// <summary>
        /// Constructeur avec identifiant
        /// </summary>
        /// <param name="id">Identifiant du noeud</param>
        public Noeud(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Constructeur avec identifiant, contenu et couleur
        /// </summary>
        /// <param name="id">Identifiant du noeud</param>
        /// <param name="contenu">Contenu du noeud</param>
        /// <param name="couleur">Couleur du noeud</param>
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

        /// <summary>
        /// Retourne le contenu du noeud
        /// </summary>
        public T? Contenu
        {
            get { return this.contenu; }
        }

        /// <summary>
        /// Retourne la couleur du noeud (pour l'affichage)
        /// </summary>
        public string? Couleur
        {
            get { return this.couleur; }
            set { this.couleur = value; }
        }

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

        /// <summary>
        /// Teste l'égalité entre deux noeuds
        /// </summary>
        /// <param name="other">Noeud à comparer</param>
        /// <returns>true si les noeuds sont égaux, false sinon</returns>
        public bool Equals(Noeud<T> other)
        {
            return this.id.Equals(other.id);
        }
        #endregion
    }
}
