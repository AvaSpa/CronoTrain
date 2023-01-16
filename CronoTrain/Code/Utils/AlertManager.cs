using Plugin.Maui.Audio;

namespace CronoTrain.Code.Utils
{
    public class AlertManager : IAlertManager
    {
        private IAudioPlayer _breakEndAudioPlayer;
        private bool _shouldVibrate;

        private readonly IAudioManager _audioManager;
        private readonly IVibration _vibration;

        public AlertManager(IAudioManager audioManager, IVibration vibration)
        {
            _audioManager = audioManager;
            _vibration = vibration;

            Task.Run(CreateBreakEndAudioPlayer).Wait();
        }

        public void AlertBreakEnd()
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

        public void Clear()
        {
            _breakEndAudioPlayer.Stop();
            _breakEndAudioPlayer.Dispose();
            _shouldVibrate = false;
            _vibration.Cancel();
        }

        public void StopBreakEndAlert()
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
    }
}
