namespace CarSharePlus.Mobile;

public partial class MapasPage : ContentPage
{
    private readonly MapasViewModel _vm;

    public MapasPage(MapasViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CargarMapaAsync(map);
    }
}
