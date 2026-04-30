using DISS_SEM_GUI;
using Simulation;

namespace DISS_sem_3;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        var sim = new MySimulation(0);
        // sim.Simulate(1);
        // return;
        var view = new MainView();
        var controller = new SimulationController(view);
        
        Application.Run(view);
    }
}