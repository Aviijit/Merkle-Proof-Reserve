using System;
using System.Security.Cryptography;
using System.Text;

namespace MerkleLib
{
    public static class TaggedHash
    {
        public static byte[] Compute(string tag, byte[] message)
        {
            using var sha = SHA256.Create();
            var tagHash = sha.ComputeHash(Encoding.UTF8.GetBytes(tag));
            var input = new byte[tagHash.Length * 2 + message.Length];
            Buffer.BlockCopy(tagHash, 0, input, 0, tagHash.Length);
            Buffer.BlockCopy(tagHash, 0, input, tagHash.Length, tagHash.Length);
            Buffer.BlockCopy(message, 0, input, tagHash.Length * 2, message.Length);
            return sha.ComputeHash(input);
        }

        public static string ComputeHex(string tag, byte[] message)
            => Convert.ToHexString(Compute(tag, message)).ToLowerInvariant();
    }
}
