using System;

namespace Suyeong.Lib.Mathematics
{
    public static class MathUtil
    {
        public static double Round(double num, double den)
        {
            return IsZero(den) ? 0f : Math.Round(num / den) * 1000d * 0.001d;
        }

        public static int ConvertToInt(double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static bool IsEqual(double val1, double val2, double epsilon = double.Epsilon)
        {
            return Math.Abs(Math.Abs(val1) - Math.Abs(val2)) <= epsilon;
        }

        public static bool IsZero(double num, double epsilon = double.Epsilon)
        {
            return Math.Abs(num) <= epsilon;
        }

        public static bool IsNegative(int val1, int val2)
        {
            return (val1 < 0 && val2 > 0) || (val1 > 0 && val2 < 0);
        }

        public static bool IsNegative(double val1, double val2)
        {
            return (val1 < 0d && val2 > 0d) || (val1 > 0d && val2 < 0d);
        }

        public static double RadianToDegree(double radian)
        {
            return radian * 180d / Math.PI;
        }

        public static double DegreeToRadian(double angle)
        {
            return angle * Math.PI / 180d;
        }
    }
}
