using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // use full form-size
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height;

            // show picture in first argument - else show openfile dialog
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2) OpenFile(args[1]);

            // quit when no file was selected
            else if (!OpenDialog()) Application.Exit();
        }

        private void DecodePic(int number)
        {
            // do not decode when cache is already filled
            if (picdata[number] == null)
            {
                Image picture = Bitmap.FromFile(filenames[number]);

                // only resize when larger as screen
                if (picture.Width > pictureBox1.Width || picture.Height > pictureBox1.Height)
                {
                    picdata[number] = ResizeImageFixedAR(picture, pictureBox1.Width, pictureBox1.Height);
                }

                else
                {
                    picdata[number] = picture;
                }
            }
        }

        private void ShowPic(int number)
        {
            // reset zoomsettings
            if (zoomfactor > 1.01)
            {
                zoomfactor = 1;
                ResetScreen();
            }

            if (picdata[number] == null) DecodePic(number);
            pictureBox1.Image = picdata[number];

            // Update titlebar & text in taskbar
            this.Text = "Peakture v" + this.ProductVersion + " - " + filenames[number];



        }

        private void ShowPicZoomed(int number)
        {
            Bitmap picture = (Bitmap)Bitmap.FromFile(filenames[number]);

            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            pictureBox1.Width = (int)(picture.Width * zoomfactor);
            pictureBox1.Height = (int)(picture.Height * zoomfactor);

            int screensize_x;
            int screensize_y;

            if (Fullscreen)
            {
                screensize_x = this.Width;
                screensize_y = this.Height;
            }
            else
            {
                screensize_x = RectangleToScreen(this.ClientRectangle).Width;
                screensize_y = RectangleToScreen(this.ClientRectangle).Height;
            }

            // move picturebox to display desired field of view
            pictureBox1.Location = new Point((int)(screensize_x / 2 - (picture.Width * zoomfactor / 2) + zoompos_x * zoomfactor), (int)(screensize_y / 2 - (picture.Height * zoomfactor / 2) + zoompos_y * zoomfactor));

            pictureBox1.Image = ResizeImageFixedAR(picture, pictureBox1.Width, pictureBox1.Height);

            // Update titlebar & text in taskbar
            this.Text = "Peakture v" + this.ProductVersion + " - " + filenames[number] + " [Zoomfactor=" + zoomfactor + "]";
        }

        private void ShowNextPic()
        {
            direction_down = true;
            if (actfile < (numfiles-1)) ShowPic(++actfile);
        }

        private void ShowPrevPic()
        {
            direction_down = false;
            if (actfile > 0) ShowPic(--actfile);
        }

        private void ShowFirstPic()
        {
            direction_down = true;
            actfile = 0;
            ShowPic(actfile);
        }

        private void ShowLastPic()
        {
            direction_down = false;
            actfile = numfiles - 1;
            ShowPic(actfile);
        }

        private void ResetScreen()
        {
            if (Fullscreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;

                // use full form-size for picbox
                pictureBox1.Width = this.Width;
                pictureBox1.Height = this.Height;
                pictureBox1.Location = new Point(0, 0);
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
                this.Width = 640;
                this.Height = 480;

                // use visible form-area for picbox
                pictureBox1.Width = RectangleToScreen(this.ClientRectangle).Width;
                pictureBox1.Height = RectangleToScreen(this.ClientRectangle).Height;
                pictureBox1.Location = new Point(0, 0);
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void ToggleFullscreen()
        {
            Fullscreen = !Fullscreen;
            ResetScreen();

            // Delete cache
            for (int i = 0; i < numfiles; i++)
            {
                picdata[i] = null;
            }
            
            // Update displayed image
            ShowPic(actfile);
        }

        private void ZoomIn()
        {
            // With first zoom-step center field of view
            if (zoomfactor > 0.99 && zoomfactor < 1.01)
            {
                zoompos_x = 0;
                zoompos_y = 0;
                zoomfactor = 1.5;
            }

            else if (zoomfactor > 1.49 && zoomfactor < 1.51) zoomfactor = 2.0;
            else if (zoomfactor > 1.99 && zoomfactor < 2.01) zoomfactor = 2.5;
            else if (zoomfactor > 2.49 && zoomfactor < 2.51) zoomfactor = 3.0;
            else if (zoomfactor > 2.99 && zoomfactor < 3.01) zoomfactor = 3.5;
            else if (zoomfactor > 3.49 && zoomfactor < 3.51) zoomfactor = 4.0;

            ShowPicZoomed(actfile);
        }

        private void ZoomOut()
        {
            if (zoomfactor > 3.99) zoomfactor = 3.5;
            else if (zoomfactor > 3.49 && zoomfactor < 3.51) zoomfactor = 3.0;
            else if (zoomfactor > 2.99 && zoomfactor < 3.01) zoomfactor = 2.5;
            else if (zoomfactor > 2.49 && zoomfactor < 2.51) zoomfactor = 2.0;
            else if (zoomfactor > 1.99 && zoomfactor < 2.01) zoomfactor = 1.5;
            else zoomfactor = 1.0;

            // are we zoomed?
            if (zoomfactor > 1.01) ShowPicZoomed(actfile);
            
            // no? then back to normal mode
            else
            {
                ResetScreen();
                ShowPic(actfile);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = RectangleToScreen(this.ClientRectangle).Width;
            int height = RectangleToScreen(this.ClientRectangle).Height;

            if (width > 0 && height > 0)
            {
                // update picbox-size to visible form-area
                pictureBox1.Width = width;
                pictureBox1.Height = height;

                // delete cache and redraw actual image
                for (int i=0;i<numfiles;i++) picdata[i] = null;
                if (filenames.Count > actfile) ShowPic(actfile);
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // Allow Drag&Drop
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
    }
}
