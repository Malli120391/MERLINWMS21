using System;

namespace MRLWMSC21Core.Library
{
    class LogicalUtilities
    {

        // these are used for VB.NET compatibility
        static public int IIF(bool condition, int a, int b)
        {
            int x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public Single IIF(bool condition, Single a, Single b)
        {
            float x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public Double IIF(bool condition, double a, double b)
        {
            double x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public decimal IIF(bool condition, decimal a, decimal b)
        {
            decimal x = 0;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

        static public String IIF(bool condition, String a, String b)
        {
            String x = String.Empty;
            if (condition)
            {
                x = a;
            }
            else
            {
                x = b;
            }
            return x;
        }

    }
}
