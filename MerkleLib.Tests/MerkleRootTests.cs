using Xunit;
using MerkleLib;

namespace MerkleLib.Tests
{
    public class MerkleRootTests
    {
        [Fact]
        public void RootMatchesExpectedForSample()
        {
            var values = new[] { "aaa", "bbb", "ccc", "ddd", "eee" };
            var root = MerkleTree.ComputeRootHex(values, "Bitcoin_Transaction", "Bitcoin_Transaction");
            Assert.Equal("4aa906745f72053498ecc74f79813370a4fe04f85e09421df2d5ef760dfa94b5", root);
        }
    }
}
