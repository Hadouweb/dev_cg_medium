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

    static int GetFirstFreePos(char[][] map, int j)
    {
        for (int i = 0; i < 12; i++)
        {
            if (map[i][j] != '.')
                return i - 1;
        }
        return 0;
    }

    static void Main(string[] args)
    {
        char[][]    map = new char[12][];
        int[]       max = new int[6];

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
            for (int j = 0; j < 6; j++)
            {
                max[j] = GetFirstFreePos(map, j);
            }
            for (int j = 0; j < 6; j++)
            {
                //Console.Error.WriteLine("Col : {0} Max : {1} Value : {2}", j, max[j], map[max[j]][j]);
            }


            int rot = GetRot0(map, c, max);
            int col = GetMinCol(map, c, rot);
            col = rot;
            //Console.Error.WriteLine("Rot : {0}", rot);
            //Console.Error.WriteLine("Col : {0}", col);
            Console.WriteLine("{0} {1}", col, 0); // "x": the column in which to drop your blocks
        }
    }

    static int Contaminate(char[][] map, int i, int j, char c)
    {
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        //Console.Error.WriteLine("{0} {1} {2} {3}", i, j, map[i][j], c);
        if (map[i][j] == c)
        {
            map[i][j] = '.';
            return (2
                    + CountSameColor(map, i - 1, j, c)
                    + CountSameColor(map, i + 1, j, c)
                    + CountSameColor(map, i, j - 1, c)
                    + CountSameColor(map, i, j + 1, c));
        }
        return 0;
    }

    static int CountSameColor(char[][] map, int i, int j, char c)
    {
        int total = 0;
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        //Console.Error.WriteLine("{0} {1} {2} {3}", i, j, map[i][j], c);
        if (i - 1 > 0 && map[i - 1][j] == c) 
        {
            total += Contaminate(map, i - 1, j, c);
        }
        if (i + 1 < 11 && map[i + 1][j] == c)
        {
            total += Contaminate(map, i + 1, j, c);
        }
        if (j - 1 > 0 && map[i][j - 1] == c)
        {
            total += Contaminate(map, i, j - 1, c);
        }
        if (j + 1 < 5 && map[i][j + 1] == c)
        {
            total += Contaminate(map, i, j + 1, c);
        }

        if (i - 1 > 0 && j - 1 > 0 && map[i - 1][j - 1] == c) 
        {
            total += Contaminate(map, i - 1, j - 1, c);
        }
        if (i - 1 > 0 && j + 1 < 5 && map[i - 1][j + 1] == c)
        {
            total += Contaminate(map, i - 1, j + 1, c);
        }
        if (i + 1 < 11 && j - 1 > 0 && map[i + 1][j - 1] == c) 
        {
            total += Contaminate(map, i = 1, j - 1, c);
        }
        if (i + 1 < 11 && j + 1 < 5 && map[i + 1][j + 1] == c) 
        {
            total += Contaminate(map, i + 1, j + 1, c);
        }
        return total;
    }

    static int GetRot0(char[][] map, Color c, int[] max)
    {
        int     oldTotal = 0;
        int     col = 0;

        for (int j = 0; j < 5; j++)
        {
            if (max[j] != -1)
            {
                char  posA = map[max[j]][j];
                char  posB = map[max[j]][j + 1];

                int nbColorA = CountSameColor(map, max[j], j, c.ColorA);
                int nbColorB = CountSameColor(map, max[j + 1], j + 1, c.ColorB);
                int total = nbColorA + nbColorB;
                if (total > oldTotal)
                {
                    oldTotal = total;
                    col = j;
                }
                Console.Error.WriteLine("TotalA : {0} ColorA : {1} | TotalB : {2} ColorB : {3} Col : {4}", 
                    nbColorA, c.ColorA, nbColorB, c.ColorB, j);
            }
            //Console.Error.Write(map[i][j]);
        }
        if (oldTotal == 0)
            return GetMinCol(map, c, 0);
        return col;
    }

    static int GetRot2(char[][] map, Color c)
    {
        for (int j = 0; j < 5; j++)
        {
            char  firstCharA = GetFirstHeightPos(map, j, 0);
            char  firstCharB = GetFirstHeightPos(map, j + 1, 0);
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

    static char GetFirstHeightPos(char[][] map, int j, int rot)
    {
        for (int i = 0; i < 12; i++)
        {
            if (map[i][j] != '.' && map[i][j] != '0')
                return map[i][j];
        }
        return 'e';
    }

    static bool IsAuthorizePos(char[][] map, int j, int rot)
    {
        int     i = GetFirstFreePos(map, j);

        if (i < 0)
            return false;
        //Console.Error.WriteLine("i : {0} j : {1} rot : {2}", i, j, rot);
        if (rot == 0 && j + 1 < 6 && map[i][j] == '.' && map[i][j + 1] == '.')
        {
            return true;
        }
        if (rot == 1 && i - 1 > 0 && map[i][j] == '.' && map[i - 1][j] == '.')
        {
            //Console.Error.WriteLine("OK");
            return true;
        }
        if (rot == 2 && j - 1 >= 0 && map[i][j] == '.' && map[i][j - 1] == '.')
        {
            return true;
        }
        if (rot == 3 && i > 0 && map[i][j] == '.' && map[i - 1][j] == '.')
        {
            return true;
        }
        return false;
    }

    static int GetHeightCol(char[][] map, Color c, int rot)
    {
        int     col = 0;

        //Console.Error.WriteLine("TEST1");
        for (int j = 0; j < 6; j++)
        {
            char    firstChar = GetFirstHeightPos(map, j, rot);
            int     firstFree = GetFirstFreePos(map, j);

            if (c.ColorA == firstChar && IsAuthorizePos(map, j, rot))
            {
                return j;
            }
        }
        return GetWidthCol(map, c, rot);
    }

    static int GetWidthCol(char[][] map, Color c, int rot)
    {
        //Console.Error.WriteLine("TEST2");
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (c.ColorA == map[i][j] && IsAuthorizePos(map, j, rot))
                {
                    if (j - 1 > 0 && map[i][j - 1] == '.')
                        return j - 1;
                    if (j + 1 < 6 && map[i][j + 1] == '.')
                        return j + 1;
                }
            }
        }
        return GetMinCol(map, c, rot);
    }

    static int GetMinCol(char[][] map, Color c, int rot)
    {
        int     maxFree = 0;
        int     col = 0;

        //Console.Error.WriteLine("TEST3");
        for (int j = 0; j < 6; j++)
        {
            int     firstFree = GetFirstFreePos(map, j);

            if (IsAuthorizePos(map, j, rot) && firstFree > maxFree)
            {
                //Console.Error.WriteLine("OK");
                maxFree = firstFree;
                col = j;
            }
        }
        return col;
    }
}