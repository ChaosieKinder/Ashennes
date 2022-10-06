using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ashennes.Views.CastSpellResultsPage
{
    public partial class CastSpellResultsPageView : ContentPage
    {
        public CastSpellResultsPageView(CastSpellResultsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}