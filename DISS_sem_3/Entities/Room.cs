using OSPABA;

namespace DISS_sem_3.Entities;

public class Room : Entity
{
    public char Type { get; set; }
    public double OccupancyTime { get; set; }
    public Room(OSPABA.Simulation mySim, char type) : base(mySim)
    {
        Type = type;
    }

    public Room(int id, OSPABA.Simulation mySim, char type) : base(id, mySim)
    {
        Type = type;
    }
}