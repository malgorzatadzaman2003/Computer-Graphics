using System.Drawing.Imaging;

namespace Project1
{
    public partial class Filters : Form
    {
        private Bitmap originalImage;
        private Bitmap filteredImage;

        private List<PointF> points;
        private bool isDragging;
        private int selectedPointIndex = -1;

        public float[] ModifiedFilter { get; private set; }

        public Filters()
        {
            InitializeComponent();
            InitializeFilterGraph();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(ofd.FileName);
                    filteredImage = new Bitmap(originalImage); 
                    imageBoxOriginal.Image = originalImage;
                    imageBoxFiltered.Image = filteredImage;
                }
            }
        }

        private void btnResetImage_Click(object sender, EventArgs e)
        {
            if (originalImage != null)
            {
                filteredImage = new Bitmap(originalImage);
                imageBoxFiltered.Image = filteredImage;

                for (int i = 0; i < checkedListFuncFilters.Items.Count; i++)
                {
                    checkedListFuncFilters.SetItemChecked(i, false);
                }

                for (int i = 0; i < checkedListConvFilters.Items.Count; i++)
                {
                    checkedListConvFilters.SetItemChecked(i, false);
                }

                for (int i = 0; i < checkedListMedianFilters.Items.Count; i++)
                {
                    checkedListMedianFilters.SetItemChecked(i, false);
                }

                InitializeFilterGraph();
                pictureBoxFuncGraph.Invalidate();
            }
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (filteredImage == null)
            {
                MessageBox.Show("No filtered image to save.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filteredImage.Save(sfd.FileName);
                    MessageBox.Show("Image saved successfully.");
                }
            }
        }
        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            filteredImage = new Bitmap(originalImage);

            var checkedFuncItems = new List<object>(checkedListFuncFilters.CheckedItems.Cast<object>());
            var checkedConvItems = new List<object>(checkedListConvFilters.CheckedItems.Cast<object>());
            var checkedMedianItems = new List<object>(checkedListMedianFilters.CheckedItems.Cast<object>());


            if (checkedFuncItems.Count > 0 || checkedConvItems.Count > 0 || checkedMedianItems.Count > 0)
            {
                foreach (var item in checkedFuncItems)
                {
                    string filterName = item.ToString();
                    switch (filterName)
                    {
                        case "Inversion":
                            filteredImage = ApplyInversion(filteredImage);
                            break;
                        case "Brightness Correction":
                            filteredImage = ApplyBrightness(filteredImage, 50);
                            break;
                        case "Contrast Enhancement":
                            filteredImage = ApplyContrast(filteredImage, 1.5f);
                            break;
                        case "Gamma Correction":
                            filteredImage = ApplyGamma(filteredImage, 1.5f);
                            break;
                    }
                }

                foreach (var item in checkedConvItems)
                {
                    string filterName = item.ToString();
                    switch (filterName)
                    {
                        case "Blur":
                            for (int i = 0; i < 5; i++)  // 5 times for stronger blur
                            {
                                filteredImage = ApplyConvolution(filteredImage, new float[,] {
                                { 1f/9, 1f/9, 1f/9 },
                                { 1f/9, 1f/9, 1f/9 },
                                { 1f/9, 1f/9, 1f/9 }
                            });
                            }
                            ApplyCustomFilter();
                            break;
                        case "Gaussian Blur":
                            filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            { 1f/16, 2f/16, 1f/16 },
                            { 2f/16, 4f/16, 2f/16 },
                            { 1f/16, 2f/16, 1f/16 }
                            });
                            ApplyCustomFilter();
                            break;
                        case "Sharpen":
                            filteredImage = filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            {  0, -1,  0 },
                            { -1,  5, -1 },
                            {  0, -1,  0 }
                            });
                            ApplyCustomFilter();
                            break;
                        case "Edge Detection":
                            filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            { -1, -1, -1 },
                            { -1,  8, -1 },
                            { -1, -1, -1 }
                            });
                            ApplyCustomFilter();
                            break;
                        case "Emboss":
                            filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            { -1, -1,  0 },
                            { -1,  1,  1 },
                            {  0,  1,  1 }
                            });
                            ApplyCustomFilter();
                            break;
                    }
                }

                foreach (var item in checkedMedianItems)
                {
                    string filterName = item.ToString();
                    switch (filterName)
                    {
                        case "Median Filter":
                            ApplyMedianFilter();
                            ApplyCustomFilter();
                            break;
                    }
                }

                imageBoxFiltered.Image = filteredImage;
            }
            else if (checkedFuncItems.Count == 0)
            {
                ApplyCustomFilter();
            }
        }

        private Bitmap ApplyInversion(Bitmap image)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride; 
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;

                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    resultBuffer[index] = (byte)(255 - b);
                    resultBuffer[index + 1] = (byte)(255 - g);
                    resultBuffer[index + 2] = (byte)(255 - r);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            SetInversionGraph();

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private Bitmap ApplyBrightness(Bitmap image, int brightnessOffset)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;

                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    int newR = Math.Clamp(r + brightnessOffset, 0, 255);
                    int newG = Math.Clamp(g + brightnessOffset, 0, 255);
                    int newB = Math.Clamp(b + brightnessOffset, 0, 255);

                    resultBuffer[index + 2] = (byte)newR;
                    resultBuffer[index + 1] = (byte)newG;
                    resultBuffer[index] = (byte)newB;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            SetBrightnessGraph(brightnessOffset);

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private Bitmap ApplyContrast(Bitmap image, float contrastFactor)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;

                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    int newR = Math.Clamp((int)((r - 128) * contrastFactor + 128), 0, 255);
                    int newG = Math.Clamp((int)((g - 128) * contrastFactor + 128), 0, 255);
                    int newB = Math.Clamp((int)((b - 128) * contrastFactor + 128), 0, 255);

                    resultBuffer[index + 2] = (byte)newR;
                    resultBuffer[index + 1] = (byte)newG;
                    resultBuffer[index] = (byte)newB;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            SetContrastGraph(contrastFactor);

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private Bitmap ApplyGamma(Bitmap image, float gamma)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);
            byte[] gammaArray = new byte[256];

            for (int i = 0; i < 256; i++)
            {
                gammaArray[i] = (byte)Math.Clamp((int)(255 * Math.Pow(i / 255.0, gamma)), 0, 255);
            }

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;

                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    resultBuffer[index + 2] = gammaArray[r];
                    resultBuffer[index + 1] = gammaArray[g];
                    resultBuffer[index] = gammaArray[b];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private Bitmap ApplyConvolution(Bitmap image, float[,] kernel)
        {
            int kernelSize = kernel.GetLength(0);
            int offset = kernelSize / 2;

            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly,
                                                PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                   ImageLockMode.WriteOnly,
                                                   PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    float newR = 0, newG = 0, newB = 0;

                    for (int ky = 0; ky < kernelSize; ky++)
                    {
                        for (int kx = 0; kx < kernelSize; kx++)
                        {
                            int pixelX = Math.Clamp(x + kx - offset, 0, image.Width - 1) * 3; 
                            int pixelY = Math.Clamp(y + ky - offset, 0, image.Height - 1) * stride;
                            int index = pixelY + pixelX;

                            newB += pixelBuffer[index] * kernel[ky, kx];      
                            newG += pixelBuffer[index + 1] * kernel[ky, kx];  
                            newR += pixelBuffer[index + 2] * kernel[ky, kx];  
                        }
                    }

                    int resultIndex = (y * stride) + (x * 3);
                    resultBuffer[resultIndex] = (byte)Math.Clamp(newB, 0, 255);
                    resultBuffer[resultIndex + 1] = (byte)Math.Clamp(newG, 0, 255);
                    resultBuffer[resultIndex + 2] = (byte)Math.Clamp(newR, 0, 255);
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private void ApplyMedianFilter()
        {
            if (filteredImage == null) return;

            if (!int.TryParse(textBoxMedian.Text, out int n) || n < 3 || n % 2 == 0 || n > 15)
            {
                MessageBox.Show("Please enter valid kernel size.");
                return;
            }

            int offset = n / 2; 
            int width = filteredImage.Width;
            int height = filteredImage.Height;

            BitmapData srcData = filteredImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * height];
            byte[] newPixelBuffer = (byte[])pixelBuffer.Clone(); 

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<byte> neighborhoodBlue = new List<byte>();
                    List<byte> neighborhoodGreen = new List<byte>();
                    List<byte> neighborhoodRed = new List<byte>();

                    for (int dy = -offset; dy <= offset; dy++)
                    {
                        for (int dx = -offset; dx <= offset; dx++)
                        {
                            int neighborX = Math.Clamp(x + dx, 0, width - 1);
                            int neighborY = Math.Clamp(y + dy, 0, height - 1);
                            int index = (neighborY * stride) + (neighborX * 3);

                            neighborhoodBlue.Add(pixelBuffer[index]);       
                            neighborhoodGreen.Add(pixelBuffer[index + 1]);  
                            neighborhoodRed.Add(pixelBuffer[index + 2]);    
                        }
                    }

                    neighborhoodBlue.Sort();
                    neighborhoodGreen.Sort();
                    neighborhoodRed.Sort();
                    int medianIndex = neighborhoodBlue.Count / 2; 

                    int pixelIndex = (y * stride) + (x * 3);
                    newPixelBuffer[pixelIndex] = neighborhoodBlue[medianIndex];      
                    newPixelBuffer[pixelIndex + 1] = neighborhoodGreen[medianIndex];  
                    newPixelBuffer[pixelIndex + 2] = neighborhoodRed[medianIndex];    
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(newPixelBuffer, 0, srcData.Scan0, newPixelBuffer.Length);

            filteredImage.UnlockBits(srcData);
            imageBoxFiltered.Image = filteredImage;
        }

        private void InitializeFilterGraph()
        {
            points = new List<PointF>
            {
                new PointF(0, 255),   
                new PointF(255, 0)   
            };

            pictureBoxFuncGraph.Paint += pictureBoxFuncGraph_Paint;
            pictureBoxFuncGraph.MouseDown += pictureBoxFuncGraph_MouseDown;
            pictureBoxFuncGraph.MouseMove += pictureBoxFuncGraph_MouseMove;
            pictureBoxFuncGraph.MouseUp += pictureBoxFuncGraph_MouseUp;
        }

        private void pictureBoxFuncGraph_Paint(object sender, PaintEventArgs e)
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
        private void pictureBoxFuncGraph_MouseDown(object sender, MouseEventArgs e)
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

            if (selectedPointIndex == -1)
            {
                points.Add(new PointF(e.X, e.Y));
                points = points.OrderBy(p => p.X).ToList();
                pictureBoxFuncGraph.Invalidate();
            }
        }

        private void pictureBoxFuncGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedPointIndex >= 0)
            {
                float newX = points[selectedPointIndex].X;
                float newY = Math.Clamp(e.Y, 0, 255);

                if (selectedPointIndex == 0 || selectedPointIndex == points.Count - 1)
                {
                    points[selectedPointIndex] = new PointF(newX, newY);
                }
                else
                {
                    newX = Math.Clamp(e.X, points[selectedPointIndex - 1].X + 1, points[selectedPointIndex + 1].X - 1);
                    points[selectedPointIndex] = new PointF(newX, newY);
                }

                pictureBoxFuncGraph.Invalidate();
            }
        }

        private void pictureBoxFuncGraph_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            points.Add(new PointF(128, 128));
            points = points.OrderBy(p => p.X).ToList();
            pictureBoxFuncGraph.Invalidate();
        }

        private void btnDeletePoint_Click(object sender, EventArgs e)
        {
            if (selectedPointIndex > 0 && selectedPointIndex < points.Count - 1)
            {
                points.RemoveAt(selectedPointIndex);
                selectedPointIndex = -1;
                pictureBoxFuncGraph.Invalidate();
            }
        }

        private void SetInversionGraph()
        {
            points = new List<PointF>
            {
                new PointF(0, 0),
                new PointF(255, 255)
            };
            pictureBoxFuncGraph.Invalidate();
        }

        private void SetBrightnessGraph(int brightnessOffset)
        {
            if (brightnessOffset < 0)
            {
                points = new List<PointF>
                {
                    new PointF(0, 255),        
                    new PointF(-brightnessOffset, 255),   
                    new PointF(255, -brightnessOffset)    
                };
            }
            else
            {
                points = new List<PointF>
                {
                    new PointF(0, 255 - brightnessOffset),
                    new PointF(255 - brightnessOffset, 0),
                    new PointF(255, 0)
                };
            }

            pictureBoxFuncGraph.Invalidate();
        }

        private void SetContrastGraph(float contrastFactor)
        {
            float midpoint = 127.5f;
            float x1 = Math.Max(0, midpoint - (midpoint / contrastFactor));
            float x2 = Math.Min(255, midpoint + (midpoint / contrastFactor)); 

            points = new List<PointF>
            {
                new PointF(0, 255),         
                new PointF(x1, 255),
                new PointF(x2, 0),      
                new PointF(255, 0)      
            };

            pictureBoxFuncGraph.Invalidate();
        }


        private void btnSaveFilter_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text File|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(sfd.FileName))
                    {
                        foreach (var point in points)
                        {
                            writer.WriteLine($"{point.X},{point.Y}");
                        }
                    }
                }
            }
        }

        private void btnLoadFilter_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (originalImage == null)
                {
                    MessageBox.Show("Please load an image first.");
                    return;
                }

                ofd.Filter = "Text File|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    points.Clear();
                    using (StreamReader reader = new StreamReader(ofd.FileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var coords = line.Split(',');
                            points.Add(new PointF(float.Parse(coords[0]), float.Parse(coords[1])));
                        }
                    }
                    pictureBoxFuncGraph.Invalidate();
                    ApplyCustomFilter();
                }
            }
        }

        private void ApplyCustomFilter()
        {
            if (points.Count < 2 || filteredImage == null) return;

            float[] lookupTable = new float[256];

            for (int i = 0; i < points.Count - 1; i++)
            {
                PointF p1 = points[i];
                PointF p2 = points[i + 1];

                float slope = (p2.Y - p1.Y) / (p2.X - p1.X);

                for (int x = (int)p1.X; x <= (int)p2.X; x++)
                {
                    lookupTable[x] = Math.Clamp(255 - (p1.Y + (x - p1.X) * slope), 0, 255);
                }
            }

            BitmapData srcData = filteredImage.LockBits(new Rectangle(0, 0, filteredImage.Width, filteredImage.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * filteredImage.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < filteredImage.Height; y++)
            {
                for (int x = 0; x < filteredImage.Width; x++)
                {
                    int index = y * stride + x * 3;

                    pixelBuffer[index + 2] = (byte)lookupTable[pixelBuffer[index + 2]];
                    pixelBuffer[index + 1] = (byte)lookupTable[pixelBuffer[index + 1]];
                    pixelBuffer[index] = (byte)lookupTable[pixelBuffer[index]];       
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixelBuffer, 0, srcData.Scan0, pixelBuffer.Length);
            filteredImage.UnlockBits(srcData);

            imageBoxFiltered.Image = filteredImage;
        }


    }
}
