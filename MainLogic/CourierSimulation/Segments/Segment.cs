using System.Numerics;

namespace MainLogic;

public abstract class Segment 
{
    public string Color {get; set;}
    public int Distance {get; set;}
    public Segment(string color, int distance)
    {
        Color = color;
        Distance = distance;
    }

    public double GetTimeLength()
    {
        return  (Distance / GetValue()) * 60;   
    }

    public abstract double GetValue();
}