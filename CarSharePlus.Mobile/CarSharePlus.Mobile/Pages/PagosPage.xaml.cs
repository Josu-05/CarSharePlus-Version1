namespace CarSharePlus.Mobile;

public partial class PagosPage : ContentPage
{
    public PagosPage(PagosViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // ✅ Conectamos la Page con el ViewModel
    }
}
