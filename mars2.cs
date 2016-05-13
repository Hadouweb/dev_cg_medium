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

            int relDist = X - gX;
            int dist = (X - gX > 0) ? X - gX : -(X - gX);
            int distY = Y - gY;
            if (vSpeed < 0)
                vSpeed = -vSpeed;
            Console.Error.WriteLine("{0} {1} {2} {3}", vSpeed, hSpeed, relDist, dist);

            r = 0;
            if (relDist < 1000 && hSpeed > 20)
            {
                Console.Error.WriteLine("3");
                r = hSpeed;
            }
            else if (relDist > -1000 && hSpeed < -20)
            {
                Console.Error.WriteLine("4");
                r = hSpeed;
            }
            else if (relDist > 1000)
            {
                Console.Error.WriteLine("1");
                r = -(relDist * 90 / 7000);
            }
            else if (relDist < -1000 )
            {
                Console.Error.WriteLine("2");
                r = relDist * 90 / 7000;
            }
        
            if (Y > 2800)
                p = 3;
            else
                p = 4;
            if (relDist > -1000 && relDist < 1000 && hSpeed < 10)
            {
                p = (3000 / distY);
            }
            if (vSpeed > 20 && r > 45)
                r = hSpeed - (vSpeed);
            else if (vSpeed > 20 && r < -45)
                r = hSpeed + (vSpeed);
            if (vSpeed > 20)
            {
                p = 4;
            }
            else
                p = 0;
            if (distY < 500)
                r = 0;
            if (p > 4)
                p = 4;
            if (r > 90)
                r = 90;
            if (r < - 90)
                r = - 90;

            // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            Console.WriteLine("{0} {1}", r, p);
        }
    }
}