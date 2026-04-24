using OSPABA;

namespace DISS_sem_3.Entities;

public class Doctor : MedicalStaff
{
    public Doctor(OSPABA.Simulation mySim) : base(mySim)
    {
    }

    public Doctor(int id, OSPABA.Simulation mySim) : base(id, mySim)
    {
    }
    
    public override string ToString()
    {
        return "DOCTOR ID" + Id;
    }
}