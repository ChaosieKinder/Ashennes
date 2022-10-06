namespace Ashennes.Views.SettingsPage
{
    public partial class SettingsPageView : ContentPage
    {
        public SettingsPageView(SettingsPage.SettingsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}