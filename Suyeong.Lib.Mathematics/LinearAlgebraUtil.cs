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

        public static bool IsPointInLine(int lineStartX, int lineStartY, int lineEndX, int lineEndY, int x, int y)
        {
            int vec1X = x - lineStartX;
            int vec1Y = y - lineStartY;

            int vec2X = lineEndX - lineStartX;
            int vec2Y = lineEndY - lineStartY;

            int normSquare1 = vec1X * vec1X + vec1Y * vec1Y;
            int normSquare2 = vec2X * vec2X + vec2Y * vec2Y;

            if (normSquare1 > normSquare2)
            {
                return false;
            }

            int dotProduct = vec1X * vec2X + vec1Y * vec2Y;

            if (dotProduct < 0)
            {
                return false;
            }

            return dotProduct * dotProduct == normSquare1 * normSquare2;
        }

        public static bool IsCrossLine(int lineAStartX, int lineAStartY, int lineAEndX, int lineAEndY, int lineBStartX, int lineBStartY, int lineBEndX, int lineBEndY)
        {
            int abX = lineAEndX - lineAStartX;
            int abY = lineAEndY - lineAStartY;

            int acX = lineBStartX - lineAStartX;
            int acY = lineBStartY - lineAStartY;

            int adX = lineBEndX - lineAStartX;
            int adY = lineBEndY - lineAStartY;

            int cdX = lineBEndX - lineBStartX;
            int cdY = lineBEndY - lineBStartY;

            int caX = lineAStartX - lineBStartX;
            int caY = lineAStartY - lineBStartY;

            int cbX = lineAEndX - lineBStartX;
            int cbY = lineAEndY - lineBStartY;

            int abac = GetCCW(ax: abX, ay: abY, bx: acX, by: acY);
            int abad = GetCCW(ax: abX, ay: abY, bx: adX, by: adY);

            int cdca = GetCCW(ax: cdX, ay: cdY, bx: caX, by: caY);
            int cdcb = GetCCW(ax: cdX, ay: cdY, bx: cbX, by: cbY);

            // 두 ccw를 곱해도 되지만, int 형의 저장 범위를 벗어나는 값이 들어오면 에러가 나서 부호 기준으로 처리
            return ((abac > 0 && abad < 0) || (abac < 0 && abad > 0)) && ((cdca > 0 && cdcb < 0) || (cdca < 0 && cdcb > 0));
        }
    }
}
