using MauiPlate.Models;
using MauiPlate.PageModels;

namespace MauiPlate.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}