namespace MauiPlate.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageModel model)
    {
        BindingContext = model;
        InitializeComponent();
    }
}