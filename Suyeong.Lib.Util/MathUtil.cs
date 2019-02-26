using System;

namespace Suyeong.Lib.Util
{
    public static class MathUtil
    {
        public static float Round(float Num, float Den)
        {
            return IsZero(Den) ? 0f : (float)Math.Round((double)Num / (double)Den) * 1000f * 0.001f;
        }
        public static double Round(double Num, double Den)
        {
            return IsZero(Den) ? 0f : Math.Round(Num / Den) * 1000d * 0.001d;
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
            return (num < float.Epsilon && num > -float.Epsilon);
        }

        public static bool IsZero(double num)
        {
            return (num < double.Epsilon && num > -double.Epsilon);
        }
    }
}
