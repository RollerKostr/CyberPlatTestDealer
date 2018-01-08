using System;

namespace CyberPlatGate.Components.Utility
{
    // See https://stackoverflow.com/questions/976646/is-this-a-good-way-to-generate-a-string-of-random-characters for details
    static class RandomStringGenerator
    {
        public const string AlphaNumericalAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwyxz0123456789";

        public static string GenerateAlphaNumericalString(int size, Random rng)
        {
            return GenerateString(size, rng, AlphaNumericalAlphabet);
        }

        public static string GenerateString(int size, Random rng, string alphabet)
        {
            var chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = alphabet[rng.Next(alphabet.Length)];
            }

            return new string(chars);
        }
    }
}
