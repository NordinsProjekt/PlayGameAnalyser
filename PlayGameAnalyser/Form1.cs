using PlayGameAnalyser.Interfaces;
using PlayGameAnalyser.Service;
using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PlayGameAnalyser
{
    public partial class Form1 : Form
    {
        private byte[] safePic { get; init; }
        private readonly DXBallAutoPlayer _service;
        public Form1()
        {
            InitializeComponent();

            Bitmap refPicture = new Bitmap($"381.png");
            _service = new DXBallAutoPlayer();
            safePic = _service.GenerateBitmapDataArray(refPicture);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                label1.Text = "You need to select a game!";
                return;
            }
            label1.Text = "Waiting......";
            label1.Invalidate();
            label1.Update();
            IScreenshotService service = new ScreenshotService();
            int num = 0;
            Thread.Sleep(7000);
            label1.Text = "START!!!";
            label1.Invalidate();
            label1.Update();
            MouseHandler.LeftMouseClick(new Point(0, 0));
            while (true)
            {
                Thread.Sleep(20);
                var gameScreen = service.GetBitmapDataAsByteArray(new Records.CaptureArea(0, 1050, 330, 2560));
                var result = SomethingOnScreen(gameScreen);
                if (result > 0)
                {
                    MouseHandler.SetCursorPosition(result, 100);
                }
                if (result == -1)
                {
                    break;
                }
            }
            label1.Text = "YOU LOST";
        }

        private int SomethingOnScreen(byte[] bitmap)
        {
            if (_service.CheckForGameOverScreen(bitmap[(8 * 4)], bitmap[(8 * 4)+1], bitmap[(8 * 4) + 2]))
                return -1;
            for (int i = 2; i < bitmap.Length; i += 8)
            {
                if (bitmap[i] == safePic[i])
                    continue;

                int sum = i / 4;
                while (sum >= 2560)
                    sum -= 2560;

                return (int)sum;
            }
            return 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}