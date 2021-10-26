using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crypto_1_OOP_
{
    class Monograms : INGrams
    {
        public Dictionary<string, double> monograms = new Dictionary<string, double>();

        public void ReadFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                monograms = sr.ReadToEnd().Split("\n")
                    .Select(x => x.Split(' '))
                        .ToDictionary(x => x[0].ToLower(), x => Double.Parse(x[1]));

                var total = monograms.Sum(x => x.Value);

                foreach ((string key, double value) in monograms)

                    monograms[key] /= total;
            }
        }

        public void GetFromText(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                string monogram = text[i].ToString();

                if (!monograms.ContainsKey(monogram))
                {
                    monograms.Add(monogram, 1);
                }
                else
                {
                    monograms[monogram] += 1;
                }
            }

            var total = monograms.Sum(x => x.Value);

            foreach ((string key, double value) in monograms)
                monograms[key] /= total;
        }

        public void SortingByDescending()
        {
            monograms = monograms.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}