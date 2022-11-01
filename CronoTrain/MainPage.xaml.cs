using Timer = System.Timers.Timer;

namespace CronoTrain;

public partial class MainPage : ContentPage
{
    private const int TickInterval = 1000;

    private CancellationTokenSource _cancellationTokenSource;
    private double _duration;
    private bool _trigger;
    private bool _countUp;

    public MainPage()
    {
        InitializeComponent();

        _cancellationTokenSource = new CancellationTokenSource();
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        _countUp = !_countUp;

        if (!_trigger)
        {
            _trigger = true;
            await Count();
        }
    }

    /// <summary>
    /// TODO: add delay before switching to up; put in a list all the up times
    /// </summary>
    /// <returns></returns>
    private async Task Count()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            if (_countUp) _duration++;
            else _duration--;

            var milliseconds = _duration * TickInterval;
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            Timer.Text = timeSpan.ToString("mm\\:ss\\.f");

            await Task.Delay(TickInterval);
        }
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        ResetToken();
        Timer.Text = string.Empty;
        _trigger = false;
        _countUp = true;
        _duration = 0;
    }

    private async void ResetToken()
    {
        _cancellationTokenSource.Cancel();
        await Task.Delay(750);
        _cancellationTokenSource = new CancellationTokenSource();
    }
}

