using CommunityToolkit.Mvvm.ComponentModel;

namespace CronoTrain.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        public abstract void Initialize();
        public abstract void Terminate();
    }
}
