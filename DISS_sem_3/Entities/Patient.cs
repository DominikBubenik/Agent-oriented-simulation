using OSPABA;

namespace DISS_sem_3.Entities;

public class Patient : Entity
{
    public double ArrivalTime { get; set; }
    public bool ArrivedByAmbulance { get; set; }
    public Patient(OSPABA.Simulation mySim, double arrivalTime, bool arrivedByAmbulance) : base(mySim)
    {
        ArrivalTime = arrivalTime;
        ArrivedByAmbulance = arrivedByAmbulance;
    }

    public Patient(int id, OSPABA.Simulation mySim, double arrivalTime, bool arrivedByAmbulance) : base(id, mySim)
    {
        ArrivalTime = arrivalTime;
        ArrivedByAmbulance = arrivedByAmbulance;
    }

    public override string ToString()
    {
        var type = ArrivedByAmbulance ? "ambulanced" : "regular";
        return $"{type}-patient {Id}";
    }
}