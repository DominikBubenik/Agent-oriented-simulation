﻿namespace DISS_SEM_GUI
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
        private System.Windows.Forms.TableLayoutPanel tblInputs;

        // Inputs
        private System.Windows.Forms.Label lblSeed = null!;
        private System.Windows.Forms.TextBox txtSeed = null!;
        private System.Windows.Forms.CheckBox chkRandomSeed = null!;
        private System.Windows.Forms.Label lblReplications = null!;
        private System.Windows.Forms.TextBox txtReplications = null!;
        private System.Windows.Forms.Label lblSystemCapacity = null!;
        private System.Windows.Forms.ComboBox cmbExperimentVariant = null!;
        private System.Windows.Forms.CheckBox chkObservationMode = null!;
        private System.Windows.Forms.Label lblEndTime = null!;
        private System.Windows.Forms.TextBox txtEndTime = null!;
        private System.Windows.Forms.Label lblTimeInterval = null!;
        private System.Windows.Forms.TextBox txtTimeInterval = null!;
        private System.Windows.Forms.Label lblMaxWaitTimeExp4 = null!;
        private System.Windows.Forms.TextBox txtMaxWaitTimeExp4 = null!;
        private System.Windows.Forms.Label lblNurses = null!;
        private System.Windows.Forms.TextBox txtNurses = null!;
        private System.Windows.Forms.Label lblDoctors = null!;
        private System.Windows.Forms.TextBox txtDoctors = null!;
        private System.Windows.Forms.Label lblMaxEntryQueue = null!;
        private System.Windows.Forms.TextBox txtMaxEntryQueue = null!;
        private System.Windows.Forms.Label lblMaxMedicalQueue = null!;
        private System.Windows.Forms.TextBox txtMaxMedicalQueue = null!;
        // Sensitivity controls
         private System.Windows.Forms.GroupBox grpSensitivity = null!;
         private System.Windows.Forms.Label lblSensCapacity = null!;
         private System.Windows.Forms.TextBox txtSensCapacity = null!;
         private System.Windows.Forms.Label lblSensReplications = null!;
         private System.Windows.Forms.TextBox txtSensReplications = null!;
         private System.Windows.Forms.Label lblSensGraphPoints = null!;
         private System.Windows.Forms.TextBox txtSensGraphPoints = null!;
         private System.Windows.Forms.CheckBox chkSensRun = null!;

        // Merged Sensitivity + Find/CSV controls
         private System.Windows.Forms.GroupBox grpSensibilityFind = null!;
         private System.Windows.Forms.CheckBox chkFindSensitivityGenerateCsv = null!; // renamed: Generate CSV file
         private System.Windows.Forms.Label lblFindComfortTime = null!;
         private System.Windows.Forms.TextBox txtFindComfortTime = null!;
         private System.Windows.Forms.Label lblFindDetectorQueueAvg = null!;
         private System.Windows.Forms.TextBox txtFindDetectorQueueAvg = null!;
         private System.Windows.Forms.Label lblFindLuggageQueueAvg = null!;
         private System.Windows.Forms.TextBox txtFindLuggageQueueAvg = null!;
         private System.Windows.Forms.Label lblCsvDirectory = null!;
         private System.Windows.Forms.TextBox txtCsvDirectory = null!;
         private System.Windows.Forms.Button btnBrowseCsvDirectory = null!;
         private System.Windows.Forms.Label lblCsvFileName = null!;
         private System.Windows.Forms.TextBox txtCsvFileName = null!;

        // Warming proof group
        private System.Windows.Forms.GroupBox grpWarmingProof = null!;
        private System.Windows.Forms.CheckBox chkWarmUpProof = null!;
        private System.Windows.Forms.Label lblRefreshRate = null!;
        private System.Windows.Forms.TextBox txtRefreshRate = null!;
        // Warm-up input (ms)
        private System.Windows.Forms.Label lblWarmUp = null!;
        private System.Windows.Forms.TextBox txtWarmUp = null!;
        

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

       private void InitializeComponent()
{
    this.pnlControls = new Panel();
    this.btnRun = new Button();
    this.btnPause = new Button();
    this.WindowState = FormWindowState.Maximized;

    // Groups
    GroupBox grpSimulation = new GroupBox();
    GroupBox grpTime = new GroupBox();
    GroupBox grpResources = new GroupBox();

    // Charts group - larger area
    GroupBox grpCharts = new GroupBox();
    grpCharts.Text = "Charts";
    grpCharts.AutoSize = false;
    // Allocate a large area for charts; designer will still respect Dock and FlowLayout
    grpCharts.Width = 2000;
    grpCharts.Height = 1200;
    grpCharts.MinimumSize = new Size(900, 600);

    // Create a scrollable container for chartsTable so user can scroll horizontally/vertically
    Panel chartsPanel = new Panel();
    chartsPanel.Dock = DockStyle.Fill;
    chartsPanel.AutoScroll = true;
    chartsPanel.AutoSize = false;
    chartsPanel.Width = 2000;
    chartsPanel.Height = 1200;

    TableLayoutPanel chartsTable = new TableLayoutPanel();
    chartsTable.ColumnCount = 2;
    chartsTable.RowCount = 3;
    // Do not dock the table to Fill inside the scrollable panel; set explicit size larger than view
    chartsTable.Dock = DockStyle.None;
    chartsTable.AutoSize = false;
    // Make the table wide so horizontal scrolling is triggered on smaller windows
    chartsTable.Width = 1900;
    chartsTable.Height = 1050;
    chartsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
    chartsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
    // Use fixed taller rows so each plot gets more vertical space
    chartsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 350));
    chartsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 350));
    chartsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 350));

    TableLayoutPanel simTable = new TableLayoutPanel();
    TableLayoutPanel timeTable = new TableLayoutPanel();
    TableLayoutPanel resTable = new TableLayoutPanel();
    // Initialize tblInputs (used by AddInput helper) to avoid it being unassigned
    tblInputs = new TableLayoutPanel();

    // INIT CONTROLS
    lblSeed = new Label(); txtSeed = new TextBox();
    lblReplications = new Label(); txtReplications = new TextBox();
    lblSystemCapacity = new Label(); cmbExperimentVariant = new ComboBox();
    chkObservationMode = new CheckBox();
    chkObservationMode.Checked = true;

    lblEndTime = new Label(); txtEndTime = new TextBox();
    lblTimeInterval = new Label(); txtTimeInterval = new TextBox();
    lblMaxWaitTimeExp4 = new Label(); txtMaxWaitTimeExp4 = new TextBox();

    // Initialize resource inputs (were declared earlier)
    lblNurses = new Label(); txtNurses = new TextBox();
    lblDoctors = new Label(); txtDoctors = new TextBox();
    lblMaxEntryQueue = new Label(); txtMaxEntryQueue = new TextBox();
    lblMaxMedicalQueue = new Label(); txtMaxMedicalQueue = new TextBox();

    // Sensibility init
    grpSensitivity = new GroupBox();
    lblSensCapacity = new Label(); txtSensCapacity = new TextBox();
    lblSensReplications = new Label(); txtSensReplications = new TextBox();
    lblSensGraphPoints = new Label(); txtSensGraphPoints = new TextBox();
    chkSensRun = new CheckBox();

    // Merged Sensibility+Find/CSV init
    grpSensibilityFind = new GroupBox();
    chkFindSensitivityGenerateCsv = new CheckBox();
    lblFindComfortTime = new Label(); txtFindComfortTime = new TextBox();
    lblFindDetectorQueueAvg = new Label(); txtFindDetectorQueueAvg = new TextBox();
    lblFindLuggageQueueAvg = new Label(); txtFindLuggageQueueAvg = new TextBox();
    lblCsvDirectory = new Label(); txtCsvDirectory = new TextBox(); btnBrowseCsvDirectory = new Button();
    lblCsvFileName = new Label(); txtCsvFileName = new TextBox();

    // WARMING PROOF init
    grpWarmingProof = new GroupBox();
    chkWarmUpProof = new CheckBox();
    lblRefreshRate = new Label(); txtRefreshRate = new TextBox();
    // Warm-up input (ms)
    lblWarmUp = new Label(); txtWarmUp = new TextBox();
    

    this.SuspendLayout();

    // PANEL
    // Use full available client area for controls including large charts
    pnlControls.Dock = DockStyle.Fill;
    // Allow scrolling when the content (charts) is larger than the window
    pnlControls.AutoScroll = true;
    // do not set fixed height so the panel can expand/shrink with the window
    pnlControls.Padding = new Padding(10);

    // BUTTON RUN
    btnRun.Text = "Run";
    btnRun.Width = 120;
    btnRun.Height = 50;
    btnRun.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    btnRun.Click += RunBtnClick;
    //BUTTON PAUSE
    btnPause.Text = "Pause";
    btnPause.Width = 120;
    btnPause.Height = 50;
    btnPause.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    btnPause.Click += PauseBtnClick;

    // BUTTON SENSITIVITY
    Button btnSensitivity = new Button();
    btnSensitivity.Text = "Sensitivity";
    btnSensitivity.Width = 120;
    btnSensitivity.Height = 50;
    btnSensitivity.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    btnSensitivity.Click += SensitivityBtnClick;
    // BUTTON OBSERVATION
    Button btnObservation = new Button();
    btnObservation.Text = "Observation";
    btnObservation.Width = 120;
    btnObservation.Height = 50;
    btnObservation.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    btnObservation.Click += ObservationBtnClick;

    // BUTTON TURBO
    Button btnTurbo = new Button();
    btnTurbo.Text = "Turbo";
    btnTurbo.Width = 120;
    btnTurbo.Height = 50;
    btnTurbo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    btnTurbo.Click += TurboBtnClick;
    
    // BUTTON WELCH
    Button btnWelch = new Button();
    btnWelch.Text = "Welch";
    btnWelch.Width = 120;
    btnWelch.Height = 50;
    btnWelch.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    btnWelch.Click += WelchBtnClick;

    FlowLayoutPanel leftPanel = new FlowLayoutPanel();
    leftPanel.Dock = DockStyle.Left;
    leftPanel.Width = 140;
    leftPanel.Controls.Add(btnRun);
    leftPanel.Controls.Add(btnPause);
    leftPanel.Controls.Add(btnSensitivity);
    leftPanel.Controls.Add(btnObservation);
    leftPanel.Controls.Add(btnTurbo);
    leftPanel.Controls.Add(btnWelch);
    // Keep a simple checkbox in main view to toggle sensitivity mode
    chkSensRun.Text = "Run Sensitivity";
    chkSensRun.AutoSize = true;
    leftPanel.Controls.Add(chkSensRun);

    // COMMON SETTINGS
    Font font = new Font("Segoe UI", 10);
    this.Font = font;

    void SetupTable(TableLayoutPanel table)
    {
        table.ColumnCount = 2;
        table.RowCount = 5;
        table.AutoSize = true;
        table.Dock = DockStyle.Fill;

        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
    }

    void StyleInput(Label l, TextBox t, string text, string defaultVal)
    {
        // Set displayed text and default value
        l.Text = text;
        t.Text = defaultVal;

        // Make the label a fixed, wider control so the label text can be longer.
        // Use AutoSize = false and explicit width so labels line up consistently.
        l.AutoSize = false;
        l.Width = 260; // increase if you want even longer labels
        l.TextAlign = ContentAlignment.MiddleLeft;

        // Slightly larger inputs for readability
        t.Width = 250;

        // Keep anchors so controls stay left-aligned inside their cell
        l.Anchor = AnchorStyles.Left;
        t.Anchor = AnchorStyles.Left;

        l.Margin = new Padding(5);
        t.Margin = new Padding(5);
    }

    // ================= SIMULATION =================
    grpSimulation.Text = "Simulation";
    grpSimulation.AutoSize = true;

    SetupTable(simTable);
    // Ensure the simulation table has enough rows for all inputs (seed, replications, system capacity, observation)
    simTable.RowCount = 6;

    StyleInput(lblSeed, txtSeed, "Seed", "12345");
    StyleInput(lblReplications, txtReplications, "Replications", "1000");
    lblSystemCapacity.Text = "Experiment Variant";
    lblSystemCapacity.AutoSize = false;
    lblSystemCapacity.Width = 260;
    lblSystemCapacity.TextAlign = ContentAlignment.MiddleLeft;
    lblSystemCapacity.Anchor = AnchorStyles.Left;
    lblSystemCapacity.Margin = new Padding(5);

    cmbExperimentVariant.Width = 250;
    cmbExperimentVariant.Anchor = AnchorStyles.Left;
    cmbExperimentVariant.Margin = new Padding(5);
    cmbExperimentVariant.DropDownStyle = ComboBoxStyle.DropDownList;
    cmbExperimentVariant.Items.Add("Exp 0 First Available");
    cmbExperimentVariant.Items.Add("Exp 1 Least utilized");
    cmbExperimentVariant.Items.Add("1500");
    cmbExperimentVariant.Items.Add("2000");

// Set default selected item
    cmbExperimentVariant.SelectedIndex = 0; 
    
    chkObservationMode.Text = "Observation";

    // Put seed textbox and random checkbox next to each other
    simTable.Controls.Add(lblSeed, 0, 0);
    // inline panel for seed input + random checkbox
    var seedPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
    seedPanel.Controls.Add(txtSeed);
    chkRandomSeed = new CheckBox { Text = "Random", AutoSize = true, CheckAlign = System.Drawing.ContentAlignment.MiddleLeft };
    seedPanel.Controls.Add(chkRandomSeed);
    simTable.Controls.Add(seedPanel, 1, 0);
    simTable.Controls.Add(lblReplications, 0, 1);
    simTable.Controls.Add(txtReplications, 1, 1);
    simTable.Controls.Add(lblSystemCapacity, 0, 2);
    simTable.Controls.Add(cmbExperimentVariant, 1, 2);
    simTable.Controls.Add(chkObservationMode, 1, 3);

    grpSimulation.Controls.Add(simTable);

    // ================= TIME =================
    grpTime.Text = "Time";
    grpTime.AutoSize = true;

    SetupTable(timeTable);

    StyleInput(lblEndTime, txtEndTime, "End Time", "672");
    // Time Interval input accepts HH:MM:SS or numeric seconds. If provided it will set End = Start + Interval
    StyleInput(lblTimeInterval, txtTimeInterval, "Time Interval (HH:MM:SS)", "24:00:00");
    // Current simulation time (read-only) - will be updated from the controller/model
    lblMaxWaitTimeExp4 = new Label(); txtMaxWaitTimeExp4 = new TextBox();
    StyleInput(lblMaxWaitTimeExp4, txtMaxWaitTimeExp4, "Exp 4 max WaitTime (minutes)", "0.5");
    
    timeTable.Controls.Add(lblEndTime, 0, 1);
    timeTable.Controls.Add(txtEndTime, 1, 1);
    timeTable.Controls.Add(lblTimeInterval, 0, 2);
    timeTable.Controls.Add(txtTimeInterval, 1, 2);
    timeTable.Controls.Add(lblMaxWaitTimeExp4, 0, 3);
    timeTable.Controls.Add(txtMaxWaitTimeExp4, 1, 3);


    // Add Warm-up input into the Time group (row 4)
    StyleInput(lblWarmUp, txtWarmUp, "Warm-up (hours)", "168");
    timeTable.Controls.Add(lblWarmUp, 0, 4);
    timeTable.Controls.Add(txtWarmUp, 1, 4);

    grpTime.Controls.Add(timeTable);

    // ================= RESOURCES =================
    grpResources.Text = "Resources";
    grpResources.AutoSize = true;

    SetupTable(resTable);

    StyleInput(lblNurses, txtNurses, "Nurses", "8");
    StyleInput(lblDoctors, txtDoctors, "Doctors", "6");
    StyleInput(lblMaxEntryQueue, txtMaxEntryQueue, "MaxEntryQueue", "3");
    StyleInput(lblMaxMedicalQueue, txtMaxMedicalQueue, "MaxMedicalQueue", "0");

    resTable.Controls.Add(lblNurses, 0, 0);
    resTable.Controls.Add(txtNurses, 1, 0);
    resTable.Controls.Add(lblDoctors, 0, 1);
    resTable.Controls.Add(txtDoctors, 1, 1); 
    resTable.Controls.Add(lblMaxEntryQueue, 0, 2);
    resTable.Controls.Add(txtMaxEntryQueue, 1, 2); 
    resTable.Controls.Add(lblMaxMedicalQueue, 0, 3);
    resTable.Controls.Add(txtMaxMedicalQueue, 1, 3);

    grpResources.Controls.Add(resTable);

    // ================= MERGED SENSIBILITY + CSV =================
    grpSensibilityFind.Text = "System Sensibility & CSV";
    grpSensibilityFind.AutoSize = true;
    TableLayoutPanel sensFindTable = new TableLayoutPanel();
    sensFindTable.ColumnCount = 2; sensFindTable.RowCount = 6; sensFindTable.AutoSize = true; sensFindTable.Dock = DockStyle.Fill;
    sensFindTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
    sensFindTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

    // sensitivity inputs
    StyleInput(lblSensCapacity, txtSensCapacity, "Capacity", "1000");
    StyleInput(lblSensReplications, txtSensReplications, "Replications", "10");
    StyleInput(lblSensGraphPoints, txtSensGraphPoints, "Graph Points", "10");
    chkSensRun.Text = "Run Sensitivity"; chkSensRun.AutoSize = true;

    // find/CSV inputs
    chkFindSensitivityGenerateCsv.Text = "Generate CSV file"; chkFindSensitivityGenerateCsv.AutoSize = true;
    // Comfort security duration: accept HH:MM:SS text (default 00:10:00)
    StyleInput(lblFindComfortTime, txtFindComfortTime, "Comfort security duration (HH:MM:SS)", "00:10:00");
    // Averages
    StyleInput(lblFindDetectorQueueAvg, txtFindDetectorQueueAvg, "Avg detector queue (int)", "20");
    StyleInput(lblFindLuggageQueueAvg, txtFindLuggageQueueAvg, "Avg waiting for luggage (int)", "10");

    StyleInput(lblCsvDirectory, txtCsvDirectory, "CSV Directory", "");
    btnBrowseCsvDirectory = new Button(); btnBrowseCsvDirectory.Text = "Browse..."; btnBrowseCsvDirectory.Width = 100;
    StyleInput(lblCsvFileName, txtCsvFileName, "CSV File name (no ext)", "sensitivity_results");

    sensFindTable.Controls.Add(lblSensCapacity, 0, 0); sensFindTable.Controls.Add(txtSensCapacity, 1, 0);
    sensFindTable.Controls.Add(lblSensReplications, 0, 1); sensFindTable.Controls.Add(txtSensReplications, 1, 1);
    sensFindTable.Controls.Add(lblSensGraphPoints, 0, 2); sensFindTable.Controls.Add(txtSensGraphPoints, 1, 2);
    sensFindTable.Controls.Add(chkSensRun, 1, 3);

    sensFindTable.Controls.Add(chkFindSensitivityGenerateCsv, 0, 4); sensFindTable.SetColumnSpan(chkFindSensitivityGenerateCsv, 2);
    sensFindTable.Controls.Add(lblFindComfortTime, 0, 5); sensFindTable.Controls.Add(txtFindComfortTime, 1, 5);
    sensFindTable.Controls.Add(lblFindDetectorQueueAvg, 0, 6); sensFindTable.Controls.Add(txtFindDetectorQueueAvg, 1, 6);
    sensFindTable.Controls.Add(lblFindLuggageQueueAvg, 0, 7); sensFindTable.Controls.Add(txtFindLuggageQueueAvg, 1, 7);
    sensFindTable.Controls.Add(lblCsvDirectory, 0, 8); sensFindTable.Controls.Add(txtCsvDirectory, 1, 8);
    sensFindTable.Controls.Add(btnBrowseCsvDirectory, 1, 9);
    sensFindTable.Controls.Add(lblCsvFileName, 0, 10); sensFindTable.Controls.Add(txtCsvFileName, 1, 10);

    grpSensibilityFind.Controls.Add(sensFindTable);

    // ================= WARMING PROOF GROUP =================
    grpWarmingProof.Text = "Warming Proof";
    grpWarmingProof.AutoSize = true;
    TableLayoutPanel warmTable = new TableLayoutPanel();
    warmTable.ColumnCount = 2;
    warmTable.RowCount = 2;
    warmTable.AutoSize = true;
    warmTable.Dock = DockStyle.Fill;
    warmTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
    warmTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

    chkWarmUpProof.Text = "Warm up proof";
    chkWarmUpProof.AutoSize = true;
    StyleInput(lblRefreshRate, txtRefreshRate, "Refresh Rate (ms)", "0");

    warmTable.Controls.Add(chkWarmUpProof, 0, 0);
    warmTable.SetColumnSpan(chkWarmUpProof, 2);
    warmTable.Controls.Add(lblRefreshRate, 0, 1);
    warmTable.Controls.Add(txtRefreshRate, 1, 1);
    
    grpWarmingProof.Controls.Add(warmTable);

    // ================= MAIN LAYOUT =================
    FlowLayoutPanel mainLayout = new FlowLayoutPanel();
    mainLayout.Dock = DockStyle.Fill;
    mainLayout.AutoSize = true;
    mainLayout.WrapContents = true;

    // Create a left column that stacks simulation-related groups vertically
    FlowLayoutPanel leftColumn = new FlowLayoutPanel();
    leftColumn.FlowDirection = FlowDirection.TopDown;
    leftColumn.WrapContents = false;
    leftColumn.AutoSize = true;
    leftColumn.Width = 380; // column width for grouped controls
    leftColumn.Controls.Add(grpSimulation);
    // Sensitivity controls moved to SensitivityWindow. Do not add grpSensibilityFind to main view.
    leftColumn.Controls.Add(grpWarmingProof);

    // Add the left column as a single element to the main layout
    mainLayout.Controls.Add(leftColumn);

    // Add the rest of groups to the main layout
    mainLayout.Controls.Add(grpTime);
    mainLayout.Controls.Add(grpResources);

    // Ensure grpStats starts on a new row after resource groups
    mainLayout.SetFlowBreak(grpResources, true);


    pnlControls.Controls.Add(mainLayout);
    pnlControls.Controls.Add(leftPanel);
    
    grpSimulation.Width = 350;
    grpTime.Width = 250;
    grpResources.Width = 350;
    grpWarmingProof.Width = 400;
    // make room for warm-up input
    grpWarmingProof.Height = Math.Max(grpWarmingProof.Height, 160);

     // ================= FORM =================
      // Give the form a larger default client size; user can still maximize
      this.ClientSize = new Size(1400, 1000);
       this.Controls.Add(pnlControls);
       this.Text = "Airport Multi-Lane Simulation";

       this.ResumeLayout(false);
 }
     }
 }
