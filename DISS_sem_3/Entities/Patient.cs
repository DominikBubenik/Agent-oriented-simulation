using OSPABA;

namespace DISS_sem_3.Entities;

public class Patient : Entity
{
    public double ArrivalTime { get; set; }
    public bool ArrivedByAmbulance { get; set; }
    public int Priority { get; set; } = int.MaxValue;
    private double _entryQueueStartWait;
    public string Name { get; set; }
    public double EntryQueueWaitingTime { get; private set; }
    public Patient(OSPABA.Simulation mySim, double arrivalTime, bool arrivedByAmbulance) : base(mySim)
    {
        ArrivalTime = arrivalTime;
        ArrivedByAmbulance = arrivedByAmbulance;
        if (arrivedByAmbulance)
        {
            Priority = 0;
            Name = "ambulanced-patient-" + Id;
        }
        else
        {
            Name = "walkIn-patient-" + Id;
        }
    }

    public Patient(int id, OSPABA.Simulation mySim, double arrivalTime, bool arrivedByAmbulance) : base(id, mySim)
    {
        ArrivalTime = arrivalTime;
        ArrivedByAmbulance = arrivedByAmbulance;
        if (arrivedByAmbulance)
        {
            Priority = 0;
            Name = "ambulanced-patient-" + Id;
        }
        else
        {
            Name = "walkIn-patient-" + Id;
        }
    }

    public override string ToString()
    {
        var type = ArrivedByAmbulance ? "ambulanced" : "regular";
        return $"{type}-patient {Id} Priority: {Priority} ArrivalTime: {ArrivalTime}";
    }

    public void StartEntryQueueWait()
    {
        _entryQueueStartWait = MySim.CurrentTime;
    }

    public void StopEntryWaiting()
    {
        EntryQueueWaitingTime = MySim.CurrentTime - _entryQueueStartWait;
    }
}