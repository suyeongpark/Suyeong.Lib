using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // float 형만

    public static partial class LinearAlgebraUtil
    {
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
            float v1X = lineX2 - lineX1;
            float v1Y = lineY2 - lineY1;

            float v2X = x - lineX1;
            float v2Y = y - lineY1;

            float v3X = x - lineX2;
            float v3Y = y - lineY2;

            float normSquare1 = v1X * v1X + v1Y * v1Y;
            float normSquare2 = v2X * v2X + v2Y * v2Y;
            float normSquare3 = v3X * v3X + v3Y * v3Y;

            if (normSquare2 > normSquare1 || normSquare3 > normSquare1)
            {
                return false;
            }

            float vecX = v2X * v1X;
            float vecY = v2Y * v1Y;

            // 곱하기를 하는게 간단하지만 정수형 타입상 숫자가 커져버리면 엉뚱한 숫자가 나올 수 있기 때문에 부호가 반대인지 확인
            if (MathUtil.IsNegative(vecX, vecY))
            {
                return false;
            }

            float dotProduct = vecX + vecY;

            return MathUtil.IsEqual(dotProduct * dotProduct, normSquare2 * normSquare1);
        }

        public static bool IsCrossLine(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2)
        {
            float abX = ax2 - ax1;
            float abY = ay2 - ay1;

            float acX = bx1 - ax1;
            float acY = by1 - ay1;

            float adX = bx2 - ax1;
            float adY = by2 - ay1;

            float cdX = bx2 - bx1;
            float cdY = by2 - by1;

            float caX = ax1 - bx1;
            float caY = ay1 - by1;

            float cbX = ax2 - bx1;
            float cbY = ay2 - by1;

            float abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            float abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            float cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            float cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            // 두 선분이 평행한 상태 - 두 곱이 모두 0
            if (MathUtil.IsZero(abac * abad) && MathUtil.IsZero(cdca * cdcb))
            {
                // 한 선분의 끝 점이 다른 선분 내부에 존재하는지 판단한다. line의 기울기가 -인 경우 min/max 겹치는 것으로는 판정할 수 없음
                return IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx1, y: by1) ||
                    IsPointInLine(lineX1: ax1, lineY1: ay1, lineX2: ax2, lineY2: ay2, x: bx2, y: by2);
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

        public static bool TryGetCrossPoint(float ax1, float ay1, float ax2, float ay2, float bx1, float by1, float bx2, float by2, out float x, out float y)
        {
            x = y = 0;

            float x1 = bx1 - ax1;
            float y1 = by1 - ay1;

            float vec1x = ax2 - ax1;
            float vec1y = ay2 - ay1;

            float vec2x = bx2 - bx1;
            float vec2y = by2 - by1;

            float ccw1 = vec1x * vec2y - vec1y * vec2x;

            if (MathUtil.IsEqual(ccw1, 0f))
            {
                return false;
            }

            float ccw2 = x1 * vec2y - y1 * vec2x;

            float t = (float)ccw2 / (float)ccw1;

            x = ax1 + vec1x * t;
            y = ay1 + vec1y * t;

            return true;
        }
    }
}
