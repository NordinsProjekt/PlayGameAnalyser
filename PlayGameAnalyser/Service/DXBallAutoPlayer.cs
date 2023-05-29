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
        private  byte[] referenceScreen { get; init; }
        internal DXBallAutoPlayer(byte[] referenceScreen) 
        {
            this.referenceScreen = referenceScreen;
        }

        internal bool CheckForGameOverScreen(byte r, byte g, byte b)
        {
            if (r == 0 && g == 0 && b == 0)
                return true;
            return false;
        }

        internal int AnalyseGameScreen(byte[] gameScreen)
            => SomethingOnScreen(gameScreen);

        private int SomethingOnScreen(byte[] bitmap)
        {
            if (CheckForGameOverScreen(bitmap[(8 * 4)], bitmap[(8 * 4) + 1], bitmap[(8 * 4) + 2]))
                return -1;

            //Dictionary<int, Pixel> mapping = new Dictionary<int, Pixel>();
            Dictionary<Pixel, int> mapping = new Dictionary<Pixel, int>();
            int sum = 0;
            for (int i = 3; i < bitmap.Length; i += 4)
            {
                if (bitmap[i - 1] == referenceScreen[i - 1])
                    continue;
                //Add the pixel that doesnt match the reference
                //mapping.Add(i / 4, new(bitmap[i], bitmap[i - 3], bitmap[i - 2], bitmap[i - 1]));
                if (mapping.TryAdd(new(bitmap[i], bitmap[i - 3], bitmap[i - 2], bitmap[i - 1]), i / 4));
                else
                {
                    Pixel p = new(bitmap[i], bitmap[i - 3], bitmap[i - 2], bitmap[i - 1]);
                    mapping[p] = i / 4;
                }
            }
            if (mapping.Count == 0)
                return 0;
            //Ball
            Pixel ball = new(255, 66, 66, 66);
            //Powerup
            Pixel powerup = new(255, 206, 0, 0);

            sum = mapping.FirstOrDefault(x => x.Key.Equals(ball)).Value;
            int powerLocation = mapping.FirstOrDefault(x => x.Key.Equals(powerup)).Value;

            if (powerLocation > sum)
                sum = powerLocation;
            //sum = mapping.FirstOrDefault(x => x.Key == match).Key;

            while (sum >= 2560)
                sum -= 2560;
            return (int)sum;
        }

    }
}
