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

public class Coordinate
{
    public Coordinate(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public int x { get; private set; }
    public int y { get; private set; }
}

class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        int[,] map = new int[surfaceN,2];
        int sY = 0;
        int[,] goal = new int[2,2];
        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            map[i,0] = int.Parse(inputs[0]);
            map[i,1] = int.Parse(inputs[1]);
            for (int j = 0; j < i; j++)
            {
                if (map[j, 1] == map[i, 1] && map[i, 1] != 0)
                {
                    sY = map[i, 1];
                }
            }
        }

        int k = 0;
        for (int i = 0; i < surfaceN; i++)
        {
            if (map[i, 1] == sY)
            {
                goal[k, 0] = map[i, 0];
                goal[k, 1] = map[i, 1];
                k++;
                if (k == 2)
                    k = 1;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            //Console.Error.WriteLine("__ {0}, {1}", goal[i, 0], goal[i, 1]);
        }
        
        int gX = goal[0, 0] + ((goal[1, 0] - goal[0, 0]) / 2);
        int gY = goal[0, 1]; 
        Console.Error.WriteLine("__ {0}, {1}", gX, gY);

        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).
            int r = 0;
            int p = 0;


            if (X - gX > 500)
                r = 15;
            else if (X - gX < -500)
                r = -15;
            else
                r = 0;
            int dist = (X - gX > 0) ? X - gX : -(X - gX);
             Console.Error.WriteLine("{0}, {1} {2}", X, Y, dist);
            if (dist > 1000)
                p = 3;
            else if (vSpeed > 60)
                p = 3;
            else if (vSpeed > 30)
                p = 2;
            else if (vSpeed > 10)
                p = 2;
            else
                p = 2;

            // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            Console.WriteLine("{0} {1}", r, p);
        }
    }
}