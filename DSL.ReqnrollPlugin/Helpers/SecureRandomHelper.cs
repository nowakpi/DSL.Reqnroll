using System;
using System.Security.Cryptography;

namespace DSL.ReqnrollPlugin.Helpers
{
    internal static class SecureRandomHelper
    {
        private static readonly RandomNumberGenerator _randomGenerator = RandomNumberGenerator.Create();

        public static int GetSecureRandomInt(int minValue, int maxValue)
        {
            if (minValue >= maxValue) throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be less than maxValue.");

            long range = (long)maxValue - minValue;
            if (range > int.MaxValue) throw new ArgumentException("Range must be ≤ Int32.MaxValue.");

            byte[] buffer = new byte[4];
            while (true)
            {
                _randomGenerator.GetBytes(buffer);
                uint randValue = BitConverter.ToUInt32(buffer, 0);
                long remainder = (uint.MaxValue + 1L) % range;

                if (randValue < uint.MaxValue + 1L - remainder)
                {
                    return (int)(minValue + randValue % range);
                }
            }
        }
    }
}