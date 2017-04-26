using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        // number of images hold in cache each before and after actual position
        static int cachefiles = 100;

        static int numfiles = 0;
        static int actfile = 0;
        static List<string> filenames = new List<string>();
        static List<Image> picdata = new List<Image>();

        static Boolean direction_down = true;
        static Boolean idle = true;
        static Boolean Fullscreen = true;

        static Boolean Mousedown = false;
        static Point Mousepos = new Point(0, 0);

        static double zoomfactor = 1.0;
        static int zoompos_x = 0;
        static int zoompos_y = 0;
    }
}
