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
    public struct Color
    {
        public char ColorA { get; private set; }
        public char ColorB { get; private set; }
        
        public Color(char _ColorA, char _ColorB) : this()
        {
            this.ColorA = _ColorA;
            this.ColorB = _ColorB;
        }
    }

    static void Main(string[] args)
    {
        char[][] map = new char[12][]; 

        while (true)
        {
            var     colors = new List<Color>();
            for (int i = 0; i < 8; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                colors.Add(new Color(char.Parse(inputs[0]), char.Parse(inputs[1])));
            }
            for (int i = 0; i < 12; i++)
            {
                map[i] = Console.ReadLine().ToCharArray();
            }
            for (int i = 0; i < 12; i++)
            {
                string ennemy = Console.ReadLine(); // One line of the map ('.' = empty, '0' = skull block, '1' to '5' = colored block)
            }
            //colors.ForEach(color => Console.Error.WriteLine("ColorA {0}, ColorB : {1}", color.ColorA, color.ColorB));
            Color c = colors.First();
            int col = GetBestCol(map, c);
            //Console.Error.WriteLine("Col resul : {0}", col);
            Console.WriteLine(col); // "x": the column in which to drop your blocks
            colors.RemoveAt(0);
        }
    }

    static int GetBestCol(char[][] map, Color c)
    {
        int     col = 0;

        //Console.Error.WriteLine("ColorA {0}, ColorB : {1}", c.ColorA, c.ColorB);
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                //Console.Error.WriteLine("TEST {0} {1}", c.ColorA, map[i][j]);
                if (c.ColorA == map[i][j])
                {
                    //Console.Error.WriteLine("TEST");
                    return j;
                }
                //Console.Error.Write(map[i][j]);
            }
            //Console.Error.WriteLine();
        }
        col = GetMinCol(map, c);
        return col;
    }

    static int GetMinCol(char[][] map, Color c)
    {
        int     minCol = 12;
        int     result = 0;

        for (int i = 11; i >= 0; i--)
        {
            for (int j = 0; j < 6; j++)
            {
                if (map[i][j] == '.')
                {
                    //Console.Error.WriteLine("TEST2 {0} {1}", i, j);
                    return j;
                }
            }
        }
        return result;
    }
}