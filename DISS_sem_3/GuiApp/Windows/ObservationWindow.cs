using MainLogic;
using System.Linq;
using DISS_sem_3;
using DISS_sem_3.Entities;
using DISS_SEM_GUI.EventsArguments;

/**
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 2
 */
namespace DISS_sem_3
{
    public partial class ObservationWindow : Form
    {
        public DataGridView? EntryQueue { get; set; }
        public DataGridView? MedicalQueueA { get; set; }
        public DataGridView? MedicalQueueB { get; set; }
        private Dictionary<int, DataGridView> _roomAGrids = new Dictionary<int, DataGridView>();
        private Dictionary<int, DataGridView> _roomBGrids = new Dictionary<int, DataGridView>();
        public DataGridView? AllNurses { get; set; }
        public DataGridView? AllDoctors { get; set; }
        public DataGridView? AllPatients { get; set; }
        
        public Action<double, double> OnChangeSpeed { get; set; }

        public event EventHandler? OnPauseRequested;
        public event EventHandler? OnStopRequested;
        public event EventHandler? OnRunRequested;
        public event EventHandler? OnOpenAnimatorRequested;
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
            
            this.FormClosing += ObservWindow_FormClosing;
        }

        // Constructor overload that accepts simulation start arguments
        public ObservationWindow(StartSimulationArgs args) : this()
        {
            SetParameters(args);
            InitializeLayout(args);
        }

        public void SetParameters(StartSimulationArgs args)
        {
            // Safely set parameter textboxes (they are readonly in the designer)
            try
            {
                txtSeed.Text = args.Seed.ToString();
                txtReplications.Text = args.Replications.ToString();
                txtObservation.Text = args.ObservationMode ? "True" : "False";
                
                try
                {
                    txtEndSimulationTime.Text = GlobalLogger.FormatTime(args.EndSimulationTime);
                }
                catch { txtEndSimulationTime.Text = args.EndSimulationTime.ToString("F2"); }

                txtNurses.Text = args.NursesCount.ToString();
                txtDoctors.Text = args.DoctorsCount.ToString();
            }
            catch
            {
                // ignore any failure to set UI values
            }
        }
        
        private void BtnRun_Click(object? sender, EventArgs e)
        {
            SetRunRunning(true);
            ClearExistingLayout();
            ClearLog();
            CreateLaneTables();
            OnRunRequested?.Invoke(this, EventArgs.Empty);
        }
        
        private void BtnStop_Click(object? sender, EventArgs e)
        {
            SetRunRunning(false);
            OnStopRequested?.Invoke(this, EventArgs.Empty);
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

        private void ObservWindow_FormClosing(object? sender, FormClosingEventArgs e)
        {
            OnPauseRequested = null;
            OnStopRequested = null;
            OnRunRequested = null;
            OnOpenAnimatorRequested = null;
            OnSleepMsChanged = null;
            OnSleepPeriodChanged = null;
            OnRefreshRateChanged = null;
            OnChangeSpeed = null;

            trackInterval.ValueChanged -= TrackInterval_ValueChanged;
            trackDuration.ValueChanged -= TrackDuration_ValueChanged;

            btnPause.Click -= BtnPause_Click;
            btnRun.Click -= BtnRun_Click;
            btnOpenAnimator.Click -= btnOpenAnimator_Click;

            flpLanes.Controls.Clear();

            _roomAGrids.Clear();
            _roomBGrids.Clear();

            EntryQueue = null;
            MedicalQueueA = null;
            MedicalQueueB = null;
            AllNurses = null;
            AllDoctors = null;
            AllPatients = null;
            
            dgvLog?.Rows.Clear();
        }
        private void ClearExistingLayout()
        {
            // Clear the UI container
            flpLanes.Controls.Clear();

            // Clear the dictionaries holding room references
            _roomAGrids.Clear();
            _roomBGrids.Clear();

            // Null out specific references to ensure we don't update stale grids
            EntryQueue = null;
            MedicalQueueA = null;
            MedicalQueueB = null;
            AllNurses = null;
            AllDoctors = null;
            AllPatients = null;
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
        private void TrackInterval_ValueChanged(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => TrackInterval_ValueChanged(sender, e));
                return;
            }

            var interval = trackInterval.Value;
            lblIntervalValue.Text = interval.ToString();
            var duration = trackDuration.Value;
            OnChangeSpeed?.Invoke(interval, duration / 10.0);
        }

        private void TrackDuration_ValueChanged(object? sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => TrackDuration_ValueChanged(sender, e));
                return;
            }

            var interval = trackInterval.Value;
            lblIntervalValue.Text = interval.ToString();
            var duration = trackDuration.Value;
            OnChangeSpeed?.Invoke(interval, duration / 10.0);
        }
        private void InitializeLayout(StartSimulationArgs args)
        {
            flpLanes.Controls.Clear();
            // _roomGrids.Clear();

            // 1. Setup Entry Queue (Waiting Room)
            EntryQueue = CreatePatientQueueGrid();
            MedicalQueueA = CreatePatientQueueGrid();
            MedicalQueueA = CreatePatientQueueGrid();
            AllNurses = CreateMedicalStaffListGrid("Nurse ID");
            AllDoctors = CreateMedicalStaffListGrid("Doctor ID");
            AllPatients = CreatePatientQueueGrid();
            // 2. Pre-render Room A Grids
            // Assuming SecurityLanesCount or similar maps to your Room counts
            for (int i = 0; i < 5; i++) 
            {
                var dgv = CreateRoomGridPlaceholder(i, $"Exam Room A #{i}");
                _roomAGrids.Add(i, dgv);
                // flpLanes.Controls.Add(dgv);
            }
            
            // 3. Pre-render Room B Grids (example using another count from args)
            for (int i = 0; i < 7; i++)
            {
                var dgv = CreateRoomGridPlaceholder(i, $"Exam Room B #{i}");
                _roomBGrids.Add(i, dgv);
                // flpLanes.Controls.Add(dgv);
            }
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
                txtCurrentTime.Text = GlobalLogger.FormatTime(time);
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
            
            this.SuspendLayout(); 
    
            try {
                UpdateSimulationTime(state.CurrentTime);
                UpdatePassengersGrid(EntryQueue, state.EntryQueue);
                UpdatePassengersGrid(MedicalQueueA, state.MedicalTreatQueueA);
                UpdatePassengersGrid(MedicalQueueB, state.MedicalTreatQueueB);
                UpdateMedicalStuffsGrid(AllNurses, state.AllNurses.Cast<MedicalStaff>().ToList());
                UpdateMedicalStuffsGrid(AllDoctors, state.AllDoctors.Cast<MedicalStaff>().ToList());
                UpdatePassengersGrid(AllPatients, state.AllPatients);
                
                UpdateGridStats(EntryQueue, state.EntryQueue.Count, state.EntryQueueAvgLength.ToString("F2"));
                UpdateGridStats(MedicalQueueA, state.MedicalTreatQueueA.Count, state.MedicalQueueAvgLengthA.ToString("F2"));
                UpdateGridStats(MedicalQueueB, state.MedicalTreatQueueB.Count, state.MedicalQueueAvgLengthB.ToString("F2"));
        
                // For All Patients (If your state DTO has an average property, replace "-" with it)
                string avgPatients = "-"; 
                // Example: string avgPatients = state.AveragePatients.ToString("F2");
                UpdateGridStats(AllPatients, state.AllPatients.Count, avgPatients);
                
                foreach (var room in state.ARooms)
                {
                    UpdateOrCreateRoomGrid(room, "Room A");
                }

                foreach (var room in state.BRooms)
                {
                    UpdateOrCreateRoomGrid(room, "Room B");
                }
            }
            finally {
                this.ResumeLayout(); 
            }
        }
        
        private void UpdateOrCreateRoomGrid(Room room, string type)
        {
            var dict = (type == "Room A") ? _roomAGrids : _roomBGrids;

            if (dict.TryGetValue(room.Id, out var dgv))
            {
                if (dgv.Rows.Count == 0)
                {
                    dgv.Rows.Add(room.Id, "Empty", "---", "---");
                }
                UpdateGridStats(dgv, room.CurrentStatus == RoomStatus.Free ? 0 : 1, room.GetUtilization().ToString("F2"));
                var row = dgv.Rows[0];
                UpdateCellIfChanged(row.Cells[1], room.Patient?.ToString() ?? "Empty");
                UpdateCellIfChanged(row.Cells[2], room.Nurse?.ToString() ?? "---");
                UpdateCellIfChanged(row.Cells[3], room.Doctor?.ToString() ?? "---");
                UpdateCellIfChanged(row.Cells[4], room.GetUtilization().ToString("F2") ?? "---");
            }
        }

        // Append a log message to the bottom log DataGridView (thread-safe)
        // Keeps history bounded by maxEntries to avoid unlimited growth.
        private const int MaxLogEntries = 2000;
        public void AppendLog(string message, double time)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => AppendLog(message, time));
                return;
            }

            try
            {
                if (dgvLog == null) return;
                
                dgvLog.Rows.Add(GlobalLogger.FormatTime(time), message);

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
            container.Height = 1200;
            container.Padding = new Padding(6);

            var panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.AutoSize = false;
            panel.WrapContents = true;
            panel.AutoScroll = true;

            EntryQueue = CreatePatientQueueGrid();
            MedicalQueueA = CreatePatientQueueGrid();
            MedicalQueueB = CreatePatientQueueGrid();
            AllNurses = CreateMedicalStaffListGrid("Nurse ID");
            AllDoctors = CreateMedicalStaffListGrid("Doctor ID");
            AllPatients = CreatePatientQueueGrid();

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
                
                dgv.Tag = stats;

                p.Controls.Add(lbl);
                p.Controls.Add(stats);
                p.Controls.Add(dgv);
                return p;
            }

            // create containers and keep references so we can extract their stats labels
            var pEntryPatients = MakeLabeledContainer("EntryQueue", EntryQueue);
            var pMedicalPatientsA = MakeLabeledContainer("MedicalQueue A", MedicalQueueA);
            var pMedicalPatientsB = MakeLabeledContainer("MedicalQueue B", MedicalQueueB);
            var pNurses = MakeLabeledContainer("All Nurses", AllNurses);
            var pDoctors = MakeLabeledContainer("All Doctors", AllDoctors);
            var pAllPatients = MakeLabeledContainer("All Patients", AllPatients);


            panel.Controls.Add(pEntryPatients);
            panel.Controls.Add(pMedicalPatientsA);
            panel.Controls.Add(pMedicalPatientsB);
            for (int i = 0; i < 5; i++) 
            {
                var dgv = CreateRoomGridPlaceholder(i, $"Exam Room A #{i}");
                _roomAGrids.Add(i, dgv);
                // flpLanes.Controls.Add(dgv);
            }
            
            // 3. Pre-render Room B Grids (example using another count from args)
            for (int i = 0; i < 7; i++)
            {
                var dgv = CreateRoomGridPlaceholder(i, $"Exam Room B #{i}");
                _roomBGrids.Add(i, dgv);
                // flpLanes.Controls.Add(dgv);
            }
            foreach (var entry in _roomAGrids)
            {
                var pRoomA = MakeLabeledContainer($"Exam Room A #{entry.Key}", entry.Value);
                panel.Controls.Add(pRoomA);
            }
            
            // 3. Add Room B Grids from Dictionary
            foreach (var entry in _roomBGrids)
            {
                var pRoomB = MakeLabeledContainer($"Exam Room B #{entry.Key}", entry.Value);
                panel.Controls.Add(pRoomB);
            }

            panel.Controls.Add(pNurses);
            panel.Controls.Add(pDoctors);
            panel.Controls.Add(pAllPatients);
            
            container.Controls.Add(panel);

            flpLanes.Controls.Add(container);

            // extract stats labels (they are the second control in each panel)
            Label? passengersStats = pEntryPatients.Controls.OfType<Label>().Skip(1).FirstOrDefault();
        }
        
        private void UpdateGridStats(DataGridView? dgv, int currentCount, string average = "-")
        {
            if (dgv?.Tag is Label statsLabel)
            {
                statsLabel.Text = $"Avg: {average}   Cur: {currentCount}";
            }
        }

        private void UpdatePassengersGrid(DataGridView? dgv, List<Patient> patients)
        {
            if (dgv == null || patients == null) return;

            // 1. Synchronize row count (Add or Remove only what is necessary)
            if (dgv.Rows.Count < patients.Count)
            {
                dgv.Rows.Add(patients.Count - dgv.Rows.Count);
            }
            else if (dgv.Rows.Count > patients.Count)
            {
                for (int i = dgv.Rows.Count - 1; i >= patients.Count; i--)
                {
                    dgv.Rows.RemoveAt(i);
                }
            }

            // 2. Update cell values only
            for (int i = 0; i < patients.Count; i++)
            {
                var p = patients[i];
                var row = dgv.Rows[i];

                // Tip: Only update if the value changed to reduce repaints
                UpdateCellIfChanged(row.Cells[0], p.Name);
                UpdateCellIfChanged(row.Cells[1], GlobalLogger.FormatTime(p.ArrivalTime));
                UpdateCellIfChanged(row.Cells[2], p.Priority.ToString());
                UpdateCellIfChanged(row.Cells[3], p.ArrivedByAmbulance.ToString());
                UpdateCellIfChanged(row.Cells[4], p.PatientStatus.ToString());
            }
        }
        
        private void UpdateMedicalStuffsGrid(DataGridView? dgv, List<MedicalStaff> medic)
        {
            if (dgv == null || medic == null) return;

            // 1. Synchronize row count (Add or Remove only what is necessary)
            if (dgv.Rows.Count < medic.Count)
            {
                dgv.Rows.Add(medic.Count - dgv.Rows.Count);
            }
            else if (dgv.Rows.Count > medic.Count)
            {
                for (int i = dgv.Rows.Count - 1; i >= medic.Count; i--)
                {
                    dgv.Rows.RemoveAt(i);
                }
            }

            // 2. Update cell values only
            for (int i = 0; i < medic.Count; i++)
            {
                var p = medic[i];
                var row = dgv.Rows[i];

                // Tip: Only update if the value changed to reduce repaints
                UpdateCellIfChanged(row.Cells[0], p.Id);
                UpdateCellIfChanged(row.Cells[1], p.Activity.ToString());
                UpdateCellIfChanged(row.Cells[2], p.GetWorkingUtilization().ToString("F2"));
            }
        }
        
        
        private void UpdateCellIfChanged(DataGridViewCell cell, object newValue)
        {
            if (cell.Value == null || !cell.Value.Equals(newValue))
            {
                cell.Value = newValue;
            }
        }
        // Controller can call this to read current slider values
        public int GetSleepMs()
        {
            if (this.InvokeRequired)
            {
                return (int)this.Invoke(new Func<int>(() => GetSleepMs()));
            }
            try { return trackInterval?.Value ?? 100; } catch { return 100; }
        }

        private void btnOpenAnimator_Click(object? sender, EventArgs e)
        {
            OnOpenAnimatorRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
