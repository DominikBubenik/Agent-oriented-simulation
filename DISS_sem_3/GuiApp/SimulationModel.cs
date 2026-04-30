using DISS_SEM_GUI.EventsArguments;
using OSPAnimator;
using Simulation;

namespace DISS_sem_3;
/**
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 2
 */
public class SimulationModel
{
    private MySimulation? _core;
    
    public event Action<SimulationStateDto> OnRefreshUI;
    public event Action<SimulationStatsDto> OnTurboUI;
    private DateTime _lastRefreshTime = DateTime.MinValue;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMilliseconds(60); // ~30 FPS

    public void StartSimulation(StartSimulationArgs args)
    {
        _core ??= new MySimulation(args.Seed);
        if (args.ObservationMode)
        {
            _core.SetSimSpeed(1.0, 0.05);
            _core.OnRefreshUI(UpdateGui);   
        }
        else if (args.TurboMode)
        {
            _core.OnReplicationDidFinish(UpdateTurboWindow);   
            _core.SetMaxSimSpeed();
        }
        
        _core.SimulateAsync(args.Replications, args.EndSimulationTime);
        _core.SetEndTime(args.EndSimulationTime);
    }

    public Animator CreateAnimator(StartSimulationArgs args)
    {
        _core ??= new MySimulation(args.Seed);
        var animator = new Animator(_core);
        _core.Animator = animator;
        animator.SetBackgroundImage(Config.BACKGROUND_IMG);
        return animator;
    }

    private void UpdateTurboWindow(OSPABA.Simulation Sim)
    {
        var mySim = (MySimulation)Sim;
        var dto = new SimulationStatsDto { Replication = mySim.CurrentReplication };

        var totalPInSystem = mySim.TotalPatientCount;
        var totalWalkinPInSystem = mySim.TotalWalkInPatientCount;
        var totalAmbulancedPInSystem = mySim.TotalAmbulancePatientCount;
        var totalTimeInSystem = mySim.TotalTimeInSystem;
        var totalTimeInSystemWalkInPatient = mySim.TotalTimeInSystemWalkInPatient;
        var totalTimeInSystemAmbulancePatient = mySim.TotalTimeInSystemAmbulancePatient;
        var totalTimeInSystemPriority1 = mySim.TotalTimeInSystemPriority1;
        var totalTimeInSystemPriority2 = mySim.TotalTimeInSystemPriority2;
        var totalTimeInSystemPriority3 = mySim.TotalTimeInSystemPriority3;
        var totalTimeInSystemPriority4 = mySim.TotalTimeInSystemPriority4;
        var totalTimeInSystemPriority5 = mySim.TotalTimeInSystemPriority5;
        // var entryQueueWaitingTime = mySim.TotalEntryWaitingTime;
        var entryQueueWaitingTimeWalkIn = mySim.TotalEntryWaitingTimeWalkInP;
        var entryQueueWaitingTimeAmbulance = mySim.TotalEntryWaitingTimeAmbulanceP;
        var entryQueueLength = mySim.TotalEntryQueueLength;
        var medicalTreatWaitTimeA = mySim.TotalMedicalTreatWaitingTimePatientsA;
        var medicalTreatWaitTimeAB = mySim.TotalMedicalTreatWaitingTimePatientsAB;
        var medicalTreatWaitTimeB = mySim.TotalMedicalTreatWaitingTimePatientsB;
        var allDoctorsUtil = mySim.TotalDoctorsUtil;
        var allNursesUtil = mySim.TotalNursesUtil;
        var allRoomAUtl = mySim.TotalRoomAUtil;
        var allRoomBUtil = mySim.TotalRoomBUtil;

        dto.Stats["TotalPatientsInSystem"] = new OneStat(totalPInSystem.GetConfidenceInterval());
        dto.Stats["TotalWalkInInSystem"] = new OneStat(totalWalkinPInSystem.GetConfidenceInterval());
        dto.Stats["TotalAmbulancedInSystem"] = new OneStat(totalAmbulancedPInSystem.GetConfidenceInterval());
        
        dto.Stats["TotalTimeInSystem"] = new OneStat(totalTimeInSystem.GetConfidenceInterval());
        dto.Stats["TotalTimeInSystemWalkIn"] = new OneStat(totalTimeInSystemWalkInPatient.GetConfidenceInterval());
        dto.Stats["TotalTimeInSystemAmbulanced"] = new OneStat(totalTimeInSystemAmbulancePatient.GetConfidenceInterval());
        
        dto.Stats["EntryQueueWaitWalkIn"] = new OneStat(entryQueueWaitingTimeWalkIn.GetConfidenceInterval());
        dto.Stats["EntryQueueWaitAmbulanced"] = new OneStat(entryQueueWaitingTimeAmbulance.GetConfidenceInterval());
        dto.Stats["EntryQueueLength"] = new OneStat(entryQueueLength.GetConfidenceInterval());
        
        dto.Stats["MedicalTreatWaitingTimeA"] = new OneStat(medicalTreatWaitTimeA.GetConfidenceInterval());
        dto.Stats["MedicalTreatWaitingTimeAB"] = new OneStat(medicalTreatWaitTimeAB.GetConfidenceInterval());
        dto.Stats["MedicalTreatWaitingTimeB"] = new OneStat(medicalTreatWaitTimeB.GetConfidenceInterval());
        
        dto.Stats["AllDoctorsUtil"] = new OneStat(allDoctorsUtil.GetConfidenceInterval());
        dto.Stats["AllNursesUtil"] = new OneStat(allNursesUtil.GetConfidenceInterval());
        dto.Stats["AllRoomAUtil"] = new OneStat(allRoomAUtl.GetConfidenceInterval());
        dto.Stats["AllRoomBUtil"] = new OneStat(allRoomBUtil.GetConfidenceInterval());

        OnTurboUI?.Invoke(dto);
    }

    // // public void OnRefreshUI(Simu)
    public void UpdateGui(OSPABA.Simulation Sim)
    {
        // if (DateTime.Now - _lastRefreshTime < _refreshInterval) 
        //     return;

        _lastRefreshTime = DateTime.Now;
        var mySim = (MySimulation)Sim;
        var state = new SimulationStateDto
        {
            CurrentTime = mySim.CurrentTime,
            EntryQueue = mySim.AgentEDepartment.EntryQueue.GetAllItems(),
            EntryQueueAvgLength = mySim.AgentEDepartment.EntryQueue.GetAverageQueueLength(mySim.CurrentTime),
            MedicalTreatQueueA = mySim.AgentEDepartment.MedicalTreatQueueA.GetAllItems(),
            MedicalQueueAvgLengthA = mySim.AgentEDepartment.MedicalTreatQueueA.GetAverageQueueLength(mySim.CurrentTime),
            MedicalTreatQueueB =  mySim.AgentEDepartment.MedicalTreatQueueB.GetAllItems(),
            MedicalQueueAvgLengthB = mySim.AgentEDepartment.MedicalTreatQueueB.GetAverageQueueLength(mySim.CurrentTime),
            AllDoctors = mySim.AgentResources.AllDoctors.ToList(),
            AllNurses = mySim.AgentResources.AllNurses.ToList(),
            AllPatients = mySim.AgentEnviroment.AllPatientsInSystem.Values.ToList(),
            ARooms = mySim.AgentResources.AllRoomsTypeA.ToList(),
            BRooms = mySim.AgentResources.AllRoomsTypeB.ToList(),
        };
        
        OnRefreshUI?.Invoke(state);
    }

    public void PauseSimulation()
    {
        _core.PauseSimulation();
    }

    public void ResumeSimulation()
    {
        _core.ResumeSimulation();
    }

    public void StopSimulation()
    {
        _core.StopSimulation();
        _core = null;
    }
    
    public bool IsPaused()
    {
        return _core.IsPaused();
    }

    public void SetSimulationSpeed(double interval, double duration)
    {
        _core.SetSimSpeed(interval, duration);
    }
}