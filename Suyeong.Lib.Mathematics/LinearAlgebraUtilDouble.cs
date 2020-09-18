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

        public static Tuple<double, double> Projection(double ax, double ay, double bx, double by)
        {
            double dotBA = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            double dotBB = DotProduct(ax: bx, ay: by, bx: bx, by: by);
            double scalar = dotBA / dotBB;

            return Tuple.Create(bx * scalar, by * scalar);
        }

        public static Tuple<double, double, double> Projection(double ax, double ay, double az, double bx, double by, double bz)
        {
            double dotBA = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            double dotBB = DotProduct(ax: bx, ay: by, az: bz, bx: bx, by: by, bz: bz);
            double scalar = dotBA / dotBB;

            return Tuple.Create(bx * scalar, by * scalar, bz * scalar);
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

        public static bool IsSameLine(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2)
        {
            return (MathUtil.IsEqual(ax1, bx1) && MathUtil.IsEqual(ay1, by1) && MathUtil.IsEqual(ax2, bx2) && MathUtil.IsEqual(ay2, by2)) ||
                (MathUtil.IsEqual(ax1, bx2) && MathUtil.IsEqual(ay1, by2) && MathUtil.IsEqual(ax2, bx1) && MathUtil.IsEqual(ay2, by1));
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
            if (IsSameLine(ax1: ax1, ay1: ay1, ax2: ax2, ay2: ay2, bx1: bx1, by1: by1, bx2: bx2, by2: by2))
            {
                return true;
            }

            double aMinX, aMinY, aMaxX, aMaxY, bMinX, bMinY, bMaxX, bMaxY;
            GetMinMax(x1: ax1, y1: ay1, x2: ax2, y2: ay2, minX: out aMinX, minY: out aMinY, maxX: out aMaxX, maxY: out aMaxY);
            GetMinMax(x1: bx1, y1: by1, x2: bx2, y2: by2, minX: out bMinX, minY: out bMinY, maxX: out bMaxX, maxY: out bMaxY);

            // 일단 두 line의 boundary가 겹치는지 본다.
            if (aMinX <= bMaxX && aMaxX >= bMinX && aMinY <= bMaxY && aMaxY >= bMinY)
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
                // a라인 위에 b의 c점이 존재
                else if (abac == 0)
                {
                    return IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx1, y: by1);
                }
                // a라인 위에 b의 d점이 존재
                else if (abad == 0)
                {
                    return IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx2, y: by2);
                }
                // b라인 위에 a의 a점이 존재
                else if (cdca == 0)
                {
                    return IsPointInLine(lineX1: bx1, lineY1: by1, lineX2: bx2, lineY2: by2, x: ax1, y: ay1);
                }
                // b라인 위에 a의 b점이 존재
                else if (cdcb == 0)
                {
                    return IsPointInLine(lineX1: bx1, lineY1: by1, lineX2: bx2, lineY2: by2, x: ax2, y: ay2);
                }
            }

            return false;
        }

        /// 나눗셈 연산이 필요하므로 교점을 구하는 것 단순 교차 판정만 하는거라면 CCW를 이용한 Cross Line을 이용하자
        public static bool TryGetCrossPoint(double ax1, double ay1, double ax2, double ay2, double bx1, double by1, double bx2, double by2, out double x, out double y)
        {
            x = y = 0d;

            // 두 직선이 동일한 경우
            if (IsSameLine(ax1: ax1, ay1: ay1, ax2: ax2, ay2: ay2, bx1: bx1, by1: by1, bx2: bx2, by2: by2))
            {
                // 중점을 반환한다.
                x = (ax1 + ax2) * 0.5d;
                y = (ay1 + ay2) * 0.5d;

                return true;
            }

            // 우선 교차하는지 본다.
            if (IsCrossLine(ax1: ax1, ay1: ay1, ax2: ax2, ay2: ay2, bx1: bx1, by1: by1, bx2: bx2, by2: by2))
            {
                double v1X = ax2 - ax1;
                double v1Y = ay2 - ay1;
                double v2X = bx2 - bx1;
                double v2Y = by2 - by1;

                bool horizontalA = MathUtil.IsZero(v1Y);
                bool verticalA = MathUtil.IsZero(v1X);
                bool horizontalB = MathUtil.IsZero(v2Y);
                bool verticalB = MathUtil.IsZero(v2X);

                // 두 라인이 수평-수직인 경우 수평의 y, 수직의 x를 쓴다.
                if (horizontalA && verticalB)
                {
                    x = bx1;
                    y = ay1;
                }
                else if (horizontalB && verticalA)
                {
                    x = ax1;
                    y = by1;
                }
                // 한쪽 선만 수평인 경우 수평의 y를 쓰고 x는 수평이 아닌 라인에 직선의 방정식을 쓴다.
                else if (horizontalA && !horizontalB)
                {
                    double a2 = v2Y / v2X;
                    double b2 = by1 - (a2 * bx1);

                    y = ay1;
                    x = (y - b2) / a2;
                }
                else if (!horizontalA && horizontalB)
                {
                    double a1 = v1Y / v1X;
                    double b1 = ay1 - (a1 * ax1);

                    y = by1;
                    x = (y - b1) / a1;
                }
                // 한쪽 선만 수직인 경우 수직의 x를 쓰고 y는 수직이 아닌 라인에 직선의 방정식을 쓴다.
                else if (verticalA && !verticalB)
                {
                    double a2 = v2Y / v2X;
                    double b2 = by1 - (a2 * bx1);

                    x = ax1;
                    y = a2 * x + b2;
                }
                else if (!verticalA && verticalB)
                {
                    double a1 = v1Y / v1X;
                    double b1 = ay1 - (a1 * ax1);

                    x = bx1;
                    y = a1 * x + b1;
                }
                else
                {
                    // 두 라인이 모두 수평/ 수직이 아닌 경우 두 라인 모두 직선의 방정식으로 푼다.
                    double a1 = v1Y / v1X;
                    double b1 = ay1 - (a1 * ax1);

                    double a2 = v2Y / v2X;
                    double b2 = by1 - (a2 * bx1);

                    x = -(b1 - b2) / (a1 - a2);
                    y = a1 * x + b1;
                }

                return true;
            }

            return false;
        }
    }
}
