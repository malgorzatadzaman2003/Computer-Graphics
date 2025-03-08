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
                    filteredImage = new Bitmap(originalImage); // Copy for modifications in filtered image
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

            if (checkedListFuncFilters.CheckedItems.Count > 0 || checkedListConvFilters.CheckedItems.Count > 0)
            {
                foreach (var item in checkedListFuncFilters.CheckedItems)
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
                            filteredImage = ApplyContrast(filteredImage, 3f);
                            break;
                        case "Gamma Correction":
                            filteredImage = ApplyGamma(filteredImage, 1.5f);
                            break;
                    }
                }

                foreach (var item in checkedListConvFilters.CheckedItems)
                {
                    string filterName = item.ToString();
                    switch (filterName)
                    {
                        case "Blur":
                            for (int i = 0; i < 5; i++)  // Apply 3 times for stronger blur
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

                imageBoxFiltered.Image = filteredImage;
            }
            else if (checkedListFuncFilters.CheckedItems.Count == 0)
            {
                ApplyCustomFilter();
            }
        }

        private Bitmap ApplyInversion(Bitmap image)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            // Lock the image data
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            // Copy pixel data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;

                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    // Inversion
                    resultBuffer[index] = (byte)(255 - b);
                    resultBuffer[index + 1] = (byte)(255 - g);
                    resultBuffer[index + 2] = (byte)(255 - r);
                }
            }

            // Copy the processed data back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            SetInversionGraph();

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private Bitmap ApplyBrightness(Bitmap image, int brightnessOffset)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            // Lock the image data
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            // Copy pixel data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;
                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    //Applying brightness
                    int newR = Math.Clamp(r + brightnessOffset, 0, 255);
                    int newG = Math.Clamp(g + brightnessOffset, 0, 255);
                    int newB = Math.Clamp(b + brightnessOffset, 0, 255);

                    // Update the result buffer
                    resultBuffer[index + 2] = (byte)newR;
                    resultBuffer[index + 1] = (byte)newG;
                    resultBuffer[index] = (byte)newB;
                }
            }

            // Copy the processed data back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            SetBrightnessGraph(brightnessOffset);

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private Bitmap ApplyContrast(Bitmap image, float contrastFactor)
        {
            Bitmap filteredImage = new Bitmap(image.Width, image.Height);

            // Lock the image data
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            // Copy pixel data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;
                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    //Applying contrast
                    int newR = Math.Clamp((int)((r - 128) * contrastFactor + 128), 0, 255);
                    int newG = Math.Clamp((int)((g - 128) * contrastFactor + 128), 0, 255);
                    int newB = Math.Clamp((int)((b - 128) * contrastFactor + 128), 0, 255);

                    // Update the result buffer
                    resultBuffer[index + 2] = (byte)newR;
                    resultBuffer[index + 1] = (byte)newG;
                    resultBuffer[index] = (byte)newB;
                }
            }

            // Copy the processed data back to the bitmap
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

            // Create gamma lookup table
            for (int i = 0; i < 256; i++)
            {
                gammaArray[i] = (byte)Math.Clamp((int)(255 * Math.Pow(i / 255.0, gamma)), 0, 255);
            }

            // Lock the image data
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                       ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            // Copy pixel data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int index = y * stride + x * 3;
                    byte r = pixelBuffer[index + 2];
                    byte g = pixelBuffer[index + 1];
                    byte b = pixelBuffer[index];

                    // Apply gamma correction
                    resultBuffer[index + 2] = gammaArray[r];
                    resultBuffer[index + 1] = gammaArray[g];
                    resultBuffer[index] = gammaArray[b];
                }
            }

            // Copy the processed data back to the bitmap
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

            // Lock bitmaps for fast pixel access
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                ImageLockMode.ReadOnly,
                                                PixelFormat.Format24bppRgb);
            BitmapData dstData = filteredImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                   ImageLockMode.WriteOnly,
                                                   PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            byte[] pixelBuffer = new byte[stride * image.Height];
            byte[] resultBuffer = new byte[stride * image.Height];

            // Copy pixels from bitmap to array
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    float newR = 0, newG = 0, newB = 0;

                    // Apply convolution kernel
                    for (int ky = 0; ky < kernelSize; ky++)
                    {
                        for (int kx = 0; kx < kernelSize; kx++)
                        {
                            int pixelX = Math.Clamp(x + kx - offset, 0, image.Width - 1) * 3; // Clamp to avoid out-of-bounds
                            int pixelY = Math.Clamp(y + ky - offset, 0, image.Height - 1) * stride;
                            int index = pixelY + pixelX;

                            newB += pixelBuffer[index] * kernel[ky, kx];      // Blue
                            newG += pixelBuffer[index + 1] * kernel[ky, kx];  // Green
                            newR += pixelBuffer[index + 2] * kernel[ky, kx];  // Red
                        }
                    }

                    int resultIndex = (y * stride) + (x * 3);
                    resultBuffer[resultIndex] = (byte)Math.Clamp(newB, 0, 255);
                    resultBuffer[resultIndex + 1] = (byte)Math.Clamp(newG, 0, 255);
                    resultBuffer[resultIndex + 2] = (byte)Math.Clamp(newR, 0, 255);
                }
            }

            // Copy result buffer back to bitmap
            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);

            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

        private void InitializeFilterGraph()
        {
            points = new List<PointF>
            {
                new PointF(0, 255),   // Leftmost (Fixed)
                new PointF(255, 0)    // Rightmost (Fixed)
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
                float newX = points[selectedPointIndex].X; // Keep X unchanged for boundary points
                float newY = Math.Clamp(e.Y, 0, 255);

                // Leftmost point (index 0) and rightmost point (last index) can only move up/down
                if (selectedPointIndex == 0 || selectedPointIndex == points.Count - 1)
                {
                    points[selectedPointIndex] = new PointF(newX, newY);
                }
                else
                {
                    // For other points, ensure X remains ordered correctly
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
            if (selectedPointIndex > 0 && selectedPointIndex < points.Count - 1) // Prevent deleting endpoints
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
                    new PointF(0, 255),        // Constant at 0
                    new PointF(-brightnessOffset, 255),   // Flat section until xStart
                    new PointF(255, -brightnessOffset)    // Then increases to (255, yEnd)
                };
            }
            else
            {
                points = new List<PointF>
                {
                    // First part: Linear increase until it reaches 255
                    new PointF(0, 255 - brightnessOffset),
                    new PointF(255 - brightnessOffset, 0),
                    // Second part: Horizontal line from (xLimit, 0) to (255, 0)
                    new PointF(255, 0)
                };
            }

            pictureBoxFuncGraph.Invalidate();
        }

        private void SetContrastGraph(float contrastFactor)
        {
            float midpoint = 127.5f; // Middle of the range
            float x1 = Math.Max(0, midpoint - (midpoint / contrastFactor)); // Start of linear section
            float x2 = Math.Min(255, midpoint + (midpoint / contrastFactor)); // End of linear section

            points = new List<PointF>
            {
                new PointF(0, 255),         // Constant at 0 until x1
                new PointF(x1, 255),
                new PointF(x2, 0),      // Linear increase between (x1,0) and (x2,255)
                new PointF(255, 0)      // Constant at 255 after x2
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

            // Generate lookup table from graph points
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

            // Lock image for direct pixel manipulation
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

                    pixelBuffer[index] = (byte)lookupTable[pixelBuffer[index]];       // Blue
                    pixelBuffer[index + 1] = (byte)lookupTable[pixelBuffer[index + 1]]; // Green
                    pixelBuffer[index + 2] = (byte)lookupTable[pixelBuffer[index + 2]]; // Red
                }
            }

            // Copy modified pixels back
            System.Runtime.InteropServices.Marshal.Copy(pixelBuffer, 0, srcData.Scan0, pixelBuffer.Length);
            filteredImage.UnlockBits(srcData);

            imageBoxFiltered.Image = filteredImage;
        }


    }
}
