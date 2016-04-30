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
    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
        int L = int.Parse(inputs[1]); // the number of links
        int E = int.Parse(inputs[2]); // the number of exit gateways

        List<Tuple<int, int>> links = new List<Tuple<int, int>>();
        List<int> gateways = new List<int>();
        
        for (int i = 0; i < L; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            links.Add(Tuple.Create(Int32.Parse(inputs[0]), Int32.Parse(inputs[1])));
        }
        for (int i = 0; i < E; i++)
        {
            gateways.Add(int.Parse(Console.ReadLine())); // the index of a gateway node
        }
        foreach (var link in links)
        {
            Console.Error.WriteLine("N1 : {0} N2 : {1}", link.Item1, link.Item2);
        }
        foreach (var gateway in gateways)
        {
            Console.Error.WriteLine("EI : {0}", gateway);
        }
        // game loop
        while (true)
        {
            int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
            Console.Error.WriteLine("SI : {0}",SI);
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");


            // Example: 0 1 are the indices of the nodes you wish to sever the link between
            Console.WriteLine("0 1");
        }
    }
}