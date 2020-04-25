using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // int 형만

    public static partial class LinearAlgebraUtil
    {
        public static void GetMinMax(int x1, int y1, int x2, int y2, out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = minY = maxX = maxY = 0;

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

        public static void GetMinMax(int x1, int y1, int z1, int x2, int y2, int z2, out int minX, out int minY, out int minZ, out int maxX, out int maxY, out int maxZ)
        {
            minX = minY = minZ = maxX = maxY = maxZ = 0;

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

        public static double Norm(int ax, int ay)
        {
            return Math.Sqrt(ax * ax + ay * ay);
        }

        public static double Norm(int ax, int ay, int az)
        {
            return Math.Sqrt(ax * ax + ay * ay + az * az);
        }

        public static int NormSqure(int ax, int ay)
        {
            return ax * ax + ay * ay;
        }

        public static int NormSqure(int ax, int ay, int az)
        {
            return ax * ax + ay * ay + az * az;
        }

        public static Tuple<double, double> Normalize(int ax, int ay)
        {
            double norm = Norm(ax: ax, ay: ay);

            return Tuple.Create(ax / norm, ay / norm);
        }

        public static Tuple<double, double, double> Normalize(int ax, int ay, int az)
        {
            double norm = Norm(ax: ax, ay: ay, az: az);

            return Tuple.Create(ax / norm, ay / norm, az / norm);
        }

        public static Tuple<int, int> Add(int ax, int ay, int bx, int by)
        {
            return Tuple.Create(ax + bx, ay + by);
        }

        public static Tuple<int, int, int> Add(int ax, int ay, int az, int bx, int by, int bz)
        {
            return Tuple.Create(ax + bx, ay + by, az + bz);
        }

        public static Tuple<int, int> Minus(int ax, int ay, int bx, int by)
        {
            return Tuple.Create(ax - bx, ay - by);
        }

        public static Tuple<int, int, int> Minus(int ax, int ay, int az, int bx, int by, int bz)
        {
            return Tuple.Create(ax - bx, ay - by, az - bz);
        }

        public static Tuple<int, int> Multiply(int ax, int ay, int k)
        {
            return Tuple.Create(ax * k, ay * k);
        }

        public static Tuple<int, int, int> Multiply(int ax, int ay, int az, int k)
        {
            return Tuple.Create(ax * k, ay * k, az * k);
        }

        public static double CosineTheta(int ax, int ay, int bx, int by)
        {
            int dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            int normSqureA = NormSqure(ax: ax, ay: ay);
            int normSqureB = NormSqure(ax: bx, ay: by);

            return (double)(dot * dot) / (double)(normSqureA * normSqureB);
        }

        public static double CosineTheta(int ax, int ay, int az, int bx, int by, int bz)
        {
            int dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            int normSqureA = NormSqure(ax: ax, ay: ay, az: az);
            int normSqureB = NormSqure(ax: bx, ay: by, az: bz);

            return (double)(dot * dot) / (double)(normSqureA * normSqureB);
        }

        public static int DotProduct(int ax, int ay, int bx, int by)
        {
            return ax * bx + ay * by;
        }

        public static int DotProduct(int ax, int ay, int az, int bx, int by, int bz)
        {
            return ax * bx + ay * by + az * bz;
        }

        public static Tuple<int, int, int> CrossProduct(int ax, int ay, int bx, int by)
        {
            return Tuple.Create(0, 0, ax * by - ay * bx);
        }

        public static Tuple<int, int, int> CrossProduct(int ax, int ay, int az, int bx, int by, int bz)
        {
            int i = ay * bz - az * by;
            int j = -(ax * bz - az * bx);
            int k = ax * by - ay * bx;

            return Tuple.Create(i, j, k);
        }

        // cross product에서 z가 0인 경우에 방향만 취하는 것
        public static int GetCCW(int ax, int ay, int bx, int by)
        {
            return ax * by - ay * bx;
        }

        public static bool IsVertical(int ax, int ay, int bx, int by)
        {
            return DotProduct(ax: ax, ay: ay, bx: bx, by: by) == 0;
        }

        public static bool IsVertical(int ax, int ay, int az, int bx, int by, int bz)
        {
            return DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz) == 0;
        }

        public static bool IsParallel(int ax, int ay, int bx, int by)
        {
            int dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            int normSquareA = NormSqure(ax: ax, ay: ay);
            int normSquareB = NormSqure(ax: bx, ay: by);

            return dot * dot == Math.Abs(normSquareA * normSquareB);
        }

        public static bool IsParallel(int ax, int ay, int az, int bx, int by, int bz)
        {
            int dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            int normSquareA = NormSqure(ax: ax, ay: ay, az: az);
            int normSquareB = NormSqure(ax: bx, ay: by, az: bz);

            return dot * dot == Math.Abs(normSquareA * normSquareB);
        }

        public static bool IsPointInLine(int lineX1, int lineY1, int lineX2, int lineY2, int x, int y)
        {
            int minX, minY, maxX, maxY;
            GetMinMax(x1: lineX1, y1: lineY1, x2: lineX2, y2: lineY2, minX: out minX, minY: out minY, maxX: out maxX, maxY: out maxY);

            // 점이 line의 boundary 안에 있는지 찾는다.
            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
            {
                int v1X = lineX2 - lineX1;
                int v1Y = lineY2 - lineY1;
                int v2X = x - lineX1;
                int v2Y = y - lineY1;

                // 점이 line의 boundary 안에 존재하는 상태에서 두 vector의 기울기가 평행하다면 점은 line 위에 존재한다.
                return GetCCW(ax: v1X, ay: v1Y, bx: v2X, by: v2Y) == 0;
            }

            return false;
        }

        public static bool IsCrossLine(int ax1, int ay1, int ax2, int ay2, int bx1, int by1, int bx2, int by2)
        {
            int aMinX, aMinY, aMaxX, aMaxY, bMinX, bMinY, bMaxX, bMaxY;
            GetMinMax(x1: ax1, y1: ay1, x2: ax2, y2: ay2, minX: out aMinX, minY: out aMinY, maxX: out aMaxX, maxY: out aMaxY);
            GetMinMax(x1: bx1, y1: by1, x2: bx2, y2: by2, minX: out bMinX, minY: out bMinY, maxX: out bMaxX, maxY: out bMaxY);

            // 일단 두 line의 boundary가 겹치는지 본다.
            if (aMinX <= bMaxX && aMaxX >= bMinX && aMinY <= bMinY && aMaxY >= bMaxY)
            {
                int abX = ax2 - ax1;
                int abY = ay2 - ay1;
                int acX = bx1 - ax1;
                int acY = by1 - ay1;
                int adX = bx2 - ax1;
                int adY = by2 - ay1;

                int abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
                int abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

                int cdX = bx2 - bx1;
                int cdY = by2 - by1;
                int caX = ax1 - bx1;
                int caY = ay1 - by1;
                int cbX = ax2 - bx1;
                int cbY = ay2 - by1;

                int cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
                int cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

                // 교차한 상태 - 두 부호가 반대
                if (MathUtil.IsNegative(abac, abad) && MathUtil.IsNegative(cdca, cdcb))
                {
                    return true;
                }
                // a라인 위에 b의 한 점이 존재
                else if (abac == 0 || abad == 0)
                {
                    return GetCCW(ax: abX, ay: abY, bx: acX, by: acY) == 0 || GetCCW(ax: abX, ay: abY, bx: adX, by: adY) == 0;
                }
                // b라인 위에 a의 한 점이 존재
                else if (cdca == 0 || cdcb == 0)
                {
                    return GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY) == 0 || GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY) == 0;
                }
            }

            return false;
        }

        /// 나눗셈 연산이 필요하므로 교점을 구하는 것 단순 교차 판정만 하는거라면 CCW를 이용한 Cross Line을 이용하자
        public static bool TryGetCrossPoint(int ax1, int ay1, int ax2, int ay2, int bx1, int by1, int bx2, int by2, out int x, out int y)
        {
            x = y = 0;

            // 두 선이 평행하면 false
            if (GetCCW(ax: ax2 - ax1, ay: ay2 - ay1, bx: bx2 - bx1, by: by2 - by1) == 0)
            {
                return false;
            }
            else
            {
                // 직선의 방정식을 만들어서 교점을 구한다.
                double a1 = (double)(ay2 - ay1) / (double)(ax2 - ax1);
                double b1 = ay1 - ax1 * a1;

                double a2 = (double)(by2 - by1) / (double)(bx2 - bx1);
                double b2 = by1 - bx1 * a2;

                x = (int)(-(b1 - b2) / (a1 - a2));
                y = (int)(a1 * x + b1);

                return true;
            }
        }
    }
}
