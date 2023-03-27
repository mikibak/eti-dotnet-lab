using System;
using System.Xml.Serialization;


[XmlType("car")]
public class Car
{
    public string model { get; set; }
    public int year { get; set; }

    [XmlElement("engine")]
    public Engine motor { get; set; }

    public Car(string model, Engine motor, int year)
    {
        this.model = model;
        this.year = year;
        this.motor = motor;
    }

    public Car()
    {
        this.model = "model";
        this.year = 1970;
        this.motor = new Engine();
    }
}