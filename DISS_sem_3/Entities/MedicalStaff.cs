using OSPABA;
using OSPAnimator;

namespace DISS_sem_3.Entities;

public class MedicalStaff : Entity
{
    public bool IsWorking { get; set; }
    public StaffActivity Activity { get; set; } = StaffActivity.Not_Working;
    public double TotalWorkingTime { get; set; }
    public AnimImageItem AnimObject { get; protected set; }
    public MedicalStaff(OSPABA.Simulation mySim) : base(mySim)
    {
    }

    public MedicalStaff(int id, OSPABA.Simulation mySim) : base(id, mySim)
    {
    }
}