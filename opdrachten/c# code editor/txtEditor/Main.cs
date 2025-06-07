

using System.Collections;
using System.Runtime.InteropServices;


string path = args[0];
string[] lines = File.ReadAllLines(path.ToString());
int y = lines.Length - 1;
int x = 0 + 3;

List<string> test = new List<string>();

while (true)
{

    RedrawScreen(lines, y, x);

    var key = Console.ReadKey(true);

    if (key.Key == ConsoleKey.RightArrow)
    {
        if (x >= lines[y].Length + 3)
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
        if (y >= lines.Length - 1)
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

    bool insertMode = false;

    if (key.Key == ConsoleKey.I)
    {
        insertMode = true;

        RedrawScreen(lines, y, x);
        while (insertMode)
        {
            Console.Write("\x1b[5 q");
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
            {
                insertMode = false;
                Console.Write("\x1b[1 q");
                RedrawScreen(lines, y, x);
                Save(lines, path);
                continue;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (x > 0 + 3)
                {
                    x--;
                    lines[y] = lines[y].Remove(x - 3, 1);
                }
                else
                {
                    continue;
                }
                RedrawScreen(lines, y, x);
                continue;
            }

            string tempLine = lines[y];
            tempLine = tempLine.Insert(x-3, key.KeyChar.ToString());
            lines[y] = tempLine;
            x++;
            RedrawScreen(lines, y, x);
        }
    }

}

void RedrawScreen(string[] lines, int x, int y)
{
    Console.Clear();

    for (int i = 0; i < lines.Length; i++)
    {
        Console.WriteLine($"{i + 1}: {lines[i]}");
    }

    Console.SetCursorPosition(y, x);
}

void Save(string[] lines, string path)
{
    StreamWriter newFile2 = File.CreateText(path);
    foreach (var line in lines)
    {
        newFile2.WriteLine(line);
    }
    newFile2.Close();

    Console.Clear();
    Console.Write("\n Saving... \n \n");
}
