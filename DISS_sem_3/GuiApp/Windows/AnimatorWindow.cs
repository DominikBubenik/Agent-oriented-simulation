using System.Windows.Forms.Integration;
using OSPAnimator;

namespace DISS_sem_3.GuiApp.Windows;

public partial class AnimatorWindow : Form
{
    private ElementHost _host;
    public AnimatorWindow()
    {
        InitializeComponent();
        
        _host = new ElementHost();
        _host.Dock = DockStyle.Fill;
        this.Controls.Add(_host);
    }
    
    public void SetAnimator(Animator animator)
    {
        // The Animator class has a property 'MyCanvas' which is the WPF control
        // We 'host' that WPF control inside the WinForms bridge
        _host.Child = animator.MyCanvas;
    }
}