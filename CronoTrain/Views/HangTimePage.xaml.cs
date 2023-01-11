using CronoTrain.ViewModels;

namespace CronoTrain.Views;

public partial class HangTimePage : ContentPage
{
    public HangTimePage(HangTimeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);

        var vm = BindingContext as HangTimeViewModel;
        vm.Terminate();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        var vm = BindingContext as HangTimeViewModel;
        vm.Initialize();
    }
}