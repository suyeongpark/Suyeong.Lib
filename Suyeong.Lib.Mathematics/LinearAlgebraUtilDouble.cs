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

        // vector가 아니라 직선의 방정식을 만들어서 교점을 구한다. 이때 매개변수를 이용해서 방정식을 구성한다.
        // http://www.gisdeveloper.co.kr/?p=89
        public static bool TryGetCrossPoint(double lineAx1, double lineAy1, double lineAx2, double lineAy2, double lineBx1, double lineBy1, double lineBx2, double lineBy2, out double x, out double y)
        {
            x = y = 0d;

            double denominator = (lineBy2 - lineBy1) * (lineAx2 - lineAx1) - (lineBx2 - lineBx1) * (lineAy2 - lineAy1);

            // 두 선이 평행
            if (denominator == 0)
            {
                return false;
            }

            // T, S는 두 선에 대한 매개변수
            double t = (lineBx2 - lineBx1) * (lineAy1 - lineBy1) - (lineBy2 - lineBy1) * (lineAx1 - lineBx1);
            double s = (lineAx2 - lineAx1) * (lineAy1 - lineBy1) - (lineAy2 - lineAy1) * (lineAx1 - lineBx1);

            double td = (double)t / (double)denominator;
            double sd = (double)s / (double)denominator;

            // 두 선이 교차하지 않음
            if (td < 0d || td > 1d || sd < 0d || sd > 1d)
            {
                return false;
            }

            x = lineAx1 + t * (lineAx2 - lineAx1);
            y = lineAy1 + t * (lineAy2 - lineAy1);

            return true;
        }
    }
}
