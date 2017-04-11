using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Windows.Forms;

namespace OpenCV
{
    public partial class Form1 : Form
    {
        string filename = @"..\..\..\Images\lena.jpg";
        double scaleFactor = 1.2;
        int neighbours = 4;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
            this.txtNeighbors.Text = neighbours.ToString();
            this.txtScaleFactor.Text = scaleFactor.ToString();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var diagImg = new OpenFileDialog();
            diagImg.ShowReadOnly = true;
            if (diagImg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = diagImg.FileName;
                this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
            }
        }

        private void btnDetectFaces_Click(object sender, EventArgs e)
        {
            var errText = ValidateInput();
            if (errText.Length > 0)
            {
                MessageBox.Show(errText);
                return;
            }

            this.lblCount.Text = "0";
            EnablePanel(false);

            #region face detection

            try
            {
                string xml_frontalfaceAltTree = @"..\..\..\..\haarcascades\haarcascade_frontalface_alt_tree.xml";
                //string xml_frontalface = @"..\..\..\..\haarcascades\haarcascade_frontalface_default.xml";
                //string xml_eye = @"..\..\..\..\haarcascades\haarcascade_eye.xml";

                var frame = new Mat(filename, ImreadModes.Color);

                DetectHaarCascadeInImage(ref frame, xml_frontalfaceAltTree, new OpenCvSharp.Size(25, 25), new OpenCvSharp.Size(300, 300));
                //DetectHaarCascadeInImage(ref frame, xml_eye, new OpenCvSharp.Size(40, 40), new OpenCvSharp.Size(100, 100));

                var bitmapFrame = BitmapConverter.ToBitmap(frame);
                pictureBox1.Image = bitmapFrame;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            #endregion

            EnablePanel(true);
        }

        private string ValidateInput()
        {
            var errText = "";
            double factor;
            int neigh;

            if (double.TryParse(this.txtScaleFactor.Text, out factor))
                scaleFactor = factor;
            else
                errText = "Invalid Scale Factor";

            if (int.TryParse(this.txtNeighbors.Text, out neigh))
                neighbours = neigh;
            else
                errText = "Invalid neighbours value.";

            return errText;
        }
        
        private void DetectHaarCascadeInImage(ref Mat frame, string xml_haarFile, OpenCvSharp.Size minBoyut, OpenCvSharp.Size maxBoyut)
        {
            var haarEyeCascade = new CascadeClassifier(xml_haarFile);
            Rect[] objRect = haarEyeCascade.DetectMultiScale(frame, scaleFactor, neighbours, 0, minBoyut, maxBoyut);

            if (objRect.Length > 0)
            {
                this.lblCount.Text = objRect.Length.ToString();
                for (int i = 0; i <= objRect.Length - 1; i++)
                {
                    int x1 = objRect[i].Location.X;
                    int y1 = objRect[i].Location.Y;
                    int x2 = objRect[i].Location.X + objRect[i].Width;
                    int y2 = objRect[i].Location.Y + objRect[i].Height;
                    OpenCvSharp.Point pt1 = new OpenCvSharp.Point(x1, y1);
                    OpenCvSharp.Point pt2 = new OpenCvSharp.Point(x2, y2);
                    Cv2.Rectangle(frame, pt1, pt2, new Scalar(0, 255, 255));
                }
            }
            
        }

        private void EnablePanel(bool isEnable)
        {
            this.panelMain.Enabled = isEnable;
            this.Cursor = isEnable ? Cursors.Arrow : Cursors.WaitCursor;
        }

        private void test()
        {
            //var _faceRecognizer = Cv2.CreateEigenFaceRecognizer();
            //_faceRecognizer.Train()
        }
    }
}
