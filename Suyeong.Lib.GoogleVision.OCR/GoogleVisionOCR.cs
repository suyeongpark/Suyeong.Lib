using System.Collections.Generic;
using Google.Cloud.Vision.V1;
using Suyeong.Lib.Google.Type;

namespace Suyeong.Lib.GoogleVision.OCR
{
    public static class GoogleVisionOCR
    {
        public static GoogleOcrTextCollection DetectTextByGoogleVision(string imagePath)
        {
            GoogleOcrTextCollection texts = new GoogleOcrTextCollection();

            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            IReadOnlyList<EntityAnnotation> response = client.DetectText(Image.FromFile(imagePath));

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

                    texts.Add(new GoogleOcrText(
                        index: index, 
                        rotate: rotate, 
                        leftX: minX, 
                        rightX: maxX, 
                        topY: minY, 
                        bottomY: maxY, 
                        text: annotation.Description
                    ));
                }

                index++;
            }

            return texts;
        }

        public static GoogleOcrTextGroup DetectTextByGoogleVision(IEnumerable<string> imagePathes)
        {
            GoogleOcrTextGroup textGroup = new GoogleOcrTextGroup();

            ImageAnnotatorClient client = ImageAnnotatorClient.Create();

            GoogleOcrTextCollection texts;
            IReadOnlyList<EntityAnnotation> response;
            int minX, maxX, minY, maxY, centerX, centerY, rotate, imageIndex, pageIndex = 0;

            foreach (string imagePath in imagePathes)
            {
                response = client.DetectText(Image.FromFile(imagePath));

                texts = new GoogleOcrTextCollection();
                imageIndex = 0;

                foreach (EntityAnnotation annotation in response)
                {
                    if (imageIndex > 0)
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

                        texts.Add(new GoogleOcrText(
                            index: imageIndex,
                            rotate: rotate,
                            leftX: minX,
                            rightX: maxX,
                            topY: minY,
                            bottomY: maxY,
                            text: annotation.Description
                        ));
                    }

                    imageIndex++;
                }

                textGroup.Add(pageIndex++, texts);
            }

            return textGroup;
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
