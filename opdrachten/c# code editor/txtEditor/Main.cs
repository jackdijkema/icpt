using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;

class MainClass
{
    static void Main(string[] args)
    {
        PrintIntro();

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a file path as an argument.");
            return;
        }

        string path = args[0];

        string[] lines = File.ReadAllLines("test.txt");

        PrintLinesFromFile(lines);

        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) break;

            if (key.Key == ConsoleKey.E)
            {
                lines = EditLine(lines);
            }

            PrintLinesFromFile(lines);

            Console.WriteLine($"You pressed: {key.KeyChar} \n");


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

    struct EditStruct
    {
        int editLineNumber;
        string edit; 
    }
      

    public static string[] EditLine(string[] file)
    {
        Console.Write("Enter line number to edit: \n");
        string? input = Console.ReadLine();
        int selectLine;
        if (!int.TryParse(input, out selectLine) || selectLine <= 0 || selectLine > file.Length)
        {
            Console.Write("Invalid line number. Try again.\n");
            return file;
        }

        string history = file[selectLine - 1];
        string edit = "";

        Console.Write("Editing line: \n");
        Console.Write(file[selectLine - 1] + "\n");
        Console.Write("Replace with: \n");
        edit = Console.ReadLine() ?? "";
        file[selectLine - 1] = edit;
        PrintLinesFromFile(file);
        return file;
    }
}
