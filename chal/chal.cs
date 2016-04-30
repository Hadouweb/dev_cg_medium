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
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //Console.Error.WriteLine("TEST {0} {1}", c.ColorA, map[i][j]);
                    //Console.Error.Write(map[i][j]);
                }
                //Console.Error.WriteLine();
            }
            int rot = GetRot0(map, c);
            int col = GetHeightCol(map, c, rot);
            Console.Error.WriteLine("Rot : {0}", rot);
            //Console.Error.WriteLine("Col : {0}", col);
            Console.WriteLine("{0} {1}", col, rot); // "x": the column in which to drop your blocks
            colors.RemoveAt(0);
        }
    }

    static int GetRot0(char[][] map, Color c)
    {
        for (int j = 0; j < 5; j++)
        {
            char  firstCharA = GetFirstHeightPos(map, j);
            char  firstCharB = GetFirstHeightPos(map, j + 1);
            //Console.Error.WriteLine("firstChar : {0}", firstChar);
            //Console.Error.WriteLine("TEST {0} {1}", c.ColorA, map[i][j]);
            if (c.ColorA == firstCharA && c.ColorB == firstCharB)
            {
                //Console.Error.WriteLine("TEST");
                return 0;
            }
            //Console.Error.Write(map[i][j]);
        }
        return GetRot2(map, c);
    }

    static int GetRot2(char[][] map, Color c)
    {
        for (int j = 0; j < 5; j++)
        {
            char  firstCharA = GetFirstHeightPos(map, j);
            char  firstCharB = GetFirstHeightPos(map, j + 1);
            //Console.Error.WriteLine("firstChar : {0}", firstChar);
            //Console.Error.WriteLine("TEST {0} {1}", c.ColorA, map[i][j]);
            if (c.ColorB == firstCharA && c.ColorA == firstCharB)
            {
                //Console.Error.WriteLine("TEST");
                return 2;
            }
            //Console.Error.Write(map[i][j]);
        }
        return 1;
    }

    static int GetRot1(char[][] map, Color c)
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (c.ColorA == map[i][j])
                {
                    if (j - 1 > 0 && map[i][j - 1] == '.')
                        return 1;
                    if (j + 1 < 6 && map[i][j + 1] == '.')
                        return 1;
                }
            }
        }
        return -1;
    }

    static char GetFirstHeightPos(char[][] map, int j)
    {
        for (int i = 0; i < 12; i++)
        {
            if (map[i][j] != '.' && map[i][j] != '0')
                return map[i][j];
        }
        return 'e';
    }

    static int GetHeightCol(char[][] map, Color c, int rot)
    {
        int     col = 0;
        int     max = 6;
        int     min = 0;

        if (rot == 0)
            max = 5;
        if (rot == 2)
            min = 1;
        for (int j = min; j < max; j++)
        {
            char  firstChar = GetFirstHeightPos(map, j);
            if (c.ColorA == firstChar)
            {
                return j;
            }
        }
        return GetWidthCol(map, c, rot);
    }

    static int GetWidthCol(char[][] map, Color c, int rot)
    {
        int     max = 6;
        int     min = 0;

        if (rot == 0)
            max = 5;
        if (rot == 2)
            min = 1;
        for (int i = 0; i < 12; i++)
        {
            for (int j = min; j < max; j++)
            {
                if (c.ColorA == map[i][j])
                {
                    if (j - 1 > 0 && map[i][j - 1] == '.')
                        return j - 1;
                    if (j + 1 < max && map[i][j + 1] == '.')
                        return j + 1;
                }
            }
        }
        return GetMinCol(map, c, rot);
    }

    static int GetMinCol(char[][] map, Color c, int rot)
    {
        int     minCol = 12;
        int     max = 6;
        int     min = 0;

        if (rot == 0)
            max = 5;
        if (rot == 2)
            min = 1;
        for (int i = 11; i >= 0; i--)
        {
            for (int j = min; j < max; j++)
            {
                if (map[i][j] == '.')
                {
                    return j;
                }
            }
        }
        return 0;
    }
}