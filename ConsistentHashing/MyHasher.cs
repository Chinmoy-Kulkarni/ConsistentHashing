using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashing
{
    class MyHasher
    {
        // Taken from https://stackoverflow.com/questions/12272296/32-bit-fast-uniform-hash-function-use-md5-sha1-and-cut-off-4-bytes
        public static uint To32BitHash(string toHash)
        {
            IEnumerable<byte> bytesToHash;
            uint hash = FnvConstants.Offset32;
            bytesToHash = toHash.ToCharArray()
                                .Select(Convert.ToByte);

            foreach (var chunk in bytesToHash)
            {
                hash ^= chunk;
                hash *= FnvConstants.Prime32;
            }
            return hash;
        }

        public static class FnvConstants
        {
            public static readonly uint Prime32 = 16777619;
            public static readonly ulong Prime64 = 1099511628211;
            public static readonly uint Offset32 = 2166136261;
            public static readonly ulong Offset64 = 14695981039346656037;
        }
    }
}
