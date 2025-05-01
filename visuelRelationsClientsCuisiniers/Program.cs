using System;
using System.Windows.Forms;
using PbSI;

namespace visuelRelationsClientsCuisiniers
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Initialiser les relations clients-cuisiniers
                var relations = new relationsClientsCuisiniers();

                // Créer et afficher le formulaire principal avec le graphe
                Application.Run(new Form1(relations.Graphe));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors du chargement du graphe:\n{ex.Message}",
                              "Erreur",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}