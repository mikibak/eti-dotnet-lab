using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;


public class MainClass
{
    static void Main(string[] args)
    {
        List<Car> myCars = new List<Car>(){
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };

        LinqQueries(myCars);

        string filename = "C:\\Users\\mikolaj\\CarsCollection.xml";

        Serialize(myCars, filename);

        ShowDeserialization(filename);

        CalculateAverageHP(filename);

        WriteCarModels(filename);

        CreateXmlFromLinq(myCars);

        ModifyCarsCollection(filename);

        XAttribute attr = new XAttribute("style", "border: 1px solid purple; background-color: magenta; color: blue");
        IEnumerable<XElement> rows = myCars
                .Select(car =>
                new XElement("tr", attr,
                    new XElement("td", attr, car.model),
                    new XElement("td", attr, car.motor.model),
                    new XElement("td", attr, car.motor.displacement),
                    new XElement("td", attr, car.motor.horsePower),
                    new XElement("td", attr, car.year)));
        XElement table = new XElement("table", attr, rows);
        table.Save("C:\\Users\\mikolaj\\Table.html");

    }

    private static void ModifyCarsCollection(string filename)
    {
        XDocument xdoc = XDocument.Load(filename);
        foreach (var car in xdoc.Elements())
        {
            foreach (var field in car.Elements())
            {
                if (field.Name == "engine")
                {
                    foreach (var engineElement in field.Elements())
                    {
                        if (engineElement.Name == "horsePower")
                        {
                            engineElement.Name = "hp";
                        }
                    }
                }
                else if (field.Name == "model")
                {
                    var yearField = car.Element("year");
                    XAttribute attribute = new XAttribute("year", yearField.Value);
                    field.Add(attribute);
                    yearField.Remove();
                }
            }
        }
        xdoc.Save("CarsCollectionModified.xml");

    }

    private static void ShowDeserialization(string filename)
    {
        List<Car> myCarsDeserialized = Deserialize(filename);
        foreach (var car in myCarsDeserialized)
        {
            Console.WriteLine(car.model + ", " + car.year);
        }
        Console.WriteLine();
    }

    private static void WriteCarModels(string filename)
    {
        XElement rootNode = XElement.Load(filename);
        IEnumerable<XElement> models = rootNode.XPathSelectElements("//car/model[not(. = preceding::car/model)]");
        Console.WriteLine("Models: ");
        foreach (var model in models)
        {
            Console.WriteLine(model.ToString());
        }
    }

    private static void CalculateAverageHP(string filename)
    {
        XElement rootNode = XElement.Load(filename);
        double avgHP = (double)rootNode.XPathEvaluate("sum(//car/engine[@model!='TDI']/horsePower) div count(//car/engine[@model!='TDI']/horsePower)");
        Console.WriteLine("Average horse power: " + avgHP);
        Console.WriteLine();
    }

    private static void LinqQueries(List<Car> myCars)
    {
        var results = myCars.Where(car => car.model.Equals("A6"))
            .Select(car => new
            {
                motorType = car.motor.model.Equals("TDI") ? "diesel" : "petrol",
                hppl = car.motor.horsePower / car.motor.displacement
            });

        foreach (var car in results)
        {
            Console.WriteLine(car);
        }
        Console.WriteLine();



        var results2 = results.GroupBy(
                p => p.motorType,
                p => p.hppl,
                (key, value) => new { motorType = key, hpplAverage = value.ToList().Average() }
            );

        foreach (var car in results2)
        {
            Console.WriteLine(car);
        }
        Console.WriteLine();
    }

    private static void CreateXmlFromLinq(List<Car> myCars)
    {
        IEnumerable<XElement> nodes = myCars
                .Select(n =>
                new XElement("car",
                    new XElement("model", n.model),
                    new XElement("year", n.year),
                    new XElement("engine",
                        new XAttribute("model", n.motor.model),
                        new XElement("displacement", n.motor.displacement),
                        new XElement("horsePower", n.motor.horsePower))));
        XElement rootNode = new XElement("cars", nodes); //create a root node to contain the query results
        rootNode.Save("C:\\Users\\mikolaj\\CarsCollectionLinq.xml");
    }

    private static List<Car> Deserialize(string filename)
    {
        List<Car> myCarsDeserialized;
        XmlSerializer serializer2 = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));

        using (Stream reader = new FileStream(filename, FileMode.Open))
        {
            // Call the Deserialize method to restore the object's state.
            myCarsDeserialized = (List<Car>)serializer2.Deserialize(reader);
        }
        return myCarsDeserialized;
    }

    private static void Serialize(List<Car> myCars, string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
        using (var writer = new StreamWriter(filename))
        {
            serializer.Serialize(writer, myCars);
        }
    }
}
