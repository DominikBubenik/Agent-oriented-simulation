namespace DISS_SEM_GUI
/**
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 7
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 11
 * Kod bol upraveny s pomocou AI, zdokumentovane v kapitole 14
 */

{
    partial class MainView
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.FlowLayoutPanel flowMain;

        // Inputs
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.NumericUpDown numSeed;
        private System.Windows.Forms.CheckBox chkRandomSeed;
        private System.Windows.Forms.Label lblReplications;
        private System.Windows.Forms.NumericUpDown numReplications;
        private System.Windows.Forms.Label lblExperimentVariant;
        private System.Windows.Forms.ComboBox cmbExperimentVariant;
        private System.Windows.Forms.CheckBox chkAllocateBestRoom;
        private System.Windows.Forms.CheckBox chkObservationMode;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.NumericUpDown numEndTime;
        private System.Windows.Forms.Label lblMaxWaitTimeExp4;
        private System.Windows.Forms.NumericUpDown numMaxWaitTimeExp4;
        private System.Windows.Forms.Label lblNurses;
        private System.Windows.Forms.NumericUpDown numNurses;
        private System.Windows.Forms.Label lblDoctors;
        private System.Windows.Forms.NumericUpDown numDoctors;
        private System.Windows.Forms.Label lblMaxEntryQueue;
        private System.Windows.Forms.NumericUpDown numMaxEntryQueue;
        private System.Windows.Forms.Label lblMaxMedicalQueue;
        private System.Windows.Forms.NumericUpDown numMaxMedicalQueue;
        private System.Windows.Forms.Label lblWarmUp;
        private System.Windows.Forms.NumericUpDown numWarmUp;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkWarmUpProof;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.flowMain = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();

            // Action Buttons
            System.Windows.Forms.Button btnObservation = new System.Windows.Forms.Button();
            System.Windows.Forms.Button btnTurbo = new System.Windows.Forms.Button();
            System.Windows.Forms.Button btnWelch = new System.Windows.Forms.Button();
            System.Windows.Forms.Button btnSensitivity = new System.Windows.Forms.Button();

            // Groups
            System.Windows.Forms.GroupBox grpSimulation = new System.Windows.Forms.GroupBox { Text = "Simulation Core", AutoSize = false, Padding = new System.Windows.Forms.Padding(10), Margin = new System.Windows.Forms.Padding(5) };
            System.Windows.Forms.GroupBox grpResources = new System.Windows.Forms.GroupBox { Text = "Staff & Resources", AutoSize = false, Padding = new System.Windows.Forms.Padding(10), Margin = new System.Windows.Forms.Padding(5) };
            System.Windows.Forms.GroupBox grpQueues = new System.Windows.Forms.GroupBox { Text = "Queue Management", AutoSize = false, Padding = new System.Windows.Forms.Padding(10), Margin = new System.Windows.Forms.Padding(5) };
            System.Windows.Forms.GroupBox grpTime = new System.Windows.Forms.GroupBox { Text = "Time Management", AutoSize = false, Padding = new System.Windows.Forms.Padding(10), Margin = new System.Windows.Forms.Padding(5) };
            System.Windows.Forms.GroupBox grpOptions = new System.Windows.Forms.GroupBox { Text = "Operational Options", AutoSize = false, Padding = new System.Windows.Forms.Padding(10), Margin = new System.Windows.Forms.Padding(5) };

            this.SuspendLayout();

            // SIDEBAR
            pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            pnlSidebar.Width = 200;
            pnlSidebar.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            pnlSidebar.Padding = new System.Windows.Forms.Padding(10);

            void StyleSidebarButton(System.Windows.Forms.Button b, string text, System.Drawing.Color backColor, int top)
            {
                b.Text = text;
                b.Width = 180;
                b.Height = 45;
                b.Location = new System.Drawing.Point(10, top);
                b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                b.ForeColor = System.Drawing.Color.White;
                b.BackColor = backColor;
                b.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                b.FlatAppearance.BorderSize = 0;
                pnlSidebar.Controls.Add(b);
            }

            StyleSidebarButton(btnRun, "RUN SIMULATION", System.Drawing.Color.FromArgb(0, 122, 204), 20);
            btnRun.Click += RunBtnClick;

            StyleSidebarButton(btnPause, "PAUSE", System.Drawing.Color.FromArgb(63, 63, 70), 75);
            btnPause.Click += PauseBtnClick;

            StyleSidebarButton(btnObservation, "Observation", System.Drawing.Color.FromArgb(63, 63, 70), 140);
            btnObservation.Click += ObservationBtnClick;

            StyleSidebarButton(btnTurbo, "Turbo Mode", System.Drawing.Color.FromArgb(63, 63, 70), 195);
            btnTurbo.Click += TurboBtnClick;

            StyleSidebarButton(btnWelch, "Welch Analysis", System.Drawing.Color.FromArgb(63, 63, 70), 250);
            btnWelch.Click += WelchBtnClick;

            StyleSidebarButton(btnSensitivity, "Sensitivity", System.Drawing.Color.FromArgb(63, 63, 70), 305);
            btnSensitivity.Click += SensitivityBtnClick;

            lblStatus.ForeColor = System.Drawing.Color.White;
            lblStatus.Location = new System.Drawing.Point(10, 400);
            lblStatus.AutoSize = true;
            lblStatus.Text = "Status: Idle";
            pnlSidebar.Controls.Add(lblStatus);

            progressBar.Width = 180;
            progressBar.Location = new System.Drawing.Point(10, 430);
            pnlSidebar.Controls.Add(progressBar);

            // MAIN CONTENT
            pnlControls.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlControls.AutoScroll = true;
            pnlControls.BackColor = System.Drawing.Color.White;

            flowMain.Dock = System.Windows.Forms.DockStyle.Fill;
            flowMain.Padding = new System.Windows.Forms.Padding(20);
            flowMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            flowMain.WrapContents = true;
            pnlControls.Controls.Add(flowMain);

            void AddToTable(System.Windows.Forms.TableLayoutPanel table, System.Windows.Forms.Control lbl, System.Windows.Forms.Control input)
            {
                int row = table.RowCount++;
                table.Controls.Add(lbl, 0, row);
                table.Controls.Add(input, 1, row);
            }

            System.Windows.Forms.TableLayoutPanel CreateTable()
            {
                var t = new System.Windows.Forms.TableLayoutPanel { ColumnCount = 2, AutoSize = true, Dock = System.Windows.Forms.DockStyle.Top };
                t.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250));
                t.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200));
                return t;
            }

            System.Windows.Forms.Label CreateLabel(string text) => new System.Windows.Forms.Label { Text = text, Anchor = System.Windows.Forms.AnchorStyles.Left, AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            System.Windows.Forms.NumericUpDown CreateNum(int min, int max, int val) => new System.Windows.Forms.NumericUpDown { Minimum = min, Maximum = max, Value = val, Width = 180, Font = new System.Drawing.Font("Segoe UI", 10) };

            // SIMULATION GROUP
            grpSimulation.MinimumSize = new System.Drawing.Size(500, 200);
            var simTable = CreateTable();
            lblSeed = CreateLabel("Random Seed:");
            numSeed = CreateNum(1, int.MaxValue, 12345);
            chkRandomSeed = new System.Windows.Forms.CheckBox { Text = "Use Random", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };

            var seedContainer = new System.Windows.Forms.FlowLayoutPanel { FlowDirection = System.Windows.Forms.FlowDirection.TopDown, AutoSize = true };
            seedContainer.Controls.Add(numSeed);
            seedContainer.Controls.Add(chkRandomSeed);

            lblReplications = CreateLabel("Replications:");
            numReplications = CreateNum(1, 1000000, 1000);

            AddToTable(simTable, lblSeed, seedContainer);
            AddToTable(simTable, lblReplications, numReplications);
            grpSimulation.Controls.Add(simTable);

            // RESOURCES GROUP
            grpResources.MinimumSize = new System.Drawing.Size(500, 200);
            var resTable = CreateTable();
            lblNurses = CreateLabel("Nurse Count:");
            numNurses = CreateNum(1, 100, 8);
            lblDoctors = CreateLabel("Doctor Count:");
            numDoctors = CreateNum(1, 100, 6);

            AddToTable(resTable, lblNurses, numNurses);
            AddToTable(resTable, lblDoctors, numDoctors);
            grpResources.Controls.Add(resTable);

            // QUEUES GROUP
            grpQueues.MinimumSize = new System.Drawing.Size(500, 200);
            var qTable = CreateTable();
            lblMaxEntryQueue = CreateLabel("Max Entry Queue:");
            numMaxEntryQueue = CreateNum(0, 1000, 3);
            lblMaxMedicalQueue = CreateLabel("Max Medical Queue:");
            numMaxMedicalQueue = CreateNum(0, 1000, 0);

            AddToTable(qTable, lblMaxEntryQueue, numMaxEntryQueue);
            AddToTable(qTable, lblMaxMedicalQueue, numMaxMedicalQueue);
            grpQueues.Controls.Add(qTable);

            // TIME GROUP
            grpTime.MinimumSize = new System.Drawing.Size(500, 200);
            var timeTable = CreateTable();
            lblEndTime = CreateLabel("End Time (hours):");
            numEndTime = CreateNum(1, 1000000, 672);
            lblWarmUp = CreateLabel("Warm-up (hours):");
            numWarmUp = CreateNum(0, 1000000, 168);
            lblMaxWaitTimeExp4 = CreateLabel("Exp 4 Wait (min):");
            numMaxWaitTimeExp4 = CreateNum(0, 60, 0);

            AddToTable(timeTable, lblEndTime, numEndTime);
            AddToTable(timeTable, lblWarmUp, numWarmUp);
            AddToTable(timeTable, lblMaxWaitTimeExp4, numMaxWaitTimeExp4);
            grpTime.Controls.Add(timeTable);

            // OPTIONS GROUP
            grpOptions.MinimumSize = new System.Drawing.Size(500, 250);
            var optTable = CreateTable();
            lblExperimentVariant = CreateLabel("Allocation Strategy:");
            cmbExperimentVariant = new System.Windows.Forms.ComboBox { DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList, Width = 180, Font = new System.Drawing.Font("Segoe UI", 10) };
            chkAllocateBestRoom = new System.Windows.Forms.CheckBox { Text = "Allocate Best Room", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            chkObservationMode = new System.Windows.Forms.CheckBox { Text = "Observation Mode", AutoSize = true, Checked = true, Font = new System.Drawing.Font("Segoe UI", 10) };
            chkWarmUpProof = new System.Windows.Forms.CheckBox { Text = "Warm-up Proof Mode", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10) };

            AddToTable(optTable, lblExperimentVariant, cmbExperimentVariant);
            AddToTable(optTable, new System.Windows.Forms.Label(), chkAllocateBestRoom);
            AddToTable(optTable, new System.Windows.Forms.Label(), chkObservationMode);
            AddToTable(optTable, new System.Windows.Forms.Label(), chkWarmUpProof);
            grpOptions.Controls.Add(optTable);

            // ASSEMBLE
            flowMain.Controls.Add(grpSimulation);
            flowMain.Controls.Add(grpResources);
            flowMain.Controls.Add(grpQueues);
            flowMain.Controls.Add(grpTime);
            flowMain.Controls.Add(grpOptions);

            this.Controls.Add(pnlControls);
            this.Controls.Add(pnlSidebar);
            this.Text = "Emergency Department Simulation System";
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.ResumeLayout(false);
        }
    }
}