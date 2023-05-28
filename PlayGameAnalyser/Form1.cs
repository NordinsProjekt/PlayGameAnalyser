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
            label1.Text = "Waiting......";
            IScreenshotService service = new ScreenshotService();
            int num = 0;
            Thread.Sleep(7000);
            label1.Text = "START!!!";
            MouseHandler.LeftMouseClick(new Point(0,0));
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

        //private int GetBallPos(string screen)
        //{
        //    Bitmap bmpReturn = null;

        //    byte[] byteBuffer = Convert.FromBase64String(screen);
        //    MemoryStream memoryStream = new MemoryStream(byteBuffer);

        //    memoryStream.Position = 0;

        //    bmpReturn = (Bitmap)Image.FromStream(memoryStream);
        //    var result = SomethingOnScreen(bmpReturn);

        //    return result;
        //}

        private int SomethingOnScreen(byte[] bitmap)
        {
            //byte[] regValues1 = _service.GenerateBitmapDataArray(bitmap);
            //bitmapdata bmp1 = bitmap.lockbits(new rectangle(0, 0, 2560, 330), imagelockmode.readonly, bitmap.pixelformat);
            //intptr ptr = bmp1.scan0;

            //int bytes1 = math.abs(bmp1.stride) * bmp1.height;

            //byte[] regvalues1 = new byte[bytes1];
            //marshal.copy(ptr, regvalues1, 0, bytes1);
            int row = 0;
            for (int i = 2; i < bitmap.Length; i += 4)
            {
                if (bitmap[i] != safePic[i])
                {
                    if (bitmap[i] == 0 && bitmap[i - 1] == 0 && bitmap[i - 2] == 0)
                    {
                        row++;
                        continue;
                    }
                    else
                    {
                        if (row > 100)
                            return -1;
                    }
                    int sum = i / 4;
                    while (sum >= 2560)
                    {
                        sum -= 2560;
                    }
                    return (int)sum;
                }
            }
            return 0;
        }
    }
}