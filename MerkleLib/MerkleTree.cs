using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerkleLib
{
    public static class MerkleTree
    {
        private static byte[] Concat(byte[] left, byte[] right)
        {
            var result = new byte[left.Length + right.Length];
            Buffer.BlockCopy(left, 0, result, 0, left.Length);
            Buffer.BlockCopy(right, 0, result, left.Length, right.Length);
            return result;
        }

        public static byte[] ComputeRoot(IEnumerable<string> values, string tagLeaf, string tagBranch)
        {
            var leaves = values.Select(v => TaggedHash.Compute(tagLeaf, Encoding.UTF8.GetBytes(v))).ToList();
            if (leaves.Count == 0)
                return new byte[32];
            var level = leaves;
            while (level.Count > 1)
            {
                if (level.Count % 2 == 1)
                    level.Add(level[^1]);
                var next = new List<byte[]>(level.Count / 2);
                for (int i = 0; i < level.Count; i += 2)
                {
                    next.Add(TaggedHash.Compute(tagBranch, Concat(level[i], level[i + 1])));
                }
                level = next;
            }
            return level[0];
        }

        public static string ComputeRootHex(IEnumerable<string> values, string tagLeaf, string tagBranch)
            => Convert.ToHexString(ComputeRoot(values, tagLeaf, tagBranch)).ToLowerInvariant();

        public static List<(byte[] Hash, bool IsRight)> GetProof(IEnumerable<string> values, int index, string tagLeaf, string tagBranch)
        {
            var leaves = values.Select(v => TaggedHash.Compute(tagLeaf, Encoding.UTF8.GetBytes(v))).ToList();
            var proof = new List<(byte[] Hash, bool IsRight)>();
            var idx = index;
            var level = leaves;
            while (level.Count > 1)
            {
                if (level.Count % 2 == 1)
                    level.Add(level[^1]);
                int siblingIndex = idx ^ 1;
                bool isRight = siblingIndex > idx;
                proof.Add((level[siblingIndex], isRight));
                var next = new List<byte[]>(level.Count / 2);
                for (int i = 0; i < level.Count; i += 2)
                {
                    next.Add(TaggedHash.Compute(tagBranch, Concat(level[i], level[i + 1])));
                }
                idx /= 2;
                level = next;
            }
            return proof;
        }
    }
}
