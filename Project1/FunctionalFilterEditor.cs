using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class FunctionalFilterEditor : Form
    {
        private List<PointF> points;
        private bool isDragging;
        private int selectedPointIndex = -1;

        public float[] ModifiedFilter { get; private set; } // Stores the final filter mapping

        public FunctionalFilterEditor()
        {
            InitializeComponent();
            InitializeFilterGraph();
        }

        private void InitializeFilterGraph()
        {
            points = new List<PointF>
            {
                new PointF(0, 255),   // Leftmost (Fixed)
                new PointF(255, 0)    // Rightmost (Fixed)
            };

            pictureBoxGraph.Paint += pictureBoxGraph_Paint;
            pictureBoxGraph.MouseDown += pictureBoxGraph_MouseDown;
            pictureBoxGraph.MouseMove += pictureBoxGraph_MouseMove;
            pictureBoxGraph.MouseUp += pictureBoxGraph_MouseUp;
        }

        private void pictureBoxGraph_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (points.Count > 1)
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    e.Graphics.DrawLines(pen, points.ToArray());
                }

                foreach (var point in points)
                {
                    e.Graphics.FillEllipse(Brushes.Red, point.X - 4, point.Y - 4, 8, 8);
                }
            }
        }

        private void pictureBoxGraph_MouseDown(object sender, MouseEventArgs e)
        {
            selectedPointIndex = -1;

            for (int i = 0; i < points.Count; i++)
            {
                if (Math.Abs(e.X - points[i].X) < 5 && Math.Abs(e.Y - points[i].Y) < 5)
                {
                    selectedPointIndex = i;
                    isDragging = true;
                    return;
                }
            }

            if(selectedPointIndex == -1)
            {
                points.Add(new PointF(e.X, e.Y));
                points = points.OrderBy(p => p.X).ToList();
                pictureBoxGraph.Invalidate();
            }
            
        }

        private void pictureBoxGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedPointIndex >= 0 && selectedPointIndex < points.Count - 1)
            {
                float newX = Math.Clamp(e.X, points[selectedPointIndex - 1].X + 1, points[selectedPointIndex + 1].X - 1);
                float newY = Math.Clamp(e.Y, 0, 255);

                points[selectedPointIndex] = new PointF(newX, newY);

                pictureBoxGraph.Invalidate();
            }
        }

        private void pictureBoxGraph_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            points.Add(new PointF(128, 128));
            points = points.OrderBy(p => p.X).ToList();
            pictureBoxGraph.Invalidate();
        }

        private void btnDeletePoint_Click(object sender, EventArgs e)
        {
            if (selectedPointIndex > 0 && selectedPointIndex < points.Count - 1) // Prevent deleting endpoints
            {
                points.RemoveAt(selectedPointIndex);
                selectedPointIndex = -1;
                pictureBoxGraph.Invalidate();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ModifiedFilter = new float[256];

            for (int i = 0; i < 256; i++)
            {
                float x1 = 0, y1 = 255, x2 = 255, y2 = 0;
                foreach (var p in points)
                {
                    if (p.X <= i)
                    {
                        x1 = p.X;
                        y1 = p.Y;
                    }
                    else
                    {
                        x2 = p.X;
                        y2 = p.Y;
                        break;
                    }
                }

                float t = (i - x1) / (x2 - x1);
                ModifiedFilter[i] = y1 + t * (y2 - y1);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
