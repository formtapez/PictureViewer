using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Fill picture-cache all the time. The maximum number of cached pictures before AND after the actual position is defined in "cachefiles".
            while (true)
            {
                idle = true;

                // Depending on the actual viewing-direction, make sure the _next_ picture will be processed first.
                if (direction_down)
                {
                    // first fill cache in actual direction
                    for (int i = actfile; i < (actfile + cachefiles) && i < (numfiles - 1); i++)
                    {
                        if (picdata[i] == null)
                        {
                            idle = false;
                            DecodePic(i);
                        }
                    }

                    // second fill cache in other direction
                    for (int i = actfile; i > (actfile - cachefiles) && i > 0; i--)
                    {
                        if (picdata[i] == null)
                        {
                            idle = false;
                            DecodePic(i);
                        }
                    }
                }

                // direction: up
                else
                {
                    // first fill cache in actual direction
                    for (int i = actfile; i > (actfile - cachefiles) && i > 0; i--)
                    {
                        if (picdata[i] == null)
                        {
                            idle = false;
                            DecodePic(i);
                        }
                    }

                    // second fill cache in other direction
                    for (int i = actfile; i < (actfile + cachefiles) && i < (numfiles - 1); i++)
                    {
                        if (picdata[i] == null)
                        {
                            idle = false;
                            DecodePic(i);
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Nothing to do? -> Garbage collection!
            if (idle)
            {
                // always hold first & last pic in cache
                DecodePic(0);
                DecodePic(numfiles - 1);

                // delete cache for pics out of cache-range (except first & last)
                for (int i = 1; i < (numfiles - 1); i++)
                {
                    if (i > (actfile + cachefiles) || i < (actfile - cachefiles)) picdata[i] = null;
                }

                // let the system free up unused memory
                GC.Collect();
            }
        }
    }
}
