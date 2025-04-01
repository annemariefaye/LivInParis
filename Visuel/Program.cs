using PbSI;
using System.Drawing.Imaging;

public class Visualisation : Form
{
    private Graphe<StationMetro> graphe;
    private Dictionary<string, Color> ligneColors;
    private float minX, maxX, minY, maxY;

    public Visualisation()
    {
        ReseauMetro reseau = new ReseauMetro("MetroParis.xlsx");
        graphe = reseau.Graphe;
        ligneColors = new Dictionary<string, Color>();
        this.Text = "Graphe du Métro de Paris";
        this.Size = new Size(1500, 1000);

        // Ajout d'un bouton pour enregistrer l'image
        Button saveButton = new Button
        {
            Text = "Enregistrer en tant qu'image",
            Dock = DockStyle.Bottom
        };
        saveButton.Click += SaveButton_Click;
        this.Controls.Add(saveButton);

        this.Paint += new PaintEventHandler(DrawGraph);
        NormalizeCoordinates();
    }

    private void NormalizeCoordinates()
    {
        minX = graphe.Noeuds.Min(n => (float)n.Contenu.Longitude);
        maxX = graphe.Noeuds.Max(n => (float)n.Contenu.Longitude);
        minY = graphe.Noeuds.Min(n => (float)n.Contenu.Latitude);
        maxY = graphe.Noeuds.Max(n => (float)n.Contenu.Latitude);
    }

    private PointF ConvertToScreenCoordinates(float lon, float lat)
    {
        float margin = 0; // Réduire la marge pour utiliser tout l'espace
        float scaleX = (this.ClientSize.Width - margin) / (maxX - minX);
        float scaleY = (this.ClientSize.Height - margin) / (maxY - minY);
        float scale = Math.Min(scaleX, scaleY) * 0.95f; // Réduire légèrement le scale pour éviter le débordement

        float screenX = (lon - minX) * scale + margin / 2;

        float upperMargin = 30; // Ajustez cette valeur pour abaisser le haut du graphe
        float screenY = (maxY - lat) * scale + margin / 2 + upperMargin; // Ajout de la marge supérieure

        return new PointF(screenX, screenY);
    }

    private void DrawArrow(Graphics g, Pen pen, PointF start, PointF end)
    {
        g.DrawLine(pen, start, end);

        float arrowSize = 10;
        double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
        PointF arrow1 = new PointF(
            end.X - (float)(arrowSize * Math.Cos(angle - Math.PI / 6)),
            end.Y - (float)(arrowSize * Math.Sin(angle - Math.PI / 6))
        );
        PointF arrow2 = new PointF(
            end.X - (float)(arrowSize * Math.Cos(angle + Math.PI / 6)),
            end.Y - (float)(arrowSize * Math.Sin(angle + Math.PI / 6))
        );
        g.DrawLine(pen, end, arrow1);
        g.DrawLine(pen, end, arrow2);
    }

    private void DrawGraph(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        Dictionary<Noeud<StationMetro>, PointF> positions = new Dictionary<Noeud<StationMetro>, PointF>();
        Dictionary<Noeud<StationMetro>, RectangleF> labelBounds = new Dictionary<Noeud<StationMetro>, RectangleF>();

        // Calcul des positions des nœuds
        foreach (var noeud in graphe.Noeuds)
        {
            StationMetro station = noeud.Contenu;
            positions[noeud] = ConvertToScreenCoordinates((float)station.Longitude, (float)station.Latitude);
        }

        // Dessiner les liens entre les nœuds
        foreach (var lien in graphe.Liens)
        {
            Noeud<StationMetro> source = lien.Source;
            Noeud<StationMetro> destination = lien.Destination;
            Color color = GetLigneColor(source.Contenu.Ligne);
            Pen pen = new Pen(color, 2);

            PointF start = positions[source];
            PointF end = positions[destination];
            PointF direction = new PointF((end.X - start.X) * 0.85f + start.X, (end.Y - start.Y) * 0.85f + start.Y);

            DrawArrow(g, pen, start, direction);
        }

        // Créer un dictionnaire pour suivre les coordonnées déjà affichées
        HashSet<PointF> displayedLabels = new HashSet<PointF>();

        // Dessiner les nœuds et les étiquettes
        foreach (var noeud in graphe.Noeuds)
        {
            PointF position = positions[noeud];
            g.FillEllipse(Brushes.Black, position.X - 5, position.Y - 5, 10, 10);

            // Vérifier si cette position a déjà été utilisée pour une étiquette
            if (!displayedLabels.Contains(position))
            {
                // Ajouter la position au set pour éviter de réécrire le même label
                displayedLabels.Add(position);

                // Créer la chaîne pour l'étiquette
                string label = noeud.Contenu.Libelle;
                Font labelFont = new Font("Arial", 3);
                SizeF labelSize = g.MeasureString(label, labelFont);

                // Déterminer la position de l'étiquette
                PointF labelPosition = new PointF(position.X + 12, position.Y - 12);

                // Dessiner l'étiquette
                g.DrawString(label, labelFont, Brushes.Black, labelPosition);
            }
        }
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        // Facteur d'échelle pour augmenter la qualité
        float scaleFactor = 3.0f;

        // Créer un bitmap de la taille de la fenêtre multipliée par le facteur d'échelle
        using (Bitmap bitmap = new Bitmap((int)(this.ClientSize.Width * scaleFactor), (int)(this.ClientSize.Height * scaleFactor)))
        {
            // Dessiner le contenu de la fenêtre sur le bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(this.BackColor);
                g.ScaleTransform(scaleFactor, scaleFactor); // Appliquer l'échelle
                DrawGraph(this, new PaintEventArgs(g, this.ClientRectangle));
            }

            // Enregistrer l'image dans un fichier
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveFileDialog.Title = "Enregistrer l'image";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog.FileName, ImageFormat.Png); // Changez le format si nécessaire
                }
            }
        }
    }

    private Color GetLigneColor(string ligne)
    {
        if (!ligneColors.ContainsKey(ligne))
        {
            Random rand = new Random();
            ligneColors[ligne] = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
        return ligneColors[ligne];
    }

    [STAThread]
    static void Main()
    {
        Application.Run(new Visualisation());
    }
}