using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        private Boolean OpenDialog()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFile(openFileDialog1.FileName);
                return true;
            }
            else return false;
        }

        private void OpenFile(string filepath)
        {
            // Delete old stuff
            filenames.Clear();
            picdata.Clear();
            numfiles = 0;
            actfile = 0;

            // get list of supported files from same directory of chosen file
            var fileArray = Directory.EnumerateFiles(Path.GetDirectoryName(filepath)).OrderBy(filename => filename).Where(s => 
                            s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase));

            // fill filenames array, save position of selected file
            numfiles = 0;
            foreach (string temp in fileArray)
            {
                filenames.Add(temp);
                picdata.Add(null);
                if (temp == filepath) actfile = numfiles;
                numfiles++;
            }

            ShowPic(actfile);

            // One-Time start of worker to prefetch images in background
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                timer1.Enabled = true;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            // open supported file when drag&dropped
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (FileList.Length >= 1 && (FileList[0].EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                        FileList[0].EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                        FileList[0].EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                        FileList[0].EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                                        FileList[0].EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                                        FileList[0].EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                                        FileList[0].EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)))
            {
                OpenFile(FileList[0]);
            }
        }
    }
}