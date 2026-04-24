using MainLogic;
using System.Linq;
using DISS_sem_3;
using DISS_sem_3.Entities;
using DISS_SEM_GUI.EventsArguments;

/**
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 8
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 9
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 22
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 24
 */
namespace Airport_GUI
{
    public partial class ObservationWindow : Form
    {
        public DataGridView? EntryQueue { get; set; }

        // Small labels displayed near tables for per-lane stats
        public Label? EntryStatsLabel { get; set; }
        public Label? DetectorStatsLabel { get; set; }
        public Label? WaitStatsLabel { get; set; }
        
        public event EventHandler? OnPauseRequested;
        public event EventHandler? OnStopRequested;
        public event EventHandler? OnRunRequested;
        public event EventHandler? OnWindowClosed;
        // Sleep control events: ms and period (seconds)
        public event Action<int>? OnSleepMsChanged;
        public event Action<double>? OnSleepPeriodChanged;
        // Refresh rate event (int 0..1000)
        public event Action<int>? OnRefreshRateChanged;

        public ObservationWindow()
        {
            InitializeComponent();

            flpLanes.Resize += (s, e) =>
            {
                foreach (Control ctrl in flpLanes.Controls)
                {
                    if (ctrl is GroupBox)
                    {
                        ctrl.Width = flpLanes.ClientSize.Width - 25;
                    }
                }
            };

            this.FormClosing += LaneWindow_FormClosing;
        }

        // Constructor overload that accepts simulation start arguments
        public ObservationWindow(StartSimulationArgs args) : this()
        {
            SetParameters(args);
        }

        public void SetParameters(StartSimulationArgs args)
        {
            // Safely set parameter textboxes (they are readonly in the designer)
            try
            {
                txtSeed.Text = args.Seed.ToString();
                txtReplications.Text = args.Replications.ToString();
                txtObservation.Text = args.ObservationMode ? "True" : "False";

                // If start/end look like seconds, format as hh:mm:ss
            

                try
                {
                    txtEndSimulationTime.Text = TimeSpan.FromSeconds(args.EndSimulationTime).ToString();
                }
                catch { txtEndSimulationTime.Text = args.EndSimulationTime.ToString("F2"); }

                txtSecurityLanes.Text = args.SecurityLanesCount.ToString();
                txtBeforeDetectors.Text = args.BeforeDetectorCount.ToString();
                txtAfterDetectors.Text = args.AfterDetectorCount.ToString();
            }
            catch
            {
                // ignore any failure to set UI values
            }
        }
        
        private void BtnRun_Click(object? sender, EventArgs e)
        {
            try
            {
                if (btnRun != null && !btnRun.IsDisposed)
                {
                    btnRun.Enabled = false;
                    btnRun.Text = "Running...";
                }
            }
            catch { }

            Console.WriteLine("ObservationWindow: Run button clicked");
            CreateLaneTables();
            // Trigger the custom event that the ObservationController is listening for
            OnRunRequested?.Invoke(this, EventArgs.Empty);
        }
        
        private void BtnStop_Click(object? sender, EventArgs e)
        {
            OnStopRequested?.Invoke(this, EventArgs.Empty);
        }

        private void StopSimulation()
        {
            // kept for backward compatibility but not used by designer's Stop button now
            OnStopRequested?.Invoke(this, EventArgs.Empty);
        }

        //Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 10
        public void SetSleepControls(int sleepMs, double sleepPeriodSeconds)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetSleepControls(sleepMs, sleepPeriodSeconds));
                return;
            }

            // trackSleepMs expects milliseconds directly
            trackSleepMs.Minimum = Math.Min(trackSleepMs.Minimum, sleepMs);
            trackSleepMs.Value = Math.Clamp(sleepMs, trackSleepMs.Minimum, trackSleepMs.Maximum);
            lblSleepMsValue.Text = trackSleepMs.Value.ToString();

            // trackSleepPeriodSec stores centiseconds (0.01s) — convert seconds to centiseconds
            int csPeriod = (int)Math.Round(sleepPeriodSeconds * 100.0);
            trackSleepPeriodSec.Minimum = Math.Min(trackSleepPeriodSec.Minimum, csPeriod);
            trackSleepPeriodSec.Value = Math.Clamp(csPeriod, trackSleepPeriodSec.Minimum, trackSleepPeriodSec.Maximum);
            lblSleepPeriodValue.Text = (trackSleepPeriodSec.Value / 100.0).ToString("F2");
        }

        public void SetRefreshRate(int rate)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetRefreshRate(rate));
                return;
            }

            trackRefreshRate.Value = Math.Clamp(rate, trackRefreshRate.Minimum, trackRefreshRate.Maximum);
            lblRefreshRateValue.Text = trackRefreshRate.Value.ToString();
        }

        // Controller can call this to toggle the Run button state and label (thread-safe)
        public void SetRunRunning(bool running)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetRunRunning(running));
                return;
            }

            try
            {
                if (btnRun != null && !btnRun.IsDisposed)
                {
                    btnRun.Enabled = !running;
                    btnRun.Text = running ? "Running..." : "Run";
                }
            }
            catch { }
        }

        // Simple enable/disable helper
        public void SetRunEnabled(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetRunEnabled(enabled));
                return;
            }

            try { if (btnRun != null && !btnRun.IsDisposed) btnRun.Enabled = enabled; } catch { }
        }

        private void LaneWindow_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Notify listeners that the window is being closed so simulation can be stopped if desired
            OnWindowClosed?.Invoke(this, EventArgs.Empty);
        }

        // Controller will manage the paused state; this setter allows controller to update the UI
        public void SetPaused(bool paused)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetPaused(paused));
                return;
            }

            try
            {
                if (btnPause != null && !btnPause.IsDisposed)
                    btnPause.Text = paused ? "Resume" : "Pause";
            }
            catch { }
        }

        private void BtnPause_Click(object? sender, EventArgs e)
        {
            // Let controller toggle the paused state and update UI via SetPaused
            OnPauseRequested?.Invoke(this, EventArgs.Empty);
        }

        // Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 10
        private void TrackSleepMs_ValueChanged(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => TrackSleepMs_ValueChanged(sender, e));
                return;
            }

            int value = trackSleepMs.Value;
            lblSleepMsValue.Text = value.ToString();
            OnSleepMsChanged?.Invoke(value);
        }

        private void TrackSleepPeriodSec_ValueChanged(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => TrackSleepPeriodSec_ValueChanged(sender, e));
                return;
            }

            // track value is in centiseconds (0.01s); convert to seconds when raising the event
            int csValue = trackSleepPeriodSec.Value; // centiseconds
            double seconds = csValue / 100.0;
            lblSleepPeriodValue.Text = seconds.ToString("F2");
            OnSleepPeriodChanged?.Invoke(seconds);
        }

        private void TrackRefreshRate_ValueChanged(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => TrackRefreshRate_ValueChanged(sender, e));
                return;
            }

            int value = trackRefreshRate.Value;
            lblRefreshRateValue.Text = value.ToString();
            OnRefreshRateChanged?.Invoke(value);
        }

        // Thread-safe method to update the current simulation time textbox.
        public void UpdateSimulationTime(double time)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => UpdateSimulationTime(time));
                return;
            }

            try
            {
                // show current simulation time in the textbox only
                var ts = TimeSpan.FromSeconds(time);
                txtCurrentTime.Text = ts.ToString();
            }
            catch
            {
                txtCurrentTime.Text = time.ToString("F2");
            }
        }

        public void RefreshView(SimulationStateDto state)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => RefreshView(state));
                return;
            }
            
            UpdateSimulationTime(state.CurrentTime);
            UpdatePassengersGrid(EntryQueue, state.EntryQueue);
            Console.WriteLine("refreshi");
         }

        // Append a log message to the bottom log DataGridView (thread-safe)
        // Keeps history bounded by maxEntries to avoid unlimited growth.
        private const int MaxLogEntries = 2000;
        public void AppendLog(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => AppendLog(message));
                return;
            }

            try
            {
                if (dgvLog == null) return;

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                dgvLog.Rows.Add(time, message);

                // Keep latest entry visible
                if (dgvLog.Rows.Count > 0)
                {
                    int last = dgvLog.Rows.Count - 1;
                    dgvLog.FirstDisplayedScrollingRowIndex = Math.Max(0, last - 10);
                }

                // Trim old entries if exceeding MaxLogEntries
                while (dgvLog.Rows.Count > MaxLogEntries)
                {
                    try { dgvLog.Rows.RemoveAt(0); } catch { break; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AppendLog failed: " + ex.Message);
            }
        }

        public void ClearLog()
        {
            if (this.InvokeRequired) { this.Invoke(() => ClearLog()); return; }
            try { dgvLog?.Rows.Clear(); } catch { }
        }


        private void CreateLaneTables()
        {
            var container = new GroupBox();
            container.Text = $"Lane";
            container.Width = flpLanes.ClientSize.Width - 25;
            container.Height = 600;
            container.Padding = new Padding(6);

            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.AutoSize = false;
            panel.WrapContents = true;
            panel.AutoScroll = true;

            EntryQueue = CreatePatientQueueGrid();

            Panel MakeLabeledContainer(string labelText, DataGridView dgv)
            {
                var p = new Panel();
                p.Width = dgv.Width + 4;
                p.Height = dgv.Height + 26 + 22; // extra space for stats label

                var lbl = new Label();
                lbl.Text = labelText;
                lbl.AutoSize = false;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Width = dgv.Width;
                lbl.Height = 22;
                lbl.Top = 2;
                lbl.Left = 0;
                lbl.Font = new Font(lbl.Font, FontStyle.Bold);

                // Stats label (small) placed under the title, above the grid
                var stats = new Label();
                stats.Text = "Avg: -   Cur: -";
                stats.AutoSize = false;
                stats.TextAlign = ContentAlignment.MiddleLeft;
                stats.Width = dgv.Width;
                stats.Height = 18;
                stats.Top = lbl.Bottom + 2;
                stats.Left = 0;
                stats.Font = new Font("Consolas", 9F);

                dgv.Top = stats.Bottom + 2;
                dgv.Left = 0;

                p.Controls.Add(lbl);
                p.Controls.Add(stats);
                p.Controls.Add(dgv);
                return p;
            }

            // create containers and keep references so we can extract their stats labels
            var pPassengers = MakeLabeledContainer("EntryQueue", EntryQueue);


            panel.Controls.Add(pPassengers);

            container.Controls.Add(panel);

            flpLanes.Controls.Add(container);

            // extract stats labels (they are the second control in each panel)
            Label? passengersStats = pPassengers.Controls.OfType<Label>().Skip(1).FirstOrDefault();
        }

        private void UpdatePassengersGrid(DataGridView? dgv, List<Patient> patients)
        {
            if (dgv == null) return;

            dgv.Rows.Clear();

            if (patients == null) return;

            foreach (var passenger in patients)
            {
                dgv.Rows.Add(passenger.Id, passenger.ArrivalTime, passenger.Priority, passenger.ArrivedByAmbulance);
            }

            dgv.AutoResizeColumns();
        }

        // Controller can call this to read current slider values
        public int GetSleepMs()
        {
            if (this.InvokeRequired)
            {
                return (int)this.Invoke(new Func<int>(() => GetSleepMs()));
            }
            try { return trackSleepMs?.Value ?? 100; } catch { return 100; }
        }

        // trackSleepPeriodSec stores centiseconds (0.01s) — convert to seconds
        public double GetSleepPeriodSeconds()
        {
            if (this.InvokeRequired)
            {
                return (double)this.Invoke(new Func<double>(() => GetSleepPeriodSeconds()));
            }
            try { return (trackSleepPeriodSec?.Value ?? 80) / 100.0; } catch { return 0.8; }
        }

        public int GetRefreshRate()
        {
            if (this.InvokeRequired)
            {
                return (int)this.Invoke(new Func<int>(() => GetRefreshRate()));
            }
            try { return trackRefreshRate?.Value ?? 0; } catch { return 0; }
        }
    }
}
