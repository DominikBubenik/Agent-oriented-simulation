using DISS_SEM_GUI.EventsArguments;
using Simulation;

namespace DISS_sem_3;
/**
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 2
 */
public class SimulationModel
{
    private MySimulation _core;
    
    public event Action<SimulationStateDto> OnRefreshUI;
    private DateTime _lastRefreshTime = DateTime.MinValue;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMilliseconds(60); // ~30 FPS

    public void StartSimulation(StartSimulationArgs args)
    {
        _core = new MySimulation(args.Seed);
        
        _core.SetSimSpeed(1.0, 0.05);
        _core.OnRefreshUI(UpdateGui);
        
        _core.SimulateAsync(args.Replications, args.EndSimulationTime);
    }
    
    // // public void OnRefreshUI(Simu)
    public void UpdateGui(OSPABA.Simulation Sim)
    {
        if (DateTime.Now - _lastRefreshTime < _refreshInterval) 
            return;

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
            FreeDoctors = mySim.AgentResources.Doctors.ToList(),
            AllNurses = mySim.AgentResources.AllNurses.ToList(),
            ARooms = mySim.AgentResources.AllRoomsTypeA.ToList(),
            BRooms = mySim.AgentResources.AllRoomsTypeB.ToList(),
        };
        
        OnRefreshUI?.Invoke(state);
        Console.WriteLine($"{_core.GetType().Name} Len              dsfdsffffffff            dsffds t");
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
    }
    
    public bool IsPaused()
    {
        return _core.IsPaused();
    }
}