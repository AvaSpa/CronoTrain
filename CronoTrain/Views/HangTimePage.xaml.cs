using CronoTrain.ViewModels;

namespace CronoTrain.Views;

public partial class HangTimePage : ContentPage
{
    public HangTimePage(HangTimeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        vm.View = this;
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

    public void CollapseHangCountInput()
    {
        Collapse(Stepper);
        Collapse(StepperValue);
    }

    private void Collapse(View control)
    {
        var inputHeight = control.Height;

        var animation = new Animation((value) =>
        {
            control.HeightRequest = value;
        }, inputHeight, 0, Easing.Linear);

        control.Animate("hide", animation);
    }
}