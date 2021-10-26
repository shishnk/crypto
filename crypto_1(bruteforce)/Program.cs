using System;
using System.Diagnostics;

namespace Crypto_1_OOP_
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();

            Key key = new Key();
            key.Generate();
            key.WriteToFile("key.txt");

            Text text = new Text("kek.txt");
            text.Encrypt(key.ToString());

            key.BruteForce();

            Console.WriteLine(key.ToString());
            Console.WriteLine(text.Decrypt(key.ToString()));

            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

        }
    }
}