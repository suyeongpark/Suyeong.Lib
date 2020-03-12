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

        public static bool IsCrossLine(int lineAStartX, int lineAStartY, int lineAEndX, int lineAEndY, int lineBStartX, int lineBStartY, int lineBEndX, int lineBEndY)
        {
            int abX = lineAEndX - lineAStartX;
            int abY = lineAEndY - lineAStartY;

            int acX = lineBStartX - lineAStartX;
            int acY = lineBStartY - lineAStartY;

            int adX = lineBEndX - lineAStartX;
            int adY = lineBEndY - lineAStartY;

            int cdX = lineBEndX - lineBStartX;
            int cdY = lineBEndY - lineBStartY;

            int caX = lineAStartX - lineBStartX;
            int caY = lineAStartY - lineBStartY;

            int cbX = lineAEndX - lineBStartX;
            int cbY = lineAEndY - lineBStartY;

            return GetCCW(ax: abX, ay: abY, bx: acX, by: acY, cx: adX, cy: adY) < 0 && GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY, cx: cbX, cy: cbY) < 0;
        }

        public static bool IsCrossLine(long lineAStartX, long lineAStartY, long lineAEndX, long lineAEndY, long lineBStartX, long lineBStartY, long lineBEndX, long lineBEndY)
        {
            long abX = lineAEndX - lineAStartX;
            long abY = lineAEndY - lineAStartY;

            long acX = lineBStartX - lineAStartX;
            long acY = lineBStartY - lineAStartY;

            long adX = lineBEndX - lineAStartX;
            long adY = lineBEndY - lineAStartY;

            long cdX = lineBEndX - lineBStartX;
            long cdY = lineBEndY - lineBStartY;

            long caX = lineAStartX - lineBStartX;
            long caY = lineAStartY - lineBStartY;

            long cbX = lineAEndX - lineBStartX;
            long cbY = lineAEndY - lineBStartY;

            return GetCCW(ax: abX, ay: abY, bx: acX, by: acY, cx: adX, cy: adY) < 0 && GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY, cx: cbX, cy: cbY) < 0;
        }

        public static bool IsCrossLine(float lineAStartX, float lineAStartY, float lineAEndX, float lineAEndY, float lineBStartX, float lineBStartY, float lineBEndX, float lineBEndY)
        {
            float abX = lineAEndX - lineAStartX;
            float abY = lineAEndY - lineAStartY;

            float acX = lineBStartX - lineAStartX;
            float acY = lineBStartY - lineAStartY;

            float adX = lineBEndX - lineAStartX;
            float adY = lineBEndY - lineAStartY;

            float cdX = lineBEndX - lineBStartX;
            float cdY = lineBEndY - lineBStartY;

            float caX = lineAStartX - lineBStartX;
            float caY = lineAStartY - lineBStartY;

            float cbX = lineAEndX - lineBStartX;
            float cbY = lineAEndY - lineBStartY;

            return GetCCW(ax: abX, ay: abY, bx: acX, by: acY, cx: adX, cy: adY) < 0 && GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY, cx: cbX, cy: cbY) < 0;
        }

        public static bool IsCrossLine(double lineAStartX, double lineAStartY, double lineAEndX, double lineAEndY, double lineBStartX, double lineBStartY, double lineBEndX, double lineBEndY)
        {
            double abX = lineAEndX - lineAStartX;
            double abY = lineAEndY - lineAStartY;

            double acX = lineBStartX - lineAStartX;
            double acY = lineBStartY - lineAStartY;

            double adX = lineBEndX - lineAStartX;
            double adY = lineBEndY - lineAStartY;

            double cdX = lineBEndX - lineBStartX;
            double cdY = lineBEndY - lineBStartY;

            double caX = lineAStartX - lineBStartX;
            double caY = lineAStartY - lineBStartY;

            double cbX = lineAEndX - lineBStartX;
            double cbY = lineAEndY - lineBStartY;

            return GetCCW(ax: abX, ay: abY, bx: acX, by: acY, cx: adX, cy: adY) < 0 && GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY, cx: cbX, cy: cbY) < 0;
        }

        public static int GetCCW(int ax, int ay, int bx, int by)
        {
            return ax * by - ay * bx;
        }

        public static int GetCCW(int ax, int ay, int bx, int by, int cx, int cy)
        {
            return ((bx - ax) * (cy - ay)) - ((cx - ax) * (by - ay));
        }

        public static long GetCCW(long ax, long ay, long bx, long by)
        {
            return ax * by - ay * bx;
        }

        public static long GetCCW(long ax, long ay, long bx, long by, long cx, long cy)
        {
            return ((bx - ax) * (cy - ay)) - ((cx - ax) * (by - ay));
        }

        public static float GetCCW(float ax, float ay, float bx, float by)
        {
            return ax * by - ay * bx;
        }

        public static float GetCCW(float ax, float ay, float bx, float by, float cx, float cy)
        {
            return ((bx - ax) * (cy - ay)) - ((cx - ax) * (by - ay));
        }

        public static double GetCCW(double ax, double ay, double bx, double by)
        {
            return ax * by - ay * bx;
        }

        public static double GetCCW(double ax, double ay, double bx, double by, double cx, double cy)
        {
            return ((bx - ax) * (cy - ay)) - ((cx - ax) * (by - ay));
        }
    }
}
