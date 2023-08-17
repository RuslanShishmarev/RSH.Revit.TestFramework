using System;

namespace RSH.Revit.TestFramework.Api
{
    internal class Asset
    {
        public static void IsEqual<T>(T input, T expected)
        {
            if (input.Equals(expected))
            {
                return;
            }

            throw new Exception("Faild");
        }

        public static void IsNotEqual<T>(T input, T expected)
        {
            if (!input.Equals(expected))
            {
                return;
            }

            throw new Exception("Faild");
        }
    }
}
