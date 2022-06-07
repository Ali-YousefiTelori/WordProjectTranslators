
using Translators.Helpers;
using Translators.UI.Helpers;

namespace Translators.UI.Views.Pages;

public partial class CategoryPage : ContentPage
{
    public CategoryPage()
    {
        InitializeComponent();
        NavigationManager.Initialize(Navigation);
    }
}