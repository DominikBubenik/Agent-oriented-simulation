using DISS_sem_3;
using DISS_sem_3.GuiApp.Windows;
using DISS_SEM_GUI;
using DISS_SEM_GUI.EventsArguments;

namespace DISS_sem_3;

public class ObservationController
{
    private SimulationModel? _currentModel;
    private StartSimulationArgs? _args;
    private ObservationWindow? _laneWindow;
    private AnimatorWindow _animatorWindow;
    private volatile bool _isStarted = false;
    
    public void ShowWindow(StartSimulationArgs args)
    {
        _args = args;
        _laneWindow = new ObservationWindow(_args);
        _currentModel = new SimulationModel();
        
        _laneWindow.OnRunRequested += OnRunRequested;
        _laneWindow.OnPauseRequested += OnPauseRequested;
        _laneWindow.OnOpenAnimatorRequested += OnOpenAnimatorRequested;

        _currentModel.OnRefreshUI += OnObservRefresh;
        
        _laneWindow.Show();
    }

    private void OnRunRequested(object? sender, EventArgs e)
    {
        if (_args == null || _currentModel == null) return;

        _currentModel.StartSimulation(_args);
    }

    private void OnObservRefresh(SimulationStateDto state)
    {
        _laneWindow?.RefreshView(state);
    }

    private void OnPauseRequested(object? sender, EventArgs e)
    {
        if (_currentModel.IsPaused())
        {
            _currentModel.ResumeSimulation();
        }
        else
        {
            _currentModel.PauseSimulation();
        }
    }
    
    private void OnOpenAnimatorRequested(object? sender, EventArgs e)
    {
        var animator = _currentModel.CreateAnimator(_args);
        if (animator != null)
        {
            _animatorWindow = new AnimatorWindow();
            _animatorWindow.SetAnimator(animator);
            _animatorWindow.Show();
        }
        else
        {
            throw new Exception("No animator found");
        }
    }
}