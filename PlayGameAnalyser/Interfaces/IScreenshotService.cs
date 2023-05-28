using PlayGameAnalyser.Records;
using System.Drawing;

namespace PlayGameAnalyser.Interfaces
{
    public interface IScreenshotService
    {
        /// <summary>
        /// Saves bitmap on disc
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="filename"></param>
        /// <returns>bool</returns>
        bool TakeScreenShot(string savePath, string filename, CaptureArea screen);

        /// <summary>
        /// Returns bitmap as base64 string
        /// </summary>
        /// <returns>Bitmap</returns>
        string TakeScreenShot(CaptureArea screen);
        Bitmap GetScreenShotAsBitmap(CaptureArea screen);
        bool SaveImage(string bitmapStream, string savePath, string filename);
        byte[] GetBitmapDataAsByteArray(CaptureArea screen);


    }
}
