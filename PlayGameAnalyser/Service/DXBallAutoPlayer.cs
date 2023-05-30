using PlayGameAnalyser.Interfaces;
using PlayGameAnalyser.Records;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        internal ScreenAnalyseResult AnalyseGameScreen(byte[] gameScreen)
            => SomethingOnScreen(gameScreen);

        private ScreenAnalyseResult SomethingOnScreen(byte[] bitmap)
        {
            if (CheckForGameOverScreen(bitmap[(8 * 4)], bitmap[(8 * 4) + 1], bitmap[(8 * 4) + 2]))
                return new(-1, false);

            return QuickAnalyser(bitmap);
        }

        private int CalculateXPosition(int sum)
        {
            while (sum >= 2560)
                sum -= 2560;
            return (int)sum;
        }

        /// <summary>
        /// Bases decision on 1 pixel
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private ScreenAnalyseResult QuickAnalyser(byte[] bitmap)
        {
            Dictionary<Pixel, int> mapping = new Dictionary<Pixel, int>();

            for (int i = 3; i < bitmap.Length; i += 4)
            {
                if (bitmap[i - 1] == referenceScreen[i - 1])
                    continue;
                //Add the pixel that doesnt match the reference
                if (mapping.TryAdd(new(bitmap[i], bitmap[i - 3], bitmap[i - 2], bitmap[i - 1]), i / 4)) ;
                else
                {
                    Pixel p = new(bitmap[i], bitmap[i - 3], bitmap[i - 2], bitmap[i - 1]);
                    mapping[p] = i / 4;
                }
            }
            if (mapping.Count == 0)
                return new ScreenAnalyseResult(0, false);

            //Caught
            Pixel magnetic = new Pixel(255, 156, 74, 74);
            int magneticLocation = mapping.FirstOrDefault(x => x.Key.Equals(magnetic)).Value;
            if (magneticLocation > 0)
                return new(-2, false);

            //TODO Need pixel for the different colored balls
            //Fireball
            Pixel fireball = new(255, 49, 165, 255);
            //Ball
            Pixel ball = new(255, 66, 66, 66);
            //Powerup
            Pixel powerup = new(255, 206, 0, 0);
            //Shoot
            Pixel paddleGuns = new(255, 41, 123, 165);

            

            int ballLocation = mapping.FirstOrDefault(x => x.Key.Equals(ball)).Value;
            int powerLocation = mapping.FirstOrDefault(x => x.Key.Equals(powerup)).Value;
            int paddleGun = mapping.FirstOrDefault(x => x.Key.Equals(paddleGuns)).Value;
            int fireballLocation = mapping.FirstOrDefault(x => x.Key.Equals(fireball)).Value;
            bool guns = paddleGun > 0;

            if (powerLocation > ballLocation)
                ballLocation = powerLocation;

            if (powerLocation < fireballLocation)
                ballLocation = fireballLocation;

            return new(CalculateXPosition(ballLocation), guns);
        }

    }
}
