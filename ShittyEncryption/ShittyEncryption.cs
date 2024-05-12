using System;
using System.Collections.Generic;

namespace ShittyEncryption
{
    public class ShittyEncryption
    {
        private char[,] keyMatrix = new char[8, 8];
        private string key;

        public ShittyEncryption()
        {
            // create a random key
            byte[] key = new byte[8];
            new Random().NextBytes(key);
            Key = key;
        }
        
        public byte[] Key
        {
            get { return Convert.FromBase64String(key); }
            set
            {
                key = Convert.ToBase64String(value);
                SetShittyKeyMatrix(key);
            }
        }

        public string Encrypt(byte[] input)
        {
            string inputStr = Convert.ToBase64String(input);
            string output = "";
            
            for (int i = 0; i < inputStr.Length - 1; i += 2)
            {
                char c1 = inputStr[i];
                char c2 = inputStr[i + 1];
                TwoDIndex[] indices = FindChars(c1, c2);
                TwoDIndex index1 = indices[0];
                TwoDIndex index2 = indices[1];
                
                if (c1 == '=')
                {
                    output += '=';
                    output += 0;
                    break;
                }
                if (c2 == '=')
                {
                    output += keyMatrix[index1.I, (index1.J + 1) % 8];
                    output += '=';
                    break;
                }
                
                if (index1.I == index2.I)
                {
                    output += keyMatrix[index1.I, (index1.J + 1) % 8];
                    output += keyMatrix[index2.I, (index2.J + 1) % 8];
                }
                else if (index1.J == index2.J)
                {
                    output += keyMatrix[(index1.I + 1) % 8, index1.J];
                    output += keyMatrix[(index2.I + 1) % 8, index2.J];
                }
                else
                {
                    output += keyMatrix[index1.I, index2.J];
                    output += keyMatrix[index2.I, index1.J];
                }   
            }
            return output;
        }
        
        public byte[] Decrypt(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length - 1; i += 2)
            {
                char c1 = input[i];
                char c2 = input[i + 1];
                TwoDIndex[] indices = FindChars(c1, c2);
                TwoDIndex index1 = indices[0];
                TwoDIndex index2 = indices[1];
                
                if (c1 == '=')
                {
                    output += '=';
                    output += "=";
                    break;
                }
                if (c2 == '=')
                {
                    output += keyMatrix[index1.I, (index1.J + 7) % 8];
                    output += '=';
                    break;
                }
                
                // edge cases
                if (index1.I == index2.I)
                {
                    output += keyMatrix[index1.I, (index1.J + 7) % 8];
                    output += keyMatrix[index2.I, (index2.J + 7) % 8];
                }
                else if (index1.J == index2.J)
                {
                    output += keyMatrix[(index1.I + 7) % 8, index1.J];
                    output += keyMatrix[(index2.I + 7) % 8, index2.J];
                }
                else
                {
                    output += keyMatrix[index1.I, index2.J];
                    output += keyMatrix[index2.I, index1.J];
                }   
            }
            return Convert.FromBase64String(output);
        }

        private TwoDIndex[] FindChars(char c1, char c2)
        {
            TwoDIndex[] indexes = new TwoDIndex[2];
            for (int i = 0; i < keyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < keyMatrix.GetLength(1); j++)
                {
                    if (keyMatrix[i, j] == c1)
                    {
                        indexes[0] = new TwoDIndex(i, j);
                    }
                    if (keyMatrix[i, j] == c2)
                    {
                        indexes[1] = new TwoDIndex(i, j);
                    }
                }
            }
            return indexes;
        }

        private void SetShittyKeyMatrix(string key)
        {
            
            List<char> addedChars = new List<char>();
            int i = 0;
            int j = 0;
            foreach (char c in key)
            {
                if (c == '=' || addedChars.Contains(c))
                {
                    continue;
                }
                keyMatrix[i, j] = c;
                addedChars.Add(c);
                j++;
                if (j == keyMatrix.GetLength(1))
                {
                    j = 0;
                    i++;
                }
            }
            
            string restOfMatrix = GetRestOfMatrix();
            foreach (char c in restOfMatrix)
            {
                if (addedChars.Contains(c))
                {
                    continue;
                }
                keyMatrix[i, j] = c;
                addedChars.Add(c);
                j++;
                if (j == keyMatrix.GetLength(1))
                {
                    j = 0;
                    i++;
                }
            }
        }
        
        private string GetRestOfMatrix()
        {
            string base64Chars = "";
            
            // Add uppercase letters A-Z
            for (int i = 65; i <= 90; i++)
            {
                base64Chars += (char)i;
            }

            // Add lowercase letters a-z
            for (int i = 97; i <= 122; i++)
            {
                base64Chars += (char)i;
            }

            // Add digits 0-9
            for (int i = 48; i <= 57; i++)
            {
                base64Chars += (char)i;
            }

            // Add '+' and '/'
            base64Chars += '+';
            base64Chars += '/';
            return base64Chars;
        }
    }
    
    public class TwoDIndex
    {
        public TwoDIndex(int i, int j)
        {
            I = i;
            J = j;
        }
        public int I { get; set; }
        public int J { get; set; }
    }
}