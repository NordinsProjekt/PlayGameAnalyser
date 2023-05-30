using PlayGameAnalyser.Handlers;
using PlayGameAnalyser.Interfaces;
using PlayGameAnalyser.Records;
using PlayGameAnalyser.Service;
using PlayGameAnalyser.Service.Extensions;
using System.Drawing;

namespace PlayGameAnalyser
{
    public partial class Form1 : Form
    {
        private byte[] safePic { get; init; }
        private readonly DXBallAutoPlayer _service;
        IScreenshotService screenshotService = new ScreenshotService();
        public Form1()
        {
            InitializeComponent();
            
            Bitmap refPicture = new Bitmap($"381.png");
            safePic = refPicture.GenerateBitmapDataArray();
            _service = new DXBallAutoPlayer(safePic);
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
            Thread.Sleep(7000);
            label1.Text = "START!!!";
            label1.Invalidate();
            label1.Update();
            int lastPos = 0;
            while (true)
            {
                Thread.Sleep(20);
                var screen = screenshotService.GetBitmapDataAsByteArray(new CaptureArea(0, 1050, 330, 2560));
                var result = _service.AnalyseGameScreen(screen);

                if (result.X > 0) //Move Paddle and shoot if laser guns is on the paddle
                {
                    if (result.paddleGuns)
                        MouseHandler.LeftMouseClick(new Point(result.X, 50));
                    if (lastPos > result.X) MouseHandler.SetCursorPosition(result.X +25, 100);
                    if (lastPos < result.X ) MouseHandler.SetCursorPosition(result.X - 25, 100);
                    lastPos = result.X;
                }
                if (result.X == -1) //Game Over
                {
                    break;
                }

                if(result.X == -2) //Ball is magneticlocked on paddle
                {
                    MouseHandler.LeftMouseClick(new Point(lastPos, 50));
                }
            }
            label1.Text = "YOU LOST";
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}