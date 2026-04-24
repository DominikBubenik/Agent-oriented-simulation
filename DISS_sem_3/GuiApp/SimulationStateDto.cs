using DISS_sem_3.Entities;

namespace DISS_sem_3;

public class OneStat((double avg, double lowerBound, double upperBound) item)
{
    public double Avg { get; set; } = item.avg;
    public double LowerBound { get; set; } = item.lowerBound;
    public double UpperBound { get; set; } = item.upperBound;
}



public class SimulationStateDto
{
    public double CurrentTime { get; set; }
    public List<Patient> EntryQueue { get; set; }
    public double EntryQueueAvgLength { get; set; }
    public List<Doctor> FreeDoctors { get; set; }
    public List<Nurse> FreeNurses { get; set; }
    public List<Room> ARooms { get; set; }
    public List<Room> BRooms { get; set; }
}