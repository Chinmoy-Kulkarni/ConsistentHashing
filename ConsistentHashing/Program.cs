using System;
using System.Collections.Generic;

namespace ConsistentHashing
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Server> rackServers = new List<Server>();
            rackServers.Add(new Server("10.0.0.1"));
            rackServers.Add(new Server("10.0.0.2"));
            int numberOfReplicas = 2;

            ConsistentHash serverDistributor = new ConsistentHash(numberOfReplicas, rackServers);

            //add a new server to the mix
            Server newServer = new Server("10.0.0.3");
            serverDistributor.addServerToHashRing(newServer);

            //Assume you have a key "key0"
            Server serverForKey = serverDistributor.GetServerForKey("key0");

            Console.WriteLine("Server: " + serverForKey.ipAddress + " holds key: Key0");

            // Now remove a server
            serverDistributor.removeServerFromHashRing(newServer);

            // Now check on which server "key0" landed up
            serverForKey = serverDistributor.GetServerForKey("key0");

            Console.WriteLine("Server: " + serverForKey.ipAddress + " holds key: Key0");
        }
    }
}
