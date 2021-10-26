using System.IO;
using System.Text.RegularExpressions;

namespace Crypto_1_OOP_
{
    class Text
    {
        private string text;

        public Text(string path)
        {
            using (var sr = new StreamReader(path))
                text = Regex.Replace(sr.ReadToEnd().ToLower(), "[^а-яё]", "");
        }

        private char[] GetLetters()
        {
            return "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();
        }

        public void Encrypt(string key)
        {
            char[] encryptedText = text.ToCharArray();
            char[] alphabet = GetLetters();
            char[] _key = key.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (encryptedText[i] == (alphabet[j]))
                    {
                        encryptedText[i] = _key[j];
                        j = alphabet.Length;
                    }
                }
            }

            text = new string(encryptedText);

            WriteToFile();
        }

        private void WriteToFile()
        {
            string path = "encryptedtext.txt";

            using (var sw = new StreamWriter(path))
                sw.Write(text);
        }

        public string Decrypt(string key)
        {
            char[] alphabet = GetLetters();
            char[] _key = key.ToCharArray();
            char[] encryptedText = text.ToCharArray();

            for (int i = 0; i < encryptedText.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (encryptedText[i] == _key[j])
                    {
                        encryptedText[i] = alphabet[j];
                        j = alphabet.Length;
                    }
                }
            }

            return new string(encryptedText);
        }

        public override string ToString()
        {
            return text;
        }
    }
}