using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.CodeGenerators;
using System.Windows.Input;
using Ashennes.Models;
using CommunityToolkit.Mvvm.Input;

namespace Ashennes.Views.SpellListPage
{
    [GenerateViewModel]
    public partial class SpellListPageViewModel
    {
        [GenerateProperty]
        public UserSettings _settings;

        public ICommand CastASpellCommand { get; }

        [ActivatorUtilitiesConstructor]
        public SpellListPageViewModel(UserSettings settings)
        {
            _settings = settings;
            CastASpellCommand = new AsyncRelayCommand(CastASpell);
        }
        protected async Task CastASpell()
        {
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.CastSpellPage);
        }
    }
}
