using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ashennes.Views.CastSpellConfirmationPage
{
    public partial class CastSpellConfirmationPageView : ContentPage
    {
        public CastSpellConfirmationPageView(CastSpellConfirmationPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}