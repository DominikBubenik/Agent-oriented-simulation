namespace MainLogic;

public abstract class SimulationCore
{
    public bool EndSimulation { get; set; } = false;
    public int CurrentReplication { get; set; } = 0;

    public void RunSimulation(int countReplications)
    {
        CurrentReplication = 0;
        BeforeSimulation();
        while (CurrentReplication < countReplications && !EndSimulation)
        {
            BeforeReplication();
            DoReplication();
            AfterReplication();
            CurrentReplication++;
        }

        AfterSimulation();
    }

    public abstract void BeforeReplication();

    public abstract void BeforeSimulation();

    public abstract void AfterReplication();

    public abstract void AfterSimulation();

    public abstract void DoReplication();
}