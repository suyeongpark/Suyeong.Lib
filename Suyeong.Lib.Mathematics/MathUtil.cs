using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
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
            return Math.Abs(num) < float.Epsilon;
        }

        public static bool IsZero(double num)
        {
            return Math.Abs(num) < double.Epsilon;
        }

        public static bool IsPointInLine(int lineStartX, int lineStartY, int lineEndX, int lineEndY, int x, int y)
        {
            int vec1X = x - lineStartX;
            int vec1Y = y - lineStartY;

            int vec2X = lineEndX - lineStartX;
            int vec2Y = lineEndY - lineStartY;

            int normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            int normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            int dotProduct = vec1X * vec2X + vec1Y * vec2Y;

            if (dotProduct < 0)
            {
                return false;
            }

            return dotProduct * dotProduct == normSquare1 * normSquare2;
        }

        public static bool IsPointInLine(long lineStartX, long lineStartY, long lineEndX, long lineEndY, long x, long y)
        {
            long vec1X = x - lineStartX;
            long vec1Y = y - lineStartY;

            long vec2X = lineEndX - lineStartX;
            long vec2Y = lineEndY - lineStartY;

            long normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            long normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            long dotProduct = vec1X * vec2X + vec1Y * vec2Y;

            if (dotProduct < 0)
            {
                return false;
            }

            return dotProduct * dotProduct == normSquare1 * normSquare2;
        }

        public static bool IsPointInLine(float lineStartX, float lineStartY, float lineEndX, float lineEndY, float x, float y)
        {
            float vec1X = x - lineStartX;
            float vec1Y = y - lineStartY;

            float vec2X = lineEndX - lineStartX;
            float vec2Y = lineEndY - lineStartY;

            float normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            float normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            float dotProduct = vec1X * vec2X + vec1Y * vec2Y;

            if (dotProduct < 0)
            {
                return false;
            }

            return IsEqual(dotProduct * dotProduct, normSquare1 * normSquare2);
        }

        public static bool IsPointInLine(double lineStartX, double lineStartY, double lineEndX, double lineEndY, double x, double y)
        {
            double vec1X = x - lineStartX;
            double vec1Y = y - lineStartY;

            double vec2X = lineEndX - lineStartX;
            double vec2Y = lineEndY - lineStartY;

            double normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            double normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            double dotProduct = vec1X * vec2X + vec1Y * vec2Y;

            if (dotProduct < 0)
            {
                return false;
            }

            return IsEqual(dotProduct * dotProduct, normSquare1 * normSquare2);
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
