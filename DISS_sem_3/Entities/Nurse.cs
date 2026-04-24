namespace DISS_sem_3.Entities;

public class Nurse : MedicalStaff
{
    public Nurse(OSPABA.Simulation mySim) : base(mySim)
    {
    }

    public Nurse(int id, OSPABA.Simulation mySim) : base(id, mySim)
    {
    }

    public override string ToString()
    {
        return "NURSE ID" + Id;
    }
}