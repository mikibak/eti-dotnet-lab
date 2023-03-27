using System;
using System.Xml.Serialization;


public class Engine
{
    [XmlAttribute]
    public string model { get; set; }
    public double displacement { get; set; }
    public double horsePower { get; set; }

    public Engine(double displacement, double horsePower, string model)
    {
        this.model = model;
        this.displacement = displacement;
        this.horsePower = horsePower;
    }

    public Engine()
    {
        this.model = "model";
        this.displacement = 0.0d;
        this.horsePower = 0.0d;
    }
}