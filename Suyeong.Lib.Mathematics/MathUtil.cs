using System;

namespace Suyeong.Lib.Mathematics
{
    public static class MathUtil
    {
        public static float Round(float num, float den)
        {
            return IsZero(den) ? 0f : (float)Math.Round((double)num / (double)den) * 1000f * 0.001f;
        }

        public static double Round(double num, double den)
        {
            return IsZero(den) ? 0f : Math.Round(num / den) * 1000d * 0.001d;
        }

        public static bool IsEqual(float val1, float val2)
        {
            return IsZero(num: val1 - val2);
        }

        public static bool IsEqual(double val1, double val2)
        {
            return IsZero(num: val1 - val2);
        }

        public static bool IsZero(float num)
        {
            return Math.Abs(num) < float.Epsilon;
        }

        public static bool IsZero(double num)
        {
            return Math.Abs(num) < double.Epsilon;
        }

        public static bool IsNegative(int val1, int val2)
        {
            return (val1 < 0 && val2 > 0) || (val1 > 0 && val2 < 0);
        }

        public static bool IsNegative(float val1, float val2)
        {
            return (val1 < 0f && val2 > 0f) || (val1 > 0f && val2 < 0f);
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
