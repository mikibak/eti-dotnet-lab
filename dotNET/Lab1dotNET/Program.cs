using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Runtime.Serialization;

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
        FileAttributes attributes = System.IO.File.GetAttributes(root.FullName);
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

[Serializable]
class CustomStringComparer : IComparer<string>
{
    private readonly IComparer<string> _baseComparer;
    public CustomStringComparer(IComparer<string> baseComparer)
    {
        _baseComparer = baseComparer;
    }

    public int Compare(string x, string y)
    {
        if (x.Length > y.Length)
            return 1;

        if (x.Length < y.Length)
            return -1;

        return string.Compare(x, y, StringComparison.CurrentCulture);
    }
}

class Program
{
    public static void Task1_write_arg(string[] args)
    {
        foreach (var arg1 in args)
        {
            Console.WriteLine(arg1 + "\r\n"); 
        }

        var arg = args[0];

        //write first line in console
        DirectoryInfo root = new DirectoryInfo(arg);
        Console.Write(root.Name);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(" (" + root.CalculateLength() + ") ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(root.GetDosAttributes());

    }

    public static void Task2_catalog_tree(string arg, string tabs = "")
    {
        tabs += "     ";

        DirectoryInfo root = new DirectoryInfo(arg);
        DirectoryInfo[] Dirs = root.GetDirectories();
        FileInfo[] Files = root.GetFiles("*");

        foreach (DirectoryInfo dir in Dirs)
        {
            Console.Write(tabs + dir.Name);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" (" + dir.CalculateLength() + ") ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(dir.GetDosAttributes());
            Task2_catalog_tree(dir.FullName, tabs);
        }

        foreach (FileInfo file in Files)
        {
            Console.Write(tabs + file.Name);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" " + file.Length + " bajtow ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(file.GetDosAttributes());
        }
    }

    public static void Task3_find_oldest(string arg)
    {
        DirectoryInfo root = new DirectoryInfo(arg);
        Console.WriteLine("Oldest file:  " + root.FindOldest(DateTime.MaxValue));
    }

    public static SortedDictionary<string, long> Task5_create_collection(string arg)
    {
        var comparer = new CustomStringComparer(StringComparer.CurrentCulture);
        SortedDictionary<string, long> collection = new SortedDictionary<string, long>(comparer);

        DirectoryInfo root = new DirectoryInfo(arg);
        DirectoryInfo[] Dirs = root.GetDirectories();
        FileInfo[] Files = root.GetFiles("*");

        foreach (DirectoryInfo dir in Dirs)
        {
            collection.Add(dir.Name, dir.CalculateLength());
        }

        foreach (FileInfo file in Files)
        {
            collection.Add(file.Name, file.Length);
        }
        return collection;
    }

    static void Task6_serialize(SortedDictionary<string, long> collection)
    {
        FileStream fs = new FileStream("DataFile.dat", FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, collection);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }

    static void Task6_deserialize()
    {
        SortedDictionary<string, long> collection;
        FileStream fs = new FileStream("DataFile.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            collection = (SortedDictionary<string, long>)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }

        //write in order
        foreach (var de in collection)
        {
            Console.WriteLine("{0} -> {1}", de.Key, de.Value);
        }
    }

    static void Main(string[] args)
    {
        Program.Task1_write_arg(args);
        Program.Task2_catalog_tree(args[0]);
        Program.Task3_find_oldest(args[0]);
        SortedDictionary<string, long> collection = Program.Task5_create_collection(args[0]);
        Program.Task6_serialize(collection);
        Program.Task6_deserialize();
    }

}
