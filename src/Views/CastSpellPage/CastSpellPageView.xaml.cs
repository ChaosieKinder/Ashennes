using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ashennes.Views.CastSpellPage
{
    public partial class CastSpellPageView : ContentPage
    {
        public CastSpellPageView(CastSpellPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}