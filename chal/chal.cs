using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Color 
{
    public char ColorA { get; private set; }
    public char ColorB { get; private set; }
    
    public Color(char _ColorA, char _ColorB)
    {
        ColorA = _ColorA;
        ColorB = _ColorB;
    }
}

class Solution 
{
    public int[] max { get; protected set; }
    public int col { get; protected set; }
    public int rot { get; protected set; }
    public char[][] map { get; protected set; }
    public List<Color> colors { get; protected set; }
    public Color c { get; protected set; }

    public Solution(char[][] _map, List<Color> _colors)
    {
        max = new int[6];
        col = 0;
        rot = 0;
        map = _map;
        colors = _colors;
        c = colors.First();
        for (int j = 0; j < 6; j++)
        {
            max[j] = GetFirstFreePos(map, j);
        }
    }

    public int GetFirstFreePos(char[][] map, int j)
    {
        for (int i = 0; i < 12; i++)
        {
            if (map[i][j] != '.')
                return i - 1;
        }
        return 11;
    }

    public bool IsAuthorizePos(int j, int rot)
    {
        int     i = max[j];

        if (i < 0)
            return false;
        if (rot == 0 && j + 1 < 6 && map[i][j] == '.' && map[i][j + 1] == '.')
        {
            return true;
        }
        if (rot == 1 && i - 1 > 0 && map[i][j] == '.' && map[i - 1][j] == '.')
        {
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

    public int GetMinCol()
    {
        int     maxFree = 0;

        for (int j = 0; j < 6; j++)
        {
            if (IsAuthorizePos(j, rot) && max[j] > maxFree)
            {
                //Console.Error.WriteLine("OK");
                maxFree = max[j];
                col = j;
            }
        }
        return col;
    }

    private int Contaminate(int i, int j, char c)
    {
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        if (map[i][j] == c)
        {
            map[i][j] = '.';
            return (1
                    + Contaminate(i - 1, j, c)
                    + Contaminate(i + 1, j, c)
                    + Contaminate(i, j - 1, c)
                    + Contaminate(i, j + 1, c));
        }
        return 0;
    }

    public int CountSameColor(int diff, int j, char c)
    {
        int total = 0;


        int i = max[j + diff];
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        //Console.Error.WriteLine("i : {0} j : {1}", i, j);
        if (i - 1 > 0 && map[i - 1][j] == c) 
        {
            total += Contaminate(i - 1, j, c);
        }
        if (i + 1 < 12 && map[i + 1][j] == c)
        {
            total += Contaminate(i + 1, j, c);
        }
        if (j - 1 > 0 && map[i][j - 1] == c)
        {
            total += Contaminate(i, j - 1, c);
        }
        if (j + 1 < 6 && map[i][j + 1] == c)
        {
            total += Contaminate(i, j + 1, c);
        }
        if (total > 0)
            total++;
        return total;
    }
}

class PrepareSmash : Solution
{
    public PrepareSmash(char[][] _map, List<Color> _colors)
        : base(_map, _colors)
    {

    }

    public int TryRot0()
    {
        int oldTotal = -1;
        for (int j = 0; j < 5; j++)
        {
            int nbColorA = CountSameColor(0, j, c.ColorA);
            int nbColorB = CountSameColor(0, j + 1, c.ColorB);
            int total = nbColorA + nbColorB;
            if (c.ColorA == c.ColorB)
            {
                nbColorA++;
                nbColorB++;
            }
            if (nbColorA > 3 || nbColorB > 3)
                total = 0;
            if (total > oldTotal)
            {
                oldTotal = total;
                col = j;
                rot = 0;
            }
        }
        return oldTotal;
    }
}

class Smash : Solution
{
    public bool itsTime { get; private set; }

    public Smash(char[][] _map, List<Color> _colors)
        : base(_map, _colors)
    {
        itsTime = false;
    }

    public bool ItsTimeToSmash()
    {
        for (int j = 0; j < 6; j++)
        {
            if (max[j] < 6)
            {
                itsTime = true;
            }
        }
        return itsTime;
    }

    public int GoSmashRot0()
    {
        Console.Error.WriteLine("Smash");
        int oldTotal = 0;
        int oldCol = 0;
        int result = -1;
        for (int j = 0; j < 5; j++)
        {
            int nbColorA = CountSameColor(0, j, c.ColorA);
            int nbColorB = CountSameColor(0, j + 1, c.ColorB);
            int total = nbColorA + nbColorB;
            if (c.ColorA == c.ColorB)
            {
                nbColorA++;
                nbColorB++;
            }
            if (total > oldTotal && (nbColorA > 3 || nbColorB > 3) && max[j] > oldCol)
            {
                oldTotal = total;
                oldCol = max[j];
                result = j;
            }
        }
        return result;
    }
}

class Player
{
    static void Main(string[] args)
    {
        char[][]    map = new char[12][];

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

            int col = 0;
            int rot = 0;
            int r = -1;
            Smash sm = new Smash(map, colors);
            PrepareSmash p = new PrepareSmash(map, colors);
            if (sm.ItsTimeToSmash())
            {
                r = sm.GoSmashRot0();
                if (r != -1)
                {
                    col = sm.col;
                    rot = 0;
                }
            }
            if (sm.ItsTimeToSmash() == false || r == -1)
            {
                r = p.TryRot0();
                if (r != -1)
                {
                    col = p.col;
                    rot = p.rot;
                }
            }
            if (r == -1)
            {
                p.GetMinCol();
                col = p.col;
                rot = p.rot;
            }
            
            //GoSmash(map, c, max);
            //GetRot0(map, c, max);
            //Console.Error.WriteLine("Rot : {0}", rot);
            //Console.Error.WriteLine("Col : {0}", col);
            Console.WriteLine("{0} {1}", col, rot); // "x": the column in which to drop your blocks
        }
    }
}