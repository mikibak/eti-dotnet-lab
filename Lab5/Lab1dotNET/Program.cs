using System;
using System.IO;

public static class MyExtensions
{   
    public static DateTime FindOldest(this DirectoryInfo root, DateTime date)
    {
        DirectoryInfo[] Dirs = root.GetDirectories();
        FileInfo[] Files = root.GetFiles("*");

        foreach (DirectoryInfo dir in Dirs)
        {
            date = FindOldest(dir, date);
        }
        foreach (FileInfo file in Files)
        {
            var currentDate = file.CreationTime;
            if(DateTime.Compare(currentDate, date)<0)
            {
                date = currentDate;
            }
        }
        return date;
    }

    public static string GetDosAttributes(this FileSystemInfo root) 
    {
        string rahs = "";
        FileAttributes attributes = File.GetAttributes(root.FullName);
        if((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) 
        {
            rahs += "r";
        } 
        else rahs += "_";

        if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
        {
            rahs += "a";
        } 
        else rahs += "_";

        if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
        {
            rahs += "h";
        } 
        else rahs += "_";

        if ((attributes & FileAttributes.System) == FileAttributes.System)
        {
            rahs += "s";
        }
        else rahs += "_";

        return rahs;
    }

    public static int CalculateLength(this DirectoryInfo root, int length = 0)
    {
        DirectoryInfo[] Dirs = root.GetDirectories();
        FileInfo[] Files = root.GetFiles("*");

        length += Files.Length;

        foreach (DirectoryInfo dir in Dirs)
        {
            length = dir.CalculateLength(length);
        }
        return length;
    }
}

class program
{
    public static void Task1(string[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg); 
        }
    }

    public static void Task2(string arg, string tabs = "")
    {
        tabs += "     ";

        DirectoryInfo root = new DirectoryInfo(arg);
        DirectoryInfo[] Dirs = root.GetDirectories();
        FileInfo[] Files = root.GetFiles("*");

        foreach (DirectoryInfo dir in Dirs)
        {
            //string line = tabs + dir.Name + " (" + dir.CalculateLength() + ") " + dir.GetDosAttributes();
            Console.Write(tabs + dir.Name);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" (" + dir.CalculateLength() + ") ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(dir.GetDosAttributes());
            Task2(dir.FullName, tabs);
        }

        foreach (FileInfo file in Files)
        {
            //string line = tabs + file.Name + "  " + file.GetDosAttributes();
            Console.Write(tabs + file.Name);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" " + file.Length + " B ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(file.GetDosAttributes());
        }
    }

    public static void Task3(string arg)
    {
        DirectoryInfo root = new DirectoryInfo(arg);
        Console.WriteLine("Oldest file:  " + root.FindOldest(DateTime.MaxValue));

        //part b
        Console.WriteLine("Oldest file:  " + root.FindOldest(DateTime.MaxValue));
    }

    static void Main(string[] args)
    {
        program.Task1(args);
        program.Task2(args[0]);
        program.Task3(args[0]);
    }

}
