using Google.Cloud.Vision.V1;

namespace Suyeong.Lib.GCP.Vision
{
    public static class GoogleVision
    {
        static ImageAnnotatorClient _client;
        static ImageAnnotatorClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = ImageAnnotatorClient.Create();
                }

                return _client;
            }
        }

        public static VisionOcrResults DetectTexts(string imagePath)
        {
            VisionOcrResults results = new VisionOcrResults();

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = 0;
            int maxY = 0;
            int index = 0;

            foreach (EntityAnnotation annotation in Client.DetectText(Image.FromFile(imagePath)))
            {
                if (index > 0 && annotation.Description != null)
                {
                    foreach (Vertex vertex in annotation.BoundingPoly.Vertices)
                    {
                        minX = vertex.X < minX ? vertex.X : minX;
                        maxX = vertex.X > maxX ? vertex.X : maxX;
                        minY = vertex.Y < minY ? vertex.Y : minY;
                        maxY = vertex.Y > maxY ? vertex.Y : maxY;
                    }

                    results.Add(new VisionOcrResult(index: index, x: minX, y: minY, width: maxX - minX, height: maxY - minY, score: annotation.Score, text: annotation.Description));
                }

                index++;
            }

            return results;
        }
    }
}
