using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using static System.Formats.Asn1.AsnWriter;

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

        private readonly int KeyLength = 3;

        private enum CharacterClass
        {
            LetterLowerCase,
            LetterUpperCase,
            Number,
            Space,
            Punctuation,
            Other
        }

        private static CharacterClass ClassifyChar(byte c)
        {
            if (c >= 'a' && c <= 'z')
                return CharacterClass.LetterLowerCase;
            else if (c >= 'A' && c <= 'Z')
                return CharacterClass.LetterUpperCase;
            else if (c >= '0' && c <= '9')
                return CharacterClass.Number;
            else if (c == ' ')
                return CharacterClass.Space;
            else if (".,:;&%?!()/-'\"".Contains((char)c))
                return CharacterClass.Punctuation;
            else
                return CharacterClass.Other;
        }

        private static bool IsPlausible(Dictionary<CharacterClass, int> histogram)
        {
            double n = histogram.Values.Sum();
            if (histogram[CharacterClass.Other] / n > 0.1)
                return false;

            if (histogram[CharacterClass.Punctuation] / n < 0.01 || histogram[CharacterClass.Punctuation] / n > 0.2)
                return false;

            if (histogram[CharacterClass.Number] / n > 0.1)
                return false;

            if (histogram[CharacterClass.Space] / n < 0.05 || histogram[CharacterClass.Space]/n > 0.3)
                return false;

            return true;
        }

        public override long Solve(long n)
        {
            byte[] cipherText = ReadFile();

            int size = cipherText.Length;

            var histograms = CreateHistograms(cipherText, KeyLength);

            var keyCandidates = new List<byte>[KeyLength];

            for (int keyIdx = 0; keyIdx < KeyLength; keyIdx++)
            {
                keyCandidates[keyIdx] = new List<byte>();
                foreach (var kv in histograms[keyIdx])
                    if (IsPlausible(kv.Value))
                        keyCandidates[keyIdx].Add(kv.Key);

                if (keyCandidates[keyIdx].Count == 0)
                {
                    Console.WriteLine("No solution found");
                    return 0;
                }
            }

            if (keyCandidates.Select(x => x.Count).Sum() != KeyLength)
            {
                Console.WriteLine("More than one solution found");
                return 0;
            }

            var key = keyCandidates.Select(x => x.First()).ToArray();

            byte[] plainText = Xor(cipherText, key);
            var sol = new SolutionCondidate(ByteToString(plainText), key, 1.0);

            return sol.LetterSum;
        }

        private Dictionary<byte, Dictionary<CharacterClass,int>>[] CreateHistograms(byte[] cipherText, int keyLength)
        {
            var histogramArray = new Dictionary<byte, Dictionary<CharacterClass, int>>[keyLength];

            for (int keyIndex = 0; keyIndex < keyLength; keyIndex++)
            {                
                var keyHistograms = new Dictionary<byte, Dictionary<CharacterClass, int>>();

                for (byte keyChar = (byte)'a'; keyChar <= (byte)'z'; keyChar++)
                {
                    var histogram = new Dictionary<CharacterClass, int>();
                    foreach (var val in Enum.GetValues(typeof(CharacterClass)))
                        histogram[(CharacterClass)val] = 0;

                    for (int idx = keyIndex; idx < cipherText.Length; idx += keyLength)
                    {
                        var chClass = ClassifyChar((byte)(cipherText[idx] ^ keyChar));
                        histogram[chClass]++;
                    }

                    keyHistograms[keyChar] = histogram;
                }
                histogramArray[keyIndex] = keyHistograms;
            }

            return histogramArray;
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
        private static byte[] Xor(byte[] input1, byte[] key)
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
        private static byte[] GenerateFullKey(byte[] key, int length)
        {
            var result = new byte[length];
            int keySize = key.Length;

            for (int i = 0; i < length; i++)
                result[i] = key[i % keySize];

            return result;
        }

        private static string ByteToString(byte[] input)
        {
            var sb = new StringBuilder();
            foreach (byte b in input)
                sb.Append((char)b);
            return sb.ToString();
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
