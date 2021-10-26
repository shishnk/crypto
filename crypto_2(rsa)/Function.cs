using System;
using System.Numerics;

namespace RSA
{
    abstract class Function
    {
        protected UInt64 ModularPow(UInt64 baza, UInt64 exp, UInt64 modulus)
        {
            BigInteger result = 1;
            BigInteger bigIntBase = baza;
            BigInteger bigIntModulus = modulus;

            bigIntBase = bigIntBase % bigIntModulus;

            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result = (result * bigIntBase) % bigIntModulus;

                bigIntBase = (bigIntBase * bigIntBase) % bigIntModulus;

                exp >>= 1;
            }

            return (UInt64)result;
        }

        protected UInt64 GCD(UInt64 a, UInt64 b)
        {
            if (b == 0)
                return a;

            return GCD(b, a % b);
        }

        protected UInt64 LCM(UInt64 a, UInt64 b)
        {
            return ((a * b) / GCD(a, b));
        }

        protected bool FermatTest(UInt32 number) // complexity O(log(N))
        {
            Random rnd = new Random();

            for (int i = 0; i < 100; i++)
            {
                UInt64 a = ((UInt64)rnd.Next() % (number - 2)) + 2;

                if (GCD(a, number) != 1)
                    return false;

                if (ModularPow(a, number - 1, number) != 1)
                    return false;
            }

            return true;
        }

        protected bool IsPrime(UInt32 number) // complexity O(sqrt(N))
        {
            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0)
                    return false;
            return true;
        }
    }
}