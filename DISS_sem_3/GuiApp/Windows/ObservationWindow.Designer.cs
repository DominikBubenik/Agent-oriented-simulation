using System.Windows.Forms;
using System.Drawing;
using DISS_sem_3.Entities;

/**
 * Kod vytvoreny s pomocou AI, zdokumentovane v kapitole 8
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 9
 * Kod upraveny s pomocou AI, zdokumentovane v kapitole 22
 */
namespace DISS_sem_3
{
    partial class ObservationWindow
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.FlowLayoutPanel flpLanes;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.TextBox txtCurrentTime;

        // Parameter display controls (readonly)
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.Label lblReplications;
        private System.Windows.Forms.TextBox txtReplications;
        private System.Windows.Forms.Label lblObservation;
        private System.Windows.Forms.TextBox txtObservation;
        private System.Windows.Forms.Label lblStartSimulationTime;
        private System.Windows.Forms.TextBox txtStartSimulationTime;
        private System.Windows.Forms.Label lblEndSimulationTime;
        private System.Windows.Forms.TextBox txtEndSimulationTime;
        private System.Windows.Forms.Label lblSecurityLanes;
        private System.Windows.Forms.TextBox txtSecurityLanes;
        private System.Windows.Forms.Label lblBeforeDetectors;
        private System.Windows.Forms.TextBox txtBeforeDetectors;
        private System.Windows.Forms.Label lblAfterDetectors;
        private System.Windows.Forms.TextBox txtAfterDetectors;
        private System.Windows.Forms.Label lblLambda;
        private System.Windows.Forms.TextBox txtLambda;
        // Logger panel at the bottom
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.DataGridView dgvLog;
        // Sleep control fields (exposed to partial class)
        private System.Windows.Forms.TrackBar trackSleepMs;
        private System.Windows.Forms.Label lblSleepMsValue;
        private System.Windows.Forms.TrackBar trackSleepPeriodSec;
        private System.Windows.Forms.Label lblSleepPeriodValue;
        // Refresh rate control
        private System.Windows.Forms.TrackBar trackRefreshRate;
        private System.Windows.Forms.Label lblRefreshRateValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flpLanes = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.txtCurrentTime = new System.Windows.Forms.TextBox();

            // Parameter top panel
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSeed = new System.Windows.Forms.Label(); this.txtSeed = new System.Windows.Forms.TextBox();
            this.lblReplications = new System.Windows.Forms.Label(); this.txtReplications = new System.Windows.Forms.TextBox();
            this.lblObservation = new System.Windows.Forms.Label(); this.txtObservation = new System.Windows.Forms.TextBox();
            this.lblStartSimulationTime = new System.Windows.Forms.Label(); this.txtStartSimulationTime = new System.Windows.Forms.TextBox();
            this.lblEndSimulationTime = new System.Windows.Forms.Label(); this.txtEndSimulationTime = new System.Windows.Forms.TextBox();
            this.lblSecurityLanes = new System.Windows.Forms.Label(); this.txtSecurityLanes = new System.Windows.Forms.TextBox();
            this.lblBeforeDetectors = new System.Windows.Forms.Label(); this.txtBeforeDetectors = new System.Windows.Forms.TextBox();
            this.lblAfterDetectors = new System.Windows.Forms.Label(); this.txtAfterDetectors = new System.Windows.Forms.TextBox();
            this.lblLambda = new System.Windows.Forms.Label(); this.txtLambda = new System.Windows.Forms.TextBox();

            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 120;
            this.topPanel.AutoSize = false;
            this.topPanel.FlowDirection = FlowDirection.LeftToRight;
            this.topPanel.WrapContents = true;
            this.topPanel.Padding = new Padding(8);

            // Configure parameter labels and readonly textboxes (small sizes)
            void ConfigureParam(Label l, TextBox t, string text)
            {
                l.Text = text;
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleLeft;
                l.Width = 110;
                l.Height = 22;

                t.Width = 80;
                t.Height = 22;
                t.ReadOnly = true;
                t.Margin = new Padding(4, 2, 12, 2);
            }

            ConfigureParam(lblSeed, txtSeed, "Seed:");
            ConfigureParam(lblReplications, txtReplications, "Replications:");
            ConfigureParam(lblObservation, txtObservation, "Observation:");
            ConfigureParam(lblStartSimulationTime, txtStartSimulationTime, "Start Time:");
            ConfigureParam(lblEndSimulationTime, txtEndSimulationTime, "End Time:");
            ConfigureParam(lblSecurityLanes, txtSecurityLanes, "Lanes:");
            ConfigureParam(lblBeforeDetectors, txtBeforeDetectors, "Before Det:");
            ConfigureParam(lblAfterDetectors, txtAfterDetectors, "After Det:");
            ConfigureParam(lblLambda, txtLambda, "Lambda:");

            // Sleep controls: TrackBars and value labels
            this.trackSleepMs = new TrackBar();
            this.trackSleepMs.Orientation = Orientation.Horizontal;
            this.trackSleepMs.Width = 200;
            this.trackSleepMs.TickFrequency = 100;
            this.trackSleepMs.SmallChange = 10;
            this.trackSleepMs.LargeChange = 100;

            Label lblSleepMs = new Label();
            lblSleepMs.Text = "Sleep ms:";
            lblSleepMs.AutoSize = false;
            lblSleepMs.Width = 80;
            lblSleepMs.Height = 22;

            this.lblSleepMsValue = new Label();
            this.lblSleepMsValue.AutoSize = false;
            this.lblSleepMsValue.Width = 60; this.lblSleepMsValue.Height = 22;

            this.trackSleepPeriodSec = new TrackBar();
            this.trackSleepPeriodSec.Orientation = Orientation.Horizontal;
            this.trackSleepPeriodSec.Width = 300; // wider for better precision and larger range
            // Represent slider in centiseconds (0.01s). Value=1 => 0.01s, Value=80 => 0.80s
            this.trackSleepPeriodSec.TickFrequency = 100; // 1.00s ticks
            this.trackSleepPeriodSec.SmallChange = 1; // 0.01s steps
            this.trackSleepPeriodSec.LargeChange = 100; // 1.00s large step

            Label lblSleepPeriod = new Label();
            lblSleepPeriod.Text = "Sleep period (s):";
            lblSleepPeriod.AutoSize = false;
            lblSleepPeriod.Width = 110;
            lblSleepPeriod.Height = 22;

            this.lblSleepPeriodValue = new Label();
            this.lblSleepPeriodValue.AutoSize = false;
            this.lblSleepPeriodValue.Width = 60; this.lblSleepPeriodValue.Height = 22;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);

            //Run button
            this.btnRun.Text = "Run Simulation";
            this.btnRun.Width = 100;
            this.btnRun.Height = 30;
            this.btnRun.Left = 230; // Position it next to your Stop button
            this.btnRun.Top = 8;
            this.btnRun.BackColor = System.Drawing.Color.LightGreen;
            
            // Add parameter controls to top panel
            this.topPanel.Controls.Add(lblSeed); this.topPanel.Controls.Add(txtSeed);
            this.topPanel.Controls.Add(lblReplications); this.topPanel.Controls.Add(txtReplications);
            this.topPanel.Controls.Add(lblObservation); this.topPanel.Controls.Add(txtObservation);
            this.topPanel.Controls.Add(lblStartSimulationTime); this.topPanel.Controls.Add(txtStartSimulationTime);
            this.topPanel.Controls.Add(lblEndSimulationTime); this.topPanel.Controls.Add(txtEndSimulationTime);
            this.topPanel.Controls.Add(lblSecurityLanes); this.topPanel.Controls.Add(txtSecurityLanes);
            this.topPanel.Controls.Add(lblBeforeDetectors); this.topPanel.Controls.Add(txtBeforeDetectors);
            this.topPanel.Controls.Add(lblAfterDetectors); this.topPanel.Controls.Add(txtAfterDetectors);
            this.topPanel.Controls.Add(lblLambda); this.topPanel.Controls.Add(txtLambda);

            // Add sleep controls to top panel (labels + trackbars + value labels)
            this.topPanel.Controls.Add(lblSleepMs); this.topPanel.Controls.Add(this.trackSleepMs); this.topPanel.Controls.Add(this.lblSleepMsValue);
            this.topPanel.Controls.Add(lblSleepPeriod); this.topPanel.Controls.Add(this.trackSleepPeriodSec); this.topPanel.Controls.Add(this.lblSleepPeriodValue);

            // Refresh rate control (0..1000)
            Label lblRefresh = new Label();
            lblRefresh.Text = "Refresh rate:";
            lblRefresh.AutoSize = false;
            lblRefresh.Width = 110;
            lblRefresh.Height = 22;

            this.trackRefreshRate = new TrackBar();
            this.trackRefreshRate.Orientation = Orientation.Horizontal;
            this.trackRefreshRate.Width = 300; // wider to accommodate larger range
            // Bigger range and coarser ticks for higher values
            this.trackRefreshRate.TickFrequency = 500;
            this.trackRefreshRate.SmallChange = 50;
            this.trackRefreshRate.LargeChange = 1000;
            this.trackRefreshRate.Minimum = 0;
            this.trackRefreshRate.Maximum = 500; // expanded maximum
            this.trackRefreshRate.Value = 0; // default to 0 (no forced refresh)

            this.lblRefreshRateValue = new Label();
            this.lblRefreshRateValue.AutoSize = false;
            this.lblRefreshRateValue.Width = 60; this.lblRefreshRateValue.Height = 22;

            this.topPanel.Controls.Add(lblRefresh); this.topPanel.Controls.Add(this.trackRefreshRate); this.topPanel.Controls.Add(this.lblRefreshRateValue);

            // Wire valueChanged events to instance handlers implemented in LaneWindow.cs
            this.trackSleepMs.ValueChanged += new System.EventHandler(this.TrackSleepMs_ValueChanged);
            this.trackSleepPeriodSec.ValueChanged += new System.EventHandler(this.TrackSleepPeriodSec_ValueChanged);
            this.trackRefreshRate.ValueChanged += new System.EventHandler(this.TrackRefreshRate_ValueChanged);

            // Set defaults (min values will be set by controller after creation to model's current settings)
            this.trackSleepMs.Minimum = 100; // initial min as in EventSimulationCore
            this.trackSleepMs.Maximum = 5000;
            this.trackSleepMs.Value = 100; // default sleep ms
            // Track uses centiseconds (0.01s). Range 1..5000 => 0.01s .. 50.00s
            this.trackSleepPeriodSec.Minimum = 1; // 0.01 s
            this.trackSleepPeriodSec.Maximum = 100000; // 50.00 s
            this.trackSleepPeriodSec.Value = 80; // default to 0.80s (80 centiseconds)

            // 
            // btnPause
            // 
            this.btnPause.Text = "Pause";
            this.btnPause.Width = 100;
            this.btnPause.Height = 30;
            this.btnPause.Left = 10;
            this.btnPause.Top = 8;
            this.btnPause.Click += new System.EventHandler(this.BtnPause_Click);

            // Stop button
            var btnStop = new System.Windows.Forms.Button();
            btnStop.Text = "Stop";
            btnStop.Width = 100;
            btnStop.Height = 30;
            btnStop.Left = 120;
            btnStop.Top = 8;
            btnStop.Click += new System.EventHandler(this.BtnStop_Click);

            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Text = "Current Time";
            this.lblCurrentTime.Top = 10;
            this.lblCurrentTime.Left = 130;
            // 
            // txtCurrentTime
            // 
            this.txtCurrentTime.Top = 10;
            this.txtCurrentTime.Left = 230;
            this.txtCurrentTime.Width = 120;
            this.txtCurrentTime.ReadOnly = true;

            // 
            // flpLanes
            // 
            this.flpLanes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpLanes.AutoScroll = true;
            this.flpLanes.Margin = new System.Windows.Forms.Padding(10);

            // Logger panel (bottom)
            this.panelLog = new System.Windows.Forms.Panel();
            this.panelLog.Dock = DockStyle.Bottom;
            this.panelLog.Height = 220;
            this.panelLog.Padding = new Padding(6);

            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.dgvLog.Dock = DockStyle.Fill;
            this.dgvLog.ReadOnly = true;
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Add columns: Time and Message
            var colTime = new DataGridViewTextBoxColumn(); colTime.Name = "colTime"; colTime.HeaderText = "Time"; colTime.Width = 100; colTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            var colMsg = new DataGridViewTextBoxColumn(); colMsg.Name = "colMessage"; colMsg.HeaderText = "Message"; colMsg.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dgvLog.Columns.AddRange(new DataGridViewColumn[] { colTime, colMsg });

            // Add dgv to panel
            this.panelLog.Controls.Add(this.dgvLog);

            // 
            // LaneWindow
            // 
            this.ClientSize = new System.Drawing.Size(1000, 800);

            // add top panel and pause + current time within it
            this.topPanel.Controls.Add(this.btnPause);
            this.topPanel.Controls.Add(this.btnRun);
            this.topPanel.Controls.Add(btnStop);
            this.topPanel.Controls.Add(this.lblCurrentTime);
            this.topPanel.Controls.Add(this.txtCurrentTime);

            // Add controls in order: top panel (top), log panel (bottom), lanes (fill middle)
            this.Controls.Add(this.flpLanes);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.topPanel);

            this.Text = "Lanes";
            this.ResumeLayout(false);
        }

        // Factories similar to MainView.Designer
        public DataGridView CreatePatientQueueGrid()
        {
            var dgvPassengers = new DataGridView();
            dgvPassengers.Width = 700;
            dgvPassengers.Height = 240;
            dgvPassengers.ReadOnly = true;
            dgvPassengers.AllowUserToAddRows = false;
            dgvPassengers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPassengers.Columns.Add("id", "Patient ID");
            dgvPassengers.Columns.Add("arrival", "Arrival Time");
            dgvPassengers.Columns.Add("priority", "Priority");
            dgvPassengers.Columns.Add("Aambulance", "Arrived by Ambulance");
            return dgvPassengers;
        }
        
        private DataGridView CreateRoomGridPlaceholder(int roomId, string title)
        {
            var dgv = new DataGridView();
            dgv.Width = 700;
            dgv.Height = 140;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns.Add("RoomID", "ID");
            dgv.Columns.Add("Patient", "Patient");
            dgv.Columns.Add("Nurse", "Nurse");
            dgv.Columns.Add("Doctor", "Doctor");
            return dgv;
        }
    }
}
