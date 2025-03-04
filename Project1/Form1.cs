using System.Drawing.Imaging;

namespace Project1
{
    public partial class Form1 : Form
    {
        private Bitmap originalImage;
        private Bitmap filteredImage;

        private Dictionary<string, Func<byte, byte>> savedFunctionalFilters = new Dictionary<string, Func<byte, byte>>();
        private string selectedFunctionalFilter = null;
        public Form1()
        {
            InitializeComponent();
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
                        filteredImage = ApplyContrast(filteredImage, 1.5f);
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
                        break;
                    case "Gaussian Blur":
                        filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            { 1f/16, 2f/16, 1f/16 },
                            { 2f/16, 4f/16, 2f/16 },
                            { 1f/16, 2f/16, 1f/16 }
                        });
                        break;
                    case "Sharpen":
                        filteredImage = filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            {  0, -1,  0 },
                            { -1,  5, -1 },
                            {  0, -1,  0 }
                        });
                        break;
                    case "Edge Detection":
                        filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            { -1, -1, -1 },
                            { -1,  8, -1 },
                            { -1, -1, -1 }
                        });
                        break;
                    case "Emboss":
                        filteredImage = ApplyConvolution(filteredImage, new float[,] {
                            { -2, -1,  0 },
                            { -1,  1,  1 },
                            {  0,  1,  2 }
                        });
                        break;
                }
            }

            imageBoxFiltered.Image = filteredImage;
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

        private void btnModifyFunctionalFilter_Click(object sender, EventArgs e)
        {
            if (filteredImage == null)
            {
                MessageBox.Show("Please load an image first.");
                return;
            }

            FunctionalFilterEditor funcFilteEditor = new FunctionalFilterEditor();
            if (funcFilteEditor.ShowDialog() == DialogResult.OK)
            {
                // Retrieve modified functional filter and apply it
                if (funcFilteEditor.ModifiedFilter != null)
                {
                    filteredImage = ApplyFunctionalFilter(filteredImage, funcFilteEditor.ModifiedFilter);
                    imageBoxFiltered.Image = filteredImage;
                }
            }
        }

        private Bitmap ApplyFunctionalFilter(Bitmap image, float[] lookupTable)
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
                    resultBuffer[index] = (byte)lookupTable[pixelBuffer[index]];
                    resultBuffer[index + 1] = (byte)lookupTable[pixelBuffer[index + 1]];
                    resultBuffer[index + 2] = (byte)lookupTable[pixelBuffer[index + 2]];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, dstData.Scan0, resultBuffer.Length);
            image.UnlockBits(srcData);
            filteredImage.UnlockBits(dstData);

            return filteredImage;
        }

    }
}
