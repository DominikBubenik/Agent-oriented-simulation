namespace DISS_SEM_GUI
{
    partial class TurboWindow
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            
            // Action Buttons
            this.btnRun = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.numTurboSkipPercent = new System.Windows.Forms.NumericUpDown();
            this.lblSkip = new System.Windows.Forms.Label();
            this.lblReplicationInfo = new System.Windows.Forms.Label();

            // Plots
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot2 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot3 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot4 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot5 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot6 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot7 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot8 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot9 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot10 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot11 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot12 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot13 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot14 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot15 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot16 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot17 = new ScottPlot.WinForms.FormsPlot();
            this.formsPlot18 = new ScottPlot.WinForms.FormsPlot();

            this.SuspendLayout();

            // SIDEBAR
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Width = 220;
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.pnlSidebar.Padding = new System.Windows.Forms.Padding(15);

            void StyleBtn(System.Windows.Forms.Button b, string text, System.Drawing.Color color, int top)
            {
                b.Text = text;
                b.Width = 190;
                b.Height = 40;
                b.Location = new System.Drawing.Point(15, top);
                b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                b.ForeColor = System.Drawing.Color.White;
                b.BackColor = color;
                b.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                b.FlatAppearance.BorderSize = 0;
                this.pnlSidebar.Controls.Add(b);
            }

            StyleBtn(btnRun, "RUN TURBO", System.Drawing.Color.FromArgb(0, 122, 204), 20);
            StyleBtn(btnPause, "PAUSE", System.Drawing.Color.FromArgb(63, 63, 70), 70);
            StyleBtn(btnResume, "RESUME", System.Drawing.Color.FromArgb(63, 63, 70), 120);
            StyleBtn(btnStop, "STOP", System.Drawing.Color.FromArgb(204, 0, 0), 170);

            lblSkip.Text = "Skip Replications %:";
            lblSkip.ForeColor = System.Drawing.Color.White;
            lblSkip.Location = new System.Drawing.Point(15, 230);
            lblSkip.AutoSize = true;
            this.pnlSidebar.Controls.Add(lblSkip);

            numTurboSkipPercent.Value = 5;
            numTurboSkipPercent.Width = 190;
            numTurboSkipPercent.Location = new System.Drawing.Point(15, 255);
            this.pnlSidebar.Controls.Add(numTurboSkipPercent);

            lblStatus.Text = "Status: Idle";
            lblStatus.ForeColor = System.Drawing.Color.White;
            lblStatus.Location = new System.Drawing.Point(15, 300);
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
            this.pnlSidebar.Controls.Add(lblStatus);

            progressBar.Width = 190;
            progressBar.Location = new System.Drawing.Point(15, 330);
            this.pnlSidebar.Controls.Add(progressBar);

            lblReplicationInfo.Text = "Replication: 0 / 0";
            lblReplicationInfo.ForeColor = System.Drawing.Color.White;
            lblReplicationInfo.Location = new System.Drawing.Point(15, 360);
            this.pnlSidebar.Controls.Add(lblReplicationInfo);

            // MAIN CONTENT
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Controls.Add(this.tabControl);

            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10);

            void AddTab(string title, params System.Windows.Forms.Control[] controls)
            {
                var page = new System.Windows.Forms.TabPage { Text = title, BackColor = System.Drawing.Color.White };
                var table = new System.Windows.Forms.TableLayoutPanel { Dock = System.Windows.Forms.DockStyle.Fill, ColumnCount = 2, RowCount = (controls.Length + 1) / 2, AutoScroll = true };
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50));
                for(int i=0; i<table.RowCount; i++) table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 400));
                
                for(int i=0; i<controls.Length; i++)
                {
                    controls[i].Dock = System.Windows.Forms.DockStyle.Fill;
                    table.Controls.Add(controls[i], i % 2, i / 2);
                }
                page.Controls.Add(table);
                this.tabControl.TabPages.Add(page);
            }

            AddTab("Patients", formsPlot1, formsPlot2, formsPlot3);
            AddTab("Service Times", formsPlot4, formsPlot5, formsPlot6, formsPlot17, formsPlot18);
            AddTab("Wait Times & Queues", formsPlot7, formsPlot8, formsPlot9, formsPlot10, formsPlot11, formsPlot12);
            AddTab("Resources", formsPlot13, formsPlot14, formsPlot15, formsPlot16);

            // EXPORT & STATS TAB (Replacement for statsGroup)
            var statsPage = new System.Windows.Forms.TabPage { Text = "Global Statistics", BackColor = System.Drawing.Color.White };
            this.statsPanel = new System.Windows.Forms.Panel { Dock = System.Windows.Forms.DockStyle.Fill, AutoScroll = true, Padding = new System.Windows.Forms.Padding(10) };
            
            // Re-adding the stats table logic here
            this.statsTable = new System.Windows.Forms.TableLayoutPanel { Dock = System.Windows.Forms.DockStyle.Top, AutoSize = true, ColumnCount = 3 };
            this.statsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300));
            this.statsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150));
            this.statsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200));
            
            // Header
            void AddHeader(string t1, string t2, string t3) {
                this.statsTable.Controls.Add(new System.Windows.Forms.Label { Text = t1, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold) });
                this.statsTable.Controls.Add(new System.Windows.Forms.Label { Text = t2, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold) });
                this.statsTable.Controls.Add(new System.Windows.Forms.Label { Text = t3, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold) });
            }
            AddHeader("Metric", "Average", "95% Confidence Interval");

            void AddRow(string label, out System.Windows.Forms.TextBox val, out System.Windows.Forms.TextBox ci) {
                this.statsTable.Controls.Add(new System.Windows.Forms.Label { Text = label, AutoSize = true });
                val = new System.Windows.Forms.TextBox { ReadOnly = true, Width = 140 };
                this.statsTable.Controls.Add(val);
                ci = new System.Windows.Forms.TextBox { ReadOnly = true, Width = 180 };
                this.statsTable.Controls.Add(ci);
            }

            AddRow("Total Patients", out txtTurboTotal, out txtTurboTotal_CI);
            AddRow("Walk-In Count", out txtTurboWalkIn, out txtTurboWalkIn_CI);
            AddRow("Ambulance Count", out txtTurboAmbulance, out txtTurboAmbulance_CI);
            AddRow("Avg Time in System (All)", out txtTurboAvgTime, out txtTurboAvgTime_CI);
            AddRow("Avg Time in System (Walk-In)", out txtTurboTimeWalkIn, out txtTurboTimeWalkIn_CI);
            AddRow("Avg Time in System (Ambulance)", out txtTurboTimeAmbulance, out txtTurboTimeAmbulance_CI);
            AddRow("Entry Wait (Walk-In)", out txtTurboEntryWaitTimeWalkIn, out txtTurboEntryWaitTimeWalkIn_CI);
            AddRow("Entry Wait (Ambulance)", out txtTurboEntryWaitAmbulance, out txtTurboEntryWaitTimeAmbulance_CI);
            AddRow("Avg Entry Queue Length", out txtTurboEntryQueueLength, out txtTurboEntryQueueLength_CI);
            AddRow("Medical Wait Type A", out txtTurboMedicalTrWaitingTimeA, out txtTurboMedicalTrWaitingTimeA_CI);
            AddRow("Medical Wait Type AB", out txtTurboMedicalTrWaitingTimeAB, out txtTurboMedicalTrWaitingTimeAB_CI);
            AddRow("Medical Wait Type B", out txtTurboMedicalTrWaitingTimeB, out txtTurboMedicalTrWaitingTimeB_CI);
            AddRow("Doctors Utilization", out txtTurboAllDoctorsUtil, out txtTurboAllDoctorsUtil_CI);
            AddRow("Nurses Utilization", out txtTurboAllNursesUtil, out txtTurboAllNursesUtil_CI);
            AddRow("Room A Utilization", out txtTurboAllRoomAUtil, out txtTurboAllRoomAUtil_CI);
            AddRow("Room B Utilization", out txtTurboAllRoomBUtil, out txtTurboAllRoomBUtil_CI);
            AddRow("Time Entry to Treatment (Walk-In)", out txtTurboTimeFromEntryToMedicalWalkIn, out txtTurboTimeFromEntryToMedicalWalkIn_CI);
            AddRow("Time Entry to Treatment (Ambulance)", out txtTurboTimeFromEntryToMedicalAmbulance, out txtTurboTimeFromEntryToMedicalAmbulance_CI);
            AddRow("Time Entry to Treatment (Priority 1)", out txtTurboTimeFromEntryToMedicalPriority1, out txtTurboTimeFromEntryToMedicalPriority1_CI);
            AddRow("Time Entry to Treatment (Priority 2)", out txtTurboTimeFromEntryToMedicalPriority2, out txtTurboTimeFromEntryToMedicalPriority2_CI);
            AddRow("Time Entry to Treatment (Priority 3)", out txtTurboTimeFromEntryToMedicalPriority3, out txtTurboTimeFromEntryToMedicalPriority3_CI);
            AddRow("Time Entry to Treatment (Priority 4)", out txtTurboTimeFromEntryToMedicalPriority4, out txtTurboTimeFromEntryToMedicalPriority4_CI);
            AddRow("Time Entry to Treatment (Priority 5)", out txtTurboTimeFromEntryToMedicalPriority5, out txtTurboTimeFromEntryToMedicalPriority5_CI);

            this.statsPanel.Controls.Add(this.statsTable);
            statsPage.Controls.Add(this.statsPanel);
            this.tabControl.TabPages.Add(statsPage);

            // EXPORT UI in a separate panel at bottom of StatsPage or a new tab
            var exportPage = new System.Windows.Forms.TabPage { Text = "Export Settings", BackColor = System.Drawing.Color.White };
            var exportTable = new System.Windows.Forms.TableLayoutPanel { Dock = System.Windows.Forms.DockStyle.Top, AutoSize = true, ColumnCount = 3, Padding = new System.Windows.Forms.Padding(20) };
            exportTable.Controls.Add(new System.Windows.Forms.Label { Text = "Export Directory:", AutoSize = true });
            this.txtExportPath = new System.Windows.Forms.TextBox { Width = 400, ReadOnly = true };
            exportTable.Controls.Add(this.txtExportPath);
            this.btnBrowsePath = new System.Windows.Forms.Button { Text = "Browse...", Width = 100 };
            exportTable.Controls.Add(this.btnBrowsePath);
            
            exportTable.Controls.Add(new System.Windows.Forms.Label { Text = "File Name:", AutoSize = true });
            this.txtExportFilename = new System.Windows.Forms.TextBox { Width = 400, Text = "emergency_dept_results.csv" };
            exportTable.Controls.Add(this.txtExportFilename);

            exportPage.Controls.Add(exportTable);
            this.tabControl.TabPages.Add(exportPage);

            // Form
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Text = "Emergency Department - Turbo Analysis Dashboard";
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.ResumeLayout(false);
        }

        public ScottPlot.WinForms.FormsPlot formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6, formsPlot7, formsPlot8, formsPlot9;
        public ScottPlot.WinForms.FormsPlot formsPlot10, formsPlot11, formsPlot12, formsPlot13, formsPlot14, formsPlot15, formsPlot16, formsPlot17, formsPlot18;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TabControl tabControl;
        public System.Windows.Forms.Button btnRun, btnPause, btnResume, btnStop;
        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.NumericUpDown numTurboSkipPercent;
        private System.Windows.Forms.Label lblSkip;
        public System.Windows.Forms.Label lblReplicationInfo;
        
        private System.Windows.Forms.Panel statsPanel;
        private System.Windows.Forms.TableLayoutPanel statsTable;
        public System.Windows.Forms.TextBox txtTurboTotal, txtTurboWalkIn, txtTurboAmbulance, txtTurboAvgTime;
        public System.Windows.Forms.TextBox txtTurboTotal_CI, txtTurboWalkIn_CI, txtTurboAmbulance_CI, txtTurboAvgTime_CI;
        public System.Windows.Forms.TextBox txtTurboTimeWalkIn, txtTurboTimeWalkIn_CI, txtTurboTimeAmbulance, txtTurboTimeAmbulance_CI;
        public System.Windows.Forms.TextBox txtTurboEntryWaitTimeWalkIn, txtTurboEntryWaitTimeWalkIn_CI, txtTurboEntryWaitAmbulance, txtTurboEntryWaitTimeAmbulance_CI;
        public System.Windows.Forms.TextBox txtTurboEntryQueueLength, txtTurboEntryQueueLength_CI, txtTurboMedicalTrWaitingTimeA, txtTurboMedicalTrWaitingTimeA_CI;
        public System.Windows.Forms.TextBox txtTurboMedicalTrWaitingTimeAB, txtTurboMedicalTrWaitingTimeAB_CI, txtTurboMedicalTrWaitingTimeB, txtTurboMedicalTrWaitingTimeB_CI;
        public System.Windows.Forms.TextBox txtTurboAllDoctorsUtil, txtTurboAllDoctorsUtil_CI, txtTurboAllNursesUtil, txtTurboAllNursesUtil_CI;
        public System.Windows.Forms.TextBox txtTurboAllRoomAUtil, txtTurboAllRoomAUtil_CI, txtTurboAllRoomBUtil, txtTurboAllRoomBUtil_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalWalkIn, txtTurboTimeFromEntryToMedicalWalkIn_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalAmbulance, txtTurboTimeFromEntryToMedicalAmbulance_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalPriority1, txtTurboTimeFromEntryToMedicalPriority1_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalPriority2, txtTurboTimeFromEntryToMedicalPriority2_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalPriority3, txtTurboTimeFromEntryToMedicalPriority3_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalPriority4, txtTurboTimeFromEntryToMedicalPriority4_CI;
        public System.Windows.Forms.TextBox txtTurboTimeFromEntryToMedicalPriority5, txtTurboTimeFromEntryToMedicalPriority5_CI;
        
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.TextBox txtExportFilename;
        private System.Windows.Forms.Button btnBrowsePath;
    }
}