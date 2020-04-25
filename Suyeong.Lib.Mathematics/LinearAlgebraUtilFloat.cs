using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // float 형만

    public static partial class LinearAlgebraUtil
    {
        public static void GetMinMax(float x1, float y1, float x2, float y2, out float minX, out float minY, out float maxX, out float maxY)
        {
            minX = minY = maxX = maxY = 0f;

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

        public static void GetMinMax(float x1, float y1, float z1, float x2, float y2, float z2, out float minX, out float minY, out float minZ, out float maxX, out float maxY, out float maxZ)
        {
            minX = minY = minZ = maxX = maxY = maxZ = 0f;

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
        public static float Norm(float ax, float ay)
        {
            return (float)Math.Sqrt(ax * ax + ay * ay);
        }

        public static float Norm(float ax, float ay, float az)
        {
            return (float)Math.Sqrt(ax * ax + ay * ay + az * az);
        }

        public static float NormSqure(float ax, float ay)
        {
            return ax * ax + ay * ay;
        }

        public static float NormSqure(float ax, float ay, float az)
        {
            return ax * ax + ay * ay + az * az;
        }

        public static Tuple<float, float> Normalize(float ax, float ay)
        {
            float norm = Norm(ax: ax, ay: ay);

            return Tuple.Create(ax / norm, ay / norm);
        }

        public static Tuple<float, float, float> Normalize(float ax, float ay, float az)
        {
            float norm = Norm(ax: ax, ay: ay, az: az);

            return Tuple.Create(ax / norm, ay / norm, az / norm);
        }

        public static Tuple<float, float> Add(float ax, float ay, float bx, float by)
        {
            return Tuple.Create(ax + bx, ay + by);
        }

        public static Tuple<float, float, float> Add(float ax, float ay, float az, float bx, float by, float bz)
        {
            return Tuple.Create(ax + bx, ay + by, az + bz);
        }

        public static Tuple<float, float> Minus(float ax, float ay, float bx, float by)
        {
            return Tuple.Create(ax - bx, ay - by);
        }

        public static Tuple<float, float, float> Minus(float ax, float ay, float az, float bx, float by, float bz)
        {
            return Tuple.Create(ax - bx, ay - by, az - bz);
        }

        public static Tuple<float, float> Multiply(float ax, float ay, float k)
        {
            return Tuple.Create(ax * k, ay * k);
        }

        public static Tuple<float, float, float> Multiply(float ax, float ay, float az, float k)
        {
            return Tuple.Create(ax * k, ay * k, az * k);
        }

        public static float CosineTheta(float ax, float ay, float bx, float by)
        {
            float dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            float normSqureA = NormSqure(ax: ax, ay: ay);
            float normSqureB = NormSqure(ax: bx, ay: by);

            return (float)(dot * dot) / (float)(normSqureA * normSqureB);
        }

        public static float CosineTheta(float ax, float ay, float az, float bx, float by, float bz)
        {
            float dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            float normSqureA = NormSqure(ax: ax, ay: ay, az: az);
            float normSqureB = NormSqure(ax: bx, ay: by, az: bz);

            return (float)(dot * dot) / (float)(normSqureA * normSqureB);
        }

        public static float DotProduct(float ax, float ay, float bx, float by)
        {
            return ax * bx + ay * by;
        }

        public static float DotProduct(float ax, float ay, float az, float bx, float by, float bz)
        {
            return ax * bx + ay * by + az * bz;
        }

        public static Tuple<float, float, float> CrossProduct(float ax, float ay, float bx, float by)
        {
            return Tuple.Create(0f, 0f, ax * by - ay * bx);
        }

        public static Tuple<float, float, float> CrossProduct(float ax, float ay, float az, float bx, float by, float bz)
        {
            float i = ay * bz - az * by;
            float j = -(ax * bz - az * bx);
            float k = ax * by - ay * bx;

            return Tuple.Create(i, j, k);
        }

        // cross product에서 z가 0인 경우에 방향만 취하는 것
        public static float GetCCW(float ax, float ay, float bx, float by)
        {
            return ax * by - ay * bx;
        }

        public static bool IsVertical(float ax, float ay, float bx, float by)
        {
            return MathUtil.IsZero(DotProduct(ax: ax, ay: ay, bx: bx, by: by));
        }

        public static bool IsVertical(float ax, float ay, float az, float bx, float by, float bz)
        {
            return MathUtil.IsZero(DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz));
        }

        public static bool IsParallel(float ax, float ay, float bx, float by)
        {
            float dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            float normSquareA = NormSqure(ax: ax, ay: ay);
            float normSquareB = NormSqure(ax: bx, ay: by);

            return MathUtil.IsEqual(dot * dot, Math.Abs(normSquareA * normSquareB));
        }

        public static bool IsParallel(float ax, float ay, float az, float bx, float by, float bz)
        {
            float dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            float normSquareA = NormSqure(ax: ax, ay: ay, az: az);
            float normSquareB = NormSqure(ax: bx, ay: by, az: bz);

            return MathUtil.IsEqual(dot * dot, Math.Abs(normSquareA * normSquareB));
        }

        public static bool IsPointInLine(float lineX1, float lineY1, float lineX2, float lineY2, float x, float y)
        {
            float minX, minY, maxX, maxY;
            GetMinMax(x1: lineX1, y1: lineY1, x2: lineX2, y2: lineY2, minX: out minX, minY: out minY, maxX: out maxX, maxY: out maxY);

            // 점이 line의 boundary 안에 있는지 찾는다.
            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
            {
                float v1X = lineX2 - lineX1;
                float v1Y = lineY2 - lineY1;
                float v2X = x - lineX1;
                float v2Y = y - lineY1;

                // 점이 line의 boundary 안에 존재하는 상태에서 두 vector의 기울기가 평행하다면 점은 line 위에 존재한다.
                return MathUtil.IsZero(GetCCW(ax: v1X, ay: v1Y, bx: v2X, by: v2Y));
            }

            return false;
        }

        public static bool IsCrossLine(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2)
        {
            float aMinX, aMinY, aMaxX, aMaxY, bMinX, bMinY, bMaxX, bMaxY;
            GetMinMax(x1: ax1, y1: ay1, x2: ax2, y2: ay2, minX: out aMinX, minY: out aMinY, maxX: out aMaxX, maxY: out aMaxY);
            GetMinMax(x1: bx1, y1: by1, x2: bx2, y2: by2, minX: out bMinX, minY: out bMinY, maxX: out bMaxX, maxY: out bMaxY);

            // 일단 두 line의 boundary가 겹치는지 본다.
            if (aMinX <= bMaxX && aMaxX >= bMinX && aMinY <= bMinY && aMaxY >= bMaxY)
            {
                float abX = ax2 - ax1;
                float abY = ay2 - ay1;
                float acX = bx1 - ax1;
                float acY = by1 - ay1;
                float adX = bx2 - ax1;
                float adY = by2 - ay1;

                float abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
                float abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

                float cdX = bx2 - bx1;
                float cdY = by2 - by1;
                float caX = ax1 - bx1;
                float caY = ay1 - by1;
                float cbX = ax2 - bx1;
                float cbY = ay2 - by1;

                float cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
                float cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

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

        /// 나눗셈 연산이 필요하므로 교점을 구하는 것 단순 교차 판정만 하는거라면 CCW를 이용한 Cross Line을 이용하자
        public static bool TryGetCrossPoint(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2, out float x, out float y)
        {
            x = y = 0f;

            // 두 선이 평행하면 false
            if (MathUtil.IsZero(GetCCW(ax: ax2 - ax1, ay: ay2 - ay1, bx: bx2 - bx1, by: by2 - by1)))
            {
                return false;
            }
            else
            {
                // 직선의 방정식을 만들어서 교점을 구한다.
                float a1 = (ay2 - ay1) / (ax2 - ax1);
                float b1 = ay1 - ax1 * a1;

                float a2 = (by2 - by1) / (bx2 - bx1);
                float b2 = by1 - bx1 * a2;

                x = -(b1 - b2) / (a1 - a2);
                y = a1 * x + b1;

                return true;
            }
        }
}
