﻿using System;
using System.Windows.Forms;
using OpenCVInterface;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;

namespace OpenCV
{
    public partial class FFaceRecognition : Form
    {
        private MLEntities db = new MLEntities();
        string filename = @"C:\Users\mukul.varshney\Desktop\DotNet\FaceRecognition\Images\Group2016.jpg";
        string fileTrainedData = @"C:\Users\mukul.varshney\Desktop\DotNet\FaceRecognition\trained.yml";
        double scaleFactor = 1.2;
        int neighbours = 4;
        string[] faceNames;
        int faceIndex = 0, prevFaceIndex = 0;
        Bitmap[] faceList;

        public FFaceRecognition()
        {
            InitializeComponent();
        }

        private void FFaceRecognition_Load(object sender, EventArgs e)
        {
            this.picMain.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picMain.Image = System.Drawing.Image.FromFile(filename);
            this.txtNeighbors.Text = neighbours.ToString();
            this.txtScaleFactor.Text = scaleFactor.ToString();

            //add filter to combobox
            this.cmbHaar.Items.Add("FrontFace");
            this.cmbHaar.Items.Add("FrontFaceAltTree");
            this.cmbHaar.Items.Add("FrontFaceAlt");
            this.cmbHaar.Items.Add("FrontFaceAlt2");
            this.cmbHaar.SelectedIndex = 0;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var diagImg = new OpenFileDialog();
            diagImg.ShowReadOnly = true;
            if (diagImg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = diagImg.FileName;
                this.picMain.Image = System.Drawing.Image.FromFile(filename);

                this.btnDetectFaces.PerformClick();
            }
        }

        private void btnDetectFaces_Click(object sender, EventArgs e)
        {
            var dbFaces = db.UserFaceInfoes.ToList();
            int[] allIds = new int[dbFaces.Count];

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
                IOpenCV obj;
                obj = new OpenCvEngine();
                var faceCount = obj.FindFaces(fileTrainedData, filename, GetHaarXMLFileName(), scaleFactor, neighbours, 25, 300, true);

                this.lblCount.Text = faceCount.ToString();
                ResetFacePanel(false);

                if (faceCount > 0)
                {
                    picMain.Image = obj.ImageWithRect;
                    faceList = obj.GetFaces();
                    faceNames = new string[faceCount];
                    ResetFacePanel(true);
                    ShowFace();
                }      
                else
                    this.picMain.Image = Image.FromFile(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            #endregion

            EnablePanel(true);
        }

        private void btnDetectFaces_Click_old(object sender, EventArgs e)
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
                IOpenCV obj;
                obj = new OpenCvEngine();
                var faceCount = obj.FindFaces(filename, GetHaarXMLFileName(), scaleFactor, neighbours, 25, 300, true);

                this.lblCount.Text = faceCount.ToString();
                ResetFacePanel(false);

                if (faceCount > 0)
                {
                    picMain.Image = obj.ImageWithRect;
                    faceList = obj.GetFaces();
                    faceNames = new string[faceCount];
                    ResetFacePanel(true);
                    ShowFace();
                }
                else
                    this.picMain.Image = Image.FromFile(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            #endregion

            EnablePanel(true);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            faceIndex--;
            ShowFace();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            faceIndex++;
            ShowFace();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveToDB();
        }

        private void txtFaceName_LostFocus(object sender, System.EventArgs e)
        {
            faceNames[prevFaceIndex] = this.txtFaceName.Text.Trim();
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

        private void EnablePanel(bool isEnable)
        {
            this.panelMain.Enabled = isEnable;
            this.Cursor = isEnable ? Cursors.Arrow : Cursors.WaitCursor;
        }

        private void ResetFacePanel(bool enable)
        {
            this.panelFace.Enabled = enable;
            this.picFace.Image = null;
            this.btnPrev.Enabled = enable;
            this.btnNext.Enabled = enable;
            this.btnSave.Enabled = enable;
            this.txtFaceName.Enabled = enable;
            this.lblPagination.Text = "0 / 0";

            if (!enable)
            {
                faceList = null;
                faceNames = null;
                faceIndex = 0;
                prevFaceIndex = 0;
            }              
        }

        private void ShowFace()
        {
            if (faceIndex < 0)
            {
                faceIndex = 0;
                return;
            }
            else if (faceIndex >= faceNames.Length)
            {
                faceIndex = faceNames.Length - 1;
                return;
            }
            
            this.btnPrev.Enabled = (faceIndex >= 1);
            this.btnNext.Enabled = (faceIndex <= faceNames.Length);
            this.lblPagination.Text = (faceIndex + 1).ToString() + " / " + faceNames.Length.ToString();
            this.txtFaceName.Text = faceNames[faceIndex];
            this.picFace.Image = faceList[faceIndex];
            prevFaceIndex = faceIndex;
        }
        
        private byte[] ImageToBytes(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private Image BytesToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }

        private void SaveToDB()
        {
            if (faceNames == null)
                return;

            bool doSave = false;
            for (int counter = 0; counter < faceNames.Length; counter++)
            {
                if (!string.IsNullOrEmpty(faceNames[counter]))
                {
                    //Test code -- so please add the names to the detector OpenCVInterface -> Detector.cs -> GetLableName()
                    //1. Bill Gates
                    //2. Steve Jobs
                    //3. Lata Mageshkar
                    //4. Sanjay Srinivasmurthy
                    var userFaceInfo = new UserFaceInfo
                    {
                        faceSample = ImageToBytes(faceList[counter]),
                        userId = 4,
                        username = faceNames[counter],
                        tag = ""
                    };

                    db.UserFaceInfoes.Add(userFaceInfo);
                    doSave = true;
                }                
            }

            if (doSave)
            {
                db.SaveChanges();
                MessageBox.Show("Saved the faces in DB.");
            }
        }

        private string GetHaarXMLFileName()
        {
            string path = @"C:\Users\mukul.varshney\Desktop\DotNet\FaceRecognition\FaceRecognition\haarcascades\";

            switch(this.cmbHaar.Text)
            {                
                case "FrontFaceAltTree":
                    return path + "haarcascade_frontalface_alt_tree.xml";
                case "FrontFaceAlt":
                    return path + "haarcascade_frontalface_alt.xml";
                case "FrontFaceAlt2":
                    return path + "haarcascade_frontalface_alt2.xml";
                default:
                    return path + "haarcascade_frontalface_default.xml";
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {      
            var dbFaces = db.UserFaceInfoes.ToList();
            //Bitmap[] allfaces = new Bitmap[dbFaces.Count];
            byte[][] byteFaces = new byte[dbFaces.Count][];
            int[] allIds = new int[dbFaces.Count];

            for (int counter = 0; counter < dbFaces.Count; counter++)
            {
                var item = db.UserFaceInfoes;
                //allfaces[counter] = new Bitmap(BytesToImage(dbFaces[counter].faceSample));
                byteFaces[counter] = dbFaces[counter].faceSample;
                allIds[counter] = dbFaces[counter].userId;
            }

            var trainer = new Trainer
            {
                //bitmapImages = allfaces,
                Ids = allIds,
                imgBytes = byteFaces
            };

            try
            {
                trainer.FaceTrainer(fileTrainedData);
                MessageBox.Show("Training done.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDetect_Click(object sender, EventArgs e)
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
                Detector dt = new Detector();
                //var bitmap = dt.Recognize(new Bitmap(this.pictureBox1.Image), fileTrainedData, GetHaarXMLFileName(), scaleFactor, neighbours, 25, 300);

                var bitmap = dt.Recognize(filename, fileTrainedData, GetHaarXMLFileName(), scaleFactor, neighbours, 25, 300);

                if (bitmap == null)
                    this.picMain.Image = Image.FromFile(filename);
                else
                    this.picMain.Image = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #endregion

            EnablePanel(true);
        }
    }
}
