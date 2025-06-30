using System.Net;
using System.Collections.Generic;

class TextEditor
{
    struct HistoryStruct
    {
        public Stack<string> BeforeEdit;
        public Stack<string> AfterEdit;
        public int EditLineNumber;
    }

    static void Main(string[] args)
    {
        List<string> lines = []; 

        HistoryStruct historyStruct = new()
        {
            BeforeEdit = new Stack<string>(),
            EditLineNumber = 0,
            AfterEdit = new Stack<string>()
        };

        if (args.Length == 0)
        {
            Console.WriteLine("You need to pass file as argument...");
            return;
        }

        string path = args[0];

        string[] liners = File.ReadAllLines(path);

        foreach (var line in liners)
        {
            lines.Add(line);
        }
        int y = lines.Count - 1;

        int LINE_OFFSET = 3;

        int x = 0 + LINE_OFFSET;

        while (true)
        {
            var key = Console.ReadKey(true);

            RedrawScreen(lines, y, x);

            if (key.Key == ConsoleKey.U)
            {
                if (historyStruct.BeforeEdit.Count == 0) continue;
                lines[historyStruct.EditLineNumber] = historyStruct.BeforeEdit.Pop();
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.R)
            {
                if (historyStruct.AfterEdit.Count == 0) continue;
                lines[historyStruct.EditLineNumber] = historyStruct.AfterEdit.Pop();
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.L)
            {
                if (x >= lines[y].Length + LINE_OFFSET)
                {
                    x--;
                }
                x++;
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.H)
            {
                if (x <= LINE_OFFSET)
                {
                    x++;
                }

                x--;
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.J)
            {
                if (y >= lines.Count - 1)
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

            if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.K)
            {
                if (y <= 0)
                {
                    y++;
                }
                y--;
                RedrawScreen(lines, y, x);
            }

            if (key.Key == ConsoleKey.Escape) break;

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

                        if (x == LINE_OFFSET && lines[y].Length == 0)
                        {
                            if (y != 0)
                            {
                                lines.RemoveAt(y);
                                lines[y - 1] += " ";
                                y--;
                                x = lines[y].Length + LINE_OFFSET;
                                RedrawScreen(lines, y, x);
                            }
                            RedrawScreen(lines, y, x);
                        }

                        if (x > 0 + 3)
                        {
                            x--;
                            lines[y] = lines[y].Remove(x - 3, 1);
                        }
                        RedrawScreen(lines, y, x);
                        continue;
                    }

                    if (key.Key == ConsoleKey.Enter)
                    {
                        lines.Add("");
                        y++;
                        x = LINE_OFFSET;
                        RedrawScreen(lines, y, x);
                        continue;
                    }

                    string tempLine = lines[y];
                    historyStruct.BeforeEdit.Push(tempLine);
                    historyStruct.EditLineNumber = y;
                    tempLine = tempLine.Insert(x - LINE_OFFSET, key.KeyChar.ToString());
                    lines[y] = tempLine;
                    x++;
                    historyStruct.AfterEdit.Push(tempLine);
                    RedrawScreen(lines, y, x);
                }
            }
        }

        void RedrawScreen(List<string> lines, int x, int y)
        {
            Console.Clear();

            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {lines[i]}");
            }

            Console.SetCursorPosition(y, x);
        }

        void Save(List<string> lines, string path)
        {
            StreamWriter newFile2 = File.CreateText(path);
            foreach (var line in lines)
            {
                newFile2.WriteLine(line);
            }
            newFile2.Close();
        }
    }
}
