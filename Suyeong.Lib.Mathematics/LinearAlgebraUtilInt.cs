using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // int 형만

    public static partial class LinearAlgebraUtil
    {
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
            int vec1X = x - lineX1;
            int vec1Y = y - lineY1;

            int vec2X = lineX2 - lineX1;
            int vec2Y = lineY2 - lineY1;

            int normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            int normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            int vecX = vec1X * vec2X;
            int vecY = vec1Y * vec2Y;

            // 곱하기를 하는게 간단하지만 정수형 타입상 숫자가 커져버리면 엉뚱한 숫자가 나올 수 있기 때문에 부호가 반대인지 확인
            if (MathUtil.IsNegative(vecX, vecY))
            {
                return false;
            }

            int dotProduct = vecX * vecY;

            return dotProduct * dotProduct == normSquare1 * normSquare2;
        }

        public static bool IsCrossLine(int ax1, int ay1, int ax2, int ay2, int bx1, int by1, int bx2, int by2)
        {
            int abX = ax2 - ax1;
            int abY = ay2 - ay1;

            int acX = bx1 - ax1;
            int acY = by1 - ay1;

            int adX = bx2 - ax1;
            int adY = by2 - ay1;

            int cdX = bx2 - bx1;
            int cdY = by2 - by1;

            int caX = ax1 - bx1;
            int caY = ay1 - by1;

            int cbX = ax2 - bx1;
            int cbY = ay2 - by1;

            int abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            int abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            int cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            int cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            // 두 선분이 평행한 상태 - 두 곱이 모두 0
            if (abac * abad == 0 && cdca * cdcb == 0)
            {
                // 한 선분의 끝 점이 다른 선분 내부에 존재하는지 판단한다. line의 기울기가 -인 경우 min/max 겹치는 것으로는 판정할 수 없음
                return IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx1, y: by1) ||
                    IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx2, y: by2) ||
                    IsPointInLine(lineX1: bx1, lineY1: by1, lineX2: bx2, lineY2: by2, x: ax1, y: ay1) ||
                    IsPointInLine(lineX1: bx1, lineY1: by1, lineX2: bx2, lineY2: by2, x: ax2, y: ay2);
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

        public static bool TryGetCrossPoint(int ax1, int ay1, int ax2, int ay2, int bx1, int by1, int bx2, int by2, out int x, out int y)
        {
            x = y = 0;

            int x1 = bx1 - ax1;
            int y1 = by1 - ay1;

            int vec1x = ax2 - ax1;
            int vec1y = ay2 - ay1;

            int vec2x = bx2 - bx1;
            int vec2y = by2 - by1;

            int ccw1 = vec1x * vec2y - vec1y * vec2x;

            if (ccw1 == 0)
            {
                return false;
            }

            int ccw2 = x1 * vec2y - y1 * vec2x;

            double t = (double)ccw2 / (double)ccw1;

            x = ax1 + (int)(vec1x * t);
            y = ay1 + (int)(vec1y * t);

            return true;
        }
    }
}
