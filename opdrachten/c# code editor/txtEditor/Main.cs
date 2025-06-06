

class MainClass
{

    public struct EditStruct
    {
        public int editLineNumber;
        public string edit;
        public string[] newFile;
        public string[] beforeEditFile;
    }
    static void Main(string[] args)
    {
        PrintIntro();
        string path = args[0];
        string[] lines = File.ReadAllLines(path.ToString());

        EditStruct editHistory = new()
        {
            beforeEditFile = lines
        };

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a file path as an argument.");
            return;
        }

        PrintLinesFromFile(lines);

        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) break;

            if (key.Key == ConsoleKey.E)
            {
                editHistory = EditLine(lines);
                lines = editHistory.newFile;
            }

            if (key.Key == ConsoleKey.R)
            {

                if (editHistory.newFile != null)
                {
                    lines = editHistory.newFile ?? lines;
                    Console.Write($"\n Re-do... \n \n");
                }
                else
                {
                    Console.Write($"\n nothing to redo... \n \n");
                }
            }

            if (key.Key == ConsoleKey.U)
            {
                if (editHistory.edit != null)
                {
                    lines = editHistory.beforeEditFile;
                    Console.Write($" \nUndo... \n \n");
                }
                else
                {
                    Console.Write($" \n nothing to undo... \n \n");
                }
            }

            if (key.Key == ConsoleKey.S)
            {




                StreamWriter newFile2 = File.CreateText(path);
                foreach (var line in lines)
                {
                    newFile2.WriteLine(line);
                }
                newFile2.Close();
                Console.Write("\n Saving... \n \n");
                break;
            }
            PrintLinesFromFile(lines);
        }
    }
    public static void PrintIntro()
    {
        Console.WriteLine(@"
    /$$$$$                    /$$       /$$                             /$$ /$$   /$$                        
   |__  $$                   | $$      | $/                            | $$|__/  | $$                        
      | $$ /$$$$$$   /$$$$$$$| $$   /$$|_/$$$$$$$        /$$$$$$   /$$$$$$$ /$$ /$$$$$$    /$$$$$$   /$$$$$$ 
      | $$|____  $$ /$$_____/| $$  /$$/ /$$_____/       /$$__  $$ /$$__  $$| $$|_  $$_/   /$$__  $$ /$$__  $$
 /$$  | $$ /$$$$$$$| $$      | $$$$$$/ |  $$$$$$       | $$$$$$$$| $$  | $$| $$  | $$    | $$  \ $$| $$  \__/
| $$  | $$/$$__  $$| $$      | $$_  $$  \____  $$      | $$_____/| $$  | $$| $$  | $$ /$$| $$  | $$| $$      
|  $$$$$$/  $$$$$$$|  $$$$$$$| $$ \  $$ /$$$$$$$/      |  $$$$$$$|  $$$$$$$| $$  |  $$$$/|  $$$$$$/| $$      
 \______/ \_______/ \_______/|__/  \__/|_______/        \_______/ \_______/|__/   \___/   \______/ |__/      
                                                                                                             
");
        Console.WriteLine("Welcome to Jack's Editor!");
        Console.WriteLine("These are your options:");
        Console.WriteLine("u (undo)  r (redo) e (edit a line) ESCAPE (escape without saving)  s (save file and exit)");
        Console.WriteLine("-");
        Console.WriteLine();
    }

    public static void PrintLinesFromFile(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            Console.Write((i + 1) + " " + lines[i] + "\n");
        }
    }

    public static Boolean CheckIfLineExist(int input, string[] file)
    {
        if (input > file.Length)
        {
            Console.Write("Line doesn't exist try again \n");
            return false;
        }
        return true;
    }
    public static EditStruct EditLine(string[] file)
    {
        string[] oldFile = (string[])file.Clone();

        EditStruct editHistory = new()
        {
            beforeEditFile = oldFile
        };

        Console.Write("Enter line 2 edit: \n");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int selectLine) || selectLine <= 0 || selectLine > file.Length)
        {
            Console.Write("Invalid line number. Try again.\n");
            return editHistory;
        }

        string history = oldFile[selectLine - 1];
        Console.Write("Editing line: \n");
        Console.Write(file[selectLine - 1] + "\n");
        Console.Write("Replace with: \n");

        string edit = Console.ReadLine() ?? "";

        file[selectLine - 1] = edit;

        editHistory.edit = history;
        editHistory.editLineNumber = selectLine;
        editHistory.newFile = file;

        return editHistory;
    }
}
