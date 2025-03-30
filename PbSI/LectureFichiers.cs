namespace PbSI
{
    public class LectureFichiers
    {
        #region Attributs

        /// <summary>
        /// Contenu du fichier
        /// </summary>
        private readonly List<int[]> contenu;

        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        /// <param name="path">Chemin du fichier à lire</param>
        public LectureFichiers(string path)
        {
            string[] lines = File.ReadAllLines(path);

            this.contenu = new List<int[]>();

            string[] infos = new string[3];
            foreach (string line in lines)
            {
                if (line[0] != '%')
                {
                    if (infos[0] == null)
                    {
                        infos = line.Split(" ");
                        string[,] relations = new string[Convert.ToInt32(infos[2]), 2];
                    }
                    else
                    {
                        string[] parts = line.Split(" ");
                        contenu.Add(
                            new int[] { Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]) }
                        );
                    }
                }
            }
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne le contenu du fichier
        /// </summary>
        public List<int[]> Contenu
        {
            get { return contenu; }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// Affiche le contenu du fichier
        /// </summary>
        public void AfficherContenu()
        {
            Console.WriteLine("Contenu du fichier :");
            foreach (var ligne in contenu)
            {
                Console.WriteLine($"{ligne[0]} {ligne[1]}");
            }
        }

        #endregion
    }
}
