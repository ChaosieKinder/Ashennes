using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.CodeGenerators;
using System.Windows.Input;
using Ashennes.Models;
using CommunityToolkit.Mvvm.Input;

namespace Ashennes.Views.SettingsPage
{
    [GenerateViewModel]
    public partial class SettingsPageViewModel
    {
        [GenerateProperty]
        public UserSettings _settings;

        [GenerateProperty]
        private int[] _levelOptions = new int[] { 4, 5 };

        public ICommand StartRunCommand { get; }
        public ICommand ResetSettingsCommand { get; }

        [ActivatorUtilitiesConstructor]
        public SettingsPageViewModel(UserSettings settings)
        {
            _settings = settings;
            StartRunCommand = new AsyncRelayCommand(StartRun);
            ResetSettingsCommand = new AsyncRelayCommand(ResetSettings);
        }
        protected async Task StartRun()
        {
            _settings.UpdateSpellsFromClass(Settings.Level);
            _settings.CanViewSpellList = true;
            _settings.CanEditInitialSettings = false;
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.MainPage_SpellList);

        }
        protected async Task ResetSettings()
        {
            _settings.ResetRunState();
        }
    }
}
