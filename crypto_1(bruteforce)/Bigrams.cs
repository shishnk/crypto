using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crypto_1_OOP_
{
    class Bigrams : INGrams
    {
        public Dictionary<string, double> bigrams = new Dictionary<string, double>();

        public void ReadFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                bigrams = sr.ReadToEnd().Split("\n")
                    .Select(x => x.Split(' '))
                        .ToDictionary(x => x[0].ToLower(), x => Double.Parse(x[1]));

                var total = bigrams.Sum(x => x.Value);

                foreach ((string key, double value) in bigrams)

                    bigrams[key] /= total;
            }
        }

        public void GetFromText(string text)
        {
            for (int i = 1; i < text.Length; i++)
            {
                string bigram = text[i - 1].ToString() + text[i].ToString();

                if (!bigrams.ContainsKey(bigram))
                {
                    bigrams.Add(bigram, 1);
                }
                else
                {
                    bigrams[bigram] += 1;
                }
            }

            var total = bigrams.Sum(x => x.Value);

            foreach ((string key, double value) in bigrams)
                bigrams[key] /= total;
        }

        public void SortingByDescending()
        {
            bigrams = bigrams.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
