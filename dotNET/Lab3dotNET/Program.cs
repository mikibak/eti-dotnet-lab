using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Runtime.Serialization;
using System.Data;
using System.Runtime.CompilerServices;

public class Car
{
    string model;
    int year;
    Engine engine;

    public Car(string model, Engine engine, int year)
    {
        this.model = model;
        this.year = year;
        this.engine = engine;
    }
}

public class Engine
{
    string model;
    double displacement;
    double horsePower;

    public Engine(double displacement, double horsePower, string model)
    {
        this.model = model;
        this.displacement = displacement;
        this.horsePower = horsePower;
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
    }

}
