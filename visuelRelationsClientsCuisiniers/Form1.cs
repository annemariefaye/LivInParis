using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using PbSI;

namespace visuelRelationsClientsCuisiniers
{
    public partial class Form1 : Form
    {
        private relationsClientsCuisiniers relations;
        private const int NODE_RADIUS = 30;
        private const int CLIENT_Y = 100;
        private const int CUISINIER_Y = 400;
        private const int HORIZONTAL_SPACING = 120;
        private Dictionary<int, Point> nodePositions = new Dictionary<int, Point>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Size = new Size(900, 600);
            this.Text = "Visualisation des relations Clients-Cuisiniers";
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            /// Bouton export
            var exportBtn = new Button
            {
                Text = "Exporter PNG",
                Location = new Point(20, 20),
                Size = new Size(150, 40),
                BackColor = Color.LightGreen
            };
            exportBtn.Click += ExportButton_Click;
            this.Controls.Add(exportBtn);

            relations = new relationsClientsCuisiniers();
            this.Shown += (s, e) => this.Invalidate();
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            graphics.Clear(Color.White);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DrawLinks(graphics);
            DrawNodes(graphics);
            DrawLegend(graphics);
        }

       

        private void ExportButton_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image|*.png";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                    this.DrawToBitmap(bmp, this.ClientRectangle);
                    bmp.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("Export réussi!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DrawLegend(Graphics graphics)
        {
            var legendFont = new Font("Arial", 10);
            graphics.DrawString("Légende:", legendFont, Brushes.Black, 20, this.Height - 80);
            graphics.FillEllipse(Brushes.LightBlue, 20, this.Height - 60, 15, 15);
            graphics.DrawEllipse(Pens.Black, 20, this.Height - 60, 15, 15);
            graphics.DrawString("Clients", legendFont, Brushes.Black, 40, this.Height - 60);

            graphics.FillEllipse(Brushes.LightSalmon, 120, this.Height - 60, 15, 15);
            graphics.DrawEllipse(Pens.Black, 120, this.Height - 60, 15, 15);
            graphics.DrawString("Cuisiniers", legendFont, Brushes.Black, 140, this.Height - 60);
        }

        private void DrawNodes(Graphics graphics)
        {
            nodePositions.Clear();
            var clients = new List<Noeud<string>>();
            var cuisiniers = new List<Noeud<string>>();

            foreach (var node in relations.Graphe.Noeuds)
            {
                if (node.Id > 0) clients.Add(node);
                else cuisiniers.Add(node);
            }

            int x = HORIZONTAL_SPACING;
            foreach (var client in clients)
            {
                var pos = new Point(x, CLIENT_Y);
                nodePositions[client.Id] = pos;
                DrawNode(graphics, client, pos, Brushes.LightBlue);
                x += HORIZONTAL_SPACING;
            }

            x = HORIZONTAL_SPACING;
            foreach (var cuisinier in cuisiniers)
            {
                var pos = new Point(x, CUISINIER_Y);
                nodePositions[cuisinier.Id] = pos;
                DrawNode(graphics, cuisinier, pos, Brushes.LightSalmon);
                x += HORIZONTAL_SPACING;
            }
        }

        private void DrawNode(Graphics graphics, Noeud<string> node, Point pos, Brush brush)
        {
            graphics.FillEllipse(brush, pos.X - NODE_RADIUS, pos.Y - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);
            graphics.DrawEllipse(Pens.Black, pos.X - NODE_RADIUS, pos.Y - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);

            string label = node.Contenu.ToString().Split(':')[1].Trim();
            if (label.Length > 10) label = label.Substring(0, 10) + "...";

            var textSize = graphics.MeasureString(label, this.Font);
            graphics.DrawString(label, this.Font, Brushes.Black,
                pos.X - textSize.Width / 2,
                pos.Y - textSize.Height / 2);
        }

        private void DrawLinks(Graphics graphics)
        {
            if (relations?.Graphe?.Liens == null) return;

            using (var linkPen = new Pen(Color.FromArgb(180, 70, 130, 180), 2.5f)
            {
                EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor,
                CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(6, 6)
            })
            {
                foreach (var lien in relations.Graphe.Liens)
                {
                    if (nodePositions.TryGetValue(lien.Source.Id, out Point sourcePos) &&
                        nodePositions.TryGetValue(lien.Destination.Id, out Point destPos))
                    {
                        Point start = new Point(sourcePos.X, sourcePos.Y + NODE_RADIUS + 2);
                        Point end = new Point(destPos.X, destPos.Y - NODE_RADIUS - 2);

                        graphics.DrawLine(linkPen, start, end);

                        if (lien.Poids != 1)
                        {
                            Point middle = new Point(
                                (start.X + end.X) / 2,
                                (start.Y + end.Y) / 2);

                            graphics.DrawString(lien.Poids.ToString("0.0"),
                                this.Font, Brushes.DarkBlue, middle);
                        }
                    }
                }
            }
            this.Refresh();
        }
    }
}