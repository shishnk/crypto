using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crypto_1_OOP_
{
    class Key
    {
        private string key;

        private char[] GetLetters() // получение алфавита в массиве
        {
            return "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
        }

        private string GetAlphabet() // получение алфавита в строке
        {
            return "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        }

        public void Generate() // Тасование Фишера — Йетса
        {
            Random rnd = new Random();
            char[] alphabet = GetLetters();
            char temp;

            for (int i = alphabet.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(0, i + 1);
                temp = alphabet[i];
                alphabet[i] = alphabet[j];
                alphabet[j] = temp;
            }

            key = new string(alphabet);
        }

        public void WriteToFile(string path) // запись ключа в файл
        {
            using (var sw = new StreamWriter(path))
                sw.WriteLine(key);
        }

        public override string ToString() // получение ключа
        {
            return key;
        }

        private void AppendLetter() // добавление букв в ключ, составленный из монограмм
        {
            for (int i = 0; i < GetAlphabet().Length; i++)
            {
                if (!key.Contains(GetAlphabet()[i]))
                    key += GetAlphabet()[i];
            }
        }

        private void SortedFromMonogramms(Monograms monogramms) // получение ключа из эталонных монограмм
        {
            foreach ((string monogram, double value) in monogramms.monograms)
                key += monogram;
        }

        private void CreatePrimary(Key keyFromMono, Monograms monogramsDef) // создание первичного ключа
        {
            string[] array = monogramsDef.monograms.Keys.ToArray();
            char[] primaryKey = new char[GetLetters().Length];

            for (int i = 0; i < GetLetters().Length; i++)
            {
                primaryKey[Array.IndexOf(GetLetters(),
                    array[i][0])] =
                    keyFromMono.key[i];
            }

            key = new string(primaryKey);
        }

        private void GenerateFromMonogramms(string text) // генерация ключа из монограмм для составления первичного ключа
        {
            Monograms monogramsDef = new Monograms();
            monogramsDef.ReadFromFile("russian_monograms.txt");

            Monograms monograms = new Monograms();
            monograms.GetFromText(text);
            monograms.SortingByDescending();

            Key keyFromMono = new Key();
            keyFromMono.SortedFromMonogramms(monograms);
            keyFromMono.AppendLetter();

            Key primaryKey = new Key();
            primaryKey.Generate();
            primaryKey.CreatePrimary(keyFromMono, monogramsDef);

            key = primaryKey.key;
        }

        private double GetScores(string text) // подсчет дисперсии
        {
            double scores = 0.0;
            double num;
            Bigrams bigrams = new Bigrams();
            Bigrams bigramsDef = new Bigrams();

            bigramsDef.ReadFromFile("russian_bigrams.txt");

            bigrams.GetFromText(text);

            foreach ((string keyBigramm, double value) in bigrams.bigrams)
            {
                if (bigramsDef.bigrams.TryGetValue(keyBigramm, out num))
                {
                    scores += Math.Pow((value - num), 2);
                }
                else
                {
                    scores += Math.Pow(value, 2);
                }
            }

            return scores;
        }

        private void GetFromArray(int[] analysPos) // перестановка первичного ключа
        {
            Key newTempKey = new Key();

            foreach (int i in analysPos)
                newTempKey.key += GetLetters()[i];

            key = newTempKey.key;
        }

        public void BruteForce() // перебор ключей
        {
            string path = "encryptedtext.txt";
            Text text = new Text(path);
            int[] analysPos = new int[GetLetters().Length];

            Key mainKey = new Key();
            mainKey.Generate();
            mainKey.GenerateFromMonogramms(text.ToString());
            Key tempKey = new Key();

            for (int i = 0; i < analysPos.Length; i++)
                analysPos[i] = Array.IndexOf(GetLetters(), mainKey.key[i]);

            int step = 1;

            double scores = GetScores(text.Decrypt(mainKey.key));

            while (true)
            {
                for (int i = 0; i < GetLetters().Length; i++)
                {
                    if ((i + step) >= GetLetters().Length)
                        break;

                    (analysPos[i], analysPos[i + step]) = (analysPos[i + step], analysPos[i]);

                    tempKey.GetFromArray(analysPos);

                    double newScores = GetScores(text.Decrypt(tempKey.key));

                    if (scores > newScores)
                    {
                        scores = newScores;
                        mainKey.key = tempKey.key;
                        step = 1;
                        break;
                    }
                    else
                    {
                        (analysPos[i], analysPos[i + step]) = (analysPos[i + step], analysPos[i]);
                    }
                }

                step += 1;

                Console.WriteLine("Key: {0}, scores: {1}", mainKey.key, scores);

                if (step > GetLetters().Length - 1)
                    break;
            }

            key = mainKey.key;
        }
    }
}