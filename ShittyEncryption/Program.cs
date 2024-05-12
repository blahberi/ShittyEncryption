using System;

namespace ShittyEncryption
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ShittyEncryption shittyEncryption = new ShittyEncryption();
            string myKey = "Hello World!";
            byte[] byteKey = System.Text.Encoding.UTF8.GetBytes(myKey);
            shittyEncryption.Key = byteKey;
            
            string input = "Hello World!";
            byte[] byteInput = System.Text.Encoding.UTF8.GetBytes(input);
            string encrypted = shittyEncryption.Encrypt(byteInput);
            Console.WriteLine(encrypted);
            
            byte[] decrypted = shittyEncryption.Decrypt(encrypted);
            string decryptedStr = System.Text.Encoding.UTF8.GetString(decrypted);
            Console.WriteLine(decryptedStr);
        }
    }
}