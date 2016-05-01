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

/*class Solution 
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
}*/

class Simulation
{
    public int[] max { get; protected set; }
    public char[][] map { get; protected set; }
    public List<Color> colors { get; protected set; }
    public Color c { get; protected set; }

    public Simulation(char[][] _map, List<Color> _colors)
    {
        max = new int[6];
        map = _map;
        colors = _colors;
        c = colors.First();
        for (int j = 0; j < 6; j++)
        {
            max[j] = GetFirstFreePos(map, j);
            Console.Error.Write("{0} ", max[j]);
        }
        Console.Error.WriteLine();
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

        //Console.Error.WriteLine("diff : {0} j : {1} i : {2}", diff, j, max[j]);
        if (j + diff > 5 || j + diff < 0)
            return -1;
        int i = max[j + diff];
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return -1;
        if (map[i][j] == c)
        {
            total += Contaminate(i, j, c);
        }
        /*if (i - 1 > 0 && map[i - 1][j] == c) 
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
        }*/
        if (total > 0)
            total++;
        return total;
    }

    public Tuple<int, int, int> TryRot0()
    {
        int oldTotal = 0;
        int total = 0;
        int col = -1;
        for (int j = 0; j < 5; j++)
        {
            if (IsAuthorizePos(j, 0) && IsAuthorizePos(j + 1, 0))
            {
                int totalA = CountSameColor(0, j, c.ColorA);
                int totalB = CountSameColor(0, j + 1, c.ColorB);
                if (totalA != -1 && totalB != -1)
                {
                    if (c.ColorA == c.ColorB)
                        total += 2;
                    total = totalA + totalB;
                    if (total > oldTotal)
                    {
                        oldTotal = total;
                        col = j;
                    }
                }
            }

        }
        return Tuple.Create(oldTotal, col, 0);
    }

    public Tuple<int, int, int> TryRot1()
    {
        int oldTotal = 0;
        int total = 0;
        int col = -1;
        for (int j = 0; j < 6; j++)
        {
            if (IsAuthorizePos(j, 1))
            {
                int totalA = CountSameColor(0, j, c.ColorA);
                int totalB = CountSameColor(1, j, c.ColorB);
                if (totalA != -1 && totalB != -1)
                {
                    if (c.ColorA == c.ColorB)
                        total += 2;
                    total = totalA + totalB;
                    if (total > oldTotal)
                    {
                        oldTotal = total;
                        col = j;
                    }
                }
            }

        }
        return Tuple.Create(oldTotal, col, 1);
    }

    public Tuple<int, int, int> TryRot2()
    {
        int oldTotal = 0;
        int total = 0;
        int col = -1;
        for (int j = 1; j < 6; j++)
        {
            if (IsAuthorizePos(j, 2) && IsAuthorizePos(j - 1, 2))
            {
                int totalA = CountSameColor(0, j, c.ColorA);
                int totalB = CountSameColor(0, j - 1, c.ColorB);
                if (totalA != -1 && totalB != -1)
                {
                    if (c.ColorA == c.ColorB)
                        total += 2;
                    total = totalA + totalB;
                    if (total > oldTotal)
                    {
                        oldTotal = total;
                        col = j;
                    }
                }
            }

        }
        return Tuple.Create(oldTotal, col, 2);
    }

    public Tuple<int, int, int> TryRot3()
    {
        int oldTotal = 0;
        int total = 0;
        int col = -1;
        for (int j = 0; j < 6; j++)
        {
            if (IsAuthorizePos(j, 3))
            {
                int totalA = CountSameColor(0, j, c.ColorA);
                int totalB = CountSameColor(-1, j, c.ColorB);
                if (totalA != -1 && totalB != -1)
                {
                    if (c.ColorA == c.ColorB)
                        total += 2;
                    total = totalA + totalB;
                    if (total > oldTotal)
                    {
                        oldTotal = total;
                        col = j;
                    }
                }
            }

        }
        return Tuple.Create(oldTotal, col, 3);
    }
}

class BestSimulation : Simulation
{
    public int bestCol { get; private set; }
    public int bestRot { get; private set; }

    public BestSimulation(char[][] _map, List<Color> _colors)
        :base(_map, _colors)
    {
    }

    public void TryRot()
    {
        Tuple<int, int, int>[] rot = new Tuple<int, int, int>[4];
        rot[0] = TryRot0();
        rot[1] = TryRot1();
        rot[2] = TryRot2();
        rot[3] = TryRot3();

        int bestScore = 0;
        foreach (var r1 in rot)
        {
            foreach (var r2 in rot)
            {
                if (r1.Item1 > r2.Item1 && r1.Item1 > bestScore)
                {
                    bestScore = r1.Item1;
                    bestCol = r1.Item2;
                    bestRot = r1.Item3;
                }
            }
            Console.Error.WriteLine("Total : {0} Col : {1} Rot : {2}", r1.Item1, r1.Item2, r1.Item3);
        }
        //Console.Error.WriteLine("Total : {0} Col : {1} Rot : {2}", r.Item1, r.Item2, r.Item3);
        Console.Error.WriteLine("bestScore : {0}, bestCol : {1} bestRot : {2}", bestScore, bestCol, bestRot);
    }

    /*public void GetMin()
    {
        Tuple<int, int>[] min = new Tuple<int, int>[4];
        min[0] = TryMinRot0();
        min[1] = TryMinRot1();
        min[2] = TryMinRot2();
        min[3] = TryMinRot3();

        foreach (var r1 in min)
        {
            foreach (var r2 in min)
            {
                if (r1.Item1 > r2.Item2)
                {
                    bestCol = r1.Item1;
                    bestRot = r1.Item2;
                }
            }
            //Console.Error.WriteLine("Total : {0} Col : {1} Rot : {2}", r1.Item1, r1.Item2, r1.Item3);
        }
        //Console.Error.WriteLine("Total : {0} Col : {1} Rot : {2}", r.Item1, r.Item2, r.Item3);
        //Console.Error.WriteLine("bestScore : {0}, bestCol : {1} bestRot : {2}", bestScore, bestCol, bestRot);
    }*/
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

            BestSimulation s = new BestSimulation(map, colors);
            s.TryRot();

            Console.WriteLine("{0} {1}", s.bestCol, s.bestRot); // "x": the column in which to drop your blocks
        }
    }
}