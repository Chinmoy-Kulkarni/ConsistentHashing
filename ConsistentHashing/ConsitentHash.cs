using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashing
{
    class ConsistentHash
    {
        private SortedDictionary<uint, Server> hashRing;
        private int numberOfReplicas; // The number of virtual nodes
        public ConsistentHash(int numberOfReplicas, List<Server> servers)
        {
            this.numberOfReplicas = numberOfReplicas;
            hashRing = new SortedDictionary<uint, Server>();

            if (servers != null)
                foreach (Server s in servers)
                {
                    this.addServerToHashRing(s);
                }
        }
        public void addServerToHashRing(Server server)
        {
            for (int i = 0; i < numberOfReplicas; i++)
            {
                //Fuse the server ip with the replica number
                string serverIdentity = String.Concat(server.ipAddress, ":", i);
                //Get the hash key of the server
                uint hashKey = MyHasher.To32BitHash(serverIdentity);
                //Insert the server at the hashkey in the Sorted Dictionary
                this.hashRing.Add(hashKey, server);
            }
        }
        public void removeServerFromHashRing(Server server)
        {
            for (int i = 0; i < numberOfReplicas; i++)
            {
                //Fuse the server ip with the replica number
                string serverIdentity = String.Concat(server.ipAddress, ":", i);
                //Get the hash key of the server
                uint hashKey = MyHasher.To32BitHash(serverIdentity);
                //Insert the server at the hashkey in the Sorted Dictionary
                this.hashRing.Remove(hashKey);
            }
        }

        // Get the Physical server where a key is mapped to
        public Server GetServerForKey(String key)
        {
            Server serverHoldingKey;
            if (this.hashRing.Count == 0)
            {
                return null;
            }

            // Get the hash for the key
            uint hashKey = MyHasher.To32BitHash(key);
            if (this.hashRing.ContainsKey(hashKey))
            {
                serverHoldingKey = this.hashRing[hashKey];
            }
            else
            {
                uint[] sortedKeys = this.hashRing.Keys.ToArray();
                //Find the first server key greater than  the hashkey
                uint firstServerKey = sortedKeys.FirstOrDefault(x => x >= hashKey);
                // Get the Server at that Hashkey
                serverHoldingKey = this.hashRing[firstServerKey];
            }
            return serverHoldingKey;
        }
    }
}
