using PlayGameAnalyser.Records;
using PlayGameAnalyser.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using PlayGameAnalyser.Service;

namespace PlayGameAnalyser
{
    /// <summary>
    /// Service only works on Windows
    /// </summary>
    public class ScreenshotService : IScreenshotService
    {
        public ScreenshotService() { }

        /// <summary>
        /// Saves bitmap on disc
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="filename"></param>
        /// <returns>bool</returns>
        public bool TakeScreenShot(string savePath, string filename, CaptureArea screen)
        {
            // Get the primary screen bounds
            Rectangle screenSize = new Rectangle(screen.X, screen.Y, screen.Width, screen.Height);
            try
            {
                using (Bitmap screenshot = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format32bppArgb))
                {
                    // Create a graphics object from the bitmap
                    using (Graphics graphics = Graphics.FromImage(screenshot))
                    {
                        // Copy the screen contents to the bitmap
                        graphics.CopyFromScreen(screenSize.X, screenSize.Y, 0, 0, screenSize.Size, CopyPixelOperation.SourceCopy);
                    }

                    // Save the screenshot as a PNG file
                    screenshot.Save($"{savePath}\\{filename}", ImageFormat.Png);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns bitmap as Base64String
        /// </summary>
        /// <returns>Bitmap</returns>
        public string TakeScreenShot(CaptureArea screen)
        {
            // Get the primary screen bounds
            Rectangle screenSize = new Rectangle(screen.X, screen.Y, screen.Width, screen.Height);
            try
            {
                using (Bitmap screenshot = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format32bppArgb))
                {
                    // Create a graphics object from the bitmap
                    using (Graphics graphics = Graphics.FromImage(screenshot))
                    {
                        // Copy the screen contents to the bitmap
                        graphics.CopyFromScreen(screenSize.X, screenSize.Y, 0, 0, screenSize.Size, CopyPixelOperation.SourceCopy);
                    }

                    // Save the bitmap for further manipulation
                    MemoryStream ms = new MemoryStream();
                    screenshot.Save(ms, ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    var SigBase64 = Convert.ToBase64String(byteImage); // Get Base64
                    return SigBase64;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte[] GetBitmapDataAsByteArray(CaptureArea screen)
        {
            Rectangle screenSize = new Rectangle(screen.X, screen.Y, screen.Width, screen.Height);
            try
            {
                using (Bitmap screenshot = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format32bppArgb))
                {
                    // Create a graphics object from the bitmap
                    using (Graphics graphics = Graphics.FromImage(screenshot))
                    {
                        // Copy the screen contents to the bitmap
                        graphics.CopyFromScreen(screenSize.X, screenSize.Y, 0, 0, screenSize.Size, CopyPixelOperation.SourceCopy);
                    }

                    // Save the bitmap for further manipulation
                    MemoryStream ms = new MemoryStream();
                    DXBallAutoPlayer dXBallAutoPlayer = new DXBallAutoPlayer();
                    return dXBallAutoPlayer.GenerateBitmapDataArray(screenshot);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SaveImage(string bitmapStream, string savePath, string filename)
        {
            Bitmap bmpReturn = null;

            byte[] byteBuffer = Convert.FromBase64String(bitmapStream);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);

            memoryStream.Position = 0;

            bmpReturn = (Bitmap)Image.FromStream(memoryStream);
            bmpReturn.Save($"{savePath}\\{filename}", ImageFormat.Png);
            return true;
        }

        public Bitmap GetScreenShotAsBitmap(CaptureArea screen)
        {
            Rectangle screenSize = new Rectangle(screen.X, screen.Y, screen.Width, screen.Height);
            try
            {
                using (Bitmap screenshot = new Bitmap(screenSize.Width, screenSize.Height, PixelFormat.Format32bppArgb))
                {
                    // Create a graphics object from the bitmap
                    using (Graphics graphics = Graphics.FromImage(screenshot))
                    {
                        // Copy the screen contents to the bitmap
                        graphics.CopyFromScreen(screenSize.X, screenSize.Y, 0, 0, screenSize.Size, CopyPixelOperation.SourceCopy);
                    }
                    return screenshot.Clone(screenSize, PixelFormat.Format32bppRgb);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
