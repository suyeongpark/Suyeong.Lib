using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // double 형만

    public static partial class LinearAlgebraUtil
    {
        public static double Norm(double ax, double ay)
        {
            return Math.Sqrt(ax * ax + ay * ay);
        }

        public static double Norm(double ax, double ay, double az)
        {
            return Math.Sqrt(ax * ax + ay * ay + az * az);
        }

        public static double NormSqure(double ax, double ay)
        {
            return ax * ax + ay * ay;
        }

        public static double NormSqure(double ax, double ay, double az)
        {
            return ax * ax + ay * ay + az * az;
        }

        public static Tuple<double, double> Normalize(double ax, double ay)
        {
            double norm = Norm(ax: ax, ay: ay);

            return Tuple.Create(ax / norm, ay / norm);
        }

        public static Tuple<double, double, double> Normalize(double ax, double ay, double az)
        {
            double norm = Norm(ax: ax, ay: ay, az: az);

            return Tuple.Create(ax / norm, ay / norm, az / norm);
        }

        public static Tuple<double, double> Add(double ax, double ay, double bx, double by)
        {
            return Tuple.Create(ax + bx, ay + by);
        }

        public static Tuple<double, double, double> Add(double ax, double ay, double az, double bx, double by, double bz)
        {
            return Tuple.Create(ax + bx, ay + by, az + bz);
        }

        public static Tuple<double, double> Minus(double ax, double ay, double bx, double by)
        {
            return Tuple.Create(ax - bx, ay - by);
        }

        public static Tuple<double, double, double> Minus(double ax, double ay, double az, double bx, double by, double bz)
        {
            return Tuple.Create(ax - bx, ay - by, az - bz);
        }

        public static Tuple<double, double> Multiply(double ax, double ay, double k)
        {
            return Tuple.Create(ax * k, ay * k);
        }

        public static Tuple<double, double, double> Multiply(double ax, double ay, double az, double k)
        {
            return Tuple.Create(ax * k, ay * k, az * k);
        }

        public static double CosineTheta(double ax, double ay, double bx, double by)
        {
            double dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            double normSqureA = NormSqure(ax: ax, ay: ay);
            double normSqureB = NormSqure(ax: bx, ay: by);

            return (double)(dot * dot) / (double)(normSqureA * normSqureB);
        }

        public static double CosineTheta(double ax, double ay, double az, double bx, double by, double bz)
        {
            double dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            double normSqureA = NormSqure(ax: ax, ay: ay, az: az);
            double normSqureB = NormSqure(ax: bx, ay: by, az: bz);

            return (double)(dot * dot) / (double)(normSqureA * normSqureB);
        }

        public static double DotProduct(double ax, double ay, double bx, double by)
        {
            return ax * bx + ay * by;
        }

        public static double DotProduct(double ax, double ay, double az, double bx, double by, double bz)
        {
            return ax * bx + ay * by + az * bz;
        }

        public static Tuple<double, double, double> CrossProduct(double ax, double ay, double bx, double by)
        {
            return Tuple.Create(0d, 0d, ax * by - ay * bx);
        }

        public static Tuple<double, double, double> CrossProduct(double ax, double ay, double az, double bx, double by, double bz)
        {
            double i = ay * bz - az * by;
            double j = -(ax * bz - az * bx);
            double k = ax * by - ay * bx;

            return Tuple.Create(i, j, k);
        }

        // cross product에서 z가 0인 경우에 방향만 취하는 것
        public static double GetCCW(double ax, double ay, double bx, double by)
        {
            return ax * by - ay * bx;
        }

        public static bool IsVertical(double ax, double ay, double bx, double by)
        {
            return MathUtil.IsZero(DotProduct(ax: ax, ay: ay, bx: bx, by: by));
        }

        public static bool IsVertical(double ax, double ay, double az, double bx, double by, double bz)
        {
            return MathUtil.IsZero(DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz));
        }

        public static bool IsParallel(double ax, double ay, double bx, double by)
        {
            double dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            double normSquareA = NormSqure(ax: ax, ay: ay);
            double normSquareB = NormSqure(ax: bx, ay: by);

            return MathUtil.IsEqual(dot * dot, Math.Abs(normSquareA * normSquareB));
        }

        public static bool IsParallel(double ax, double ay, double az, double bx, double by, double bz)
        {
            double dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            double normSquareA = NormSqure(ax: ax, ay: ay, az: az);
            double normSquareB = NormSqure(ax: bx, ay: by, az: bz);

            return MathUtil.IsEqual(dot * dot, Math.Abs(normSquareA * normSquareB));
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

            return dotProduct * dotProduct == normSquare1 * normSquare2;
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

            double abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            double abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            double cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            double cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            // 1. 두 선분이 한 점에서 만나는 경우
            // 2. 두 선분이 교차하는 경우
            return (MathUtil.IsZero(abac * abad) && MathUtil.IsZero(cdca * cdcb)) ||
                ((abac > 0d && abad < 0d) || (abac < 0d && abad > 0d)) && ((cdca > 0d && cdcb < 0d) || (cdca < 0d && cdcb > 0d));
        }

        public static bool TryGetCrossPoint(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2, out double x, out double y)
        {
            x = y = 0;

            double x1 = bx1 - ax1;
            double y1 = by1 - ay1;

            double dx1 = ax2 - ax1;
            double dy1 = ay2 - ay1;

            double dx2 = bx2 - bx1;
            double dy2 = by2 - by1;

            double ccw1 = dx1 * dy2 - dy1 * dx2;

            if (MathUtil.IsEqual(ccw1, 0d))
            {
                return false;
            }

            double ccw2 = x1 * dy2 - y1 * dx2;

            double t = ccw2 / ccw1;

            x = ax1 + dx1 * t;
            y = ay1 + dy1 * t;

            return true;
        }
    }
}
