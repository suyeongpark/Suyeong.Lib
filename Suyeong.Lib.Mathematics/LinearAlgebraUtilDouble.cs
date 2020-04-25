using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // double 형만

    public static partial class LinearAlgebraUtil
    {
        public static void GetMinMax(double x1, double y1, double x2, double y2, out double minX, out double minY, out double maxX, out double maxY)
        {
            minX = minY = maxX = maxY = 0d;

            // line의 boundary를 찾는다.
            if (x1 < x2)
            {
                minX = x1;
                maxX = x2;
            }
            else
            {
                minX = x2;
                maxX = x1;
            }

            if (y1 < y2)
            {
                minY = y1;
                maxY = y2;
            }
            else
            {
                minY = y2;
                maxY = y1;
            }
        }

        public static void GetMinMax(double x1, double y1, double z1, double x2, double y2, double z2, out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ)
        {
            minX = minY = minZ = maxX = maxY = maxZ = 0d;

            // line의 boundary를 찾는다.
            if (x1 < x2)
            {
                minX = x1;
                maxX = x2;
            }
            else
            {
                minX = x2;
                maxX = x1;
            }

            if (y1 < y2)
            {
                minY = y1;
                maxY = y2;
            }
            else
            {
                minY = y2;
                maxY = y1;
            }

            if (z1 < z2)
            {
                minZ = z1;
                maxZ = z2;
            }
            else
            {
                minZ = z2;
                maxZ = z1;
            }
        }

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

        public static bool IsPointInLine(double lineX1, double lineY1, double lineX2, double lineY2, double x, double y)
        {
            double minX, minY, maxX, maxY;
            GetMinMax(x1: lineX1, y1: lineY1, x2: lineX2, y2: lineY2, minX: out minX, minY: out minY, maxX: out maxX, maxY: out maxY);

            // 점이 line의 boundary 안에 있는지 찾는다.
            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
            {
                double v1X = lineX2 - lineX1;
                double v1Y = lineY2 - lineY1;
                double v2X = x - lineX1;
                double v2Y = y - lineY1;

                // 점이 line의 boundary 안에 존재하는 상태에서 두 vector의 기울기가 평행하다면 점은 line 위에 존재한다.
                return MathUtil.IsZero(GetCCW(ax: v1X, ay: v1Y, bx: v2X, by: v2Y));
            }

            return false;
        }

        public static bool IsCrossLine(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2)
        {
            double aMinX, aMinY, aMaxX, aMaxY, bMinX, bMinY, bMaxX, bMaxY;
            GetMinMax(x1: ax1, y1: ay1, x2: ax2, y2: ay2, minX: out aMinX, minY: out aMinY, maxX: out aMaxX, maxY: out aMaxY);
            GetMinMax(x1: bx1, y1: by1, x2: bx2, y2: by2, minX: out bMinX, minY: out bMinY, maxX: out bMaxX, maxY: out bMaxY);

            // 일단 두 line의 boundary가 겹치는지 본다.
            if (aMinX <= bMaxX && aMaxX >= bMinX && aMinY <= bMinY && aMaxY >= bMaxY)
            {
                double abX = ax2 - ax1;
                double abY = ay2 - ay1;
                double acX = bx1 - ax1;
                double acY = by1 - ay1;
                double adX = bx2 - ax1;
                double adY = by2 - ay1;

                double abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
                double abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

                double cdX = bx2 - bx1;
                double cdY = by2 - by1;
                double caX = ax1 - bx1;
                double caY = ay1 - by1;
                double cbX = ax2 - bx1;
                double cbY = ay2 - by1;
                
                double cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
                double cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

                // 교차한 상태 - 두 부호가 반대
                if (MathUtil.IsNegative(abac, abad) && MathUtil.IsNegative(cdca, cdcb))
                {
                    return true;
                }
                // a라인 위에 b의 한 점이 존재
                else if (MathUtil.IsZero(abac) || MathUtil.IsZero(abad))
                {
                    return MathUtil.IsZero(GetCCW(ax: abX, ay: abY, bx: acX, by: acY)) || MathUtil.IsZero(GetCCW(ax: abX, ay: abY, bx: adX, by: adY));
                }
                // b라인 위에 a의 한 점이 존재
                else if (MathUtil.IsZero(cdca) || MathUtil.IsZero(cdcb))
                {
                    return MathUtil.IsZero(GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY)) || MathUtil.IsZero(GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY));
                }
            }

            return false;
        }

        public static bool TryGetCrossPoint(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2, out double x, out double y)
        {
            x = y = 0d;

            //double x1 = bx1 - ax1;
            //double y1 = by1 - ay1;

            //double vec1x = ax2 - ax1;
            //double vec1y = ay2 - ay1;

            //double vec2x = bx2 - bx1;
            //double vec2y = by2 - by1;

            //double ccw1 = vec1x * vec2y - vec1y * vec2x;

            //if (MathUtil.IsEqual(ccw1, 0d))
            //{
            //    return false;
            //}

            //double ccw2 = x1 * vec2y - y1 * vec2x;

            //double t = ccw2 / ccw1;

            //x = ax1 + vec1x * t;
            //y = ay1 + vec1y * t;

            return true;
        }
    }
}
