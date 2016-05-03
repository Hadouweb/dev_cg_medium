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

class BestScore
{
    public int GB { get; private set; }
    public int B { get; private set; }
    public int step { get; private set; }
    public char[][] tryMap { get; private set; }
    public char[][] saveMap { get; private set; }
    public int[] max { get; private set; }
    public int score { get; private set; }

    public BestScore(char[][] _map, int[] _max)
    {
        tryMap = _map.Select(a => a.ToArray()).ToArray();
        max = (int[])_max.Clone();
        B = 0;
        score = 0;
        CalculScore();
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

    private int Contaminate(char[][] _map, int i, int j, char c)
    {
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
        int nb_block = Contaminate2(tryMap, i, j, tryMap[i][j]);

        if (nb_block - 4 > 0)
            GB += nb_block;
        tryMap = PushColorInMap(tryMap);
        saveMap = tryMap.Select(a => a.ToArray()).ToArray();
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
                //Console.Error.WriteLine("B {0} CP {1} CB {2} GB {3}", _B, _CP, _CB, _GB);
                //Console.Error.WriteLine("Score {0}", score);
            }
        }
    }

    public void CalculScore()
    {
        saveMap = tryMap.Select(a => a.ToArray()).ToArray();
        GB = 0;
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (saveMap[i][j] != '.' && saveMap[i][j] != '0')
                {
                    B = Contaminate(saveMap, i, j, saveMap[i][j]);
                    //saveMap = tryMap.Select(a => a.ToArray()).ToArray();
                    //Console.Error.WriteLine("B {0}", B);
                    if (B > 3)
                    {   
                        step++;
                        CalculStep(i, j);
                    }
                }
            }
        }
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

class SimulationMap
{
    public List<Color> colors { get; private set; }
    public char[][] simMap { get; private set; }
    public char[][] baseMap { get; private set; }
    public int[] baseMax { get; private set; }
    public int[] simMax { get; private set; }
    public int bestCol { get; private set; }
    public int finalScore { get; private set; }
    public List<int> lastSim { get; private set; }
    public char[][] lastMap { get; private set; }


    public SimulationMap(List<Color> _colors, char[][] _baseMap, List<int> _lastSim)
    {
        colors = _colors;
        bestCol = 0;
        finalScore = 0;
        baseMap = _baseMap.Select(a => a.ToArray()).ToArray();
        lastSim = _lastSim;
        //Console.Error.WriteLine("Size {0}", lastSim.Count);

        baseMax = new int[6];
        for (int j = 0; j < 6; j++)
        {
            baseMax[j] = GetFirstFreePos(baseMap, j);
        }
        simMax = (int[])baseMax.Clone();

        if (lastSim.Count > 0)
        {
            /*foreach(int l in lastSim)
                Console.Error.Write("{0} ", l);
            Console.Error.WriteLine();*/   
            RunLastBestSimulation();
        }
        RunSimulation();
        /*foreach (int m in baseMax)
            Console.Error.Write("{0} ", m);
        Console.Error.WriteLine();
        foreach (int m in simMax)
            Console.Error.Write("{0} ", m);*/
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

    public void RunLastBestSimulation()
    {
        int currentCol = -1;
        simMax = (int[])baseMax.Clone();
        simMap = baseMap.Select(a => a.ToArray()).ToArray();
        for (int i = 0; i < lastSim.Count; i++)
        {
            int col = lastSim.ElementAt(i);
            PushPiece(colors.ElementAt(i), col);
            if (currentCol == -1)
                currentCol = col;
        }
        BestScore bs = new BestScore(simMap, simMax);
        bs.CalculScore();
        bestCol = currentCol;
        //Console.Error.WriteLine("LOL {0}", bestCol);
        lastSim.RemoveAt(0);
    }

    public void RunSimulation()
    {
        for (int i = 0; i < 2000; i++) 
        {
            int currentCol = -1;
            simMax = (int[])baseMax.Clone();
            simMap = baseMap.Select(a => a.ToArray()).ToArray();
            int seed = i;
            List<int> last = new List<int>();
            foreach (Color c in colors)
            {
                Random rnd = new Random(seed++);
                int col = rnd.Next(0,5);
                int j = 0;
                if (PushPiece(c, col) == 0)
                {
                    PushPiece(c, 0);
                }
                if (currentCol == -1)
                    currentCol = col;
                else
                    last.Add(col);
            }
            //PrintMap(simMap);
            BestScore bs = new BestScore(simMap, simMax);
            int score = bs.score;
            //Console.Error.WriteLine("bestScore {0}", score);
            if (score > finalScore)
            {
                bestCol = currentCol;
                finalScore = score;
                lastSim = last;
                //PrintMap(simMap);
                lastMap = simMap.Select(a => a.ToArray()).ToArray();
            }
            //PrintMap(simMap);
        }
        //if (finalScore > 0)
        //    Console.Error.WriteLine("bestScore {0}", finalScore);
        //PrintMap(lastMap);
        //Console.Error.WriteLine(i + 1);
        //Console.Error.WriteLine("__ {0}", lastSim.First());
    }

    public void setPieceInMap(char[][] map, int j, int j2, Color c)
    {
        simMap[simMax[j]][j] = c.ColorA;
        simMap[simMax[j2]][j2] = c.ColorB;
        simMax[j]--;
        simMax[j + 1]--;
    }

    public int PushPiece(Color c, int start)
    {
        for (int j = start; j < 5; j++)
        {
            if (simMax[j] > 0 && simMax[j + 1] > 0 && simMax[j] < 12 && simMax[j + 1] < 12)
            {
                setPieceInMap(simMap, j, j + 1, c);
                return 1;
            }
        }
        return 0;
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

    public void TryMin()
    {
        int oldMin = 0;
        bestCol = 0;
        for (int j = 0; j < 6; j++)
        {
            if (baseMax[j] > 0 && j + 1 < 6 && baseMax[j + 1] > 0)
            {
                int minA = baseMax[j];
                int minB = baseMax[j + 1];
                int min = (minA > minB) ? minA : minB;
                if (min > oldMin)
                {
                    oldMin = min;
                    bestCol = j;
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
        List<int>   lastSim = new List<int>();

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

            SimulationMap sm = new SimulationMap(colors, map, lastSim);
            lastSim = sm.lastSim;
            if (sm.baseMax[sm.bestCol] < 0 || 
                sm.baseMax[sm.bestCol + 1] < 0 || 
                sm.baseMax[sm.bestCol] > 11 || 
                sm.baseMax[sm.bestCol + 1] > 11)
            {
                Console.Error.WriteLine("ERROR");
                sm.TryMin();
            }
            /*Console.Error.Write("MAIN : ");
            foreach(int l in lastSim)
                Console.Error.Write("{0} ", l);
            Console.Error.WriteLine();*/

            Console.WriteLine("{0} {1}", sm.bestCol,  0);
        }
    }
}