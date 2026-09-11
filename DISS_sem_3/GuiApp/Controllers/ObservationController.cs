using DISS_sem_3;
using DISS_sem_3.GuiApp.Windows;
using DISS_SEM_GUI;
using DISS_SEM_GUI.EventsArguments;

namespace DISS_sem_3;

public class ObservationController
{
    private SimulationModel? _currentModel;
    private StartSimulationArgs? _args;
    private ObservationWindow? _observWindow;
    private AnimatorWindow _animatorWindow;
    private volatile bool _isStarted = false;
    
    public void ShowWindow(StartSimulationArgs args)
    {
        _args = args;
        _observWindow = new ObservationWindow(_args);
        _currentModel = new SimulationModel();
        
        _observWindow.OnRunRequested += OnRunRequested;
        _observWindow.OnPauseRequested += OnPauseRequested;
        _observWindow.OnOpenAnimatorRequested += OnOpenAnimatorRequested;
        _observWindow.OnStopRequested += OnStopRequested;
        _observWindow.OnChangeSpeed += OnChangeSpeed;
        _currentModel.OnRefreshUI += OnObservRefresh;
        _currentModel.OnLoggerUpdate += OnLoggerOutput;
        
        _observWindow.FormClosed += OnWindowClosed;
        _observWindow.Show();
    }

    private void OnStopRequested(object? sender, EventArgs e)
    {
        if (_currentModel == null) return;
        _currentModel.StopSimulation();
    }

    private void OnRunRequested(object? sender, EventArgs e)
    {
        if (_args == null || _currentModel == null) return;
        _args.ObservationMode = true;
        _currentModel.StartSimulation(_args);
    }

    private void OnObservRefresh(SimulationStateDto state)
    {
        if (_observWindow == null || _observWindow.IsDisposed) return;
        try { _observWindow?.RefreshView(state); } catch { }
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

    private void OnChangeSpeed(double interval, double duration)
    {
        _currentModel.SetSimulationSpeed(interval, duration);
    }
    
    private void OnWindowClosed(object? sender, FormClosedEventArgs e)
    {
        try
        {
            _currentModel?.StopSimulation();
            if (_currentModel != null)
            {
                _currentModel.OnRefreshUI -= OnObservRefresh;
            }
            if (_observWindow != null)
            {
                _observWindow.OnRunRequested -= OnRunRequested;
                _observWindow.OnPauseRequested -= OnPauseRequested;
                _observWindow.OnOpenAnimatorRequested -= OnOpenAnimatorRequested;
                _observWindow.OnStopRequested -= OnStopRequested;
                _observWindow.OnChangeSpeed -= OnChangeSpeed;

                _observWindow.FormClosed -= OnWindowClosed;
            }
            
            _observWindow = null;
            _currentModel = null;
            _args = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Controller cleanup failed: " + ex.Message);
        }
    }

    private void OnLoggerOutput(string message, double time)
    {
        _observWindow.AppendLog(message, time);
    }
}