using Airport_GUI;
using DISS_sem_3.GuiApp.Windows;
using DISS_SEM_GUI.EventsArguments;
using Simulation;

namespace DISS_sem_3;

public class WelchController
{
    // ── Welch parameters ──────────────────────────────────────────────
        // How many replications to average across. More = smoother curve.
        private const int NumberOfReplications = 150;
 
        // Half-width of the moving-average window (full width = 2w+1).
        // Increase if the resulting curve is still too noisy.
        private const int WelchWindowSize = 10;
 
        // Cap on ScottPlot data points to keep rendering fast.
        private const int MaxPlotPoints = 5000;
 
        private const int MetricCount = 30;
 
        // ── State ─────────────────────────────────────────────────────────
        private StartSimulationArgs _args;
        private readonly object _lock;
        private WelchWindow? _window;
        private MySimulation? _activeCore;   // kept so Pause/Stop can reach it
        private volatile bool _isRunning;
 
        // ── Public API ────────────────────────────────────────────────────
 
        public void ShowWindow(StartSimulationArgs args)
        {
            _window = new WelchWindow();
            _args  = args;
            _window.OnRunRequested    += (s, e) => { _ = StartAsync(); };
            _window.OnPauseRequested  += (s, e) => Pause();
            _window.OnResumeRequested += (s, e) => Resume();
            _window.OnStopRequested   += (s, e) => Stop();
            _window.FormClosed        += (s, e) => { Stop(); _window = null; };
            _window.Show();
        }
 
 
        public void Pause()  => _activeCore?.PauseSimulation();
        public void Resume() => _activeCore?.ResumeSimulation();
        public void Stop()
        {
            _isRunning = false;
            _activeCore?.StopSimulation();
        }
 
        // ── Core loop ─────────────────────────────────────────────────────
 
        public async Task StartAsync()
        {
            _isRunning = true;
 
            // repVals[metricIndex][replicationIndex] = list of per-tick samples
            var repVals = new List<List<double>>[MetricCount];
            for (int m = 0; m < MetricCount; m++) repVals[m] = new List<List<double>>();
 
            List<double>? commonTimes = null;   // x-axis: simulation time at each tick
 
            try
            {
                for (int r = 0; r < NumberOfReplications; r++)
                {
                    if (!_isRunning) break;
                    Console.WriteLine($"[Welch] Replication {r + 1}/{NumberOfReplications}");
 
                    // ── 1. Build per-replication args ───────────────────────
                    // Unique seed per replication; single-replication run so
                    // we capture the full within-replication time series.
                    var repArgs = new StartSimulationArgs(
                        seed:            _args.Seed + r,
                        replications:    1,
                        observationMode: false,
                        endSimulationTime: _args.EndSimulationTime,
                        nursesCount:     _args.NursesCount,
                        doctorsCount:    _args.DoctorsCount, 
                        entryMax: _args.EntryMax,
                        medicalMax: _args.MedicalMax,// unused in ED model
                        timeIntervalSeconds: _args.TimeIntervalSeconds,
                        exp4WaitTime: 0,
                        experimentVariant:  0,             // unused
                        refreshRate:     0,             // turbo — no GUI refresh
                        warmUpProof:     true,          // signals simulation to emit graph events
                        warmUp:          0,
                        turboMode:       true
                    );
 
                    // ── 2. Wire a lightweight simulation that emits graph ticks ──
                    var sim = new SimulationModel();
                    // _activeCore = sim;
                    var tcs = new TaskCompletionSource<bool>();
                    sim.OnTurboUI += (stats) => { if (stats.Replication >= 0) tcs.TrySetResult(true); };
                    
                    var tickTimes = new List<double>();
                    var tickVals  = new List<double>[MetricCount];
                    for (int m = 0; m < MetricCount; m++) tickVals[m] = new List<double>();
 
                    // OnGraphTick fires at regular simulation-time intervals.
                    // Collect current running averages from AgentEnviroment.
                    sim.OnWelchUpdate += (welchDto) =>
                    {
                        tickTimes.Add(welchDto.CurrentTime);
                        tickVals[0].Add(welchDto.CurrentPatientCount);
                        tickVals[1].Add(welchDto.CurrentWalkInPatientCount);
                        tickVals[2].Add(welchDto.CurrentAmbulancePatientCount);
                        tickVals[3].Add(welchDto.CurrentEntryQueueLength);
                        tickVals[4].Add(welchDto.CurrentMedicalTreatWaitingCount);
                        tickVals[5].Add(welchDto.CurrentAllDoctorsUtil);
                        tickVals[6].Add(welchDto.CurrentAllNursesUtil);
                        tickVals[7].Add(welchDto.CurrentAllRoomAUtil);
                        tickVals[8].Add(welchDto.CurrentAllRoomBUtil);
                        tickVals[10].Add(welchDto.TotalPatientsInSystem);
                        tickVals[11].Add(welchDto.TotalWalkInPatientsInSystem);
                        tickVals[12].Add(welchDto.TotalAmbulancePatientsInSystem);
                        tickVals[13].Add(welchDto.TotalTimeInSystemAll);
                        tickVals[14].Add(welchDto.TotalTimeInSystemWalkIn);
                        tickVals[15].Add(welchDto.TotalTimeInSystemAmbulance);
                        tickVals[16].Add(welchDto.EntryQueueWaitingTimeWalkIn);
                        tickVals[17].Add(welchDto.EntryQueueWaitingTimeAmbulanced);
                        tickVals[18].Add(welchDto.EntryQueueLength);
                        tickVals[19].Add(welchDto.MedicalTreatWaitingTimeA);
                        tickVals[20].Add(welchDto.MedicalTreatWaitingTimeAB);
                        tickVals[21].Add(welchDto.MedicalTreatWaitingTimeB);
                        tickVals[22].Add(welchDto.AllDoctorsUtil);
                        tickVals[23].Add(welchDto.AllNursesUtil);
                        tickVals[24].Add(welchDto.AllRoomsAUtil);
                        tickVals[25].Add(welchDto.AllRoomsBUtil);
                        tickVals[26].Add(welchDto.FromEntryToMedicalWalkIn);
                        tickVals[27].Add(welchDto.FromEntryToMedicalAmbulanced);
                        tickVals[28].Add(welchDto.MedicalQueueLengthTypeA);
                        tickVals[29].Add(welchDto.MedicalQueueLengthTypeB);
                    };
 
                    // Run on background thread, await completion
                    sim.StartSimulation(repArgs);
                    await tcs.Task;
                    _activeCore = null;
 
                    if (!_isRunning) break;
 
                    // ── 3. Store replication data ───────────────────────────
                    // All replications must align on the same x-axis.
                    // Use the first replication's tick times as the reference.
                    if (commonTimes == null) commonTimes = tickTimes;
 
                    int obs = commonTimes.Count;
                    for (int m = 0; m < MetricCount; m++)
                        repVals[m].Add(PadOrTrim(tickVals[m], obs));
 
                    // ── 4. Compute Welch's moving average ───────────────────
                    int completedReps = r + 1;
                    var welch = ComputeWelch(repVals, completedReps, obs);
 
                    // ── 5. Downsample + render ──────────────────────────────
                    //here  was lock
                    var (sx, snapshots) = Downsample(commonTimes, welch, obs);
                    try { _window?.RenderSnapshot(sx, snapshots); } catch { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Welch] Error: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
            }
        }
 
        // ── Welch algorithm ───────────────────────────────────────────────
 
        /// <summary>
        /// Step A: average across replications at each observation index.
        /// Step B: apply moving-average smoothing with window width WelchWindowSize.
        /// </summary>
        private double[][] ComputeWelch(List<List<double>>[] repVals, int completedReps, int obs)
        {
            var result = new double[MetricCount][];
 
            for (int m = 0; m < MetricCount; m++)
            {
                // Step A: cross-replication average at each point i
                var crossAvg = new double[obs];
                for (int i = 0; i < obs; i++)
                {
                    double sum = 0;
                    for (int k = 0; k < completedReps; k++)
                    {
                        if (i < repVals[m][k].Count)
                            sum += repVals[m][k][i];
                    }
                    crossAvg[i] = sum / completedReps;
                }
 
                // Step B: moving average smoothing
                var smoothed = new double[obs];
                for (int i = 0; i < obs; i++)
                {
                    int start = Math.Max(0, i - WelchWindowSize);
                    int end   = Math.Min(obs - 1, i + WelchWindowSize);
                    double sum = 0;
                    for (int j = start; j <= end; j++) sum += crossAvg[j];
                    smoothed[i] = sum / (end - start + 1);
                }
 
                result[m] = smoothed;
            }
 
            return result;
        }
 
        // ── Helpers ───────────────────────────────────────────────────────
 
        /// <summary>
        /// Uniform-index downsample so ScottPlot never gets more than MaxPlotPoints.
        /// </summary>
        private (double[] sx, double[][] snapshots) Downsample(
            List<double> times, double[][] welch, int obs)
        {
            int take = Math.Min(obs, MaxPlotPoints);
            var sx        = new double[take];
            var snapshots = new double[MetricCount][];
            for (int m = 0; m < MetricCount; m++) snapshots[m] = new double[take];
 
            if (obs <= take)
            {
                sx = times.ToArray();
                for (int m = 0; m < MetricCount; m++) snapshots[m] = welch[m];
            }
            else
            {
                for (int k = 0; k < take; k++)
                {
                    int idx = (int)Math.Round((double)k * (obs - 1) / (take - 1));
                    idx = Math.Clamp(idx, 0, obs - 1);
                    sx[k] = times[idx];
                    for (int m = 0; m < MetricCount; m++) snapshots[m][k] = welch[m][idx];
                }
            }
 
            return (sx, snapshots);
        }
 
        /// <summary>
        /// Ensures a replication's sample list has exactly <paramref name="length"/> entries.
        /// Shorter lists are padded with their last value; longer lists are trimmed.
        /// </summary>
        private List<double> PadOrTrim(List<double> src, int length)
        {
            if (src.Count == length) return src;
 
            var result = new List<double>(length);
            result.AddRange(src);
 
            double last = src.Count > 0 ? src[^1] : 0.0;
            while (result.Count < length) result.Add(last);
 
            if (result.Count > length) result.RemoveRange(length, result.Count - length);
            return result;
        }
}