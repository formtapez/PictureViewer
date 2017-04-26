using System;
using System.Drawing;
using System.Windows.Forms;


/*

Normal key mapping
==================
Open file dialog:   O
Toggle fullscreen:  F
Quit:               Escape, Enter
Next Pic:           PageDown, Space, Down, Right, LeftMouseButton, MouseWheelDown
Prev Pic:           PageUp, Up, Left, RightMouseButton, MouseWheelUp
First Pic:          Home
Last Pic:           End
Zoom in:            +
Zoom out:           -

Key remapping while in zoom mode
=================================
Zoom in:            MouseWheelUp
Zoom out:           MouseWheelDown
Move image section: Up, Down, Left, Right, LeftMouseButton+Drag

*/

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.PageDown || e.KeyData == Keys.Space)
            {
                ShowNextPic();
            }

            else if (e.KeyData == Keys.PageUp)
            {
                ShowPrevPic();
            }

            else if (e.KeyData == Keys.Home)
            {
                ShowFirstPic();
            }

            else if (e.KeyData == Keys.End)
            {
                ShowLastPic();
            }

            else if (e.KeyData == Keys.F)
            {
                ToggleFullscreen();
            }

            else if (e.KeyData == Keys.Add)
            {
                ZoomIn();
            }

            else if (e.KeyData == Keys.Subtract)
            {
                ZoomOut();
            }

            if (e.KeyData == Keys.Down)
            {
                if (zoomfactor > 1.01)
                {
                    zoompos_y++;
                    pictureBox1.Location = new Point(pictureBox1.Left, (int)(pictureBox1.Top - 1.0 * zoomfactor));
                }
                else ShowNextPic();
            }

            else if (e.KeyData == Keys.Up)
            {
                if (zoomfactor > 1.01)
                {
                    zoompos_y--;
                    pictureBox1.Location = new Point(pictureBox1.Left, (int)(pictureBox1.Top + 1.0 * zoomfactor));
                }
                else ShowPrevPic();
            }

            if (e.KeyData == Keys.Left)
            {
                if (zoomfactor > 1.01)
                {
                    zoompos_x--;
                    pictureBox1.Location = new Point((int)(pictureBox1.Left + 1.0 * zoomfactor), pictureBox1.Top);
                }
                else ShowPrevPic();
            }

            else if (e.KeyData == Keys.Right)
            {
                if (zoomfactor > 1.01)
                {
                    zoompos_x++;
                    pictureBox1.Location = new Point((int)(pictureBox1.Left - 1.0 * zoomfactor), pictureBox1.Top);
                }
                else ShowNextPic();
            }

            else if (e.KeyData == Keys.Escape || e.KeyData == Keys.Enter)
            {
                Application.Exit();
            }

            // Open
            else if (e.KeyData == Keys.O)
            {
                OpenDialog();
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            // Mousewheel down
            if (e.Delta < 0)
            {
                if (zoomfactor > 1.01) ZoomOut();
                else ShowNextPic();
            }

            // Mousewheel up
            else
            {
                if (zoomfactor > 1.01) ZoomIn();
                else ShowPrevPic();
            }

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            // Cheat to make mousewheel working
            pictureBox1.Focus();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (zoomfactor > 1.01)
                {
                    Mousepos = e.Location;
                    Mousedown = true;
                }
                else ShowNextPic();
            }

            else if (e.Button == MouseButtons.Right)
            {
                ShowPrevPic();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mousedown)
            {
                int dx = e.X - Mousepos.X;
                int dy = e.Y - Mousepos.Y;
                pictureBox1.Location = new Point(pictureBox1.Left + dx, pictureBox1.Top + dy);

                if (zoomfactor > 1.01)
                {
                    zoompos_x += (int)(dx / zoomfactor);
                    zoompos_y += (int)(dy / zoomfactor);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Mousedown = false;
            }
        }
    }
}
