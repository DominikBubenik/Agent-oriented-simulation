using OSPABA;

namespace DISS_sem_3.Entities;

public class Room : Entity
{
    public char Type { get; set; }
    public double OccupancyTime { get; set; }
    public RoomStatus CurrentStatus { get; set; } =  RoomStatus.Empty;
    public Patient? Patient { get; set; }
    public Nurse? Nurse { get; set; }
    public Doctor? Doctor { get; set; }
    public PointF[] PathFreeDoctorToRoom { get; set; } = new PointF[] { };
    public Room(OSPABA.Simulation mySim, char type) : base(mySim)
    {
        Type = type;
    }

    public Room(int id, OSPABA.Simulation mySim, char type) : base(id, mySim)
    {
        Type = type;
    }

    public bool IsTypeA() => Type == 'A';

    public override string ToString()
    {
        return "ID: " + Id + " Type: " + Type + " OccupancyTime: " + OccupancyTime + "Patient " + Patient?.ToString() + " Nurse " + Nurse?.ToString() + " Doctor " + Doctor?.ToString() ;
    }
}