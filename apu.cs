using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        int[,] tab = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            char[] characters = Console.ReadLine().ToCharArray(); // width characters, each either 0 or .
            for (int j = 0; j < width; j++)
            {
                tab[i,j] = characters[j];
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Error.WriteLine((char)tab[i,j]);
                if (tab[i, j] == '0')
                {
                    Console.Write(j + " " + i + " ");
                    PrintRight(i, j + 1, height, width, tab);
                    PrintBot(i + 1, j, height, width, tab);
                    Console.WriteLine();
                }
            }
        }

        // Three coordinates: a node, its right neighbor, its bottom neighbor
        //Console.WriteLine("0 0 1 0 0 1");
    }

    static void PrintRight(int _i, int _j, int height, int width, int[,] tab)
    {
        for (int j = _j; j < width; j++)
        {
            if (tab[_i, j] == '0')
            {
                Console.Write(j + " " + _i + " ");
                return;
            }
        }
        Console.Write("-1 -1 ");
    }

    static void PrintBot(int _i, int _j, int height, int width, int[,] tab)
    {
        for (int i = _i; i < height; i++)
        {
            if (tab[i, _j] == '0')
            {
                Console.Write(_j + " " + i + " ");
                return;
            }
        }
        Console.Write("-1 -1 ");
    }
}