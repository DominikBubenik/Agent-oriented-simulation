using System.Diagnostics;
using OSPABA;
using Simulation;

namespace DISS_sem_3.Entities;

public class Room : Entity
{
    public char Type { get; set; }
    public double TotalOccupancyTime { get; set; }
    private double _startOccupancyTime { get; set; }
    public RoomStatus CurrentStatus { get; set; } =  RoomStatus.Free;
    public Patient? Patient { get; set; }
    public Nurse? Nurse { get; set; }
    public Doctor? Doctor { get; set; }
    public Room(OSPABA.Simulation mySim, char type) : base(mySim)
    {
        Type = type;
    }

    public Room(int id, OSPABA.Simulation mySim, char type) : base(id, mySim)
    {
        Type = type;
    }

    public bool IsTypeA() => Type == 'A';

    public void StartOccupancy()
    {
        _startOccupancyTime = MySim.CurrentTime;
        CurrentStatus = RoomStatus.Occupied;
    }
    
    public void StopOccupancy()
    {
        TotalOccupancyTime += MySim.CurrentTime - _startOccupancyTime;
        CurrentStatus = RoomStatus.Free;
    }
    
    public double GetUtilization()  
    {
        var occupancyTime = TotalOccupancyTime;
        if (CurrentStatus == RoomStatus.Occupied)
        {
            occupancyTime += MySim.CurrentTime - _startOccupancyTime;
        }

        var myCastSim = (MySimulation)MySim;
        var time = MySim.CurrentTime > myCastSim.WarmUpTime ? MySim.CurrentTime - myCastSim.WarmUpTime : MySim.CurrentTime;
        return occupancyTime / time;
    }

    public void Reset()
    {
        _startOccupancyTime = MySim.CurrentTime;
        TotalOccupancyTime = 0;
    }
    
    public bool Equals(Room room)
    {
        return Id == room.Id && Type ==  room.Type;
    }

    public override string ToString()
    {
        return "ID: " + Id + " Type: " + Type + " OccupancyTime: " + TotalOccupancyTime + "Patient " + Patient?.ToString() + " Nurse " + Nurse?.ToString() + " Doctor " + Doctor?.ToString() ;
    }
}