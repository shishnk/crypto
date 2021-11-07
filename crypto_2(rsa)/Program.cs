using System;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Choose what to encrypt:\n1) Number \n2) Text");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PrimeNumbers primeNumbers = new PrimeNumbers("numbers.txt");
                        primeNumbers.GenerateKeys();

                        while (primeNumbers.PublicExp == primeNumbers.PrivateExp)
                            primeNumbers.GenerateKeys();

                        Message message = new Message("message.txt");

                        if (UInt64.Parse(message.ToString()) >= primeNumbers.Mult)
                            throw new Exception("Message length is exceeded");

                        Console.WriteLine(primeNumbers.ToString());

                        Console.WriteLine("Number encryption");

                        message.Encrypt(primeNumbers.Mult, primeNumbers.PublicExp);
                        Console.WriteLine($"{message.ToString()} - encrypted message");

                        message.Decrypt(primeNumbers.Mult, primeNumbers.PrivateExp);
                        Console.WriteLine($"{message.ToString()} - decrypted message");

                        break;

                    case "2":
                        PrimeNumbers primeNumbersForText = new PrimeNumbers("numbers.txt");
                        primeNumbersForText.GenerateKeys();

                        while (primeNumbersForText.PublicExp == primeNumbersForText.PrivateExp)
                            primeNumbersForText.GenerateKeys();

                        Text text = new Text("text.txt");

                        if (primeNumbersForText.Mult <= UInt32.MaxValue)
                            throw new Exception("n must be greater than max value of UInt32");

                        Console.WriteLine(primeNumbersForText.ToString());

                        Console.WriteLine("Text encryption");

                        text.Encrypt(primeNumbersForText.Mult, primeNumbersForText.PublicExp);
                        Console.WriteLine($"{text.ToString()} - encrypted text");

                        text.Decrypt(primeNumbersForText.Mult, primeNumbersForText.PrivateExp);
                        Console.WriteLine($"{text.ToString()} - decrypted text");

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Program is complete");
            }
        }
    }
}