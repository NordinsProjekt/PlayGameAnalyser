using PlayGameAnalyser.Handlers;
using PlayGameAnalyser.Interfaces;
using PlayGameAnalyser.Service;
using PlayGameAnalyser.Service.Extensions;

namespace PlayGameAnalyser
{
    public partial class Form1 : Form
    {
        private byte[] safePic { get; init; }
        private readonly DXBallAutoPlayer _service;
        public Form1()
        {
            InitializeComponent();
            IScreenshotService screenshotService = new ScreenshotService();
            Bitmap refPicture = new Bitmap($"381.png");
            safePic = refPicture.GenerateBitmapDataArray();
            _service = new DXBallAutoPlayer(screenshotService, safePic);
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
            MouseHandler.LeftMouseClick(new Point(0, 0));
            while (true)
            {
                Thread.Sleep(20);
                var result = _service.AnalyseGameScreen();
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


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}