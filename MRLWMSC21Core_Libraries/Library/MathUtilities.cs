using System;

namespace MRLWMSC21Core.Library
{
    class MathUtilities
    {

        public static int Min(int a, int b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static int Max(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static decimal Min(decimal a, decimal b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static decimal Max(decimal a, decimal b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

        public static Single Min(Single a, Single b)
        {
            if (a < b)
            {
                return a;
            }
            return b;
        }

        public static Single Max(Single a, Single b)
        {
            if (a > b)
            {
                return a;
            }
            return b;
        }

    }
}
