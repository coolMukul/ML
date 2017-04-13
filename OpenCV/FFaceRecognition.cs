using System;
using System.Windows.Forms;
using OpenCVInterface;
using System.Drawing;

namespace OpenCV
{
    public partial class FFaceRecognition : Form
    {
        private MLEntities db = new MLEntities();
        string filename = @"C:\Users\mukul.varshney\Desktop\DotNet\FaceRecognition\Images\lena.jpg";
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
            this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
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
                this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);

                this.btnDetectFaces.PerformClick();
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
                IOpenCV obj;
                obj = new OpenCvEngine();
                var faceCount = obj.FindFaces(filename, GetHaarXMLFileName(), scaleFactor, neighbours, 25, 300, true);

                this.lblCount.Text = faceCount.ToString();
                ResetFacePanel(false);

                if (faceCount > 0)
                {
                    pictureBox1.Image = obj.ImageWithRect;
                    faceList = obj.GetFaces();
                    faceNames = new string[faceCount];
                    ResetFacePanel(true);
                    ShowFace();
                }      
                else
                    pictureBox1.Image = this.pictureBox1.Image = System.Drawing.Image.FromFile(filename);
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
        
        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
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
                    var userFaceInfo = new UserFaceInfo
                    {
                        faceSample = ImageToByte(faceList[counter]),
                        userId = 1,
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
    }
}
