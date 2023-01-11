using CronoTrain.Views;

namespace CronoTrain;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        DeviceDisplay.Current.KeepScreenOn = true;

        Shell.Current.GoToAsync(nameof(HangTimePage));
    }
}

