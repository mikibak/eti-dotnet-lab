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
        if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
        {
            rahs += "a";
        }
        if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
        {
            rahs += "h";
        }
        if ((attributes & FileAttributes.System) == FileAttributes.System)
        {
            rahs += "s";
        }

        return rahs;
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

    public static void Task2(string arg, string tabs)
    {
        tabs += "   ";

        DirectoryInfo root = new DirectoryInfo(arg);
        DirectoryInfo[] Dirs = root.GetDirectories();
        FileInfo[] Files = root.GetFiles("*");
        int elements_in_root = 0;

        foreach (DirectoryInfo dir in Dirs)
        {
            string line = tabs + dir.Name;
            Console.WriteLine(line);
            Task2(dir.FullName, tabs);
            elements_in_root++;
        }
        foreach (FileInfo file in Files)
        {
            string line = tabs + file.Name + "  " + file.GetDosAttributes();
            Console.WriteLine(line + " " + file.Length + " B");
            elements_in_root++;
        }

        //Console.WriteLine(root.FindOldest(DateTime.MaxValue));
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
        program.Task2(args[0], "   ");
        program.Task3(args[0]);
    }

}
