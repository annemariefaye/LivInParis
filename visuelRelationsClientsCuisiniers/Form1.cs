using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using PbSI;

namespace visuelRelationsClientsCuisiniers
{
    public partial class Form1 : Form
    {
        private readonly Graphe<string> graphe;

        public Form1(Graphe<string> graphe)
        {
            InitializeComponent();
            this.graphe = graphe;
            this.Text = "Visualisation Clients-Cuisiniers";
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(panel);

            DessinerGraphe(panel);
        }

        private void DessinerGraphe(Panel panel)
        {
            int startX = 50;
            int startY = 50;
            int espacement = 100;
            int largeurNoeud = 200;
            int hauteurNoeud = 50;
            var police = new Font("Arial", 10);

            using (var g = panel.CreateGraphics())
            {
                /// Dictionnaire pour les positions
                var positions = new System.Collections.Generic.Dictionary<int, Point>();

                /// Dessiner les clients (à gauche)
                int y = startY;
                foreach (var noeud in graphe.Noeuds.Where(n => n.Contenu.StartsWith("Client:")))
                {
                    var rect = new Rectangle(startX, y, largeurNoeud, hauteurNoeud);
                    g.FillEllipse(Brushes.LightBlue, rect);
                    g.DrawEllipse(Pens.Black, rect);
                    g.DrawString(noeud.Contenu, police, Brushes.Black, startX + 10, y + 15);
                    positions[noeud.Id] = new Point(rect.Right, rect.Top + hauteurNoeud / 2);
                    y += espacement;
                }

                /// Dessiner les cuisiniers (à droite)
                y = startY;
                foreach (var noeud in graphe.Noeuds.Where(n => n.Contenu.StartsWith("Cuisinier:")))
                {
                    var rect = new Rectangle(startX + 400, y, largeurNoeud, hauteurNoeud);
                    g.FillEllipse(Brushes.LightGreen, rect);
                    g.DrawEllipse(Pens.Black, rect);
                    g.DrawString(noeud.Contenu, police, Brushes.Black, startX + 410, y + 15);
                    positions[noeud.Id] = new Point(rect.Left, rect.Top + hauteurNoeud / 2);
                    y += espacement;
                }

                /// Dessiner les liens
                foreach (var lien in graphe.Liens)
                {
                    if (positions.TryGetValue(lien.Source.Id, out var debut) &&
                        positions.TryGetValue(lien.Destination.Id, out var fin))
                    {
                        using (var pen = new Pen(Color.Gray, 2))
                        {
                            g.DrawLine(pen, debut, fin);
                            DessinerFleche(g, pen, debut, fin);
                        }
                    }
                }
            }
        }

        private void DessinerFleche(Graphics g, Pen pen, Point debut, Point fin)
        {
            /// Taille de la flèche
            int tailleFleche = 10;

            /// Calcul de l'angle
            double angle = Math.Atan2(fin.Y - debut.Y, fin.X - debut.X);

            /// Points de la flèche
            Point[] points =
            {
                fin,
                new Point(
                    (int)(fin.X - tailleFleche * Math.Cos(angle - Math.PI / 6)),
                    (int)(fin.Y - tailleFleche * Math.Sin(angle - Math.PI / 6))),
                new Point(
                    (int)(fin.X - tailleFleche * Math.Cos(angle + Math.PI / 6)),
                    (int)(fin.Y - tailleFleche * Math.Sin(angle + Math.PI / 6)))
            };

            g.FillPolygon(pen.Brush, points);
        }
    }
}