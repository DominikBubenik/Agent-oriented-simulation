using OSPABA;
using OSPAnimator;
using Simulation;

namespace DISS_sem_3.Entities;

public class Doctor : MedicalStaff
{
    public Doctor(OSPABA.Simulation mySim) : base(mySim)
    {
    }

    public Doctor(int id, OSPABA.Simulation mySim) : base(id, mySim)
    {
        AnimObject = new AnimImageItem(Config.DOCTOR_IMG);
        if (MySim.AnimatorExists) MySim.Animator.Register(AnimObject);
    }
    
    public override string ToString()
    {
        return "DOCTOR ID" + Id;
    }
}