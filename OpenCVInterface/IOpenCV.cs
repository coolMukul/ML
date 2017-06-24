using OpenCvSharp;
using System.Drawing;

namespace OpenCVInterface
{
    public interface IOpenCV
    {
        Rect[] FaceList { get; }
        Bitmap ImageWithRect { get; }
        int[] DetectedIds { get; }
        double[] ConfidenceOfIds { get; }

        Bitmap[] GetFaces();
        int FindFaces(string filename, string xml_haarFile, double scaleFactor, int neighbours, int minSquare, int maxSquare, bool isColorImage);
        int FindFaces(string trainedDataFile, string imgFilename, string xml_haarFile, double scaleFactor, int neighbours, int minSquare, int maxSquare, bool isColor);
     }
}
