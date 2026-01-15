namespace CarSharePlus.Mobile;

public partial class EditarEvaluacionPage : ContentPage
{
    public EditarEvaluacionPage(EditarEvaluacionViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // ✅ Conectamos la Page con el ViewModel
    }
}
