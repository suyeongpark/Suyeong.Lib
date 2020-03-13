using System;
using System.Numerics;

namespace Suyeong.Lib.Mathematics
{
    // long 형만

    public static partial class LinearAlgebraUtil
    {
        public static double Norm(long ax, long ay)
        {
            return Math.Sqrt(ax * ax + ay * ay);
        }

        public static double Norm(long ax, long ay, long az)
        {
            return Math.Sqrt(ax * ax + ay * ay + az * az);
        }

        public static long NormSqure(long ax, long ay)
        {
            return ax * ax + ay * ay;
        }

        public static long NormSqure(long ax, long ay, long az)
        {
            return ax * ax + ay * ay + az * az;
        }

        public static Tuple<double, double> Normalize(long ax, long ay)
        {
            double norm = Norm(ax: ax, ay: ay);

            return Tuple.Create(ax / norm, ay / norm);
        }

        public static Tuple<double, double, double> Normalize(long ax, long ay, long az)
        {
            double norm = Norm(ax: ax, ay: ay, az: az);

            return Tuple.Create(ax / norm, ay / norm, az / norm);
        }

        public static Tuple<long, long> Add(long ax, long ay, long bx, long by)
        {
            return Tuple.Create(ax + bx, ay + by);
        }

        public static Tuple<long, long, long> Add(long ax, long ay, long az, long bx, long by, long bz)
        {
            return Tuple.Create(ax + bx, ay + by, az + bz);
        }

        public static Tuple<long, long> Minus(long ax, long ay, long bx, long by)
        {
            return Tuple.Create(ax - bx, ay - by);
        }

        public static Tuple<long, long, long> Minus(long ax, long ay, long az, long bx, long by, long bz)
        {
            return Tuple.Create(ax - bx, ay - by, az - bz);
        }

        public static Tuple<long, long> Multiply(long ax, long ay, long k)
        {
            return Tuple.Create(ax * k, ay * k);
        }

        public static Tuple<long, long, long> Multiply(long ax, long ay, long az, long k)
        {
            return Tuple.Create(ax * k, ay * k, az * k);
        }

        public static double CosineTheta(long ax, long ay, long bx, long by)
        {
            long dot = DotProduct(ax: ax, ay: ay, bx: bx, by: by);
            long normA = NormSqure(ax: ax, ay: ay);
            long normB = NormSqure(ax: bx, ay: by);

            return (double)(dot * dot) / (double)(normA * normB);
        }

        public static double CosineTheta(long ax, long ay, long az, long bx, long by, long bz)
        {
            long dot = DotProduct(ax: ax, ay: ay, az: az, bx: bx, by: by, bz: bz);
            long normA = NormSqure(ax: ax, ay: ay, az: az);
            long normB = NormSqure(ax: bx, ay: by, az: bz);

            return (double)(dot * dot) / (double)(normA * normB);
        }

        public static long DotProduct(long ax, long ay, long bx, long by)
        {
            return ax * bx + ay * by;
        }

        public static long DotProduct(long ax, long ay, long az, long bx, long by, long bz)
        {
            return ax * bx + ay * by + az * bz;
        }

        public static Tuple<long, long, long> CrossProduct(long ax, long ay, long bx, long by)
        {
            return Tuple.Create(0L, 0L, ax * by - ay * bx);
        }

        public static Tuple<long, long, long> CrossProduct(long ax, long ay, long az, long bx, long by, long bz)
        {
            long i = ay * bz - az * by;
            long j = -(ax * bz - az * bx);
            long k = ax * by - ay * bx;

            return Tuple.Create(i, j, k);
        }

        // cross product에서 z가 0인 경우에 방향만 취하는 것
        public static long GetCCW(long ax, long ay, long bx, long by)
        {
            return ax * by - ay * bx;
        }

        public static bool IsPointInLine(long lineStartX, long lineStartY, long lineEndX, long lineEndY, long x, long y)
        {
            long vec1X = x - lineStartX;
            long vec1Y = y - lineStartY;

            long vec2X = lineEndX - lineStartX;
            long vec2Y = lineEndY - lineStartY;

            long normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            long normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            long dotProduct = vec1X * vec2X + vec1Y * vec2Y;

            if (dotProduct < 0)
            {
                return false;
            }

            return dotProduct * dotProduct == normSquare1 * normSquare2;
        }

        public static bool IsCrossLine(long lineAStartX, long lineAStartY, long lineAEndX, long lineAEndY, long lineBStartX, long lineBStartY, long lineBEndX, long lineBEndY)
        {
            long abX = lineAEndX - lineAStartX;
            long abY = lineAEndY - lineAStartY;

            long acX = lineBStartX - lineAStartX;
            long acY = lineBStartY - lineAStartY;

            long adX = lineBEndX - lineAStartX;
            long adY = lineBEndY - lineAStartY;

            long cdX = lineBEndX - lineBStartX;
            long cdY = lineBEndY - lineBStartY;

            long caX = lineAStartX - lineBStartX;
            long caY = lineAStartY - lineBStartY;

            long cbX = lineAEndX - lineBStartX;
            long cbY = lineAEndY - lineBStartY;

            long abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            long abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            long cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            long cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            return abac * abad < 0 && cdca * cdcb < 0;
        }
    }
}
