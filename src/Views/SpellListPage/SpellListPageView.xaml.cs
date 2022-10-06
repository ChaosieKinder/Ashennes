namespace Ashennes.Views.SpellListPage
{
    public partial class SpellListPageView : ContentPage
    {
        public SpellListPageView(SpellListPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
