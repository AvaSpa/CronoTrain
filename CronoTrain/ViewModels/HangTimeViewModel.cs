using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CronoTrain.Models;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;

namespace CronoTrain.ViewModels
{
    /// <summary>
    /// TODO: add set count configuration (now when you are done it keeps alerting about break end)
    /// </summary>
    public partial class HangTimeViewModel : BaseViewModel
    {
        private const string IdleButtonText = "Start";
        private const string HangingButtonText = "Break";
        private const int TimerInverval = 10;

        private bool _isHanging;
        private int _hangTicks;
        private IDispatcherTimer _timer;
        private IAudioPlayer _breakEndAudioPlayer;
        private bool _shouldVibrate;

        private readonly IAudioManager _audioManager;
        private readonly IVibration _vibration;

        [ObservableProperty]
        private HangTime _runningTime = HangTime.Zero;

        [ObservableProperty]
        private Color _timeColor = Colors.Transparent;

        [ObservableProperty]
        private string _buttonText = IdleButtonText;

        [ObservableProperty]
        private ObservableCollection<HangTime> _hangTimes;

        public HangTimeViewModel(IAudioManager audioManager, IVibration vibration)
        {
            _audioManager = audioManager;
            _vibration = vibration;

            HangTimes = new ObservableCollection<HangTime>();

            SetupTimer();
            Task.Run(CreateBreakEndAudioPlayer).Wait();
        }

        [RelayCommand]
        private async Task ToggleTimer()
        {
            if (!_timer.IsRunning)
                _timer.Start();

            if (_isHanging)
                HangTimes.Add(RunningTime);
            else
            {
                _hangTicks = 0;
                StopBreakEndAlert();
            }

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
                    AlertBreakEnd();
                }

                RunningTime = new HangTime(TimeSpan.FromMilliseconds(_hangTicks * TimerInverval));
            };
        }

        /// <summary>
        /// TODO: change background color to "go hang"
        /// </summary>
        private void AlertBreakEnd()
        {
            _breakEndAudioPlayer.Play();

            _shouldVibrate = true;
            Task.Run(async () =>
            {
                while (_shouldVibrate)
                {
                    _vibration.Vibrate(750);
                    await Task.Delay(1500);
                }
            });
        }

        private void StopBreakEndAlert()
        {
            _breakEndAudioPlayer.Stop();

            _shouldVibrate = false;
        }

        private async Task CreateBreakEndAudioPlayer()
        {
            if (_breakEndAudioPlayer == null)
            {
                _breakEndAudioPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Sounds/BreakEndBeep.wav"));
                _breakEndAudioPlayer.Volume = 1;
                _breakEndAudioPlayer.Loop = true;
            }
        }

        public override void Terminate()
        {
            _timer.Stop();
            HangTimes.Clear();
            _breakEndAudioPlayer.Stop();
            _breakEndAudioPlayer.Dispose();
            _shouldVibrate = false;
            _vibration.Cancel();

            //TODO: save stats (create new session and save HangTimes)
        }

        public override void Initialize()
        {
            //TODO: load stats (calculate label values based on session and HangTimes)
        }
    }
}
