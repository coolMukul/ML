using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Face;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVInterface
{
    public class Trainer
    {
        public Bitmap[] bitmapImages { get; set; }
        public int[] Ids { get; set; }
        public byte[][] imgBytes { get; set; }

        public void FaceTrainer(string trainedDataFile)
        {
            FaceRecognizer fr = Cv2.CreateLBPHFaceRecognizer();
            Mat[] matFaces = new Mat[imgBytes.Length];

            for (int counter = 0; counter < imgBytes.Length; counter++)
            {
                //var colorImg = BitmapConverter.ToMat(bitmapImages[counter]);


                //Mat colorMat = Mat.FromImageData(imgBytes[counter], ImreadModes.Color);
                Mat grayscaleMat = Mat.FromImageData(imgBytes[counter], ImreadModes.GrayScale);
                matFaces[counter] = grayscaleMat;

                //matFaces[counter].ConvertTo(matFaces[counter], 1);

                //Mat grayMat;
                //Cv2.CvtColor(colorImg, grayMat, ColorConversionCodes.BayerBG2GRAY, 1);
                ////Cv2.CvtColor(colorImg, Cv2.COLOR_BGR2GRAY);
                ////Cv2.Im
                //matFaces[counter] = grayMat;
            }

            fr.Train(matFaces, Ids);
            fr.Save(trainedDataFile);

            ////function def
            ////imagepath
            //Mat[] faces = null; // Faces array in gray scale which is Mat[]
            //int[] ids = null; //Id array

            ////for each img in path
            ////convert img in gray scale
            ////faceNP <- convert to Mat
            ////add to faces array
            //// add id to array
            ////Cv2.ImShow("training", faceNP); faceNP is MatName
            ////Cv2.WaitKey()
            ////return Ids and faces
            ////
            ////function def

            ////ids and FindFaces from function def
            //fr.Train(faces, ids);
            //fr.Save("path_of_recognizer");
            ////Cv2.DestroyAllWindows();
        }
    }
}
