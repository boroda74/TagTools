using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

using static NativeMethods;


namespace MusicBeePlugin
{
    internal class AlphaBlendedImage
    {
        private readonly Bitmap _image;
        private readonly Bitmap _mask;
        private Bitmap _preparedMask;
        private Bitmap _finalMaskedOrBlendedImage;

        private readonly bool? _invertedMaskOrMaskIsImageToBlend = true; //true: inverted mask, false: not inverted mask, null: mask is image to blend

        ~AlphaBlendedImage()
        {
            _image?.Dispose();
            _mask?.Dispose();
            _preparedMask?.Dispose();
        }

        internal AlphaBlendedImage(Bitmap image, Bitmap mask)
        {
            _image = Create32bppImageAndClearAlpha(image);
            _mask = Create32bppImageAndClearAlpha(mask);
        }

        internal AlphaBlendedImage(Bitmap image, Bitmap mask, bool? invertedMaskOrMaskIsImageToBlend) : this(image, mask)
        {
            _invertedMaskOrMaskIsImageToBlend = invertedMaskOrMaskIsImageToBlend;
        }

        internal Bitmap AlphaBlendImages(float contrast)
        {
            PrepareMaskImage(contrast);
            return _finalMaskedOrBlendedImage;
        }

        #region This is ugly stuff
        private void PrepareMaskedImage(float contrast)
        {
            if (_image != null && _preparedMask != null)
            {
                if (_image.Width != _preparedMask.Width || _image.Height != _preparedMask.Height)
                {
                    throw new BadImageFormatException(Plugin.ExMaskAndImageMustBeTheSameSize);
                }
                else
                {
                    _finalMaskedOrBlendedImage = Plugin.CopyBitmap(_image);

                    var finalBmpData = _finalMaskedOrBlendedImage.LockBits(new Rectangle(0, 0, _finalMaskedOrBlendedImage.Width, _finalMaskedOrBlendedImage.Height), ImageLockMode.ReadWrite, _finalMaskedOrBlendedImage.PixelFormat);
                    var finalMaskedImageRGBAData = new byte[Math.Abs(finalBmpData.Stride) * finalBmpData.Height];
                    System.Runtime.InteropServices.Marshal.Copy(finalBmpData.Scan0, finalMaskedImageRGBAData, 0, finalMaskedImageRGBAData.Length);

                    var preparedMaskBmpData = _preparedMask.LockBits(new Rectangle(0, 0, _preparedMask.Width, _preparedMask.Height), ImageLockMode.ReadOnly, _preparedMask.PixelFormat);
                    var preparedMaskRGBAData = new byte[Math.Abs(preparedMaskBmpData.Stride) * preparedMaskBmpData.Height];
                    System.Runtime.InteropServices.Marshal.Copy(preparedMaskBmpData.Scan0, preparedMaskRGBAData, 0, preparedMaskRGBAData.Length);

                    //copy the mask to the Alpha layer
                    for (var i = 0; i + 2 < finalMaskedImageRGBAData.Length; i += 4)
                    {
                        int alpha = (int)Math.Round(contrast * preparedMaskRGBAData[i + 3]);
                        alpha = alpha > 255 ? 255 : alpha;
                        alpha = alpha < 0 ? 0 : alpha;
                        finalMaskedImageRGBAData[i + 3] = (byte)alpha; //Alpha
                    }

                    System.Runtime.InteropServices.Marshal.Copy(finalMaskedImageRGBAData, 0, finalBmpData.Scan0, finalMaskedImageRGBAData.Length);

                    _finalMaskedOrBlendedImage.UnlockBits(finalBmpData);
                    _preparedMask.UnlockBits(preparedMaskBmpData);
                }
            }
        }

        private void BlendImages()
        {
            if (_image != null && _mask != null)
            {
                if (_image.Width != _mask.Width || _image.Height != _mask.Height)
                {
                    throw new BadImageFormatException(Plugin.ExMaskAndImageMustBeTheSameSize);
                }
                else
                {
                    _finalMaskedOrBlendedImage = Plugin.CopyBitmap(_image);

                    var finalBmpData = _finalMaskedOrBlendedImage.LockBits(new Rectangle(0, 0, _finalMaskedOrBlendedImage.Width, _finalMaskedOrBlendedImage.Height), ImageLockMode.ReadWrite, _finalMaskedOrBlendedImage.PixelFormat);
                    var finalBlendedImageRGBAData = new byte[Math.Abs(finalBmpData.Stride) * finalBmpData.Height];
                    System.Runtime.InteropServices.Marshal.Copy(finalBmpData.Scan0, finalBlendedImageRGBAData, 0, finalBlendedImageRGBAData.Length);

                    var image2BmpData = _mask.LockBits(new Rectangle(0, 0, _mask.Width, _mask.Height), ImageLockMode.ReadOnly, _mask.PixelFormat);
                    var image2RGBAData = new byte[Math.Abs(image2BmpData.Stride) * image2BmpData.Height];
                    System.Runtime.InteropServices.Marshal.Copy(image2BmpData.Scan0, image2RGBAData, 0, image2RGBAData.Length);

                    //copy the mask to the Alpha layer
                    for (var i = 0; i + 2 < finalBlendedImageRGBAData.Length; i += 4)
                    {
                        double rgbDataR = Math.Round((double)finalBlendedImageRGBAData[i] * finalBlendedImageRGBAData[i + 3] / 255
                                                     + (double)image2RGBAData[i] * image2RGBAData[i + 3] / 255);
                        double rgbDataG = Math.Round((double)finalBlendedImageRGBAData[i + 1] * finalBlendedImageRGBAData[i + 3] / 255
                                                     + (double)image2RGBAData[i + 1] * image2RGBAData[i + 3] / 255);
                        double rgbDataB = Math.Round((double)finalBlendedImageRGBAData[i + 2] * finalBlendedImageRGBAData[i + 3] / 255
                                                     + (double)image2RGBAData[i + 2] * image2RGBAData[i + 3] / 255);
                        double rgbDataA = Math.Round((double)finalBlendedImageRGBAData[i + 3] * image2RGBAData[i + 3] / 255);


                        if (rgbDataR < 0)
                            rgbDataR = 0;
                        else if (rgbDataR > 255)
                            rgbDataR = 255;

                        if (rgbDataG < 0)
                            rgbDataG = 0;
                        else if (rgbDataG > 255)
                            rgbDataG = 255;

                        if (rgbDataB < 0)
                            rgbDataB = 0;
                        else if (rgbDataB > 255)
                            rgbDataB = 255;

                        if (rgbDataA < 0)
                            rgbDataA = 0;
                        else if (rgbDataA > 255)
                            rgbDataA = 255;


                        finalBlendedImageRGBAData[i] = (byte)rgbDataR;
                        finalBlendedImageRGBAData[i + 1] = (byte)rgbDataG;
                        finalBlendedImageRGBAData[i + 2] = (byte)rgbDataB;
                        finalBlendedImageRGBAData[i + 3] = (byte)rgbDataA; //Alpha
                    }

                    System.Runtime.InteropServices.Marshal.Copy(finalBlendedImageRGBAData, 0, finalBmpData.Scan0, finalBlendedImageRGBAData.Length);

                    _finalMaskedOrBlendedImage.UnlockBits(finalBmpData);
                    _mask.UnlockBits(image2BmpData);
                }
            }
        }

        private void PrepareMaskImage(float contrast)
        {
            if (_invertedMaskOrMaskIsImageToBlend == null && _mask != null)
            {
                if (_image != null)
                    BlendImages();
            }
            else if (_mask != null)
            {
                _preparedMask = Plugin.CopyBitmap(_mask);

                var bmpData = _preparedMask.LockBits(new Rectangle(0, 0, _preparedMask.Width, _preparedMask.Height), ImageLockMode.ReadWrite, _preparedMask.PixelFormat);

                var preparedMaskRGBData = new byte[Math.Abs(bmpData.Stride) * bmpData.Height];

                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, preparedMaskRGBData, 0, preparedMaskRGBData.Length);


                var opaque = false;
                //int OpacityThreshold = trackBar1.Value;
                for (var i = 0; i + 2 < preparedMaskRGBData.Length; i += 4)
                {
                    //convert to brightness R=0.30 G=0.59 B=0.11
                    var brightnessF = 0.3f * preparedMaskRGBData[i] + 0.59f * preparedMaskRGBData[i + 1] + 0.11f * preparedMaskRGBData[i + 2];

                    if (_invertedMaskOrMaskIsImageToBlend == true)
                        brightnessF = 255 - brightnessF;

                    if (brightnessF < 0)
                        brightnessF = 0;
                    else if (brightnessF > 255)
                        brightnessF = 255;

                    var brightness = (byte)Math.Round(brightnessF);

                    if (opaque) //-V3022
                        brightness = (brightnessF < 420/* MUST BE "some constant" - OpacityThreshold*/) ? byte.MinValue : byte.MaxValue; //----- greyLevelF LESS THAN 420 ???

                    preparedMaskRGBData[i] = brightness;
                    preparedMaskRGBData[i + 1] = brightness;
                    preparedMaskRGBData[i + 2] = brightness;
                    preparedMaskRGBData[i + 3] = brightness;
                }

                System.Runtime.InteropServices.Marshal.Copy(preparedMaskRGBData, 0, bmpData.Scan0, preparedMaskRGBData.Length);

                _preparedMask.UnlockBits(bmpData);
                if (_image != null)
                    PrepareMaskedImage(contrast);
            }
        }

        private Bitmap Create32bppImageAndClearAlpha(Bitmap tmpImage)
        {
            //declare the new image that will be returned by the function
            var returnedImage = new Bitmap(tmpImage.Width, tmpImage.Height, PixelFormat.Format32bppArgb);

            //create a graphics instance to draw the original image in the new one
            var rect = new Rectangle(0, 0, tmpImage.Width, tmpImage.Height);
            using (var g = Graphics.FromImage(returnedImage))
            {
                //create an image attribs to force a clearing of the alpha layer
                var imageAttributes = new ImageAttributes();
                float[][] colorMatrixElements = {
                        new float[] {1,0,0,0,0},
                        new float[] {0,1,0,0,0},
                        new float[] {0,0,1,0,0},
                        new float[] {0,0,0,0,0},
                        new float[] {0,0,0,1,1}};

                var colorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //draw the original image 
                g.DrawImage(tmpImage, rect, 0, 0, tmpImage.Width, tmpImage.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return returnedImage;
        }
        #endregion
    }

    partial class Plugin
    {
        internal static Color HsbToRgb(double h, double s, double b, int a = 255)
        {
            const double tolerance = 0.000000000000001;


            h = Math.Max(0D, Math.Min(360D, h));
            s = Math.Max(0D, Math.Min(1D, s));
            b = Math.Max(0D, Math.Min(1D, b));
            a = Math.Max(0, Math.Min(255, a));

            var r = 0D;
            var g = 0D;
            var bl = 0D;

            if (Math.Abs(s) < tolerance)
            {
                r = g = bl = b;
            }
            else
            {
                //the argb wheel consists of 6 sectors. Figure out which sector
                //you're in.
                var sectorPos = h / 60D;
                var sectorNumber = (int)Math.Floor(sectorPos);
                //get the fractional part of the sector
                var fractionalSector = sectorPos - sectorNumber;

                //calculate values for the three axes of the argb.
                var p = b * (1D - s);
                var q = b * (1D - (s * fractionalSector));
                var t = b * (1D - (s * (1D - fractionalSector)));

                //assign the fractional colors to r, g, and b based on the sector
                //the angle is in.
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

        internal static Color GetHighlightColor(Color highlightColor, Color foreColor, Color backColor, float highlightWeight = 0.50f) //---
        {
            var sampleColor = GetWeightedColor(foreColor, backColor);

            var highlightHue = highlightColor.GetHue();
            var highlightSat = highlightColor.GetSaturation();
            var highlightBr = highlightColor.GetBrightness();

            var sampleHue = sampleColor.GetHue();
            var sampleSat = sampleColor.GetSaturation();
            var sampleBr = sampleColor.GetBrightness();

            var backBr = backColor.GetBrightness();

            var resultHue = (highlightHue * highlightWeight + sampleHue * (1 - highlightWeight));
            var resultSat = highlightSat * highlightWeight + sampleSat * (1 - highlightWeight);
            var resultBr = sampleBr;

            var resultColor = HsbToRgb(resultHue, resultSat, resultBr);

            return resultColor;
        }

        internal static Color GetWeightedColor(Color sampleColor1, Color sampleColor2, float sampleColor1Weight = 0.5f) //---
        {
            var resultR = (int)Math.Round(sampleColor1.R * sampleColor1Weight + sampleColor2.R * (1 - sampleColor1Weight));
            var resultG = (int)Math.Round(sampleColor1.G * sampleColor1Weight + sampleColor2.G * (1 - sampleColor1Weight));
            var resultB = (int)Math.Round(sampleColor1.B * sampleColor1Weight + sampleColor2.B * (1 - sampleColor1Weight));

            resultR = resultR < 0 ? 0 : resultR;
            resultG = resultG < 0 ? 0 : resultG;
            resultB = resultB < 0 ? 0 : resultB;

            resultR = resultR > 255 ? 255 : resultR;
            resultG = resultG > 255 ? 255 : resultG;
            resultB = resultB > 255 ? 255 : resultB;

            var resultColor = Color.FromArgb(resultR, resultG, resultB);

            return resultColor;
        }

        internal static Color GetColorAt(Control control, Point location)
        {
            Bitmap controlPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            using (Graphics gdest = Graphics.FromImage(controlPixel))
            {
                using (Graphics gbtn = Graphics.FromHwnd(IntPtr.Zero))
                {
                    var screenLocation = control.PointToScreen(location);

                    IntPtr hSrcDC = gbtn.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, screenLocation.X, screenLocation.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gbtn.ReleaseHdc();
                }
            }

            return controlPixel.GetPixel(0, 0);
        }

        internal static Color GetInvertedColor(Color sampleColor)
        {
            return Color.FromArgb(255 - sampleColor.R, 255 - sampleColor.G, 255 - sampleColor.B);
        }

        internal static float GetBrightnessDifference(Color sampleColor1, Color sampleColor2)
        {
            return Math.Abs((sampleColor1.R - sampleColor2.R + sampleColor1.G - sampleColor2.G + sampleColor1.B - sampleColor2.B) / 3.0f / 256f);
        }

        internal static float GetAverageBrightness(Color sampleColor)
        {
            return (sampleColor.R + sampleColor.G + sampleColor.B) / 3.0f / 255f;
        }

        internal static Color GetInvertedAverageBrightnessColor(Color sampleColor, Color baseColor, float contrast)
        {
            float avgBr = GetAverageBrightness(baseColor);

            int R;
            int G;
            int B;

            if (avgBr > 0.5f) //Let's decrease sample color brightness
            {
                R = (int)Math.Round((sampleColor.R - 127) / 4f + 63f / contrast);
                G = (int)Math.Round((sampleColor.G - 127) / 4f + 63f / contrast);
                B = (int)Math.Round((sampleColor.B - 127) / 4f + 63f / contrast);
            }
            else //Let's increase sample color brightness
            {
                R = (int)Math.Round((sampleColor.R - 127) / 4f + 255f - 63f * contrast);
                G = (int)Math.Round((sampleColor.G - 127) / 4f + 255f - 63f * contrast);
                B = (int)Math.Round((sampleColor.B - 127) / 4f + 255f - 63f * contrast);
            }


            if (R < 0)
                R = 0;

            if (G < 0)
                G = 0;

            if (B < 0)
                B = 0;


            if (R > 255)
                R = 255;

            if (G > 255)
                G = 255;

            if (B > 255)
                B = 255;


            var invertedBrightnessColor = Color.FromArgb(sampleColor.A, R, G, B);

            return invertedBrightnessColor;
        }

        internal static Color IncreaseColorContrast(Color sampleColor, float scale = 1.5f)
        {
            float r = sampleColor.R;
            float g = sampleColor.G;
            float b = sampleColor.B;

            float brt = (r + g + b) / 3f;

            if (brt < 127)
            {
                r *= scale;
                g *= scale;
                b *= scale;

            }
            else
            {
                r /= scale;
                g /= scale;
                b /= scale;
            }

            r = r > 255 ? 255 : r;
            r = r < 0 ? 0 : r;

            g = g > 255 ? 255 : g;
            g = g < 0 ? 0 : g;

            b = b > 255 ? 255 : b;
            b = b < 0 ? 0 : b;

            return Color.FromArgb(255, (int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
        }

        internal static Color GetBitmapAverageColor(Bitmap img)
        {
            int avgR = 0, avgG = 0, avgB = 0;
            var blurPixelCount = 0;

            for (var y = 0; y < img.Height; y++)
            {
                for (var x = 0; x < img.Width; x++)
                {
                    var pixel = img.GetPixel(x, y);
                    avgR += pixel.R;
                    avgG += pixel.G;
                    avgB += pixel.B;

                    blurPixelCount++;
                }
            }

            blurPixelCount = blurPixelCount == 0 ? 1 : blurPixelCount;

            avgR = avgR / blurPixelCount;
            avgR = avgR > 255 ? 255 : avgR;

            avgG = avgG / blurPixelCount;
            avgG = avgG > 255 ? 255 : avgG;

            avgB = avgB / blurPixelCount;
            avgB = avgB > 255 ? 255 : avgB;


            return Color.FromArgb(avgR, avgG, avgB);
        }

        internal static Bitmap GetSolidImageByBitmapMask(Color foreColor, Bitmap maskBitmap)
        {
            var templateBitmap = new Bitmap(maskBitmap.Width, maskBitmap.Height, PixelFormat.Format32bppArgb);

            using (var gfx = Graphics.FromImage(templateBitmap))
                gfx.Clear(foreColor);

            var alphaBlendedImage = new AlphaBlendedImage(templateBitmap, maskBitmap);
            templateBitmap.Dispose();

            return alphaBlendedImage.AlphaBlendImages(1f);
        }

        internal static Bitmap GetSolidImageByBitmapMask(Color foreColor, Bitmap maskBitmap,
            int newWidth, int newHeight, float contrast = 1f, bool invertedMask = true,
            InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic)
        {
            var scaledForeColorFilledBitmap = FillBitmapByColor(foreColor, PixelFormat.Format32bppArgb, newWidth, newHeight);
            var scaledMaskBitmap = ScaleBitmap(maskBitmap, PixelFormat.Format32bppArgb, interpolationMode, newWidth, newHeight);
            var alphaBlendedImage = new AlphaBlendedImage(scaledForeColorFilledBitmap, scaledMaskBitmap, invertedMask);

            scaledForeColorFilledBitmap.Dispose();
            scaledMaskBitmap.Dispose();

            return alphaBlendedImage.AlphaBlendImages(contrast);
        }

        internal static Bitmap GetSolidImageByBitmapMask2(Color foreColor, Color backColor, Bitmap maskBitmap,
            int newWidth, int newHeight, bool invertedMask = true, InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic)
        {
            var foreColorMaskedBitmap = GetSolidImageByBitmapMask(foreColor, maskBitmap, newWidth, newHeight, 1f, invertedMask, interpolationMode);
            var backColorFilledBitmap = FillBitmapByColor(backColor, PixelFormat.Format32bppArgb, newWidth, newHeight);

            BlendImageWithBackground(foreColorMaskedBitmap, backColorFilledBitmap);

            backColorFilledBitmap.Dispose();

            return foreColorMaskedBitmap;
        }

        internal static Bitmap BlendImageWithBackground(Bitmap foreImage, Bitmap backImage)
        {
            if (foreImage != null && backImage != null)
            {
                if (foreImage.Width != backImage.Width || foreImage.Height != backImage.Height
                                                || foreImage.PixelFormat != PixelFormat.Format32bppArgb || backImage.PixelFormat != PixelFormat.Format32bppArgb)
                {
                    throw new BadImageFormatException(Plugin.ExMaskAndImageMustBeTheSameSize + " REQUIED PIXEL FORMAT: " + PixelFormat.Format32bppArgb);
                }
                else
                {
                    var alphaBlendedImage = new AlphaBlendedImage(foreImage, backImage, null);

                    return alphaBlendedImage.AlphaBlendImages(1f);
                }
            }

            return null;
        }

        internal static Bitmap ApplyMaskToImage(Bitmap image, Bitmap maskBitmap, float contrast, bool invertedMask)
        {
            var alphaBlendedImage = new AlphaBlendedImage(image, maskBitmap, invertedMask);

            return alphaBlendedImage.AlphaBlendImages(contrast);
        }

        internal static Bitmap ApplyMaskToImage(Bitmap image, Bitmap maskBitmap,
            int newWidth, int newHeight, float contrast, bool invertedMask)
        {
            var scaledImage = ScaleBitmap(image, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                newWidth, newHeight);
            var scaledMaskBitmap = ScaleBitmap(maskBitmap, PixelFormat.Format32bppArgb, InterpolationMode.HighQualityBicubic,
                newWidth, newHeight);
            var alphaBlendedImage = new AlphaBlendedImage(scaledImage, scaledMaskBitmap, invertedMask);

            scaledImage.Dispose();
            scaledMaskBitmap.Dispose();

            return alphaBlendedImage.AlphaBlendImages(contrast);
        }

        internal static Bitmap ScaleBitmap(Bitmap bitmap, int newWidth, int newHeight)
        {
            var scaledBitmap = MbForm.Invoke(new Func<Bitmap>(() => { return new Bitmap(newWidth, newHeight, bitmap.PixelFormat); })) as Bitmap;
            using (var gfx = Graphics.FromImage(scaledBitmap))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(bitmap, -1, -1, newWidth + 1, newHeight + 1);
            }

            return scaledBitmap;
        }

        internal static Bitmap ScaleBitmap(Bitmap bitmap, PixelFormat pixelFormat, InterpolationMode interpolationMode, int newWidth, int newHeight)
        {
            var scaledBitmap = new Bitmap(newWidth, newHeight, pixelFormat);
            using (var gfx = Graphics.FromImage(scaledBitmap))
            {
                gfx.InterpolationMode = interpolationMode;
                gfx.DrawImage(bitmap, -1, -1, newWidth + 1, newHeight + 1);
            }

            return scaledBitmap;
        }

        //Thread-safe copy operation
        internal static Bitmap CopyBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;


            var scaledBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
            using (var gfx = Graphics.FromImage(scaledBitmap))
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
            }

            return scaledBitmap;
        }

        //rotationAngle measurement units: degrees
        internal static Bitmap RotateBitmap(Bitmap bitmap, int rotationAngle)
        {
            var rotatedBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
            using (var gfx = Graphics.FromImage(rotatedBitmap))
            {
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //now we set the rotation point to the center of our image
                gfx.TranslateTransform((float)rotatedBitmap.Width / 2, (float)rotatedBitmap.Height / 2);
                //now rotate the image
                gfx.RotateTransform(rotationAngle);
                gfx.TranslateTransform(-(float)rotatedBitmap.Width / 2, -(float)rotatedBitmap.Height / 2);

                //now draw our new image onto the graphics object
                gfx.DrawImage(bitmap, new Point(0, 0));
            }

            return rotatedBitmap;
        }

        internal static Bitmap MirrorBitmap(Bitmap bitmap, bool mirrorVertically)
        {
            var mirroredBitmap = new Bitmap(bitmap);

            //Mirroring
            if (mirrorVertically)
                mirroredBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            else
                mirroredBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);

            return mirroredBitmap;
        }

        internal static Bitmap FillBitmapByColor(Color backColor, PixelFormat pixelFormat, int newWidth, int newHeight)
        {
            var filledBitmap = new Bitmap(newWidth, newHeight, pixelFormat);
            using (var gfx = Graphics.FromImage(filledBitmap))
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.Clear(backColor);
            }

            return filledBitmap;
        }

        private static void DrawYRepeatedImageInternal(Graphics gfx, Image image, int x, int y, int srcImageWidth, int destWidth, int height, bool scaleY)
        {
            if (height > image.Height && !scaleY)
            {
                int yOffset;
                for (yOffset = 0; yOffset < height - image.Height; yOffset += image.Height)
                {
                    gfx.DrawImage(image, new Rectangle(x, y + yOffset, destWidth, image.Height),
                        new Rectangle(0, 0, srcImageWidth, image.Height), GraphicsUnit.Pixel);
                }

                gfx.DrawImage(image, new Rectangle(x, y + yOffset, destWidth, height - yOffset),
                    new Rectangle(0, 0, srcImageWidth, height - yOffset), GraphicsUnit.Pixel);
            }
            else if (!scaleY)
            {
                gfx.DrawImage(image, new Rectangle(x, y, destWidth, height),
                    new Rectangle(0, 0, srcImageWidth, height), GraphicsUnit.Pixel);
            }
            else //scaleY == true
            {
                gfx.DrawImage(image, new Rectangle(x, y, destWidth, height),
                    new Rectangle(0, 0, srcImageWidth, image.Height), GraphicsUnit.Pixel);
            }
        }

        internal static void DrawRepeatedImage(Graphics gfx, Image image, int x, int y, int width, int height, bool scaleX, bool scaleY)
        {
            if (width > image.Width && !scaleX)
            {
                int xOffset;
                for (xOffset = 0; xOffset < width - image.Width; xOffset += image.Width)
                    DrawYRepeatedImageInternal(gfx, image, x + xOffset, y, image.Width, image.Width, height, scaleY);

                DrawYRepeatedImageInternal(gfx, image, x + xOffset, y, width - xOffset, width - xOffset, height, scaleY);
            }
            else if (!scaleX)
            {
                DrawYRepeatedImageInternal(gfx, image, x, y, width, width, height, scaleY);
            }
            else //scaleX == true
            {
                DrawYRepeatedImageInternal(gfx, image, x, y, image.Width, width, height, scaleY);
            }
        }

        internal static int AppsUseLightTheme()  //  0 : dark theme  / 1 : light theme / -1 : AppsUseLightTheme could not be found
        {
            string keyName = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            try { return (int)Microsoft.Win32.Registry.GetValue(keyName, "AppsUseLightTheme", -1); }
            catch { return -1; }
        }

        internal static int TitleBarsUseColor()  //  0 : no  / 1 : yes / -1 : ColorPrevalence could not be found
        {
            string keyName = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM";
            try { return (int)Microsoft.Win32.Registry.GetValue(keyName, "ColorPrevalence", -1); }
            catch { return -1; }
        }
    }
}
