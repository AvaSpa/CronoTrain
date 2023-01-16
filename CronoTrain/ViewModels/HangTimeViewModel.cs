using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CronoTrain.Code.Utils;
using CronoTrain.Models;
using CronoTrain.Views;
using System.Collections.ObjectModel;

namespace CronoTrain.ViewModels
{
    public partial class HangTimeViewModel : BaseViewModel
    {
        private const string IdleButtonText = "Start";
        private const string HangingButtonText = "Break";
        private const string DoneButtonText = "Done";
        private const int TimerInverval = 10;

        //TODO: Tweak colors
        private static readonly Color StartColor = Colors.Transparent;
        private static readonly Color HangingColor = Colors.Green;
        private static readonly Color BreakColor = Colors.GreenYellow;
        private static readonly Color AlertColor = Colors.OrangeRed;
        private readonly IAlertManager _alertManager;
        private bool _isHanging;
        private int _hangTicks;
        private IDispatcherTimer _timer;

        [ObservableProperty]
        private HangTime _runningTime = HangTime.Zero;

        [ObservableProperty]
        private Color _backgroundColor = StartColor;

        [ObservableProperty]
        private string _buttonText = IdleButtonText;

        [ObservableProperty]
        private ObservableCollection<HangTime> _hangTimes;

        [ObservableProperty]
        private int _hangCount = 1;

        [ObservableProperty]
        private bool _buttonIsEnabled = true;

        public HangTimeViewModel(IAlertManager alertManager)
        {
            HangTimes = new ObservableCollection<HangTime>();

            SetupTimer();
            _alertManager = alertManager;
        }

        [RelayCommand]
        private async Task ToggleTimer()
        {
            if (!_timer.IsRunning)
            {
                var page = (HangTimePage)View;
                page.CollapseHangCountInput();
                //TODO: make new event, raise it here and handle it in the view
                // then remove the View property from base

                _timer.Start();
            }

            if (_isHanging)
            {
                HangTimes.Add(RunningTime); //TODO: add all empty times based on set input and just update them here               
            }
            else
            {
                _hangTicks = 0;
                _alertManager.StopBreakEndAlert();
            }

            _isHanging = !_isHanging;

            ButtonText = _isHanging ? HangingButtonText : IdleButtonText;
            BackgroundColor = _isHanging ? HangingColor : BreakColor;

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

                    if (HangTimes.Count >= HangCount) //TODO: check against HangTimes.Count(ht=>ht.Updated) after all in list is implemented
                    {
                        Terminate();
                        Initialize();
                        ButtonIsEnabled = false;
                        ButtonText = DoneButtonText;
                        //TODO: fix this: you can click durring break and that leads to one extra hang time

                        return;
                    }
                    else
                    {
                        BackgroundColor = AlertColor;
                        _alertManager.AlertBreakEnd();
                    }
                }

                RunningTime = new HangTime(TimeSpan.FromMilliseconds(_hangTicks * TimerInverval));
            };
        }

        public override void Terminate()
        {
            _timer.Stop();
            _alertManager.Clear();
            //TODO: save stats (create new session and save HangTimes)
        }

        public override void Initialize()
        {
            _isHanging = false;
            _hangTicks = 0;
            BackgroundColor = StartColor;
            ButtonText = IdleButtonText;
            RunningTime = HangTime.Zero;

            //TODO: load stats (calculate label values based on session and HangTimes)
        }
    }
}
