using DISS_SEM_GUI;
using DISS_SEM_GUI.EventsArguments;

namespace DISS_sem_3;

public class SimulationController
{
    private readonly ObservationController _observationController;
    private readonly MainView _view;
    
     public SimulationController(MainView view)
    {
        _view = view;
        
        _observationController = new ObservationController();
        
        _view.OnOpenObservationRequested += (s, e) => {
            Console.WriteLine("lets gooo");
            var args = _view.GetCurrentArguments();
            _observationController.ShowWindow(args);
        };
    }

    public async void StartSimulation(StartSimulationArgs args)
    {
        
    }
}