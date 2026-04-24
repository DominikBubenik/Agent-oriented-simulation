namespace DISS_SEM_GUI.EventsArguments;

public class OneStat((double avg, double lowerBound, double upperBound) item)
{
    public double Avg { get; set; } = item.avg;
    public double LowerBound { get; set; } = item.lowerBound;
    public double UpperBound { get; set; } = item.upperBound;
}

public class StatisticsList
{
    public int Replication { get; set; }
    public string? TotalPassengersInSystem { get; set; }
    public string? AvgTimeInSystem { get; set; }
    public string? EntryQueueLength { get; set; }
    public string? PassengerDetectorQueueLength { get; set; }
    public string? WaitForLuggageQueueLength { get; set; }
    public OneStat? SumTotalPassengersInSystem { get; set; }
    public OneStat? SumAvgTimeInSystem {get; set; }
    public OneStat? SumAvgEntryQueue {get; set; }
    public OneStat? SumAvgDetectorQueue {get; set; }
    public OneStat? SumAvgWaitingQueue {get; set; }
    public OneStat? SumAvgBeforeDetector {get; set; }
    public OneStat? SumAvgAfterDetector {get; set; }
}