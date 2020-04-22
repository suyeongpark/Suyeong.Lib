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

        public static bool IsPointInLine(double lineX1, double lineY1, double lineX2, double lineY2, double x, double y)
        {
            double v1X = lineX2 - lineX1;
            double v1Y = lineY2 - lineY1;

            double v2X = x - lineX1;
            double v2Y = y - lineY1;

            double v3X = x - lineX2;
            double v3Y = y - lineY2;

            double normSquare1 = v1X * v1X + v1Y * v1Y;
            double normSquare2 = v2X * v2X + v2Y * v2Y;
            double normSquare3 = v3X * v3X + v3Y * v3Y;

            if (normSquare2 > normSquare1 || normSquare3 > normSquare1)
            {
                return false;
            }

            double vecX = v2X * v1X;
            double vecY = v2Y * v1Y;

            // 곱하기를 하는게 간단하지만 정수형 타입상 숫자가 커져버리면 엉뚱한 숫자가 나올 수 있기 때문에 부호가 반대인지 확인
            if (MathUtil.IsNegative(vecX, vecY))
            {
                return false;
            }

            double dotProduct = vecX + vecY;

            return MathUtil.IsEqual(dotProduct * dotProduct, normSquare2 * normSquare1);
        }

        public static bool IsCrossLine(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2)
        {
            double abX = ax2 - ax1;
            double abY = ay2 - ay1;

            double acX = bx1 - ax1;
            double acY = by1 - ay1;

            double adX = bx2 - ax1;
            double adY = by2 - ay1;

            double cdX = bx2 - bx1;
            double cdY = by2 - by1;

            double caX = ax1 - bx1;
            double caY = ay1 - by1;

            double cbX = ax2 - bx1;
            double cbY = ay2 - by1;

            double abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            double abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            double cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            double cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            // 두 선분이 평행한 상태 - 두 곱이 모두 0
            if (MathUtil.IsZero(abac * abad) && MathUtil.IsZero(cdca * cdcb))
            {
                // 한 선분의 끝 점이 다른 선분 내부에 존재하는지 판단한다. line의 기울기가 -인 경우 min/max 겹치는 것으로는 판정할 수 없음
                return IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx1, y: by1) ||
                    IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx2, y: by2);
            }
            // 한 선의 끝점이 다른 선의 위에 존재
            else if (MathUtil.IsZero(abac * abad) || MathUtil.IsZero(cdca * cdcb))
            {
                return true;
            }
            // 교차한 상태 - 두 부호가 반대
            else if (MathUtil.IsNegative(abac, abad) && MathUtil.IsNegative(cdca, cdcb))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryGetCrossPoint(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2, out double x, out double y)
        {
            x = y = 0;

            double x1 = bx1 - ax1;
            double y1 = by1 - ay1;

            double vec1x = ax2 - ax1;
            double vec1y = ay2 - ay1;

            double vec2x = bx2 - bx1;
            double vec2y = by2 - by1;

            double ccw1 = vec1x * vec2y - vec1y * vec2x;

            if (MathUtil.IsEqual(ccw1, 0d))
            {
                return false;
            }

            double ccw2 = x1 * vec2y - y1 * vec2x;

            double t = ccw2 / ccw1;

            x = ax1 + vec1x * t;
            y = ay1 + vec1y * t;

            return true;
        }
    }
}
