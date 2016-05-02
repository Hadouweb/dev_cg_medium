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

class Score
{
    public int[] destroyedColor { get; private set; }
    public int GB { get; private set; }
    public int B { get; private set; }
    public int step { get; private set; }
    public char[][] tryMap { get; private set; }
    public char[][] saveMap { get; private set; }
    public Color c { get; private set; }
    public int[] max { get; private set; }
    public int score { get; private set; }

    public Score(char[][] _map, Color _c, int[] _max)
    {
        tryMap = _map.Select(a => a.ToArray()).ToArray();
        c = _c;
        max = _max;
        B = 0;
        score = 0;
        destroyedColor = Enumerable.Repeat(0, 5).ToArray();
    }

    public void PrintMap(char[][] map)
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Error.Write(" {0}", map[i][j]);
            }
            Console.Error.WriteLine();
        }
        Console.Error.WriteLine("____________");
    }

    public int CalculCB()
    {
        int cb = -1;
        foreach (var c in destroyedColor)
        {
            if (c == 1)
            {
                cb++;
            }
        }
        return cb;
    }

    private int Contaminate(char[][] _map, int i, int j, char c)
    {
        //Console.Error.WriteLine("i {0} j {1}", i, j);
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        if (_map[i][j] == c)
        {
            _map[i][j] = '.';
            return (1
                    + Contaminate(_map, i - 1, j, c)
                    + Contaminate(_map, i + 1, j, c)
                    + Contaminate(_map, i, j - 1, c)
                    + Contaminate(_map, i, j + 1, c));
        }
        return 0;
    }

    public char[][] PushColorInMap(char[][] _map)
    {
        char[][] mapClean = Enumerable.Range(0, 12)
            .Select(x => Enumerable.Repeat('.', 6).ToArray()).ToArray();
        //PrintMap(mapClean);
        int[] m = Enumerable.Repeat(11, 6).ToArray();
        for (int j = 0; j < 6; j++)
        {
            for (int i = 11; i >= 0; i--)
            {
                if (_map[i][j] != '.')
                {
                    mapClean[m[j]][j] = _map[i][j];
                    m[j]--;
                }
            }
        }
        //PrintMap(mapClean);
        return (mapClean);
    }

    public int CalculCP()
    {
        int     cp = 0;

        if (step > 1)
            cp = 8;
        for (int i = 2; i < step; i++)
            cp *= 2;
        return cp;
    }

    public void CalculStep(int i, int j)
    {
        //PrintMap(saveMap);
        //destroyedColor[saveMap[i][j] - '1'] = 1;
        int nb_block = Contaminate2(tryMap, i, j, tryMap[i][j]);

        if (nb_block - 4 > 0)
            GB += nb_block;
        //Console.Error.WriteLine("GB : {0}", gb);
        tryMap = PushColorInMap(tryMap);
        saveMap = tryMap.Select(a => a.ToArray()).ToArray();
        //PrintMap(saveMap);

        if (B > 3)
        {
            int _B = B;
            int _CP = CalculCP();
            int _CB = 0;
            int _GB = GB;
            int C = _CP + _CB + _GB; 
            if (C <= 0)
                C = 1;
            else if (C > 999)
                C = 999;
            score += (10 * _B) * C;
            if (score > 0)
            {
                Console.Error.WriteLine("C2 : {0} ColorA : {1} ColorB : {2}", C, c.ColorA, c.ColorB);
                Console.Error.WriteLine("B {0} CP {1} CB {2} GB {3}", _B, _CP, _CB, _GB);
                Console.Error.WriteLine("Score {0}", score);
            }
        }
    }

    public void CalculScore(int _i, int _j, int _i2, int _j2, int rot)
    {
        tryMap[_i][_j] = c.ColorA;
        tryMap[_i2][_j2] = c.ColorB;
        saveMap = tryMap.Select(a => a.ToArray()).ToArray();
        GB = 0;
        //Console.Error.WriteLine("__ROT : {0}", rot);
        //PrintMap(saveMap);
        //Console.Error.WriteLine("i : {0} j : {1}", max[col], col);
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (saveMap[i][j] != '.' && saveMap[i][j] != '0')
                {
                    B = Contaminate(saveMap, i, j, saveMap[i][j]);
                    saveMap = tryMap.Select(a => a.ToArray()).ToArray();
                    if (B > 3)
                    {   
                        step++;
                        CalculStep(i, j);
                        //PrintMap(tryMap);
                    }
                }
                //Console.Error.Write(" {0}", tryMap[i][j]);
            }
            //Console.Error.WriteLine();
        }
        //Console.Error.WriteLine("B {0} step {1}", B, step);
        //Console.Error.WriteLine("____________");
    }

    private int Contaminate2(char[][] _map, int i, int j, char c)
    {
        //Console.Error.WriteLine("i {0} j {1}", i, j);
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        if (_map[i][j] == '0')
        {
            _map[i][j] = '.';
            return 0;
        }
        if (_map[i][j] == c)
        {
            _map[i][j] = '.';
            return (Contaminate2(_map, i - 1, j, c)
                    + Contaminate2(_map, i + 1, j, c)
                    + Contaminate2(_map, i, j - 1, c)
                    + Contaminate2(_map, i, j + 1, c));
        }
        return 0;
    }
}

class Simulation
{
    public int[] max { get; protected set; }
    public char[][] map { get; protected set; }
    public char[][] tryMap { get; protected set; }
    public List<Color> colors { get; protected set; }
    public Color c { get; protected set; }
    public Score s { get; private set; }

    public Simulation(char[][] _map, List<Color> _colors)
    {
        max = new int[6];
        map = _map;
        tryMap = map;
        colors = _colors;
        c = colors.First();
        for (int j = 0; j < 6; j++)
        {
            max[j] = GetFirstFreePos(map, j);
            //Console.Error.Write("{0} ", max[j]);
        }
        //Console.Error.WriteLine();
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

    private int Contaminate(char[][] _map, int i, int j, char c)
    {
        //Console.Error.WriteLine("i {0} j {1}", i, j);
        if (i < 0 || i > 11 || j < 0 || j > 5)
            return 0;
        if (_map[i][j] == c)
        {
            //Console.Error.WriteLine("i {0} j {1} Color : {2}", i, j, c);
            _map[i][j] = '.';
            return (1
                    + Contaminate(_map, i - 1, j, c)
                    + Contaminate(_map, i + 1, j, c)
                    + Contaminate(_map, i, j - 1, c)
                    + Contaminate(_map, i, j + 1, c));
        }
        return 0;
    }

    /*public int CountSameColor(char[][] _map, int i, int j, char c)
    {
        int         total = 0;
        char[][]    saveMap = _map.Select(a => a.ToArray()).ToArray();

        if (i < 0 || i > 11 || j < 0 || j > 5)
            return -1;
        total += Contaminate(saveMap, i + 1, j, c);
        saveMap = _map.Select(a => a.ToArray()).ToArray();
        total += Contaminate(saveMap, i, j - 1, c);
        saveMap = _map.Select(a => a.ToArray()).ToArray();
        total += Contaminate(saveMap, i, j + 1, c);
        saveMap = _map.Select(a => a.ToArray()).ToArray();
        if (total > 0)
            total++;
        return total;
    }*/

    public void PrintMap(char[][] map)
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Error.Write(" {0}", map[i][j]);
            }
            Console.Error.WriteLine();
        }
        Console.Error.WriteLine("____________");
    }

    public Tuple<int, int, int> TryRot0()
    {
        int oldScore = 0;
        int col = -1;
        for (int j = 0; j < 5; j++)
        {
            if (max[j] >= 0 && max[j + 1] >= 0)
            {
                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j + 1]][j + 1] = c.ColorB;
                //PrintMap(tryMap);
                int totalA = Contaminate(tryMap, max[j], j, c.ColorA);

                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j + 1]][j + 1] = c.ColorB;
                //PrintMap(tryMap);
                int totalB = Contaminate(tryMap, max[j + 1], j + 1, c.ColorB);

                //Console.Error.WriteLine("totalA : {0} totalB : {1}", totalA, totalB);
                //PrintMap(map);
                s = new Score(map, c, max);
                s.CalculScore(max[j], j, max[j + 1], j + 1, 0);
                if (s.score > oldScore)
                {
                    Console.Error.WriteLine("Score : {0} Rot 0", s.score);
                    oldScore = s.score;
                    col = j;
                }
            }
        }
        return Tuple.Create(oldScore, col, 0);
    }

    public Tuple<int, int, int> TryRot1()
    {
        int oldScore = 0;
        int col = -1;
        for (int j = 0; j < 6; j++)
        {
            if (max[j] - 1 > 0)
            {
                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j] - 1][j] = c.ColorB;
                //PrintMap(tryMap);
                int totalA = Contaminate(tryMap, max[j], j, c.ColorA);

                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j] - 1][j] = c.ColorB;
                //PrintMap(tryMap);
                int totalB = Contaminate(tryMap, max[j] - 1, j, c.ColorB);

                //Console.Error.WriteLine("totalA : {0} totalB : {1}", totalA, totalB);
                //PrintMap(map);
                s = new Score(map, c, max);
                s.CalculScore(max[j], j, max[j], j, 1);
                if (s.score > oldScore)
                {
                    Console.Error.WriteLine("Score : {0} Rot 1", s.score);
                    oldScore = s.score;
                    col = j;
                }
            }
        }
        return Tuple.Create(oldScore, col, 1);
    }

    public Tuple<int, int, int> TryRot2()
    {
        int oldScore = 0;
        int col = -1;
        for (int j = 1; j < 6; j++)
        {
            if (max[j] > 0 && max[j - 1] > 0)
            {
                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j - 1]][j - 1] = c.ColorB;
                //PrintMap(tryMap);
                int totalA = Contaminate(tryMap, max[j], j, c.ColorA);

                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j - 1]][j - 1] = c.ColorB;
                //PrintMap(tryMap);
                int totalB = Contaminate(tryMap, max[j - 1], j - 1, c.ColorB);

                //Console.Error.WriteLine("totalA : {0} totalB : {1}", totalA, totalB);
                //PrintMap(map);
                s = new Score(map, c, max);
                s.CalculScore(max[j], j, max[j - 1], j - 1, 2);
                if (s.score > oldScore)
                {
                    Console.Error.WriteLine("Score : {0} Rot 2", s.score);
                    oldScore = s.score;
                    col = j;
                }
            }
        }
        return Tuple.Create(oldScore, col, 2);
    }

    public Tuple<int, int, int> TryRot3()
    {
        int oldScore = 0;
        int col = -1;
        for (int j = 0; j < 6; j++)
        {
            if (max[j] + 1 < 11 && max[j] > 0)
            {
                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j] + 1][j] = c.ColorB;
                //PrintMap(tryMap);
                int totalA = Contaminate(tryMap, max[j], j, c.ColorA);

                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j] + 1][j] = c.ColorB;
                //PrintMap(tryMap);
                int totalB = Contaminate(tryMap, max[j] + 1, j, c.ColorB);

                //Console.Error.WriteLine("totalA : {0} totalB : {1}", totalA, totalB);
                //PrintMap(map);
                s = new Score(map, c, max);
                s.CalculScore(max[j] - 1, j, max[j], j, 3);
                if (s.score > oldScore)
                {
                    Console.Error.WriteLine("Score : {0} Rot 3", s.score);
                    oldScore = s.score;
                    col = j;
                }
            }
        }
        return Tuple.Create(oldScore, col, 3);
    }

    /*public Tuple<int, int, int> TryRot2()
    {
        int oldTotal = 0;
        int total = 0;
        int col = -1;
        for (int j = 1; j < 6; j++)
        {
            if (max[j] > 0 && max[j - 1] > 0)
            {
                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j - 1]][j - 1] = c.ColorB;
                //PrintMap(tryMap);
                //tryMap = map.Select(a => a.ToArray()).ToArray();
                int totalA = CountSameColor(tryMap, max[j], j, c.ColorA);
                int totalB = -1;
                if (totalA != -1)
                {
                    tryMap = map.Select(a => a.ToArray()).ToArray();
                    tryMap[max[j]][j] = c.ColorA;
                    tryMap[max[j - 1]][j - 1] = c.ColorB;
                    //PrintMap(tryMap);   
                    totalB = CountSameColor(map, max[j - 1], j - 1, c.ColorB);
                    if (totalB != -1)
                    {
                        total = totalA + totalB;
                        if (c.ColorA == c.ColorB)
                            total /= 2;
                        if (totalA > 3)
                            total *= 2;
                        if (totalB > 3)
                            total *= 2;
                        if (total >= oldTotal)
                        {
                            oldTotal = total;
                            col = j;
                        }
                        //Console.Error.WriteLine("Total : {0} Col : {1} totalA : {2} totalB {3} colA : {4} colB: {5}", 
                        //    total, col, totalA, totalB, j, j + 1);
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
            if (max[j] + 1 < 11 && max[j] > 0)
            {
                tryMap = map.Select(a => a.ToArray()).ToArray();
                tryMap[max[j]][j] = c.ColorA;
                tryMap[max[j] + 1][j] = c.ColorB;
                //PrintMap(tryMap);
                //tryMap = map.Select(a => a.ToArray()).ToArray();
                int totalA = CountSameColor(tryMap, max[j], j, c.ColorA);
                int totalB = -1;
                if (totalA != -1)
                {
                    tryMap = map.Select(a => a.ToArray()).ToArray();
                    tryMap[max[j]][j] = c.ColorA;
                    tryMap[max[j] + 1][j] = c.ColorB;
                    //PrintMap(tryMap);   
                    totalB = CountSameColor(map, max[j] + 1, j, c.ColorB);
                    if (totalB != -1)
                    {
                        total = totalA + totalB;
                        if (c.ColorA == c.ColorB)
                            total /= 2;
                        if (totalA > 3)
                            total *= 2;
                        if (totalB > 3)
                            total *= 2;
                        if (total >= oldTotal)
                        {
                            oldTotal = total;
                            col = j;
                        }
                        //Console.Error.WriteLine("Total : {0} Col : {1} totalA : {2} totalB {3}", 
                        //    total, col, totalA, totalB);
                    }
                }
            }
        }
        return Tuple.Create(oldTotal, col, 3);
    }*/
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
                if (r1.Item1 >= r2.Item1 && r1.Item1 > bestScore)
                {
                    bestScore = r1.Item1;
                    bestCol = r1.Item2;
                    bestRot = r1.Item3;
                }
            }
            //Console.Error.WriteLine("Total : {0} Col : {1} Rot : {2}", r1.Item1, r1.Item2, r1.Item3);
        }
        //Console.Error.WriteLine("Total : {0} Col : {1} Rot : {2}", r.Item1, r.Item2, r.Item3);
        //Console.Error.WriteLine("bestScore : {0}, bestCol : {1} bestRot : {2}", bestScore, bestCol, bestRot);
        if (bestScore == 0)
            TryMin();
    }

    public void TryMin()
    {
        int oldMin = 0;
        bestCol = 0;
        bestRot = 0;
        for (int j = 0; j < 6; j++)
        {
            if (max[j] > 0 && j + 1 < 6 && max[j + 1] > 0)
            {
                int minA = max[j];
                int minB = max[j + 1];
                int min = (minA > minB) ? minA : minB;
                if (min > oldMin)
                {
                    oldMin = min;
                    bestCol = j;
                    bestRot = 0;
                }
            }
            if (max[j] - 1 > 0)
            {
                int min = max[j] - 1;
                if (min > oldMin)
                {
                    oldMin = min;
                    bestCol = j;
                    bestRot = 1;
                }
            }
            if (max[j] > 0 && j - 1 > 0 && max[j - 1] > 0)
            {
                int min = max[j] - 1;
                if (min > oldMin)
                {
                    oldMin = min;
                    bestCol = j;
                    bestRot = 1;
                }
            }
            if (max[j] + 1 < 11)
            {
                int min = max[j] - 1;
                if (min > oldMin)
                {
                    oldMin = min;
                    bestCol = j;
                    bestRot = 1;
                }
            }
        }
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

            BestSimulation s = new BestSimulation(map, colors);
            s.TryRot();

            Console.WriteLine("{0} {1}", s.bestCol, s.bestRot); // "x": the column in which to drop your blocks
        }
    }
}