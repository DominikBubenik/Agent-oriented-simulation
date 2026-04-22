using OSPABA;

namespace DISS_sem_3.Entities;

public class MedicalStaff : Entity
{
    public bool IsWorking { get; set; }
    public double TotalWorkingTime { get; set; }
    public MedicalStaff(OSPABA.Simulation mySim) : base(mySim)
    {
    }

    public MedicalStaff(int id, OSPABA.Simulation mySim) : base(id, mySim)
    {
    }
}