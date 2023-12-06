using System;
using System.Drawing;
using System.Drawing.Imaging;


namespace MusicBeePlugin
{
    public class AlphaBlendedImage
    {
        Bitmap _image;
        Bitmap _mask;
        Bitmap preparedMask;
        Bitmap finalMaskedImage;

        ~AlphaBlendedImage()
        {
            _image?.Dispose();
            _mask?.Dispose();
            preparedMask?.Dispose();
        }

        public AlphaBlendedImage(Bitmap image, Bitmap mask)
        {
            _image = Create32bppImageAndClearAlpha(image);
            _mask = Create32bppImageAndClearAlpha(mask);
        }

        public Bitmap AlphaBlendImages()
        {
            PrepareMaskImage();
            return finalMaskedImage;
        }

        #region This is ugly stuff
        private void PrepareMaskedImage()
        {
            if (_image != null && preparedMask != null)
            {
                if (_image.Width != preparedMask.Width || _image.Height != preparedMask.Height)
                {
                    throw new BadImageFormatException("Mask and image must be the same size");
                }
                else
                {
                    finalMaskedImage = Create32bppImageAndClearAlpha(_image);

                    BitmapData bmpData1 = finalMaskedImage.LockBits(new Rectangle(0, 0, finalMaskedImage.Width, finalMaskedImage.Height), ImageLockMode.ReadWrite, finalMaskedImage.PixelFormat);
                    byte[] finalMaskedImageRGBAData = new byte[bmpData1.Stride * bmpData1.Height];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData1.Scan0, finalMaskedImageRGBAData, 0, finalMaskedImageRGBAData.Length);

                    BitmapData bmpData2 = preparedMask.LockBits(new Rectangle(0, 0, preparedMask.Width, preparedMask.Height), ImageLockMode.ReadOnly, preparedMask.PixelFormat);
                    byte[] preparedMaskRGBAData = new byte[bmpData2.Stride * bmpData2.Height];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData2.Scan0, preparedMaskRGBAData, 0, preparedMaskRGBAData.Length);

                    //copy the mask to the Alpha layer
                    for (int i = 0; i + 2 < finalMaskedImageRGBAData.Length; i += 4)
                    {
                        finalMaskedImageRGBAData[i + 3] = preparedMaskRGBAData[i];

                    }
                    System.Runtime.InteropServices.Marshal.Copy(finalMaskedImageRGBAData, 0, bmpData1.Scan0, finalMaskedImageRGBAData.Length);
                    finalMaskedImage.UnlockBits(bmpData1);
                    preparedMask.UnlockBits(bmpData2);
                }
            }
        }

        private void PrepareMaskImage()
        {
            if (_mask != null)
            {

                preparedMask = Create32bppImageAndClearAlpha(_mask);

                BitmapData bmpData = preparedMask.LockBits(new Rectangle(0, 0, preparedMask.Width, preparedMask.Height), ImageLockMode.ReadWrite, preparedMask.PixelFormat);

                byte[] preparedMaskRGBData = new byte[bmpData.Stride * bmpData.Height];

                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, preparedMaskRGBData, 0, preparedMaskRGBData.Length);


                byte greyLevel;
                bool opaque = false;
                //int OpacityThreshold = this.trackBar1.Value;
                bool invertedMask = true;//this.checkBoxInvertMask.Checked;
                for (int i = 0; i + 2 < preparedMaskRGBData.Length; i += 4)
                {
                    //convert to gray scale R:0.30 G=0.59 B 0.11
                    float greyLevelF = 0.3f * preparedMaskRGBData[i + 2] + 0.59f * preparedMaskRGBData[i + 1] + 0.11f * preparedMaskRGBData[i];

                    if (opaque)
                    {
                        greyLevel = (greyLevelF < 420/* MUST BE "/ some constant" - OpacityThreshold*/) ? byte.MinValue : byte.MaxValue;
                    }
                    if (invertedMask)
                    {
                        greyLevel = (byte)(255 - greyLevelF);
                    }
                    else
                    {
                        greyLevel = (byte)greyLevelF;
                    }

                    preparedMaskRGBData[i] = greyLevel;
                    preparedMaskRGBData[i + 1] = greyLevel;
                    preparedMaskRGBData[i + 2] = greyLevel;

                }
                System.Runtime.InteropServices.Marshal.Copy(preparedMaskRGBData, 0, bmpData.Scan0, preparedMaskRGBData.Length);
                preparedMask.UnlockBits(bmpData);
                //this.spriteMask.Image = ThemedBitmapAddRef(this, preparedMask);
                // if the loaded image is available, we have everything to compute the masked image
                if (_image != null)
                {
                    PrepareMaskedImage();
                }
            }
        }

        private Bitmap Create32bppImageAndClearAlpha(Bitmap tmpImage)
        {
            // declare the new image that will be returned by the function
            Bitmap returnedImage = new Bitmap(tmpImage.Width, tmpImage.Height, PixelFormat.Format32bppArgb);

            // create a graphics instance to draw the original image in the new one
            Rectangle rect = new Rectangle(0, 0, tmpImage.Width, tmpImage.Height);
            Graphics g = Graphics.FromImage(returnedImage);

            // create an image attribe to force a clearing of the alpha layer
            ImageAttributes imageAttributes = new ImageAttributes();
            float[][] colorMatrixElements = {
                        new float[] {1,0,0,0,0},
                        new float[] {0,1,0,0,0},
                        new float[] {0,0,1,0,0},
                        new float[] {0,0,0,0,0},
                        new float[] {0,0,0,1,1}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // draw the original image 
            g.DrawImage(tmpImage, rect, 0, 0, tmpImage.Width, tmpImage.Height, GraphicsUnit.Pixel, imageAttributes);
            g.Dispose();
            return returnedImage;
        }
        #endregion
    }

    partial class Plugin
    {
        public static Color HsBtoRgb(double h, double s, double b, int a = 255)
        {
            const double tolerance = 0.000000000000001;


            h = Math.Max(0D, Math.Min(360D, h));
            s = Math.Max(0D, Math.Min(1D, s));
            b = Math.Max(0D, Math.Min(1D, b));
            a = Math.Max(0, Math.Min(255, a));

            double r = 0D;
            double g = 0D;
            double bl = 0D;

            if (Math.Abs(s) < tolerance)
                r = g = bl = b;
            else
            {
                // the argb wheel consists of 6 sectors. Figure out which sector
                // you're in.
                double sectorPos = h / 60D;
                int sectorNumber = (int)Math.Floor(sectorPos);
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the argb.
                double p = b * (1D - s);
                double q = b * (1D - (s * fractionalSector));
                double t = b * (1D - (s * (1D - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector
                // the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = b;
                        g = t;
                        bl = p;
                        break;
                    case 1:
                        r = q;
                        g = b;
                        bl = p;
                        break;
                    case 2:
                        r = p;
                        g = b;
                        bl = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        bl = b;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        bl = b;
                        break;
                    case 5:
                        r = b;
                        g = p;
                        bl = q;
                        break;
                }
            }

            return Color.FromArgb(
                    a,
                    Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{r * 255D:0.00}")))),
                    Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{g * 255D:0.00}")))),
                    Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{bl * 250D:0.00}")))));
        }

        public static Color GetHighlightColor(Color highlightColor, Color sampleColor, Color backColor, float highlightWeight = 0.80f)//***
        {
            float highlightHue = highlightColor.GetHue();
            float highlightSat = highlightColor.GetSaturation();
            float highlightBr = highlightColor.GetBrightness();

            float sampleHue = sampleColor.GetHue();
            float sampleSat = sampleColor.GetSaturation();
            float sampleBr = sampleColor.GetBrightness();

            float backBr = backColor.GetBrightness();

            float resultHue = (highlightHue * highlightWeight * highlightSat * highlightBr + sampleHue * (1 - highlightWeight) * sampleSat * sampleBr) 
                / (highlightWeight * highlightSat * highlightBr + (1 - highlightWeight) * sampleSat * sampleBr);
            float resultSat = highlightSat * 0.3f + sampleSat * 0.7f;
            float resultBr = highlightBr * highlightWeight + sampleBr * (1 - highlightWeight);

            if (Math.Abs(resultBr - backBr) < 0.3)
            {
                if (backBr > 0.5)
                    resultBr = resultBr - Math.Abs(resultBr - backBr);
                else
                    resultBr = resultBr + Math.Abs(resultBr - backBr);
            }

            Color resultColor = HsBtoRgb(resultHue, resultSat, resultBr);

            return resultColor;
        }

        public static Color GetWeightedColor(Color sampleColor1, Color sampleColor2, float sampleColor1Weight = 0.5f)//***
        {
            int resultR = (int)(sampleColor1.R * sampleColor1Weight + sampleColor2.R * (1 - sampleColor1Weight));
            int resultG = (int)(sampleColor1.G * sampleColor1Weight + sampleColor2.G * (1 - sampleColor1Weight));
            int resultB = (int)(sampleColor1.B * sampleColor1Weight + sampleColor2.B * (1 - sampleColor1Weight));

            resultR = resultR > 255 ? 255 : resultR;
            resultG = resultG > 255 ? 255 : resultG;
            resultB = resultB > 255 ? 255 : resultB;

            Color resultColor = Color.FromArgb(resultR, resultG, resultB);

            return resultColor;
        }

        public static Color GetInvertedColor(Color sampleColor)
        {
            return Color.FromArgb(255 - sampleColor.R, 255 - sampleColor.G, 255 - sampleColor.B);
        }

        public static float GetBrightnessDifference(Color sampleColor1, Color sampleColor2)
        {
            return (sampleColor1.R - sampleColor2.R + sampleColor1.G - sampleColor2.G + sampleColor1.B - sampleColor2.B) / 3.0f / 255f;
        }

        public static float GetAverageBrightness(Color sampleColor1)
        {
            return (sampleColor1.R + sampleColor1.G + sampleColor1.B) / 3.0f / 255f;
        }

        public static Color GetBitmapAverageColor(Bitmap img)
        {
            int avgR = 0, avgG = 0, avgB = 0;
            int blurPixelCount = 0;

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color pixel = img.GetPixel(x, y);
                    avgR += pixel.R;
                    avgG += pixel.G;
                    avgB += pixel.B;

                    blurPixelCount++;
                }
            }

            avgR = avgR * 3 / 2 / blurPixelCount;
            avgR = avgR > 255 ? 255 : avgR;

            avgG = avgG * 3 / 2 / blurPixelCount;
            avgG = avgG > 255 ? 255 : avgG;

            avgB = avgB * 3 / 2 / blurPixelCount;
            avgB = avgB > 255 ? 255 : avgB;


            return Color.FromArgb(avgR, avgG, avgB);
        }

        public static Bitmap GetSolidImageByBitmapMask(Color accentColor, Bitmap maskBitmap)
        {
            Bitmap templateBitmap = new Bitmap(maskBitmap.Width, maskBitmap.Height, maskBitmap.PixelFormat);

            using (Graphics gfx = Graphics.FromImage(templateBitmap))
            {
                gfx.Clear(accentColor);
            }

            AlphaBlendedImage alphaBlendedImage = new AlphaBlendedImage(templateBitmap, maskBitmap);

            return alphaBlendedImage.AlphaBlendImages();
        }

        public static Bitmap GetSolidImageByBitmapMask(Color accentColor, Bitmap maskBitmap, int newWidth, int newHeight)
        {

            Bitmap scaledBitmap = new Bitmap(newWidth, newHeight, maskBitmap.PixelFormat);
            using (Graphics gfx = Graphics.FromImage(scaledBitmap))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfx.Clear(accentColor);
            }

            Bitmap scaledMaskedBitmap = new Bitmap(newWidth, newHeight, maskBitmap.PixelFormat);
            using (Graphics gfx = Graphics.FromImage(scaledMaskedBitmap))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(maskBitmap, -1, -1, newWidth + 1, newHeight + 1);
            }

            AlphaBlendedImage alphaBlendedImage = new AlphaBlendedImage(scaledBitmap, scaledMaskedBitmap);

            scaledBitmap.Dispose();
            scaledMaskedBitmap.Dispose();

            return alphaBlendedImage.AlphaBlendImages();
        }

        public static Bitmap ScaleBitmap(Bitmap bitmap, int newWidth, int newHeight)
        {

            Bitmap scaledBitmap = new Bitmap(newWidth, newHeight, bitmap.PixelFormat);
            using (Graphics gfx = Graphics.FromImage(scaledBitmap))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(bitmap, -1, -1, newWidth + 1, newHeight + 1);
            }

            return scaledBitmap;
        }
    }
}
