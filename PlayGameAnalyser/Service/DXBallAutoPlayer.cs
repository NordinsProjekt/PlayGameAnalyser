using PlayGameAnalyser.Interfaces;
using PlayGameAnalyser.Records;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlayGameAnalyser.Service
{
    internal class DXBallAutoPlayer
    {
        private readonly IScreenshotService _screenshotService;
        private  byte[] referenceScreen { get; init; }
        internal DXBallAutoPlayer(IScreenshotService screenshotService, byte[] referenceScreen) 
        {
            _screenshotService = screenshotService;
            this.referenceScreen = referenceScreen;
        }

        internal bool CheckForGameOverScreen(byte r, byte g, byte b)
        {
            if (r == 0 && g == 0 && b == 0)
                return true;
            return false;
        }

        internal int AnalyseGameScreen()
        {
            var screen = _screenshotService.GetBitmapDataAsByteArray(new Records.CaptureArea(0, 1050, 330, 2560));
            return SomethingOnScreen(screen);
        }
        private int SomethingOnScreen(byte[] bitmap)
        {
            if (CheckForGameOverScreen(bitmap[(8 * 4)], bitmap[(8 * 4) + 1], bitmap[(8 * 4) + 2]))
                return -1;

            Dictionary<int, Pixel> mapping = new Dictionary<int, Pixel>();
            int sum = 0;
            for (int i = 3; i < bitmap.Length; i += 4)
            {
                if (bitmap[i - 1] == referenceScreen[i - 1])
                    continue;
                //Add the pixel that doesnt match the reference
                mapping.Add(i / 4, new(bitmap[i], bitmap[i - 3], bitmap[i - 2], bitmap[i - 1]));
                sum = i / 4;
            }
            if (sum == 0)
                return 0;
            Pixel match = new(255, 66, 66, 66);
            sum = mapping.FirstOrDefault(x => x.Value == match).Key;

            while (sum >= 2560)
                sum -= 2560;
            return (int)sum;
        }

    }
}
