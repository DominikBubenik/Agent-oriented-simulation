using OSPABA;
using OSPAnimator;
using Simulation;

namespace DISS_sem_3.Entities;

public class MedicalStaff : Entity
{
    public StaffActivity Activity { get; set; } = StaffActivity.Not_Working;
    public double TotalWorkingTime { get; set; }
    public double TotalTransferTime { get; set; }
    protected double _startWorkingTime;
    protected double _startTransferTime;
    public AnimImageItem AnimObject { get; protected set; }
    public MedicalStaff(OSPABA.Simulation mySim) : base(mySim)
    {
    }

    public MedicalStaff(int id, OSPABA.Simulation mySim) : base(id, mySim)
    {
    }

    public void StartWork()
    {
        Activity = StaffActivity.Working;
        _startWorkingTime = MySim.CurrentTime;
    }

    public void StartTransfer()
    {
        Activity = StaffActivity.Moving;
        _startTransferTime = MySim.CurrentTime;
    }
    
    public void StopWork()
    {
        Activity = StaffActivity.Not_Working;
        TotalWorkingTime +=  MySim.CurrentTime - _startWorkingTime;
    }

    public void StopTransfer()
    {
        Activity = StaffActivity.Standing;
        
    }

    public double GetWorkingUtilization(double endTime)
    {
        var workingTime = TotalWorkingTime;
        if (Activity == StaffActivity.Working)
        {
            workingTime += endTime - _startWorkingTime;
        }
        var movingTime = TotalTransferTime;
        if (Activity == StaffActivity.Moving)
        {
            movingTime += endTime - _startTransferTime;
        }

        return (workingTime + movingTime) / endTime;
    }
}