using System.Diagnostics;
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
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        var view = new MainView();
        var controller = new SimulationController(view);
        
        Application.Run(view);
    }
}