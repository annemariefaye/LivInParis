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
        this.BackColor = Color.White; // Ajout d'un fond blanc

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
        float scale = Math.Min(scaleX, scaleY) * 0.95f;

        float offsetX = 50; 
        float screenX = (lon - minX) * scale + margin / 2 + offsetX;
        float upperMargin = 30;
        float screenY = (maxY - lat) * scale + margin / 2 + upperMargin;

        return new PointF(screenX, screenY);
    }

    private void DrawArrow(Graphics g, Pen pen, PointF start, PointF end)
    {
        g.DrawLine(pen, start, end);

        float arrowSize = 4;
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
        g.Clear(Color.White); // S'assurer que le fond est blanc
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
            Pen pen = new Pen(color, 2);

            PointF start = positions[source];
            PointF end = positions[destination];
            PointF direction = new PointF((end.X - start.X) * 0.85f + start.X, (end.Y - start.Y) * 0.85f + start.Y);

            DrawArrow(g, pen, start, direction);
        }

        foreach (var noeud in graphe.Noeuds)
        {
            PointF position = positions[noeud];
            g.FillEllipse(Brushes.Black, position.X - 5, position.Y - 5, 5, 5);

            string label = noeud.Contenu.Libelle;
            Font labelFont = new Font("Arial", 3);
            SizeF labelSize = g.MeasureString(label, labelFont);
            PointF labelPosition = new PointF(position.X - labelSize.Width / 2, position.Y - labelSize.Height - 8); // Positionner le texte au-dessus du nœud

            // Dessiner le fond blanc pour le libellé
            RectangleF backgroundRect = new RectangleF(labelPosition.X - 2, labelPosition.Y - 2, labelSize.Width + 4, labelSize.Height + 4);
            g.FillRectangle(Brushes.White, backgroundRect);
            g.DrawRectangle(Pens.Black, backgroundRect.X, backgroundRect.Y, backgroundRect.Width, backgroundRect.Height); // Bordure noire

            // Dessiner le texte
            g.DrawString(label, labelFont, Brushes.Black, labelPosition);
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
