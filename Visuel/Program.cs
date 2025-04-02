using PbSI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Visualisation : Form
{
    private Graphe<StationMetro> graphe;
    private Dictionary<string, Color> ligneColors;
    private float minX, maxX, minY, maxY;
    private List<Noeud<StationMetro>> nodesSousGraphe; // Liste pour le sous-graphe
    private bool afficherSousGraphe = false; // État de l'affichage

    public Visualisation()
    {
        ReseauMetro reseau = new ReseauMetro("MetroParis.xlsx");
        graphe = reseau.Graphe;
        ligneColors = new Dictionary<string, Color>();
        this.Text = "Graphe du Métro de Paris";
        this.Size = new Size(6000, 4000);
        this.BackColor = Color.White;

        // Créer un FlowLayoutPanel pour contenir les boutons
        FlowLayoutPanel buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true, // Ajuster la taille automatiquement
            FlowDirection = FlowDirection.LeftToRight, // Disposer les boutons horizontalement
            WrapContents = false, // Ne pas permettre le passage à la ligne
            Padding = new Padding(10), // Ajouter un peu d'espace autour
            Margin = new Padding(10) // Ajouter un peu de marge
        };

        // Bouton pour enregistrer
        Button saveButton = new Button
        {
            Text = "Enregistrer en tant qu'image",
            Width = 200, // Largeur personnalisée
            Height = 30, // Hauteur personnalisée
            BackColor = Color.LightBlue, // Couleur de fond
            FlatStyle = FlatStyle.Flat // Style plat
        };
        saveButton.Click += SaveButton_Click;
        buttonPanel.Controls.Add(saveButton); // Ajouter au FlowLayoutPanel

        // Bouton pour afficher tout le graphe
        Button allGraphButton = new Button
        {
            Text = "Afficher tout le graphe",
            Width = 200, // Largeur personnalisée
            Height = 30, // Hauteur personnalisée
            BackColor = Color.LightGreen, // Couleur de fond
            FlatStyle = FlatStyle.Flat // Style plat
        };
        allGraphButton.Click += (sender, e) =>
        {
            afficherSousGraphe = false; // Afficher tout le graphe
            this.Invalidate(); // Rafraîchir l'affichage
        };
        buttonPanel.Controls.Add(allGraphButton); // Ajouter au FlowLayoutPanel

        // Bouton pour afficher le sous-graphe
        Button subGraphButton = new Button
        {
            Text = "Afficher sous-graphe",
            Width = 200, // Largeur personnalisée
            Height = 30, // Hauteur personnalisée
            BackColor = Color.LightCoral, // Couleur de fond
            FlatStyle = FlatStyle.Flat // Style plat
        };
        subGraphButton.Click += async (sender, e) =>
        {
            await ChargerSousGrapheAsync(); // Appel d'une méthode asynchrone pour définir le sous-graphe
            afficherSousGraphe = true; // Activer l'affichage du sous-graphe
            this.Invalidate(); // Rafraîchir l'affichage
        };
        buttonPanel.Controls.Add(subGraphButton); // Ajouter au FlowLayoutPanel

        // Ajouter le FlowLayoutPanel au formulaire
        this.Controls.Add(buttonPanel);

        this.Paint += new PaintEventHandler(DrawGraph);
        NormalizeCoordinates();

        // Charger les données de manière asynchrone lors du chargement du formulaire
        this.Load += async (sender, e) => await ChargerDonneesAsync();
    }

    private async Task ChargerSousGrapheAsync()
    {
        RechercheStationProche recherche = new RechercheStationProche("55 Rue du Faubourg Saint-Honoré, 75008 Paris, France", graphe);
        RechercheStationProche recherche2 = new RechercheStationProche("40 allée de Bercy, 75012 Paris", graphe);
        await recherche.InitialiserAsync();
        await recherche2.InitialiserAsync();

        List<int> depart = recherche.IdStationsProches;
        List<int> arrivee = recherche2.IdStationsProches;

        try
        {
            // Utiliser Dijkstra pour obtenir les nœuds du sous-graphe
            List<int> nodesSousGrapheIds = RechercheChemin<StationMetro>.DijkstraListe(graphe, depart, arrivee);
            nodesSousGraphe = nodesSousGrapheIds.Select(id => graphe.Noeuds.FirstOrDefault(n => n.Id == id)).ToList();
        }
        catch (Exception e)
        {
            MessageBox.Show($"Erreur : {e.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private async Task ChargerDonneesAsync()
    {
        // Cette méthode peut rester vide ou être utilisée pour d'autres chargements de données.
        await Task.CompletedTask; // Placeholder pour une logique future si nécessaire
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
        float scale = Math.Min(scaleX, scaleY) * 0.9f;

        float offsetX = 50;
        float screenX = (lon - minX) * scale + margin / 2 + offsetX;
        float upperMargin = 80;
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

        // Calcul des positions écran pour chaque nœud
        foreach (var noeud in graphe.Noeuds)
        {
            positions[noeud] = ConvertToScreenCoordinates((float)noeud.Contenu.Longitude, (float)noeud.Contenu.Latitude);
        }

        // Vérifier si nous devons dessiner le sous-graphe
        if (afficherSousGraphe && nodesSousGraphe != null && nodesSousGraphe.Count > 0)
        {
            DrawSousGraphe(g, positions); // Appel à la nouvelle fonction
        }
        else
        {

            // Séparer les liens en deux listes : ceux à dessiner en ligne droite et ceux en courbe
            List<Lien<StationMetro>> linksStraight = new List<Lien<StationMetro>>();
            List<Lien<StationMetro>> linksCurved = new List<Lien<StationMetro>>();

            foreach (var lien in graphe.Liens)
            {
                bool duplicateFound = false;
                foreach (var straight in linksStraight)
                {
                    if (straight.Source.Contenu.Libelle == lien.Source.Contenu.Libelle &&
                        straight.Destination.Contenu.Libelle == lien.Destination.Contenu.Libelle)
                    {
                        // Si la ligne est différente, on considère le lien comme doublon
                        if (straight.Source.Contenu.Ligne != lien.Source.Contenu.Ligne)
                        {
                            duplicateFound = true;
                            break;
                        }
                    }
                }
                if (duplicateFound)
                {
                    linksCurved.Add(lien);
                }
                else
                {
                    linksStraight.Add(lien);
                }
            }

            // Phase 1 : Dessin des liens en ligne droite
            foreach (var lien in linksStraight)
            {
                Noeud<StationMetro> source = lien.Source;
                Noeud<StationMetro> destination = lien.Destination;
                Color color = GetLigneColor(source.Contenu.Ligne);
                Pen pen = new Pen(color, 1);

                PointF start = positions[source];
                PointF end = positions[destination];
                // On se rapproche du destinataire pour le dessin de la flèche
                PointF direction = new PointF((end.X - start.X) * 0.98f + start.X, (end.Y - start.Y) * 0.98f + start.Y);

                DrawArrow(g, pen, start, direction);
            }

            // Phase 2 : Dessin des liens courbés pour les doublons
            foreach (var lien in linksCurved)
            {
                Noeud<StationMetro> source = lien.Source;
                Noeud<StationMetro> destination = lien.Destination;
                Color color = GetLigneColor(source.Contenu.Ligne);
                Pen pen = new Pen(color, 1);

                // Déterminer le point de départ et d'arrivée en fonction de leurs positions
                PointF start, end;
                bool switched = false; // Variable pour indiquer si les points ont été inversés
                if (positions[source].X < positions[destination].X ||
                   (positions[source].X == positions[destination].X && positions[source].Y < positions[destination].Y))
                {
                    start = positions[source];
                    end = positions[destination];
                }
                else
                {
                    start = positions[destination];
                    end = positions[source];
                    switched = true; // Indiquer que nous avons inversé les points
                }

                // Calculer un décalage perpendiculaire pour créer une légère courbe
                float dx = end.X - start.X;
                float dy = end.Y - start.Y;
                float length = (float)Math.Sqrt(dx * dx + dy * dy);
                if (length == 0)
                    length = 1;

                float offsetFactor = 20; // Ajuster la force de la courbe
                float offsetX = -dy / length * offsetFactor; // Décalage horizontal
                float offsetY = -dx / length * offsetFactor;  // Décalage vertical (négatif, vers le bas)

                // Points de contrôle pour la courbe de Bézier
                PointF controlPoint1 = new PointF(start.X + offsetX, start.Y + offsetY);
                PointF controlPoint2 = new PointF(end.X + offsetX, end.Y + offsetY);

                // Dessiner la courbe de Bézier
                g.DrawBezier(pen, start, controlPoint1, controlPoint2, end);

                // Calculer un point proche de 'start' pour dessiner la flèche
                float tStart = 0.05f; // T pour le point proche de 'start'
                float bezierXStart = (float)(Math.Pow(1 - tStart, 3) * start.X +
                    3 * Math.Pow(1 - tStart, 2) * tStart * controlPoint1.X +
                    3 * (1 - tStart) * Math.Pow(tStart, 2) * controlPoint2.X +
                    Math.Pow(tStart, 3) * end.X);
                float bezierYStart = (float)(Math.Pow(1 - tStart, 3) * start.Y +
                    3 * Math.Pow(1 - tStart, 2) * tStart * controlPoint1.Y +
                    3 * (1 - tStart) * Math.Pow(tStart, 2) * controlPoint2.Y +
                    Math.Pow(tStart, 3) * end.Y);
                PointF arrowStartEnd = new PointF(bezierXStart, bezierYStart); // Point proche de 'start'

                // Calculer un point proche de 'end' pour dessiner la flèche
                float tEnd = 0.95f; // T pour le point proche de 'end'
                float bezierXEnd = (float)(Math.Pow(1 - tEnd, 3) * start.X +
                    3 * Math.Pow(1 - tEnd, 2) * tEnd * controlPoint1.X +
                    3 * (1 - tEnd) * Math.Pow(tEnd, 2) * controlPoint2.X +
                    Math.Pow(tEnd, 3) * end.X);
                float bezierYEnd = (float)(Math.Pow(1 - tEnd, 3) * start.Y +
                    3 * Math.Pow(1 - tEnd, 2) * tEnd * controlPoint1.Y +
                    3 * (1 - tEnd) * Math.Pow(tEnd, 2) * controlPoint2.Y +
                    Math.Pow(tEnd, 3) * end.Y);
                PointF arrowEnd = new PointF(bezierXEnd, bezierYEnd); // Point proche de 'end'

                // Dessiner les têtes de flèche si les libellés sont différents
                if (source.Contenu.Libelle != destination.Contenu.Libelle)
                {
                    switch (switched)
                    {
                        case true:
                            // Si les points ont été inversés, dessiner la flèche près de start
                            DrawArrowHead(g, pen, arrowStartEnd, start);
                            break;
                        case false:
                            // Si les points sont dans l'ordre normal, dessiner la flèche près de end
                            DrawArrowHead(g, pen, arrowEnd, end);
                            break;
                    }
                }
            }

            // Dessin des nœuds et labels (inchangé)
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
                for (int offset = 15; offset <= 60; offset += 5)
                {
                    potentialPositions.Add(new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - offset));
                    potentialPositions.Add(new PointF(position.X - labelSize.Width / 2, position.Y + offset));
                }

                // Choisir la meilleure position
                PointF chosenLabelPosition = PointF.Empty;
                float closestDistance = float.MaxValue;

                foreach (var potentialPosition in potentialPositions)
                {
                    RectangleF backgroundRect = new RectangleF(potentialPosition.X - 2, potentialPosition.Y - 2, labelSize.Width + 4, labelSize.Height + 4);
                    bool intersects = placedElements.Any(rect => rect.IntersectsWith(backgroundRect));

                    if (!intersects)
                    {
                        float distance = Math.Abs(potentialPosition.Y - position.Y);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            chosenLabelPosition = potentialPosition;
                        }
                    }
                }

                if (chosenLabelPosition.IsEmpty)
                {
                    chosenLabelPosition = new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - 8);
                }

                RectangleF chosenBackgroundRect = new RectangleF(chosenLabelPosition.X - 2, chosenLabelPosition.Y - 2, labelSize.Width + 4, labelSize.Height + 4);
                placedElements.Add(chosenBackgroundRect);

                g.FillRectangle(Brushes.White, chosenBackgroundRect);
                g.DrawRectangle(contourPen, chosenBackgroundRect.X, chosenBackgroundRect.Y, chosenBackgroundRect.Width, chosenBackgroundRect.Height);
                g.DrawString(label, labelFont, Brushes.Black, chosenLabelPosition);
            }
        }
    }

    private void DrawSousGraphe(Graphics g, Dictionary<Noeud<StationMetro>, PointF> positions)
    {
        // Dessin des liens pour le sous-graphe
        List<Lien<StationMetro>> linksToDraw = graphe.Liens
            .Where(lien => nodesSousGraphe.Contains(lien.Source) && nodesSousGraphe.Contains(lien.Destination))
            .ToList();

        // Phase 1 : Dessin des liens en ligne droite
        foreach (var lien in linksToDraw)
        {
            Noeud<StationMetro> source = lien.Source;
            Noeud<StationMetro> destination = lien.Destination;
            Color color = GetLigneColor(source.Contenu.Ligne);
            Pen pen = new Pen(color, 1);

            // Utiliser uniquement les positions des nœuds du sous-graphe
            PointF start = ConvertToScreenCoordinates((float)source.Contenu.Longitude, (float)source.Contenu.Latitude);
            PointF end = ConvertToScreenCoordinates((float)destination.Contenu.Longitude, (float)destination.Contenu.Latitude);

            // On se rapproche du destinataire pour le dessin de la flèche
            PointF direction = new PointF((end.X - start.X) * 0.98f + start.X, (end.Y - start.Y) * 0.98f + start.Y);

            DrawArrow(g, pen, start, direction);
        }

        // Dessin des nœuds du sous-graphe
        foreach (var noeud in nodesSousGraphe)
        {
            int tailleNoeud = 5;
            PointF position = ConvertToScreenCoordinates((float)noeud.Contenu.Longitude, (float)noeud.Contenu.Latitude);

            // Dessiner le nœud (ellipse)
            RectangleF nodeRect = new RectangleF(position.X - 0.5f * tailleNoeud, position.Y - 0.5f * tailleNoeud, tailleNoeud, tailleNoeud);
            g.FillEllipse(Brushes.Black, nodeRect);

            string label = noeud.Contenu.Libelle;

            // Position du label
            Font labelFont = new Font("Arial", 3);
            SizeF labelSize = g.MeasureString(label, labelFont);
            PointF labelPosition = new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - 8);
            g.DrawString(label, labelFont, Brushes.Black, labelPosition);

            // Déterminer la couleur de l'encadrement
            Color borderColor;
            var lignes = linksToDraw
                .Where(l => l.Source == noeud || l.Destination == noeud)
                .Select(l => l.Source.Contenu.Ligne)
                .Distinct()
                .ToList();

            // Si le nœud a plusieurs lignes, utiliser noir
            borderColor = lignes.Count > 1 ? Color.Black : GetLigneColor(lignes.First());

            // Dessiner le rectangle autour du libellé
            Pen borderPen = new Pen(borderColor, 1);
            RectangleF labelBackgroundRect = new RectangleF(labelPosition.X - 2, labelPosition.Y - 2, labelSize.Width + 4, labelSize.Height + 4);
            g.DrawRectangle(borderPen, labelBackgroundRect);
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

    private void DrawArrowHead(Graphics g, Pen pen, PointF arrowEnd, PointF direction)
    {
        float arrowSize = 5; // Taille de la tête de la flèche

        // Calculer l'angle entre le point d'arrivée et la direction
        float angle = (float)Math.Atan2(direction.Y - arrowEnd.Y, direction.X - arrowEnd.X);

        // Calculer les points de la tête de la flèche avec une symétrie correcte
        PointF point1 = new PointF(
            arrowEnd.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6), // Point gauche de la flèche
            arrowEnd.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6)); // Point gauche de la flèche

        PointF point2 = new PointF(
            arrowEnd.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6), // Point droit de la flèche
            arrowEnd.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6)); // Point droit de la flèche

        // Dessiner les lignes de la tête de la flèche
        g.DrawLine(pen, arrowEnd, point1);
        g.DrawLine(pen, arrowEnd, point2);
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
