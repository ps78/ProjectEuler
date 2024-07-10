using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=59
    /// Each character on a computer is assigned a unique code and the preferred standard is ASCII (American Standard Code for Information Interchange). 
    /// For example, uppercase A = 65, asterisk (*) = 42, and lowercase k = 107.
    /// 
    /// A modern encryption method is to take a text file, convert the bytes to ASCII, then XOR each byte with a given value, 
    /// taken from a secret key.The advantage with the XOR function is that using the same encryption key on the cipher text, 
    /// restores the plain text; for example, 65 XOR 42 = 107, then 107 XOR 42 = 65.
    /// 
    /// For unbreakable encryption, the key is the same length as the plain text message, and the key is made up of random bytes.
    /// The user would keep the encrypted message and the encryption key in different locations, and without both "halves", 
    /// it is impossible to decrypt the message.
    /// 
    /// Unfortunately, this method is impractical for most users, so the modified method is to use a password as a key. 
    /// If the password is shorter than the message, which is likely, the key is repeated cyclically throughout the message.
    /// The balance for this method is using a sufficiently long password key for security, but short enough to be memorable.
    /// 
    /// Your task has been made easy, as the encryption key consists of three lower case characters.Using cipher.txt(right click and 'Save Link/Target As...'),
    /// a file containing the encrypted ASCII codes, and the knowledge that the plain text must contain common English words,
    /// decrypt the message and find the sum of the ASCII values in the original text.
    /// </summary>
    public class Problem059 : EulerProblemBase
    {
        public Problem059() : base(59, "XOR decryption", 0, 129448) { }

        private string[] commonWords = new string[] { "the", "The", "and", "who" };

        public override long Solve(long n)
        {
            const double minScore = 0.5;

            byte[] cipherText = ReadFile();

            int size = cipherText.Length;

            // test all 3 letter keys consisting of lower case letters
            var solutions = new List<SolutionCondidate>();
            Parallel.For(97, 128, (k1) =>
            {
                var key = new byte[] { (byte)k1, 0, 0 };
                for (byte k2 = 97; k2 < 128; k2++)
                {
                    key[1] = k2;
                    for (byte k3 = 97; k3 < 128; k3++)
                    {
                        key[2] = k3;
                        byte[] plainText = Xor(cipherText, key);
                        double score = GetPlaintextScore(plainText);
                        if (score >= minScore)
                        {
                            var sol = new SolutionCondidate(ByteToString(plainText), (byte[])key.Clone(), score);
                            solutions.Add(sol);
                            //Console.WriteLine("Score = {0:f2} / Key = {1}", sol.Score, sol.KeyAsString);
                            //Console.WriteLine(sol.PlainText + "\n");
                        }
                    }
                }
            });

            solutions.Sort((s1, s2) => s2.Score.CompareTo(s1.Score));

            if (solutions.Count == 0)
            {
                Console.WriteLine("no solution found");
                return 0;
            }
            else
                return solutions.First().LetterSum;
        }


        private struct SolutionCondidate
        {
            public string PlainText { get; set; }
            public byte[] Key { get; set; }
            public double Score { get; set; }
            public int LetterSum
            {
                get { return PlainText.Sum(c => c); }
            }
            public string KeyAsString
            {
                get
                {
                    var sb = new StringBuilder();
                    foreach (var b in Key)
                        sb.Append((char)b);
                    return sb.ToString();
                }
            }
            public SolutionCondidate(string plainText, byte[] key, double score)
            {
                this.PlainText = plainText;
                this.Key = key;
                this.Score = score;
            }
        }
       
        /// <summary>
        /// returns input1 xor key. Key is repeated as long as necessary
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private byte[] Xor(byte[] input1, byte[] key)
        {
            int keyLen = key.Length, keyIdx = 0; 
            var result = new byte[input1.Length];
            for (int i = 0; i < input1.Length; i++)
            {
                result[i] = (byte)(input1[i] ^ key[keyIdx++]);
                if (keyIdx == keyLen)
                    keyIdx %= keyLen;
            }
            return result;
        }

        /// <summary>
        /// replicates key to a total length as given
        /// </summary>
        /// <param name="key"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private byte[] GenerateFullKey(byte[] key, int length)
        {
            var result = new byte[length];
            int keySize = key.Length;

            for (int i = 0; i < length; i++)
                result[i] = key[i % keySize];

            return result;
        }

        private string ByteToString(byte[] input)
        {
            var sb = new StringBuilder();
            foreach (byte b in input)
                sb.Append((char)b);
            return sb.ToString();
        }

        /// <summary>
        /// checks if the input is an english plain text
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private double GetPlaintextScore(byte[] candidate)
        {
            //int numOfSpaces = candidate.Count((b) => b == 32);
            //int numLowerCaseChars = candidate.Count((b) => (b >= 97) && (b <= 127));
            //int numUpperCaseChars = candidate.Count((b) => (b >= 65) && (b <= 90));

            double s = candidate.Length;
            //if ((numOfSpaces / s > 0.05) && (numLowerCaseChars > 0.6) && (numUpperCaseChars > 0.01))
            //    if (CountWords(candidate, "and") >= 3)
            //        return true;                       

            double freqSpace = candidate.Count((b) => b == 32) / s;
            double freqE = candidate.Count((b) => (b == 101 || b == 69)) / s; // 0.127
            //double freqT = candidate.Count((b) => (b == 116 || b == 84)) / s; // 0.091
            //double freqA = candidate.Count((b) => (b == 97 || b == 65)) / s;  // 0.082
            //double freqO = candidate.Count((b) => (b == 111 || b == 79)) / s; // 0.075
            //double freqI = candidate.Count((b) => (b == 105 || b == 73)) / s; // 0.070
            double freqSpecChar = candidate.Count((b) => (b < 32 || b > 122)) / s;
            if (
                ((freqSpecChar < 0.01)) &&
                ((freqE >= 0.08) && (freqE <= 0.16)) &&
                //((freqT >= 0.05) && (freqT <= 0.15)) &&
                //((freqA >= 0.03) && (freqA <= 0.14)) &&
                //((freqO >= 0.03) && (freqO <= 0.12)) &&
                //((freqI >= 0.03) && (freqI <= 0.12)) &&
                ((freqSpace >= 0.15) && (freqSpace <= 0.25))
               )
                return 1.0;

            return 0.0;
        }

        private byte[] ReadFile(int maxBytesToRead = -1)
        {
            string cipherStr = File.ReadAllText(Path.Combine(ResourcePath, "problem059.txt"));
            var allCipher = cipherStr.Split(new char[] { ',' }).ToList().ConvertAll((c) => byte.Parse(c)).ToArray();

            if (maxBytesToRead >= 0)
                allCipher = allCipher.Take(maxBytesToRead).ToArray();
            
            return allCipher;
        }
    
    }
}
