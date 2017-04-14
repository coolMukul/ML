using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp.Face;

namespace OpenCVInterface
{
    public class OpenCvEngine : IOpenCV
    {
        private Mat frameGray { get; set; }
        private Mat frameOriginal { get; set; }
        private Rect[] faceRectList { get; set; }
        private bool isColorImage { get; set; }
        public string fileName { get; set; }

        public int FindFaces(string imgFilename, string xml_haarFile, double scaleFactor, int neighbours, int minSquare, int maxSquare, bool isColor)
        {
            fileName = imgFilename;
            isColorImage = isColor;
            frameOriginal = new Mat(fileName, ImreadModes.AnyColor);
            frameGray = new Mat(fileName, ImreadModes.GrayScale);
            return DetectHaarCascadeInImage(xml_haarFile, scaleFactor, neighbours, minSquare, maxSquare);
        }

        public Bitmap[] GetFaces()
        {            
            if (faceRectList == null)
                return null;

            var frameImg = new Mat(fileName, ImreadModes.AnyColor);
            var faces = new Bitmap[faceRectList.Length];
            for (int index = 0; index <= faceRectList.Length - 1; index++)
            {
                faces[index] = BitmapConverter.ToBitmap(new Mat(frameImg, faceRectList[index]));
            }
            return faces;
        }

        public Rect[] FaceList
        {
            get
            {
                return faceRectList;
            }
        }

        public Bitmap ImageWithRect
        {
            get { 
                if (isColorImage)
                    return BitmapConverter.ToBitmap(frameOriginal);
                else
                    return BitmapConverter.ToBitmap(frameGray);
            }
        }

        private int DetectHaarCascadeInImage(string xml_haarFile, double scaleFactor, int neighbours, int minSquare, int maxSquare)
        {
            var minBoyut = new OpenCvSharp.Size(minSquare, minSquare);
            var maxBoyut = new OpenCvSharp.Size(maxSquare, maxSquare);
            var haarCascade = new CascadeClassifier(xml_haarFile);
            faceRectList = haarCascade.DetectMultiScale(frameGray, scaleFactor, neighbours, 0, minBoyut, maxBoyut);
            if (faceRectList.Length > 0)
            {                
                for (int index = 0; index <= faceRectList.Length - 1; index++)
                {
                    int x1 = faceRectList[index].Location.X;
                    int y1 = faceRectList[index].Location.Y;
                    int x2 = faceRectList[index].Location.X + faceRectList[index].Width;
                    int y2 = faceRectList[index].Location.Y + faceRectList[index].Height;
                    OpenCvSharp.Point pt1 = new OpenCvSharp.Point(x1, y1);
                    OpenCvSharp.Point pt2 = new OpenCvSharp.Point(x2, y2);

                    if (isColorImage)
                        Cv2.Rectangle(frameOriginal, pt1, pt2, new Scalar(0, 255, 255));
                    else
                        Cv2.Rectangle(frameGray, pt1, pt2, new Scalar(0, 255, 255));                    
                }
                return faceRectList.Length;
            }
            else
            {
                frameGray = null;
                frameOriginal = null;
                faceRectList = null;
                return 0;
            }
        }

    }
}
