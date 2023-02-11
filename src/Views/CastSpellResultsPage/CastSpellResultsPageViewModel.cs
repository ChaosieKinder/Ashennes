using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Mvvm.CodeGenerators;
using System.Windows.Input;
using Ashennes.Models;

namespace Ashennes.Views.CastSpellResultsPage
{
    [GenerateViewModel]
    public partial class CastSpellResultsPageViewModel
    {
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        UserSettings _settings;

        public ICommand CompelteTurnCommand { get; }

        public CastSpellResultsPageViewModel(UserSettings settings)
        {
            _settings = settings;
            CompelteTurnCommand = new AsyncRelayCommand(CompleteTurn);
        }
        protected async Task CompleteTurn()
        {
            _settings.CurrentTurn.Spells.Clear();
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.MainPage_SpellList);
        }
    }
}
