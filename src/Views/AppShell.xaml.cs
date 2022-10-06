namespace Ashennes.Views
{
    public partial class AppShell : Microsoft.Maui.Controls.Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}