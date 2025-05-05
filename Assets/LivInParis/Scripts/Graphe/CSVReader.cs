using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace PbSI 
{ 

    /// <summary>
    /// Classe permettant de lire des fichiers CSV et de les convertir en liste de dictionnaires.
    /// </summary>
    public class CSVReader
    {
        #region Constantes
        /// <summary>
        /// Expression régulière pour séparer les colonnes.
        /// </summary>
        static string SPLIT_RE = @";(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

        /// <summary>
        /// Expression régulière pour séparer les lignes.
        /// </summary>
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";

        /// <summary>
        /// Caractères à supprimer lors du traitement des valeurs.
        /// </summary>
        static char[] TRIM_CHARS = { '\"' };
        #endregion

        #region Méthodes
        /// <summary>
        /// Lit un fichier CSV et retourne une liste de dictionnaires représentant les données.
        /// </summary>
        /// <param name="file">Nom du fichier CSV à lire.</param>
        /// <returns>Liste de dictionnaires contenant les données du fichier CSV.</returns>
        public static List<Dictionary<string, object>> Read(string file)
        {
            var list = new List<Dictionary<string, object>>();
            TextAsset data = Resources.Load(file) as TextAsset;

            var lines = Regex.Split(data.text, LINE_SPLIT_RE);

            if (lines.Length <= 1)
                return list;

            var header = Regex.Split(lines[0], SPLIT_RE);

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "")
                    continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }

                list.Add(entry);
            }
            return list;
        }
        #endregion
    }

}