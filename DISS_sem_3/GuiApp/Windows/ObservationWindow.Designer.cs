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
        private System.Windows.Forms.Button btnOpenAnimator;
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
        private System.Windows.Forms.Label lblEndSimulationTime;
        private System.Windows.Forms.TextBox txtEndSimulationTime;
        private System.Windows.Forms.Label lblNurses;
        private System.Windows.Forms.TextBox txtNurses;
        private System.Windows.Forms.Label lblDoctors;
        private System.Windows.Forms.TextBox txtDoctors;
        
        // Logger panel at the bottom
        private System.Windows.Forms.Panel panelLog;
        private System.Windows.Forms.DataGridView dgvLog;
        // Sleep control fields (exposed to partial class)
        private System.Windows.Forms.TrackBar trackInterval;
        private System.Windows.Forms.Label lblIntervalValue;
        private System.Windows.Forms.TrackBar trackDuration;
        private System.Windows.Forms.Label lblDurationValue;

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
            this.btnOpenAnimator = new System.Windows.Forms.Button();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.txtCurrentTime = new System.Windows.Forms.TextBox();

            // Parameter top panel
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSeed = new System.Windows.Forms.Label(); this.txtSeed = new System.Windows.Forms.TextBox();
            this.lblReplications = new System.Windows.Forms.Label(); this.txtReplications = new System.Windows.Forms.TextBox();
            this.lblObservation = new System.Windows.Forms.Label(); this.txtObservation = new System.Windows.Forms.TextBox();
            this.lblEndSimulationTime = new System.Windows.Forms.Label(); this.txtEndSimulationTime = new System.Windows.Forms.TextBox();
            this.lblNurses = new System.Windows.Forms.Label(); this.txtNurses = new System.Windows.Forms.TextBox();
            this.lblDoctors = new System.Windows.Forms.Label(); this.txtDoctors = new System.Windows.Forms.TextBox();

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
            ConfigureParam(lblEndSimulationTime, txtEndSimulationTime, "End Time:");
            ConfigureParam(lblNurses, txtNurses, "Nurses:");
            ConfigureParam(lblDoctors, txtDoctors, "Doctors");

            // Sleep controls: TrackBars and value labels
            this.trackInterval = new TrackBar();
            this.trackInterval.Orientation = Orientation.Horizontal;
            this.trackInterval.Width = 200;
            this.trackInterval.TickFrequency = 1;
            this.trackInterval.SmallChange = 1;
            this.trackInterval.LargeChange = 2;

            Label lblSleepMs = new Label();
            lblSleepMs.Text = "Sleep ms:";
            lblSleepMs.AutoSize = false;
            lblSleepMs.Width = 80;
            lblSleepMs.Height = 22;

            this.lblIntervalValue = new Label();
            this.lblIntervalValue.AutoSize = false;
            this.lblIntervalValue.Width = 60; this.lblIntervalValue.Height = 22;

            this.trackDuration = new TrackBar();
            this.trackDuration.Orientation = Orientation.Horizontal;
            this.trackDuration.Width = 300; // wider for better precision and larger range
            // Represent slider in centiseconds (0.01s). Value=1 => 0.01s, Value=80 => 0.80s
            this.trackDuration.TickFrequency = 100; // 1.00s ticks
            this.trackDuration.SmallChange = 1; // 0.01s steps
            this.trackDuration.LargeChange = 100; // 1.00s large step

            Label lblSleepPeriod = new Label();
            lblSleepPeriod.Text = "Sleep period (s):";
            lblSleepPeriod.AutoSize = false;
            lblSleepPeriod.Width = 110;
            lblSleepPeriod.Height = 22;

            this.lblDurationValue = new Label();
            this.lblDurationValue.AutoSize = false;
            this.lblDurationValue.Width = 60; this.lblDurationValue.Height = 22;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            this.btnOpenAnimator.Click += new System.EventHandler(this.btnOpenAnimator_Click);

            //Run button
            this.btnRun.Text = "Run Simulation";
            this.btnRun.Width = 100;
            this.btnRun.Height = 30;
            this.btnRun.Left = 230; // Position it next to your Stop button
            this.btnRun.Top = 8;
            this.btnRun.BackColor = System.Drawing.Color.LightGreen;
            
            //OpenAnimator button
            this.btnOpenAnimator.Text = "Animator";
            this.btnOpenAnimator.Width = 100;
            this.btnOpenAnimator.Height = 30;
            this.btnOpenAnimator.Left = 400; // Position it next to your Stop button
            this.btnOpenAnimator.Top = 8;
            this.btnOpenAnimator.BackColor = System.Drawing.Color.Yellow;
            
            // Add parameter controls to top panel
            this.topPanel.Controls.Add(lblSeed); this.topPanel.Controls.Add(txtSeed);
            this.topPanel.Controls.Add(lblReplications); this.topPanel.Controls.Add(txtReplications);
            this.topPanel.Controls.Add(lblObservation); this.topPanel.Controls.Add(txtObservation);
            this.topPanel.Controls.Add(lblEndSimulationTime); this.topPanel.Controls.Add(txtEndSimulationTime);
            this.topPanel.Controls.Add(lblNurses); this.topPanel.Controls.Add(txtNurses);
            this.topPanel.Controls.Add(lblDoctors); this.topPanel.Controls.Add(txtDoctors);

            // Add sleep controls to top panel (labels + trackbars + value labels)
            this.topPanel.Controls.Add(lblSleepMs); this.topPanel.Controls.Add(this.trackInterval); this.topPanel.Controls.Add(this.lblIntervalValue);
            this.topPanel.Controls.Add(lblSleepPeriod); this.topPanel.Controls.Add(this.trackDuration); this.topPanel.Controls.Add(this.lblDurationValue);
            



            // Wire valueChanged events to instance handlers implemented in LaneWindow.cs
            this.trackInterval.ValueChanged += new System.EventHandler(this.TrackInterval_ValueChanged);
            this.trackDuration.ValueChanged += new System.EventHandler(this.TrackDuration_ValueChanged);

            // Set defaults (min values will be set by controller after creation to model's current settings)
            this.trackInterval.Minimum = 1; // initial min as in EventSimulationCore
            this.trackInterval.Maximum = 1000;
            this.trackInterval.Value = 1; // default sleep ms
        
            this.trackDuration.Minimum = 1; 
            this.trackDuration.Maximum = 10; 
            this.trackDuration.Value = 1; 

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
            this.txtCurrentTime.Width = 180;
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
            this.topPanel.Controls.Add(this.btnOpenAnimator);
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
            dgvPassengers.Columns.Add("Aambulance", "Ambulanced");
            dgvPassengers.Columns.Add("Status", "Status");
            dgvPassengers.Columns["id"].FillWeight = 40;
            dgvPassengers.Columns["arrival"].FillWeight = 20;
            dgvPassengers.Columns["priority"].FillWeight = 10;
            dgvPassengers.Columns["Aambulance"].FillWeight = 10;
            dgvPassengers.Columns["Status"].FillWeight = 30;
            return dgvPassengers;
        }
        
        public DataGridView CreateMedicalStaffListGrid(string header)
        {
            var dgvPassengers = new DataGridView();
            dgvPassengers.Width = 500;
            dgvPassengers.Height = 240;
            dgvPassengers.ReadOnly = true;
            dgvPassengers.AllowUserToAddRows = false;
            dgvPassengers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPassengers.Columns.Add("id", header);
            dgvPassengers.Columns.Add("Activity", "Current Activity");
            dgvPassengers.Columns.Add("Util", "Utilization");
            return dgvPassengers;
        }
        
        private DataGridView CreateRoomGridPlaceholder(int roomId, string title)
        {
            var dgv = new DataGridView();
            dgv.Width = 700;
            dgv.Height = 90;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Columns.Add("RoomID", "ID");
            dgv.Columns.Add("Patient", "Patient");
            dgv.Columns.Add("Nurse", "Nurse");
            dgv.Columns.Add("Doctor", "Doctor");
            dgv.Columns.Add("Util", "Utilization");
            return dgv;
        }
    }
}
