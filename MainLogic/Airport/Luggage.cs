namespace MainLogic;

public class Luggage(string id, Passenger passenger)
{
    public string Id { get; set; } = $"lug_{id}";
    public Passenger Passenger  { get; set; } = passenger;

    public override string ToString()
    {
        return "Luggage " + Id +  ": " + Passenger.ToString();
    }
}