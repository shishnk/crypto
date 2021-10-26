using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace RSA
{
    class Text : Function
    {
        private string text;

        public Text(string path)
        {
            using (var sr = new StreamReader(path))
            {
                text = sr.ReadToEnd();

                if (text.Length % 2 != 0)
                    text += " ";
            }
        }

        public void Encrypt(UInt64 n, UInt64 e)
        {
            List<UInt64> nums = new List<UInt64>();
            List<UInt64> encNums = new List<UInt64>();

            UInt64 num = 0;
            int count = 0;

            for (int i = 0; i < text.Length; i++)
            {
                num <<= 16;
                num += (UInt16)text[i];

                count++;

                if (count == 2)
                {
                    nums.Add(num);
                    num = 0;
                    count = 0;
                }
            }

            text = "";

            foreach (UInt64 number in nums)
            {
                num = ModularPow(number, e, n);
                encNums.Add(num);
                text += num;
                text += " ";
            }

            WriteToFile(encNums);
        }

        private void WriteToFile(List<UInt64> numbers)
        {
            using (var sw = new StreamWriter("encryptedtext.txt"))
            {
                foreach (UInt64 number in numbers)
                    sw.WriteLine(number);
            }
        }

        public void Decrypt(UInt64 n, UInt64 d)
        {
            text = "";

            using (var sr = new StreamReader("encryptedtext.txt"))
            {
                foreach (var number in sr.ReadToEnd().Split("\n").Where(value => value != ""))
                {
                    UInt32 num = (UInt32)ModularPow(UInt64.Parse(number), d, n);

                    UInt32 fstSymb = num >> 16;
                    UInt32 scdSymb = num & UInt16.MaxValue;

                    text += (char)fstSymb;
                    text += (char)scdSymb;
                }
            }
        }

        public override string ToString()
        {
            return text;
        }
    }
}