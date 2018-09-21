using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading.Tasks;
using NDX_Base;

namespace ScreenPixNameSpace._0
{
    public partial class MainForm : Form
    {
        #region Variabels

        Bitmap ImageWindow = new Bitmap( 50, 50, PixelFormat.Format32bppArgb);
        Point Pointer = new Point();
        Pen BlackPen = new Pen(Color.Black, 1);
        Graphics GFXScreenshot;
        NDX CustomGraphics;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            CustomGraphics = new NDX(this);
            CustomGraphics.Sizable = false;
            CustomGraphics.DoubleClickToMaximize = false;
            CustomGraphics.AutoScroll = false;
            CustomGraphics.HasMaxButton = false;
            CustomGraphics.DragbarBackColor = Color.FromArgb(200,200,200);
            CustomGraphics.ParentFormDragStyle = NDXDragStyle.Opacity;
            CustomGraphics.TitleTextColor = Color.Black;
            CustomGraphics.InitializeNDX();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    Bitmap ScreenImage = GenerateBitMap();
                    ImageWindowPictureBox.Image = GenerateBitMap();

                    var Color = ScreenImage.GetPixel(ScreenImage.Width / 2, ScreenImage.Height / 2);

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
                catch
                {

                }
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
