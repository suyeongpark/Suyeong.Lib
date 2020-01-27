using System;

namespace Suyeong.Lib.Util
{
    public static class MathUtil
    {
        public static float Round(float Num, float Den)
        {
            return IsZero(Den) ? 0f : (float)Math.Round((double)Num / (double)Den) * 1000f * 0.001f;
        }
        public static double Round(double Num, double Den)
        {
            return IsZero(Den) ? 0f : Math.Round(Num / Den) * 1000d * 0.001d;
        }

        public static bool IsEqual(float val1, float val2)
        {
            return IsZero(num: val1 - val2);
        }

        public static bool IsEqual(double val1, double val2)
        {
            return IsZero(num: val1 - val2);
        }

        public static bool IsZero(float num)
        {
            return (num < float.Epsilon && num > -float.Epsilon);
        }

        public static bool IsZero(double num)
        {
            return (num < double.Epsilon && num > -double.Epsilon);
        }

        public static bool IsPointBetweenLine(int lineStartX, int lineStartY, int lineEndX, int lineEndY, int x, int y)
        {
            int lengthPointWithStart = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);
            int lengthLine = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);

            if (lengthPointWithStart > lengthLine)
            {
                return false;
            }

            int dotProduct = (x - lineStartX) * (lineEndX - lineStartX) + (y - lineStartY) * (lineEndY - lineStartY);

            if (dotProduct < 0)
            {
                return false;
            }

            int deltaX = lineEndX - lineStartX;
            int deltaY = lineEndY - lineStartY;

            int length = deltaX * deltaX + deltaY * deltaY;

            if (dotProduct > length)
            {
                return false;
            }

            return true;
        }

        public static bool IsPointBetweenLine(long lineStartX, long lineStartY, long lineEndX, long lineEndY, long x, long y)
        {
            long lengthPointWithStart = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);
            long lengthLine = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);

            if (lengthPointWithStart > lengthLine)
            {
                return false;
            }

            long dotProduct = (x - lineStartX) * (lineEndX - lineStartX) + (y - lineStartY) * (lineEndY - lineStartY);

            if (dotProduct < 0)
            {
                return false;
            }

            long deltaX = lineEndX - lineStartX;
            long deltaY = lineEndY - lineStartY;

            long length = deltaX * deltaX + deltaY * deltaY;

            if (dotProduct > length)
            {
                return false;
            }

            return true;
        }

        public static bool IsPointBetweenLine(float lineStartX, float lineStartY, float lineEndX, float lineEndY, float x, float y)
        {
            float lengthPointWithStart = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);
            float lengthLine = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);

            if (lengthPointWithStart > lengthLine)
            {
                return false;
            }

            float dotProduct = (x - lineStartX) * (lineEndX - lineStartX) + (y - lineStartY) * (lineEndY - lineStartY);

            if (dotProduct < 0)
            {
                return false;
            }

            float deltaX = lineEndX - lineStartX;
            float deltaY = lineEndY - lineStartY;

            float length = deltaX * deltaX + deltaY * deltaY;

            if (dotProduct > length)
            {
                return false;
            }

            return true;
        }

        public static bool IsPointBetweenLine(double lineStartX, double lineStartY, double lineEndX, double lineEndY, double x, double y)
        {
            double lengthPointWithStart = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);
            double lengthLine = (x - lineStartX) * (x - lineStartX) + (y - lineStartY) * (y - lineStartY);

            if (lengthPointWithStart > lengthLine)
            {
                return false;
            }

            double dotProduct = (x - lineStartX) * (lineEndX - lineStartX) + (y - lineStartY) * (lineEndY - lineStartY);

            if (dotProduct < 0)
            {
                return false;
            }

            double deltaX = lineEndX - lineStartX;
            double deltaY = lineEndY - lineStartY;

            double length = deltaX * deltaX + deltaY * deltaY;

            if (dotProduct > length)
            {
                return false;
            }

            return true;
        }

        public static double RadianToDegree(double radian)
        {
            return radian * 180d / Math.PI;
        }

        public static double DegreeToRadian(double angle)
        {
            return angle * Math.PI / 180d;
        }

        public static double PixelToPoint(double dpi, double pointPerInch, double value)
        {
            return value * pointPerInch / dpi;
        }

        public static double PointToPixel(double dpi, double pointPerInch, double value)
        {
            return value * dpi / pointPerInch;
        }
    }
}
