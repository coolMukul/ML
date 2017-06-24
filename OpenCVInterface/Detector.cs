using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCVInterface
{
    public class Detector
    {

        public Bitmap Recognize(string imgFilename, string trainedDataFile, string xml_haarFile,
                                double scaleFactor, int neighbours, int minSquare, int maxSquare)
        {
            int predictedId = 0;
            double dblConfidence = 0.0;
            var recognizer = Cv2.CreateLBPHFaceRecognizer();
            recognizer.Load(trainedDataFile);
            var frameOriginal = new Mat(imgFilename, ImreadModes.AnyColor);
            var frameGray = new Mat(imgFilename, ImreadModes.GrayScale);
            var obj = new OpenCvEngine();
            var minBoyut = new OpenCvSharp.Size(minSquare, minSquare);
            var maxBoyut = new OpenCvSharp.Size(maxSquare, maxSquare);
            var haarCascade = new CascadeClassifier(xml_haarFile);
            var faceRectList = haarCascade.DetectMultiScale(frameGray, scaleFactor, neighbours, 0, minBoyut, maxBoyut);
            if (faceRectList.Length > 0)
            {
                for (int index = 0; index <= faceRectList.Length - 1; index++)
                {
                    int x1 = faceRectList[index].Location.X;
                    int y1 = faceRectList[index].Location.Y;
                    int x2 = faceRectList[index].Location.X + faceRectList[index].Width;
                    int y2 = faceRectList[index].Location.Y + faceRectList[index].Height;
                    recognizer.Predict(frameGray[faceRectList[index]], out predictedId, out dblConfidence);
                    if (predictedId > 0)
                    {
                        var labelName = GetLableName(predictedId);
                        var fnt = new HersheyFonts();
                        Cv2.PutText(frameOriginal, labelName, new OpenCvSharp.Point(x1, y1 + 10), fnt, 1, new Scalar(255, 0, 255));
                    }
                    else { }

                    OpenCvSharp.Point pt1 = new OpenCvSharp.Point(x1, y1);
                    OpenCvSharp.Point pt2 = new OpenCvSharp.Point(x2, y2);

                    Cv2.Rectangle(frameOriginal, pt1, pt2, new Scalar(0, 255, 255));
                }
                return BitmapConverter.ToBitmap(frameOriginal);
            }
            else
            {
                frameGray = null;
                frameOriginal = null;
                faceRectList = null;
                return null;
            }
        }

        public Bitmap Recognize(Bitmap bitmapImage, string trainedDataFile, string xml_haarFile, 
                                double scaleFactor, int neighbours, int minSquare, int maxSquare)
        {
            int predictedId = 0;
            double dblConfidence = 0.0;
            var recognizer = Cv2.CreateLBPHFaceRecognizer();
            recognizer.Load(trainedDataFile);
            var frameOriginal = BitmapConverter.ToMat(bitmapImage);
            Mat frameGray = frameOriginal;
            Cv2.CvtColor(frameOriginal, frameGray, ColorConversionCodes.BayerBG2GRAY);
            var obj = new OpenCvEngine();
            var minBoyut = new OpenCvSharp.Size(minSquare, minSquare);
            var maxBoyut = new OpenCvSharp.Size(maxSquare, maxSquare);
            var haarCascade = new CascadeClassifier(xml_haarFile);
            var faceRectList = haarCascade.DetectMultiScale(frameGray, scaleFactor, neighbours, 0, minBoyut, maxBoyut);
            if (faceRectList.Length > 0)
            {
                for (int index = 0; index <= faceRectList.Length - 1; index++)
                {                   
                    int x1 = faceRectList[index].Location.X;
                    int y1 = faceRectList[index].Location.Y;
                    int x2 = faceRectList[index].Location.X + faceRectList[index].Width;
                    int y2 = faceRectList[index].Location.Y + faceRectList[index].Height;
                    recognizer.Predict(frameGray[faceRectList[index]], out predictedId, out dblConfidence);
                    if (predictedId > 0)
                    {
                        var labelName = GetLableName(predictedId);
                        
                        var fnt = new HersheyFonts();
                        Cv2.PutText(frameOriginal, labelName, new OpenCvSharp.Point(x1, y1 + 10), fnt, 5, new Scalar(255, 0, 255));
                    }
                    else { }

                    OpenCvSharp.Point pt1 = new OpenCvSharp.Point(x1, y1);
                    OpenCvSharp.Point pt2 = new OpenCvSharp.Point(x2, y2);

                    Cv2.Rectangle(frameOriginal, pt1, pt2, new Scalar(0, 255, 255));
                }
                return BitmapConverter.ToBitmap(frameOriginal);
            }
            else
            {
                frameGray = null;
                frameOriginal = null;
                faceRectList = null;
                return null;
            }

            



            //var recognizer = Cv2.CreateLBPHFaceRecognizer();
            //recognizer.Load(trainedDataFile);
            //while (true)
            //{
            //    //read image
            //    //convert to gray
            //    //detect faces in gray
            //    //for each faces
            //    {
            //        //rec.Predict(InputArray, out predictedId, out dblConfidence);
            //        //if (predictedId == 1)
            //        //{
            //        //    labelText = " Bill Gates
            //        //}
            //    }
            //    //
            //}
        }

        private string GetLableName(int predictedId)
        {
            var labelName = "";
            switch (predictedId)
            {
                case 1:
                    labelName = "Bill Gates";
                    break;
                case 2:
                    labelName = "Steve Jobs";
                    break;
                case 3:
                    labelName = "Lata Mangeshkar";
                    break;
                case 4:
                    labelName = "Sanjay Srinivasmurthy";
                    break;
            }
            return labelName;
        }

    }
}
