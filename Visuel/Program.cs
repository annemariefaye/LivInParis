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
        this.Size = new Size(6000, 000);
        this.BackColor = Color.White;

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
        float margin = 0;
        float scaleX = (this.ClientSize.Width - margin) / (maxX - minX);
        float scaleY = (this.ClientSize.Height - margin) / (maxY - minY);
        float scale = Math.Min(scaleX, scaleY)*0.9f;

        float offsetX = 50; 
        float screenX = (lon - minX) * scale + margin / 2 + offsetX;
        float upperMargin = 30;
        float screenY = (maxY - lat) * scale + margin / 2 + upperMargin;

        return new PointF(screenX, screenY);
    }

    private void DrawArrow(Graphics g, Pen pen, PointF start, PointF end)
    {
        g.DrawLine(pen, start, end);

        float arrowSize = 6;
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
        g.Clear(Color.White); 
        Dictionary<Noeud<StationMetro>, PointF> positions = new Dictionary<Noeud<StationMetro>, PointF>();

        foreach (var noeud in graphe.Noeuds)
        {
            positions[noeud] = ConvertToScreenCoordinates((float)noeud.Contenu.Longitude, (float)noeud.Contenu.Latitude);
        }

        foreach (var lien in graphe.Liens)
        {
            Noeud<StationMetro> source = lien.Source;
            Noeud<StationMetro> destination = lien.Destination;
            Color color = GetLigneColor(source.Contenu.Ligne);
            Pen pen = new Pen(color, 1);

            PointF start = positions[source];
            PointF end = positions[destination];
            PointF direction = new PointF((end.X - start.X)*0.98f + start.X, (end.Y - start.Y)*0.98f + start.Y);

            DrawArrow(g, pen, start, direction);
        }

        List<RectangleF> placedElements = new List<RectangleF>(); // Stocke les labels ET les ellipses
        HashSet<string> displayedLabels = new HashSet<string>(); // Évite les doublons de labels

        foreach (var noeud in graphe.Noeuds)
        {
            int tailleNoeud = 5;
            PointF position = positions[noeud];

            // Dessiner le nœud (ellipse)
            RectangleF nodeRect = new RectangleF(position.X - 0.5f * tailleNoeud, position.Y - 0.5f * tailleNoeud, tailleNoeud, tailleNoeud);
            g.FillEllipse(Brushes.Black, nodeRect);

            placedElements.Add(nodeRect); // Ajouter l'ellipse aux éléments placés

            string label = noeud.Contenu.Libelle;

            // Vérifier si ce label a déjà été affiché
            if (displayedLabels.Contains(label))
                continue;

            displayedLabels.Add(label); // Marquer ce label comme affiché

            // Récupérer les lignes associées pour la couleur du contour
            var lignes = graphe.Liens
                .Where(lien => lien.Source.Contenu.Libelle == label || lien.Destination.Contenu.Libelle == label)
                .Select(lien => lien.Source.Contenu.Ligne)
                .Distinct()
                .ToList();

            Color contourColor = (lignes.Count == 1) ? GetLigneColor(lignes.First()) : Color.Black;
            Pen contourPen = new Pen(contourColor, 1);

            // Position du label
            Font labelFont = new Font("Arial", 3);
            SizeF labelSize = g.MeasureString(label, labelFont);

            // Liste des positions possibles (au-dessus et en dessous)
            List<PointF> potentialPositions = new List<PointF>
    {
        new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - 8), // Au-dessus
        new PointF(position.X - labelSize.Width / 2, position.Y + 8), // En dessous
    };

            // Tester des positions avec des décalages supplémentaires
            for (int offset = 15; offset <= 60; offset += 5) // Ajustements pour un espacement plus grand
            {
                potentialPositions.Add(new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - offset)); // Plus haut
                potentialPositions.Add(new PointF(position.X - labelSize.Width / 2, position.Y + offset)); // Plus bas
            }

            // Variable pour la meilleure position
            PointF chosenLabelPosition = PointF.Empty;
            float closestDistance = float.MaxValue; // Initialiser à une grande valeur

            // Vérifier les positions et choisir la plus proche qui ne chevauche pas
            foreach (var potentialPosition in potentialPositions)
            {
                RectangleF backgroundRect = new RectangleF(potentialPosition.X - 2, potentialPosition.Y - 2, labelSize.Width + 4, labelSize.Height + 4);

                // Vérifie les chevauchements avec d'autres labels et le nœud
                bool intersects = placedElements.Any(rect => rect.IntersectsWith(backgroundRect));

                if (!intersects) // Si aucune intersection
                {
                    // Calculer la distance absolue au nœud
                    float distance = Math.Abs(potentialPosition.Y - position.Y);

                    // Vérifier si cette position est plus proche que la précédente
                    if (distance < closestDistance)
                    {
                        closestDistance = distance; // Met à jour la distance la plus proche
                        chosenLabelPosition = potentialPosition; // Met à jour la position choisie
                    }
                }
            }

            // Si aucune position valide n'a été trouvée, utiliser la position par défaut au-dessus
            if (chosenLabelPosition.IsEmpty)
            {
                chosenLabelPosition = new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - 8);
            }

            // Créer le rectangle de fond pour le label
            RectangleF chosenBackgroundRect = new RectangleF(chosenLabelPosition.X - 2, chosenLabelPosition.Y - 2, labelSize.Width + 4, labelSize.Height + 4);

            // Ajouter la position validée
            placedElements.Add(chosenBackgroundRect);

            // Dessiner le label
            g.FillRectangle(Brushes.White, chosenBackgroundRect);
            g.DrawRectangle(contourPen, chosenBackgroundRect.X, chosenBackgroundRect.Y, chosenBackgroundRect.Width, chosenBackgroundRect.Height);
            g.DrawString(label, labelFont, Brushes.Black, chosenLabelPosition);
        }


    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        float scaleFactor = 3.0f;
        using (Bitmap bitmap = new Bitmap((int)(this.ClientSize.Width * scaleFactor), (int)(this.ClientSize.Height * scaleFactor)))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.ScaleTransform(scaleFactor, scaleFactor);
                DrawGraph(this, new PaintEventArgs(g, this.ClientRectangle));
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveFileDialog.Title = "Enregistrer l'image";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
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
