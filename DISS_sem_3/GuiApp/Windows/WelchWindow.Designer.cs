using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DISS_sem_3.GuiApp.Windows;

partial class WelchWindow : Form
{
    private IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new Container();

        // === INIT PLOTS ===
        formsPlot1 = new ScottPlot.WinForms.FormsPlot();
        formsPlot2 = new ScottPlot.WinForms.FormsPlot();
        formsPlot3 = new ScottPlot.WinForms.FormsPlot();
        formsPlot4 = new ScottPlot.WinForms.FormsPlot();
        formsPlot5 = new ScottPlot.WinForms.FormsPlot();
        formsPlot6 = new ScottPlot.WinForms.FormsPlot();
        formsPlot7 = new ScottPlot.WinForms.FormsPlot();
        formsPlot8 = new ScottPlot.WinForms.FormsPlot();
        formsPlot9 = new ScottPlot.WinForms.FormsPlot();
        formsPlot10 = new ScottPlot.WinForms.FormsPlot();
        formsPlot11 = new ScottPlot.WinForms.FormsPlot();
        formsPlot12 = new ScottPlot.WinForms.FormsPlot();
        formsPlot13 = new ScottPlot.WinForms.FormsPlot();
        formsPlot14 = new ScottPlot.WinForms.FormsPlot();
        formsPlot15 = new ScottPlot.WinForms.FormsPlot();
        formsPlot16 = new ScottPlot.WinForms.FormsPlot();
        formsPlot17 = new ScottPlot.WinForms.FormsPlot();
        formsPlot18 = new ScottPlot.WinForms.FormsPlot();
        formsPlot19 = new ScottPlot.WinForms.FormsPlot();
        formsPlot20 = new ScottPlot.WinForms.FormsPlot();
        formsPlot21 = new ScottPlot.WinForms.FormsPlot();
        formsPlot22 = new ScottPlot.WinForms.FormsPlot();
        formsPlot23 = new ScottPlot.WinForms.FormsPlot();
        formsPlot24 = new ScottPlot.WinForms.FormsPlot();
        formsPlot25 = new ScottPlot.WinForms.FormsPlot();
        formsPlot26 = new ScottPlot.WinForms.FormsPlot();
        formsPlot27 = new ScottPlot.WinForms.FormsPlot();
        formsPlot28 = new ScottPlot.WinForms.FormsPlot();
        formsPlot29 = new ScottPlot.WinForms.FormsPlot();
        formsPlot30 = new ScottPlot.WinForms.FormsPlot();

        // === TOP PANEL ===
        topPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 45,
            Padding = new Padding(5)
        };

        btnRun = new Button { Text = "Run", Width = 100 };
        btnPause = new Button { Text = "Pause", Width = 80 };
        btnResume = new Button { Text = "Resume", Width = 80 };
        btnStop = new Button { Text = "Stop", Width = 80 };

        lblStatus = new Label
        {
            Text = "Stopped",
            Width = 120,
            TextAlign = ContentAlignment.MiddleLeft
        };

        topPanel.Controls.AddRange(new Control[]
        {
            btnRun, btnPause, btnResume, btnStop, lblStatus
        });
        
        // === SCROLLABLE PANEL ===
        var scrollPanel = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };

        // === PLOTS TABLE ===
        var table = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 15,
            Dock = DockStyle.Top,   // important for full width
            AutoSize = true
        };

        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

        // fixed height rows so plots stay large
        for (int i = 0; i < 15; i++)
        {
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 500F));
        }

        // dock plots
        var plots = new[]
        {
            formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6, formsPlot7, formsPlot8, formsPlot9, 
            formsPlot10, formsPlot12, formsPlot13, formsPlot14, formsPlot15, formsPlot16, formsPlot17, formsPlot18, formsPlot19, 
            formsPlot20, formsPlot21, formsPlot22, formsPlot23, formsPlot24, formsPlot25, formsPlot26, formsPlot27, formsPlot28,
            formsPlot29, formsPlot30
        };

        foreach (var p in plots)
            p.Dock = DockStyle.Fill;

        // add plots
        table.Controls.Add(formsPlot1, 0, 0);
        table.Controls.Add(formsPlot2, 1, 0);
        table.Controls.Add(formsPlot3, 0, 1);
        table.Controls.Add(formsPlot4, 1, 1);
        table.Controls.Add(formsPlot5, 0, 2);
        table.Controls.Add(formsPlot6, 1, 2);
        table.Controls.Add(formsPlot7, 0, 3);
        table.Controls.Add(formsPlot8, 1, 3);
        table.Controls.Add(formsPlot9, 0, 4);
        table.Controls.Add(formsPlot10, 1, 4); 
        table.Controls.Add(formsPlot11, 0, 5);
        table.Controls.Add(formsPlot12, 1, 5);
        table.Controls.Add(formsPlot13, 0, 6);
        table.Controls.Add(formsPlot14, 1, 6);
        table.Controls.Add(formsPlot15, 0, 7);
        table.Controls.Add(formsPlot16, 1, 7);
        table.Controls.Add(formsPlot17, 0, 8);
        table.Controls.Add(formsPlot18, 1, 8);
        table.Controls.Add(formsPlot19, 0, 9);
        table.Controls.Add(formsPlot20, 1, 9);
        table.Controls.Add(formsPlot21, 0, 10);
        table.Controls.Add(formsPlot22, 1, 10); 
        table.Controls.Add(formsPlot23, 0, 11);
        table.Controls.Add(formsPlot24, 1, 11);
        table.Controls.Add(formsPlot25, 0, 12);
        table.Controls.Add(formsPlot26, 1, 12);
        table.Controls.Add(formsPlot27, 0, 13);
        table.Controls.Add(formsPlot28, 1, 13);
        table.Controls.Add(formsPlot29, 0, 14);
        table.Controls.Add(formsPlot30, 1, 14);

        // add table to scroll panel
        scrollPanel.Controls.Add(table);

        // === FORM ===
        this.Text = "Simulation Turbo Mode - Full Width";
        this.ClientSize = new Size(1400, 850);

        this.Controls.Add(scrollPanel);  // fills remaining space
        this.Controls.Add(statsGroup);   // top
        this.Controls.Add(topPanel);     // top
    }

    // === CONTROLS ===
    public ScottPlot.WinForms.FormsPlot formsPlot1, formsPlot2, formsPlot3, formsPlot4, formsPlot5, formsPlot6, formsPlot7, formsPlot8, formsPlot9, 
        formsPlot10,  formsPlot11, formsPlot12, formsPlot13, formsPlot14, formsPlot15, formsPlot16, formsPlot17, formsPlot18, formsPlot19, 
        formsPlot20, formsPlot21, formsPlot22, formsPlot23, formsPlot24, formsPlot25, formsPlot26, formsPlot27, formsPlot28,
        formsPlot29, formsPlot30;

    public FlowLayoutPanel topPanel;
    public Button btnRun, btnPause, btnResume, btnStop;
    public Label lblStatus;
    public GroupBox statsGroup;
}