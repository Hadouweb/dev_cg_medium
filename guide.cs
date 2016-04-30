class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        
        List<Tuple<int, int>> nodes = new List<Tuple<int, int>>();
        for (int i = 0; i < height; i++)
        {
            string line = Console.ReadLine(); // width characters, each either 0 or .
            
            for (int j =0; j < line.Length; j++)
                if (line.ToCharArray()[j] == '0')
                    nodes.Add(Tuple.Create(j, i));
        }
        

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        foreach (Tuple<int, int> node in nodes)
        {
            var down = nodes.FirstOrDefault(t => t.Item1 == node.Item1 && t.Item2 > node.Item2) ?? Tuple.Create(-1, -1);
            var right = nodes.FirstOrDefault(t => t.Item1 > node.Item1 && t.Item2 == node.Item2) ?? Tuple.Create(-1, -1);
            Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5}", node.Item1, node.Item2, right.Item1, right.Item2, down.Item1, down.Item2));
        }
        
        Console.WriteLine("0 0 1 0 0 1"); // Three coordinates: a node, its right neighbor, its bottom neighbor
    }
}