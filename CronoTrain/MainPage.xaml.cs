using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace CronoTrain;

public partial class MainPage : ContentPage
{
    private readonly Timer _timer;
    private readonly Stopwatch _stopwatch;

    public MainPage()
    {
        InitializeComponent();

        _timer = new Timer();
        _stopwatch = new Stopwatch();
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        _stopwatch.Start();
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        _stopwatch.Stop();
        Timer.Text = _stopwatch.Elapsed.ToString("mm\\:ss\\.fff");
    }
}

