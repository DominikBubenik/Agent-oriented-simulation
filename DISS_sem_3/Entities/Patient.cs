using OSPABA;
using OSPAnimator;
using Simulation;

namespace DISS_sem_3.Entities;

public class Patient : Entity
{
    public double ArrivalTime { get; set; }
    public bool ArrivedByAmbulance { get; set; }
    public int Priority { get; set; } = int.MaxValue;
    private double _entryQueueStartWait;
    private double _medicalQueueStartWait;
    public string Name { get; set; }
    public double EntryQueueWaitingTime { get; private set; }
    public double MedicalQueueWaitingTime { get; private set; }
    public PatientStatus PatientStatus { get; set; }
    public AnimImageItem AnimObject { get; private set; }
    public Patient(OSPABA.Simulation mySim, double arrivalTime, bool arrivedByAmbulance) : base(mySim)
    {
        ArrivalTime = arrivalTime;
        ArrivedByAmbulance = arrivedByAmbulance;
        if (arrivedByAmbulance)
        {
            Priority = 0;
            Name = "am-patient-" + Id;
            if (MySim.AnimatorExists) AnimObject = new AnimImageItem(Config.AMBULANCE_PATIENT_IMG);
        }
        else
        {
            Name = "wa-patient-" + Id;
            if (MySim.AnimatorExists)  AnimObject = new AnimImageItem(Config.WALK_IN_PATIENT_IMG);
        }
        if (MySim.AnimatorExists) MySim.Animator.Register(AnimObject);
    }

    public Patient(int id, OSPABA.Simulation mySim, double arrivalTime, bool arrivedByAmbulance) : base(id, mySim)
    {
        ArrivalTime = arrivalTime;
        ArrivedByAmbulance = arrivedByAmbulance;
        if (arrivedByAmbulance)
        {
            Priority = 0;
            Name = "am-patient-" + Id;
            AnimObject = new AnimImageItem(Config.AMBULANCE_PATIENT_IMG);
        }
        else
        {
            Name = "wa-patient-" + Id;
            AnimObject = new AnimImageItem(Config.WALK_IN_PATIENT_IMG);
        }
        if (MySim.AnimatorExists) MySim.Animator.Register(AnimObject);
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

    public void StartMedicalQueueWait()
    {
        _medicalQueueStartWait = MySim.CurrentTime;
    }

    public void StopMedicalQueueWaiting()
    {
        MedicalQueueWaitingTime = MySim.CurrentTime - _medicalQueueStartWait;
    }
}