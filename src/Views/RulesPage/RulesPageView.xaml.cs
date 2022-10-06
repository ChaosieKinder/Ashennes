
namespace Ashennes.Views.RulesPage
{
	public partial class RulesPageView : ContentPage
	{
		public RulesPageView(RulesPageViewModel viewModel)
		{
			InitializeComponent();
            BindingContext = viewModel;
        }
	}
}