using System;
using System.Collections.Generic;
using System.IO;

namespace RSA
{
    class PrimeNumbers : Function
    {
        private UInt32 p;
        private UInt32 q;
        private UInt64 n;
        private UInt64 eulerFuncValue;
        private UInt64 carmichaelFuncValue;
        private UInt64 e;
        private UInt64 d;

        public UInt64 PublicExp => e;
        public UInt64 PrivateExp => d;
        public UInt64 Mult => n;

        public PrimeNumbers(string path)
        {
            using (var sr = new StreamReader(path))
            {
                List<UInt32> numbers = new List<UInt32>(2);

                while (!sr.EndOfStream)
                    numbers.Add(UInt32.Parse(sr.ReadLine()));

                p = numbers[0];
                q = numbers[1];
            }

            if (!(FermatTest(p) && FermatTest(q)))
                throw new Exception("Numbers are not prime");
        }

        private void CalcMultiply()
        {
            n = (UInt64)p * (UInt64)q;
        }

        public void GenerateKeys()
        {
            CalcMultiply();
            //CalcEulerFuncValue();
            CalcCarmichaelFuncValue();
            GenPublicExp();
            GenPrivateExp();
        }

        private void CalcEulerFuncValue()
        {
            eulerFuncValue = (UInt64)(p - 1) * (UInt64)(q - 1);
        }

        private void CalcCarmichaelFuncValue()
        {
            carmichaelFuncValue = LCM(p - 1, q - 1);
        }

        private void GenPublicExp()
        {
            Random rnd = new Random();

            double temp = (rnd.NextDouble() + 1.0) / 3;
            e = (UInt64)(n * temp);

            while (GCD(e, carmichaelFuncValue) != 1)
                e += 1;
        }

        private void GenPrivateExp()
        {
            d = 0;
            UInt64 newd = 1;
            UInt64 r = carmichaelFuncValue;
            UInt64 newr = e;

            while (newr != 0)
            {
                UInt64 quotient = r / newr;

                (d, newd) = (newd, d - quotient * newd);
                (r, newr) = (newr, r - quotient * newr);
            }

            d += carmichaelFuncValue;
        }

        public override string ToString()
        {
            return $"({n}, {e}) - public key\n({n}, {d}) - private key";
        }
    }
}