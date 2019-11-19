﻿using Google.Cloud.Vision.V1;

namespace Suyeong.Lib.GoogleVision.OCR
{
    public static class GoogleVisionOCR
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

        public static OcrTexts DetectTextByGoogleVision(string imagePath)
        {
            OcrTexts textBlocks = new OcrTexts();

            var response = Client.DetectText(Image.FromFile(imagePath));

            int minX, maxX, minY, maxY, centerX, centerY, rotate, index = 0;

            foreach (EntityAnnotation annotation in response)
            {
                if (index > 0)
                {
                    minX = minY = int.MaxValue;
                    maxX = maxY = 0;

                    foreach (Vertex vertex in annotation.BoundingPoly.Vertices)
                    {
                        minX = vertex.X < minX ? vertex.X : minX;
                        maxX = vertex.X > maxX ? vertex.X : maxX;
                        minY = vertex.Y < minY ? vertex.Y : minY;
                        maxY = vertex.Y > maxY ? vertex.Y : maxY;
                    }

                    centerX = (int)((minX + maxX) * 0.5d);
                    centerY = (int)((minY + maxY) * 0.5d);
                    rotate = GetRotate(centerX: centerX, centerY: centerY, firstVertex: annotation.BoundingPoly.Vertices[0]);

                    textBlocks.Add(new OcrText(index: index, rotate: rotate, x: minX, y: minY, width: maxX - minX, height: maxY - minY, text: annotation.Description));
                }

                index++;
            }

            return textBlocks;
        }

        static int GetRotate(Vertex firstVertex, int centerX, int centerY)
        {
            if (firstVertex.X < centerX)
            {
                if (firstVertex.Y < centerY)
                {
                    return 0;
                }
                else
                {
                    return 270;
                }
            }
            else
            {
                if (firstVertex.Y < centerY)
                {
                    return 90;
                }
                else
                {
                    return 180;
                }
            }
        }
    }
}
