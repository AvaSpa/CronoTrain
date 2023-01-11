using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CronoTrain.Models;
using System.Collections.ObjectModel;

namespace CronoTrain.ViewModels
{
    public partial class HangTimeViewModel : BaseViewModel
    {
        private const string IdleButtonText = "Start";
        private const string HangingButtonText = "Break";
        private const int TimerInverval = 10;

        private bool _isHanging;
        private int _hangTicks;
        private IDispatcherTimer _timer;

        [ObservableProperty]
        private HangTime _runningTime = HangTime.Zero;

        [ObservableProperty]
        private Color _timeColor = Colors.Transparent;

        [ObservableProperty]
        private string _buttonText = IdleButtonText;

        [ObservableProperty]
        private ObservableCollection<HangTime> _hangTimes;

        public HangTimeViewModel()
        {
            HangTimes = new ObservableCollection<HangTime>();

            SetupTimer();
        }

        [RelayCommand]
        private async Task ToggleTimer()
        {
            if (!_timer.IsRunning)
                _timer.Start();

            if (_isHanging)
                HangTimes.Add(RunningTime);
            else
                _hangTicks = 0;

            _isHanging = !_isHanging;

            ButtonText = _isHanging ? HangingButtonText : IdleButtonText;
            TimeColor = _isHanging ? Colors.Green : Colors.GreenYellow;
            //TODO: Tweak colors

            await Task.CompletedTask;
        }

        private void SetupTimer()
        {
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TimerInverval);

            _timer.Tick += (s, e) =>
            {
                if (_isHanging)
                    _hangTicks++;
                else if (_hangTicks > 0)
                    _hangTicks--;
                else
                {
                    _timer.Stop();
                    //TODO: alert with sound and vibration
                }

                RunningTime = new HangTime(TimeSpan.FromMilliseconds(_hangTicks * TimerInverval));
            };
        }

        public override void Terminate()
        {
            _timer.Stop();
            HangTimes.Clear();

            //TODO: save stats (create new session and save HangTimes)
        }

        public override void Initialize()
        {
            //TODO: load stats (calculate label values based on session and HangTimes)
        }
    }
}
