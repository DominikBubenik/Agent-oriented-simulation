namespace DISS_SEM_GUI.EventsArguments;

public class SimulationStatsDto
{
    public int Replication { get; set; }
    public Dictionary<string, OneStat> Stats { get; set; } = new();
    // public int TotalPatientsInSystem { get; set; }
    // public OneStat? TotalTimeInSystemAll { get; set; }
    // public OneStat? TotalTimeInSystemPriority1 { get; set; }
    // public OneStat? TotalTimeInSystemPriority2 { get; set; }
    // public OneStat? TotalTimeInSystemPriority3 { get; set; }
    // public OneStat? TotalTimeInSystemPriority4 { get; set; }
    // public OneStat? TotalTimeInSystemPriority5 { get; set; }
    // public OneStat? EntryQueueLength { get; set; }
    // public OneStat? EntryQueueWaitingTimeWalkIn { get; set; }
    // public OneStat? EntryQueueWaitingTimeAmbulanced { get; set; }
    // public OneStat? MedicalQueueWaitingTimePriority1 { get; set; }
    // public OneStat? MedicalQueueWaitingTimePriority2 { get; set; }
    // public OneStat? MedicalQueueWaitingTimePriority3 { get; set; }
    // public OneStat? MedicalQueueWaitingTimePriority4 { get; set; }
    // public OneStat? MedicalQueueWaitingTimePriority5 { get; set; }
    // public OneStat? MedicalQueueLengthTypeA { get; set; }
    // public OneStat? MedicalQueueLengthTypeB { get; set; }
}