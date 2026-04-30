using System.ComponentModel;
using System.Windows.Forms;

namespace Airport_GUI;

partial class TurboWindow
{
    private IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();

        // Initialize Plots
        this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        this.formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        this.formsPlot3 = new ScottPlot.WinForms.FormsPlot();
        this.formsPlot4 = new ScottPlot.WinForms.FormsPlot();
        this.formsPlot5 = new ScottPlot.WinForms.FormsPlot();
        this.formsPlot6 = new ScottPlot.WinForms.FormsPlot();

        // Top Panel
        this.topPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(5) };
        this.btnRun = new Button { Text = "Run Simulation", Width = 110, Height = 30, BackColor = System.Drawing.Color.LightGreen };
        this.btnPause = new Button { Text = "Pause", Width = 80, Height = 30 };
        this.btnResume = new Button { Text = "Resume", Width = 80, Height = 30 };
        this.btnStop = new Button { Text = "Stop", Width = 80, Height = 30, BackColor = System.Drawing.Color.MistyRose };
        // Skip percent controls (do not display/save first N% of replications)
        this.lblTurboSkipPercent = new Label { Text = "Skip %:", AutoSize = true, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Padding = new Padding(8, 8, 0, 0) };
        this.txtTurboSkipPercent = new TextBox { Text = "5%", Width = 60, Height = 26, TextAlign = HorizontalAlignment.Right };
        // Status label: shows Running / Paused / Stopped
        this.lblStatus = new Label { Text = "Stopped", AutoSize = false, Width = 160, Height = 30, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Margin = new Padding(12, 6, 0, 0), Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold) };
        this.topPanel.Controls.AddRange(new Control[] { btnRun, btnPause, btnResume, btnStop, lblTurboSkipPercent, txtTurboSkipPercent, lblStatus });

        // Stats Group (Right Side)
        this.statsGroup = new GroupBox { Text = "Global Statistics (Inter-Replication)", Width = 600, Dock = DockStyle.Right, Padding = new Padding(10) };
        this.statsGroup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

        var statsTable = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 9 };
        // Column 0: Descriptive Labels (Fixed width to prevent cutting)
        statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        // Column 1: Mean Value
        statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        // Column 2: Confidence Interval
        statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        for (int i = 0; i < 9; i++) statsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

        // Header Row
        statsTable.Controls.Add(new Label { Text = "Metric", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.BottomLeft }, 0, 0);
        statsTable.Controls.Add(new Label { Text = "Average", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.BottomCenter }, 1, 0);
        statsTable.Controls.Add(new Label { Text = "90% C.I.", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.BottomCenter }, 2, 0);

        // Helper to add rows
        void AddStatRow(int row, string label, out TextBox txtVal, out TextBox txtCi, bool hasCi = true)
        {
            var lbl = new Label { Text = label, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            lbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            statsTable.Controls.Add(lbl, 0, row);

            txtVal = new TextBox { ReadOnly = true, Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Right, BorderStyle = BorderStyle.FixedSingle, Font = new System.Drawing.Font("Consolas", 10F) };
            statsTable.Controls.Add(txtVal, 1, row);

            txtCi = new TextBox { ReadOnly = true, Dock = DockStyle.Fill, TextAlign = HorizontalAlignment.Center, BorderStyle = BorderStyle.FixedSingle, Font = new System.Drawing.Font("Consolas", 9F), ForeColor = System.Drawing.Color.DimGray };
            if (hasCi) statsTable.Controls.Add(txtCi, 2, row);
            else txtCi.Visible = false;
        }

        AddStatRow(1, "Replication Index", out txtTurboReplication, out _, false);
        AddStatRow(2, "Total Passengers", out txtTurboTotal, out txtTurboTotal_CI);
        AddStatRow(3, "Avg Time in System (s)", out txtTurboAvgTime, out txtTurboAvgTime_CI);
        AddStatRow(4, "Avg Entry Queue Length", out txtTurboEntryQueue, out txtTurboEntryQueue_CI);
        AddStatRow(5, "Avg Detector Queue Length", out txtTurboDetectorQueue, out txtTurboDetectorQueue_CI);
        AddStatRow(6, "Avg Before-Lug Detector", out txtTurboBeforeDetector, out txtTurboBeforeDetector_CI);
        AddStatRow(7, "Avg After-Lug Detector", out txtTurboAfterDetector, out txtTurboAfterDetector_CI);
        AddStatRow(8, "Avg Waiting for Luggage", out txtTurboLuggageQueue, out txtTurboLuggageQueue_CI);

        this.statsGroup.Controls.Add(statsTable);

        // Plots Area (Center)
        var plotsTable = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3 };
        plotsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        plotsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        plotsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3F));
        plotsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3F));
        plotsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 33.4F));

        this.formsPlot1.Dock = DockStyle.Fill; this.formsPlot2.Dock = DockStyle.Fill;
        this.formsPlot3.Dock = DockStyle.Fill; this.formsPlot4.Dock = DockStyle.Fill;
        this.formsPlot5.Dock = DockStyle.Fill; this.formsPlot6.Dock = DockStyle.Fill;

        plotsTable.Controls.AddRange(new Control[] { formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6 });
        // Correct 2D positioning
        plotsTable.Controls.Add(formsPlot1, 0, 0); plotsTable.Controls.Add(formsPlot2, 1, 0);
        plotsTable.Controls.Add(formsPlot3, 0, 1); plotsTable.Controls.Add(formsPlot4, 1, 1);
        plotsTable.Controls.Add(formsPlot5, 0, 2); plotsTable.Controls.Add(formsPlot6, 1, 2);

        // Form Assembly
        this.Controls.Add(plotsTable);
        this.Controls.Add(this.statsGroup);
        this.Controls.Add(this.topPanel);

        this.Text = "Simulation Turbo Mode - Real-time Analysis";
        this.ClientSize = new System.Drawing.Size(1400, 850);
    }

    public ScottPlot.WinForms.FormsPlot formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6;
    public FlowLayoutPanel topPanel;
    public Button btnRun, btnPause, btnResume, btnStop;
    // Skip percent control (string like '5%' or '0.05')
    public Label lblTurboSkipPercent;
    public TextBox txtTurboSkipPercent;
    // Status label (shows Running / Paused / Stopped)
    public Label lblStatus;
    public GroupBox statsGroup;
    public TextBox txtTurboReplication, txtTurboTotal, txtTurboAvgTime, txtTurboEntryQueue, txtTurboDetectorQueue, txtTurboLuggageQueue;
    public TextBox txtTurboTotal_CI, txtTurboAvgTime_CI, txtTurboEntryQueue_CI, txtTurboDetectorQueue_CI, txtTurboLuggageQueue_CI;
    public TextBox txtTurboBeforeDetector, txtTurboBeforeDetector_CI, txtTurboAfterDetector, txtTurboAfterDetector_CI;
}