using DISS_SEM_GUI.EventsArguments;
using Simulation;

namespace DISS_sem_3;

public class SimulationModel
{
    private MySimulation _core;
    
    public event Action<SimulationStateDto> OnRefreshUI;

    public void StartSimulation(StartSimulationArgs args)
    {
        _core = new MySimulation(args.Seed);
        
        _core.SetSimSpeed(1.0, 0.05);
        _core.OnRefreshUI(UpdateGui);
        
        _core.SimulateAsync(args.Replications, args.EndSimulationTime);
        
        // _core.OnRefreshUI(OnRefreshUI);
    }
    
    // // public void OnRefreshUI(Simu)
    public void UpdateGui(OSPABA.Simulation Sim)
    {
        var mySim = (MySimulation)Sim;
        var state = new SimulationStateDto
        {
            CurrentTime = mySim.CurrentTime,
            EntryQueue = mySim.AgentEDepartment.EntryQueue.GetAllItems(),
            EntryQueueAvgLength = mySim.AgentEDepartment.EntryQueue.GetAverageQueueLength(mySim.CurrentTime),
            FreeDoctors = mySim.AgentResources.Doctors.ToList(),
            FreeNurses = mySim.AgentResources.Nurses.ToList(),
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