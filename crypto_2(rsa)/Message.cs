using System;
using System.IO;

namespace RSA
{
    class Message : Function
    {
        private UInt64 message;

        public Message(string path)
        {
            using (var sr = new StreamReader(path))
            {
                message = UInt64.Parse(sr.ReadToEnd());
            }
        }

        public void Encrypt(UInt64 n, UInt64 e)
        {
            message = ModularPow(message, e, n);
        }

        public void Decrypt(UInt64 n, UInt64 d)
        {
            message = ModularPow(message, d, n);
        }

        public override string ToString()
        {
            return message.ToString();
        }
    }
}