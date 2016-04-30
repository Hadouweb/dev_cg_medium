using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{

    public struct SNode
    {
        public int          id;
        public bool         is_gate;
        public List<SNode>  nlinks;
    }

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways

        List<SNode> nodes = new List<SNode>();

        for (int i = 0; i < N; i++) // Create nodes
        {
            SNode n;

            n.id = i;
            n.is_gate = false;
            n.nlinks = new List<SNode>();
            nodes.Add(n);
        }

        for (int i = 0; i < L; i++) // Create links
        {
            var tab = Console.ReadLine().Split(' ').Select(str => int.Parse(str)).ToArray();

            for (int j = 0; j < N; j++) // Update nodes with links
            {
                if (tab[0] == nodes[j].id)
                {
                    var index = nodes.FindIndex(x => x.id.Equals(tab[1]));
                    var n = nodes[index];
                    nodes[j].nlinks.Add(n);
                }
                if (tab[1] == nodes[j].id)
                {
                    var index = nodes.FindIndex(x => x.id.Equals(tab[0]));
                    var n = nodes[index];
                    nodes[j].nlinks.Add(n);
                }
            }
        }

        for (int i = 0; i < E; i++) // Update nodes with gate
        {
            int gate = int.Parse(Console.ReadLine()); // the index of a gateway node
            var index = nodes.FindIndex(x => x.id.Equals(gate));
            var n = nodes[index];
            n.is_gate = true;
            nodes[index] = n;
        }

        nodes.ForEach(delegate(SNode n)
        {
            Console.Error.WriteLine("ID : {0}, IS_GATE : {1}", n.id, n.is_gate);
            n.nlinks.ForEach(delegate(SNode l)
            {
                Console.Error.WriteLine("   ID : {0}, IS_GATE : {1}", l.id, l.is_gate);
            });
        });

        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
            Console.Error.WriteLine("SI : {0}",SI);

            Console.WriteLine("0 1");
        }
    }
}