using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.CodeGenerators;
using System.Windows.Input;
using Ashennes.Models;
using CommunityToolkit.Mvvm.Input;

namespace Ashennes.Views.CastSpellConfirmationPage
{

    [GenerateViewModel]
    public partial class CastSpellConfirmationPageViewModel
    {
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private UserSettings _settings;
        [GenerateProperty]
        private SpellCast _selectedSpell;
        [GenerateProperty]
        private bool _isSpellSelected = false;


        protected void OnSelectedSpellChanged()
        {
            IsSpellSelected = _selectedSpell is not null;
        }

        public ICommand RemoveSpellCommand { get; }
        public ICommand EditSpellCommand { get; }
        public ICommand GotoResultsPageCommand { get; }
        public ICommand CastAnoutherSpellCommand { get; }
        public CastSpellConfirmationPageViewModel(UserSettings settings)
        {
            _settings = settings;
            _settings.CurrentTurn.Spells.CollectionChanged += Spells_CollectionChanged;
            RemoveSpellCommand = new AsyncRelayCommand(RemoveSpell);
            EditSpellCommand = new AsyncRelayCommand(EditSpell);
            GotoResultsPageCommand = new AsyncRelayCommand(GotoResultsPage, CanGotoResultsPage);
            CastAnoutherSpellCommand = new AsyncRelayCommand(CastAnoutherSpell);
        }

        private void Spells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            (GotoResultsPageCommand as AsyncRelayCommand).NotifyCanExecuteChanged();
        }

        protected async Task RemoveSpell()
        {
            if (SelectedSpell is null)
                return; // sanity check
            var success = _settings.CurrentTurn.Spells.Remove(SelectedSpell);
            SelectedSpell = null;
        }
        protected async Task EditSpell()
        {
            var currentSpell = SelectedSpell;
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.CastSpellPage, new Dictionary<string, object>()
            {
                {"edit" ,currentSpell}
            });
        }

        protected async Task GotoResultsPage()
        {
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.CastSpellResults);
        }

        protected bool CanGotoResultsPage()
        {
            return _settings.CurrentTurn.Spells.Count > 0;
        }

        protected async Task CastAnoutherSpell()
        {
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.CastSpellPage);
        }
    }
}
