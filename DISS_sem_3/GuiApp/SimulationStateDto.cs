using DISS_sem_3.Entities;

namespace DISS_sem_3;

public class SimulationStateDto
{
    public double CurrentTime { get; set; }
    public List<Patient> EntryQueue { get; set; }
    public double EntryQueueAvgLength { get; set; }
    public List<Patient> MedicalTreatQueueA { get; set; }
    public double MedicalQueueAvgLengthA { get; set; }
    public List<Patient> MedicalTreatQueueB { get; set; }
    public double MedicalQueueAvgLengthB { get; set; }
    public List<Doctor> AllDoctors { get; set; }
    public List<Nurse> AllNurses { get; set; }
    public List<Room> ARooms { get; set; }
    public List<Room> BRooms { get; set; }
}