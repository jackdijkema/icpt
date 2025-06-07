

class TekstClass
{
    static void Main(string[] args)
    {

        string path = args[0];
        string[] lines = File.ReadAllLines(path.ToString());
        int y = lines.Length - 1;
        int x = 0 + 3;

        while (true)
        {

            RedrawScreen(lines, y, x);

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.RightArrow)
            {
                 if (x >= lines[y].Length + 2)
                {
                    x--;
                }
                x++;
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.LeftArrow)
            {
                if (x <= 3)
                {
                    x++;
                }

                x--;
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.DownArrow)
            {
                if (y >= lines.Length -1)
                {
                    y--;
                }

                if (x == lines[y].Length + 2)
                {
                    x = lines[y + 1].Length + 2; 
                }

                y++;
                RedrawScreen(lines, y, x);
            }
            if (key.Key == ConsoleKey.UpArrow)
            {
                if (y <= 0)
                {
                    y++;
                }
                y--;
                RedrawScreen(lines, y, x);
            }
        }
    }

    static void RedrawScreen(string[] lines, int x, int y)
    {
        Console.Clear();

        for (int i = 0; i < lines.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {lines[i]}");
        }

        Console.SetCursorPosition(y, x);
    }
}