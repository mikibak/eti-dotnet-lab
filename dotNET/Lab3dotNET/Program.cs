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

[Serializable]
public class Car
{
    public string model { get; set; }
    public int year { get; set; }
    public Engine engine { get; set; }

    public Car(string model, Engine engine, int year)
    {
        this.model = model;
        this.year = year;
        this.engine = engine;
    }

    public Car()
    {
        this.model = "model";
        this.year = 1970;
        this.engine = new Engine();
    }
}


[Serializable]
public class Engine
{
    public string model { get; set; }
    public double displacement { get; set; }
    public double horsePower { get; set; }

    public Engine(double displacement, double horsePower, string model)
    {
        this.model = model;
        this.displacement = displacement;
        this.horsePower = horsePower;
    }

    public Engine() { 
        this.model = "model";
        this.displacement = 0.0d;
        this.horsePower = 0.0d;
    }
}


public static class MainClass
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

        var results = myCars.Where(car => car.model.Equals("A6"))
            .Select(car => new
            {
                engineType = car.engine.model.Equals("TDI") ? "diesel" : "petrol",
                hppl = car.engine.horsePower/car.engine.displacement
            });

        foreach (var car in results)
        {
            Console.WriteLine(car);
        }
        Console.WriteLine();
        /*
        var results2 = results.GroupBy(
                p => p.engineType,
                p => p.hppl,
                (key, value) => new { engineType = key, hpplAverage = value }
            );
        */
        var results2 = results.GroupBy(
                p => p.engineType,
                p => p.hppl,
                (key, value) => new { engineType = key, hpplAverage = value.ToList().Average() }
            );


        foreach (var car in results2)
        {
            Console.WriteLine(car.hpplAverage);
        }

        //FileStream fs = new FileStream("C:\\Users\\mikolaj\\test.xml", FileMode.OpenOrCreate);
        XmlSerializer serializer = new XmlSerializer(typeof(List<Car>));
        using (var writer = new StreamWriter("C:\\Users\\mikolaj\\test.xml"))
        {
            serializer.Serialize(writer, myCars);
        }
        //s.Serialize(fs, myCars);

    }

}
