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
        public int[] Ids { get; set; }
        public byte[][] imgBytes { get; set; }

        public void FaceTrainer(string trainedDataFile)
        {
            FaceRecognizer fr = Cv2.CreateLBPHFaceRecognizer();
            Mat[] matFaces = new Mat[imgBytes.Length];

            for (int counter = 0; counter < imgBytes.Length; counter++)
            {
                Mat grayscaleMat = Mat.FromImageData(imgBytes[counter], ImreadModes.GrayScale);
                matFaces[counter] = grayscaleMat;
            }

            fr.Train(matFaces, Ids);
            fr.Save(trainedDataFile);
        }
    }
}
