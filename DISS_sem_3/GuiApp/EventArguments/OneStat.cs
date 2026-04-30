namespace DISS_SEM_GUI.EventsArguments;

public class OneStat((double avg, double lowerBound, double upperBound) item)
{
    public double Avg { get; set; } = item.avg;
    public double LowerBound { get; set; } = item.lowerBound;
    public double UpperBound { get; set; } = item.upperBound;
}