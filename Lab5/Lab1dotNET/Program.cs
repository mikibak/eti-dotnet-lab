using System;
using System.IO;

class program
{
    public static void zad1(string[] args)
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg); 
        }
    }

    public static void zad2(string arg, string tabs)
    {
        tabs += "   ";

        DirectoryInfo root = new DirectoryInfo(arg);
        DirectoryInfo[] Dirs = root.GetDirectories(); //Assuming Test is your Folder
        FileInfo[] Files = root.GetFiles("*");

        foreach (DirectoryInfo dir in Dirs)
        {
            string line = tabs + dir.Name;
            Console.WriteLine(line);
            zad2(dir.FullName, tabs);
        }
        foreach (FileInfo file in Files)
        {
            string line = tabs + file.Name;
            Console.WriteLine(line);
        }
    }

    static void Main(string[] args)
    {
        program.zad1(args);
        program.zad2(args[0], "   ");
    }

}
