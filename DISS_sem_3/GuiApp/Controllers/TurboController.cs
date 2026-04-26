using Airport_GUI;
using DISS_SEM_GUI;
using DISS_SEM_GUI.EventsArguments;

namespace DISS_sem_3;

public class TurboController
{
    private TurboWindow? _turboWindow;
    private SimulationModel? _currentModel;
    private StartSimulationArgs? _args;
    private volatile bool _isStarted = false;
    private int _skipCount;
    private List<double> xs;
    private List<double>[] valsAvg;
    private List<double>[] valsLower;
    private List<double>[] valsUpper;

    private Dictionary<string, StatHistory> _history;
    
    public void ShowWindow(StartSimulationArgs args)
    {
        _args = args;
        _turboWindow = new TurboWindow();
        _currentModel = new SimulationModel();
        _history = new();
        
        _turboWindow.OnRunRequested += OnRunRequested;
        _turboWindow.OnPauseRequested += OnPauseRequested;

        _currentModel.OnTurboUI += OnTurboRefresh;
        
        _turboWindow.Show();
    }
      
    private void OnRunRequested(object? sender, EventArgs e)
    {
        if (_args == null || _currentModel == null) return;
        InitBeforeRun();
        _currentModel.StartSimulation(_args);
        try { _turboWindow?.SetStatus("Running"); } catch { }
    }

    private void InitBeforeRun()
    {
        var skipFrac = 0.05; // default 5%
        try { if (_turboWindow != null) skipFrac = _turboWindow.GetSkipFractionFromUI(); } catch { }
        _skipCount = Math.Max(0, (int)Math.Floor(_args.Replications * skipFrac));
        
        xs = new List<double>();
        valsAvg = new List<double>[6];
        valsLower = new List<double>[6];
        valsUpper = new List<double>[6];
        for (int m = 0; m < 6; m++) { valsAvg[m] = new List<double>(); valsLower[m] = new List<double>(); valsUpper[m] = new List<double>(); }
    }

    private void OnTurboRefresh(SimulationStatsDto st)
    {
         // Skip initial replications as requested by UI
         if (st.Replication <= _skipCount) return;
         
         
         foreach (var kvp in st.Stats)
         {
             if (kvp.Value == null) continue;

             if (!_history.ContainsKey(kvp.Key))
                 _history[kvp.Key] = new StatHistory();

             var h = _history[kvp.Key];
             h.X.Add(st.Replication);
             h.Avg.Add(kvp.Value.Avg);
             h.Lower.Add(kvp.Value.LowerBound);
             h.Upper.Add(kvp.Value.UpperBound);
         }

         // Send the complete history to the window
         _turboWindow?.UpdateStats(st);
         _turboWindow?.UpdateDashboard(st.Replication, _history);
        
         // xs.Add(st.Replication);
         
        //  // helper to safely extract avg and CI bounds
        //  double a0 = st.SumAvgTimeInSystem?.Avg ?? double.NaN;
        //  double l0 = st.SumAvgTimeInSystem?.LowerBound ?? double.NaN;
        //  double u0 = st.SumAvgTimeInSystem?.UpperBound ?? double.NaN;
        //  
        //  double a1 = st.SumAvgEntryQueue?.Avg ?? double.NaN;
        //  double l1 = st.SumAvgEntryQueue?.LowerBound ?? double.NaN;
        //  double u1 = st.SumAvgEntryQueue?.UpperBound ?? double.NaN;
        //  
        //  double a2 = st.SumAvgDetectorQueue?.Avg ?? double.NaN;
        //  double l2 = st.SumAvgDetectorQueue?.LowerBound ?? double.NaN;
        //  double u2 = st.SumAvgDetectorQueue?.UpperBound ?? double.NaN;
        //  
        //  double a3 = st.SumAvgBeforeDetector?.Avg ?? double.NaN;
        //  double l3 = st.SumAvgBeforeDetector?.LowerBound ?? double.NaN;
        //  double u3 = st.SumAvgBeforeDetector?.UpperBound ?? double.NaN;
        //  
        //  double a4 = st.SumAvgAfterDetector?.Avg ?? double.NaN;
        //  double l4 = st.SumAvgAfterDetector?.LowerBound ?? double.NaN;
        //  double u4 = st.SumAvgAfterDetector?.UpperBound ?? double.NaN;
        //  
        //  double a5 = st.SumAvgWaitingQueue?.Avg ?? double.NaN;
        //  double l5 = st.SumAvgWaitingQueue?.LowerBound ?? double.NaN;
        //  double u5 = st.SumAvgWaitingQueue?.UpperBound ?? double.NaN;
        //  
        //  valsAvg[0].Add(a0); valsLower[0].Add(l0); valsUpper[0].Add(u0);
        //  valsAvg[1].Add(a1); valsLower[1].Add(l1); valsUpper[1].Add(u1);
        //  valsAvg[2].Add(a2); valsLower[2].Add(l2); valsUpper[2].Add(u2);
        //  valsAvg[3].Add(a3); valsLower[3].Add(l3); valsUpper[3].Add(u3);
        //  valsAvg[4].Add(a4); valsLower[4].Add(l4); valsUpper[4].Add(u4);
        //  valsAvg[5].Add(a5); valsLower[5].Add(l5); valsUpper[5].Add(u5);
        //  
        //  // Convert to arrays and render the accumulated history so far
        //  double[] ax = xs.ToArray();
        //  double[][] avgSnaps = new double[6][];
        //  double[][] lowerSnaps = new double[6][];
        //  double[][] upperSnaps = new double[6][];
        //  for (int m = 0; m < 6; m++) { avgSnaps[m] = valsAvg[m].ToArray(); lowerSnaps[m] = valsLower[m].ToArray(); upperSnaps[m] = valsUpper[m].ToArray(); }
        // try { _turboWindow?.RenderSnapshot(ax, avgSnaps, lowerSnaps, upperSnaps); } catch (Exception ex) { Console.WriteLine("RenderSnapshot failed: " + ex.Message); }
        // try { _turboWindow?.UpdateStats(st); } catch (Exception ex) { Console.WriteLine("UpdateStats failed: " + ex.Message); }
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
}