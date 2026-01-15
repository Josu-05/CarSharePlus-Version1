namespace CarSharePlus.Mobile;

public partial class EvaluacionesPage : ContentPage
{
    public EvaluacionesPage(EvaluacionesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // ✅ Conectamos la Page con el ViewModel
    }
}
