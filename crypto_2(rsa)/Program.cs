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
                        SimpleNumbers simpleNumbers = new SimpleNumbers("numbers.txt");
                        simpleNumbers.GenerateKeys();

                        while (simpleNumbers.PublicExp == simpleNumbers.PrivateExp)
                            simpleNumbers.GenerateKeys();

                        Message message = new Message("message.txt");

                        if (UInt64.Parse(message.ToString()) >= simpleNumbers.Mult)
                            throw new Exception("Message length is exceeded");

                        Console.WriteLine(simpleNumbers.ToString());

                        Console.WriteLine("Number encryption");

                        message.Encrypt(simpleNumbers.Mult, simpleNumbers.PublicExp);
                        Console.WriteLine($"{message.ToString()} - encrypted message");

                        message.Decrypt(simpleNumbers.Mult, simpleNumbers.PrivateExp);
                        Console.WriteLine($"{message.ToString()} - decrypted message");

                        break;

                    case "2":
                        SimpleNumbers simpleNumbersForText = new SimpleNumbers("numbers.txt");
                        simpleNumbersForText.GenerateKeys();

                        while (simpleNumbersForText.PublicExp == simpleNumbersForText.PrivateExp)
                            simpleNumbersForText.GenerateKeys();

                        Text text = new Text("text.txt");

                        if (simpleNumbersForText.Mult <= UInt32.MaxValue)
                            throw new Exception("n must be greater than max value of UInt32");

                        Console.WriteLine(simpleNumbersForText.ToString());

                        Console.WriteLine("Text encryption");

                        text.Encrypt(simpleNumbersForText.Mult, simpleNumbersForText.PublicExp);
                        Console.WriteLine($"{text.ToString()} - encrypted text");

                        text.Decrypt(simpleNumbersForText.Mult, simpleNumbersForText.PrivateExp);
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