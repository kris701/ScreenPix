using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ScreenPixNameSpace._0
{
    public partial class MainForm : Form
    {
        #region Variabels

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap ImageWindow = new Bitmap( 50, 50, PixelFormat.Format32bppArgb);
        Point Pointer = new Point();
        Pen BlackPen = new Pen(Color.Black, 1);
        Graphics GFXScreenshot;
        Bitmap ScreenPixelColor = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            while (true)
            {
                ImageWindowPictureBox.Image = GenerateBitMap();

                var Color = GetColorAt(Cursor.Position);

                ColorLabel.Text =
                    "Red: " + Color.R.ToString() +
                    Environment.NewLine +
                    "Green: " + Color.G.ToString() +
                    Environment.NewLine +
                    "Blue: " + Color.B.ToString();
                PositionLabel.Text =
                    "X: " + Cursor.Position.X +
                    Environment.NewLine +
                    "Y: " + Cursor.Position.Y;

                ColorShowPanel.BackColor = Color.FromArgb(Color.R, Color.G, Color.B);

                await Task.Delay(10);
            }
        }

        Bitmap GenerateBitMap()
        {
            using (GFXScreenshot = Graphics.FromImage(ImageWindow))
            {
                if (Cursor.Position.X < 25)
                    Pointer.X = 0;
                else
                    if (Cursor.Position.X + 25 > SystemInformation.VirtualScreen.Width)
                        Pointer.X = SystemInformation.VirtualScreen.Width - 50;
                    else
                        Pointer.X = Cursor.Position.X - 25;
                if (Cursor.Position.Y < 25)
                    Pointer.Y = 0;
                else
                    if (Cursor.Position.Y + 25 > SystemInformation.VirtualScreen.Height)
                        Pointer.Y = SystemInformation.VirtualScreen.Height - 50;
                    else
                        Pointer.Y = Cursor.Position.Y - 25;

                GFXScreenshot.CopyFromScreen( Pointer.X, Pointer.Y, 0, 0, new Size(ImageWindow.Width, ImageWindow.Height), CopyPixelOperation.SourceCopy);

                GFXScreenshot.DrawRectangle(BlackPen, ImageWindow.Width / 2 - 1, ImageWindow.Height / 2 - 1, 2, 2);
            }
            GFXScreenshot.Dispose();
            return ImageWindow;
        }

        public Color GetColorAt(Point _Location)
        {
            using (Graphics GraphicsDestination = Graphics.FromImage(ScreenPixelColor))
            {
                using (Graphics GraphicsSource = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr ScreenDC = GraphicsSource.GetHdc();
                    IntPtr HDC = GraphicsDestination.GetHdc();
                    int retval = BitBlt(HDC, 0, 0, 1, 1, ScreenDC, _Location.X, _Location.Y, (int)CopyPixelOperation.SourceCopy);
                    GraphicsDestination.ReleaseHdc();
                    GraphicsSource.ReleaseHdc();
                }
            }

            return ScreenPixelColor.GetPixel(0, 0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
            }
            if (e.KeyCode == Keys.Down)
            {
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
            }
            if (e.KeyCode == Keys.Left)
            {
                Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
            }
            if (e.KeyCode == Keys.Right)
            {
                Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
            }
        }
    }
}
