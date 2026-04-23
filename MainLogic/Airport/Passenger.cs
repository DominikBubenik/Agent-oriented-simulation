namespace MainLogic;

public class Passenger
{
    public string Id { get; }
    public double ArrivalTime { get; set; }
    public int LuggageCount { get; set; }
    public List<Luggage> LuggageList { get; set; }
    public int CurrentlyHolding { get; set; }

    public Passenger(string id, double arrivalTime, int luggageCount)
    {
        Id = $"passenger_{id}";
        ArrivalTime = arrivalTime;
        LuggageCount = luggageCount;
        CurrentlyHolding = LuggageCount;
        LuggageList = [];
        for (int i = 0; i < LuggageCount; i++)
        {
            LuggageList.Add(new Luggage($"passenger_{id}+{i}", this));
        }
    }
    
    public string ToString()
    {
        return $"{Id}: | Luggage Count >  {LuggageCount}   |   Arrival time >  {ArrivalTime}";
    }
}