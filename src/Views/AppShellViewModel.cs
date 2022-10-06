using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ashennes.Models;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Mvvm.CodeGenerators;

namespace Ashennes.Views
{
    [GenerateViewModel]
    public partial class AppShellViewModel
    {
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private UserSettings _settings;

        public AppShellViewModel(UserSettings settings)
        {
            _settings = settings;
            _settings.UpdateSpellsFromClass(Settings.Level);
        }
        
    }
}
