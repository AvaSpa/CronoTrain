using CronoTrain.Views;

namespace CronoTrain;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(HangTimePage), typeof(HangTimePage));
    }
}
