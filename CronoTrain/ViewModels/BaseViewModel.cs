using CommunityToolkit.Mvvm.ComponentModel;

namespace CronoTrain.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        public ContentPage View { get; set; }

        public abstract void Initialize();
        public abstract void Terminate();
    }
}
