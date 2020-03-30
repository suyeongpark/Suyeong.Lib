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

            return dotProduct * dotProduct == normSquare1 * normSquare2;
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

            float abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            float abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            float cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            float cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            // 1. 두 선분이 한 점에서 만나는 경우
            // 2. 두 선분이 교차하는 경우
            return (MathUtil.IsZero(abac * abad) && MathUtil.IsZero(cdca * cdcb)) ||
                ((abac > 0f && abad < 0f) || (abac < 0f && abad > 0f)) && ((cdca > 0f && cdcb < 0f) || (cdca < 0f && cdcb > 0f));
        }

        // vector가 아니라 직선의 방정식을 만들어서 교점을 구한다. 이때 매개변수를 이용해서 방정식을 구성한다.
        // http://www.gisdeveloper.co.kr/?p=89
        public static bool TryGetCrossPoint(float lineAx1, float lineAy1, float lineAx2, float lineAy2, float lineBx1, float lineBy1, float lineBx2, float lineBy2, out float x, out float y)
        {
            x = y = 0f;

            float denominator = (lineBy2 - lineBy1) * (lineAx2 - lineAx1) - (lineBx2 - lineBx1) * (lineAy2 - lineAy1);

            // 두 선이 평행
            if (denominator == 0)
            {
                return false;
            }

            // T, S는 두 선에 대한 매개변수
            float t = (lineBx2 - lineBx1) * (lineAy1 - lineBy1) - (lineBy2 - lineBy1) * (lineAx1 - lineBx1);
            float s = (lineAx2 - lineAx1) * (lineAy1 - lineBy1) - (lineAy2 - lineAy1) * (lineAx1 - lineBx1);

            float td = (float)t / (float)denominator;
            float sd = (float)s / (float)denominator;

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
