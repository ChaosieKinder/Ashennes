using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.CodeGenerators;
using System.Windows.Input;
using Ashennes.Models;
using CommunityToolkit.Mvvm.Input;


namespace Ashennes.Views.RulesPage
{
    [GenerateViewModel]
    public partial class RulesPageViewModel
    {
        public const string FILENAME_EXPANDED = "caret_down.svg";
        public const string FILENAME_COLLAPSED = "caret_right.svg";

        [GenerateProperty]
        private UserSettings _settings;


        [GenerateProperty]
        private bool _isRulesVisible = true;

        [GenerateProperty]
        private bool _isMecVisible = false;
        [GenerateProperty]
        private bool _isAbsorbVisible = false;
        [GenerateProperty]
        private bool _isAlterVisible = false;
        [GenerateProperty]
        private bool _isAssureVisible = false;
        [GenerateProperty]
        private bool _isBoostVisible = false;
        [GenerateProperty]
        private bool _isConserveVisible = false;
        [GenerateProperty]
        private bool _isForkVisible = false;
        [GenerateProperty]
        private bool _isIntensifyVisible = false;
        [GenerateProperty]
        private bool _isQuickenVisible = false;
        [GenerateProperty]
        private bool _isSharpenVisible = false;

        [GenerateProperty]
        private string _mecIcon;
        [GenerateProperty]
        private string _rulesIcon;
        [GenerateProperty]
        private string _absorbIcon;
        [GenerateProperty]
        private string _alterIcon;
        [GenerateProperty]
        private string _assureIcon;
        [GenerateProperty]
        private string _boostIcon;
        [GenerateProperty]
        private string _conserveIcon;
        [GenerateProperty]
        private string _forkIcon;
        [GenerateProperty]
        private string _intensifyIcon;
        [GenerateProperty]
        private string _quickenIcon;
        [GenerateProperty]
        private string _sharpenIcon;

        public ICommand ToggleMecHeaderCommand { get; }
        public ICommand ToggleRulesHeaderCommand { get; }
        public ICommand ToggleAbsorbHeaderCommand { get; }
        public ICommand ToggleAlterHeaderCommand { get; }
        public ICommand ToggleAssureHeaderCommand { get; }
        public ICommand ToggleBoostHeaderCommand { get; }
        public ICommand ToggleConserveHeaderCommand { get; }
        public ICommand ToggleForkHeaderCommand { get; }
        public ICommand ToggleIntensifyHeaderCommand { get; }
        public ICommand ToggleQuickenHeaderCommand { get; }
        public ICommand ToggleSharpenHeaderCommand { get; }

        [ActivatorUtilitiesConstructor]
        public RulesPageViewModel(UserSettings settings)
        {
            _settings = settings;
            _settings.PropertyChanged += _settings_PropertyChanged;
            ToggleMecHeaderCommand = new AsyncRelayCommand(ToggleMecHeader);
            ToggleRulesHeaderCommand = new AsyncRelayCommand(ToggleRulesHeader);
            ToggleAbsorbHeaderCommand = new AsyncRelayCommand(ToggleAbsorbHeader);
            ToggleAlterHeaderCommand = new AsyncRelayCommand(ToggleAlterHeader);
            ToggleAssureHeaderCommand = new AsyncRelayCommand(ToggleAssureHeader);
            ToggleBoostHeaderCommand = new AsyncRelayCommand(ToggleBoostHeader);
            ToggleConserveHeaderCommand = new AsyncRelayCommand(ToggleConserveHeader);
            ToggleForkHeaderCommand = new AsyncRelayCommand(ToggleForkHeader);
            ToggleIntensifyHeaderCommand = new AsyncRelayCommand(ToggleIntensifyHeader);
            ToggleQuickenHeaderCommand = new AsyncRelayCommand(ToggleQuickenHeader);
            ToggleSharpenHeaderCommand = new AsyncRelayCommand(ToggleSharpenHeader);
            _initializeValues();
        }

        private void _settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _initializeValues();
        }

        private void _initializeValues()
        {
            RulesIcon = _isRulesVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            AbsorbIcon = _isAbsorbVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            AlterIcon = _isAlterVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            AssureIcon = _isAssureVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            BoostIcon = _isBoostVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            ConserveIcon = _isConserveVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            ForkIcon = _isForkVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            IntensifyIcon = _isIntensifyVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            QuickenIcon = _isQuickenVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            SharpenIcon = _isSharpenVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
            MecIcon = _isMecVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }

        private async Task ToggleMecHeader()
        {
            IsMecVisible = !_isMecVisible;
            MecIcon = _isMecVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleRulesHeader()
        {
            IsRulesVisible = !_isRulesVisible;
            RulesIcon = _isRulesVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleAbsorbHeader()
        {
            IsAbsorbVisible = !_isAbsorbVisible;
            AbsorbIcon = _isAbsorbVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleAlterHeader()
        {
            IsAlterVisible = !_isAlterVisible;
            AlterIcon = _isAlterVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleAssureHeader()
        {
            IsAssureVisible = !_isAssureVisible;
            AssureIcon = _isAssureVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleBoostHeader()
        {
            IsBoostVisible = !_isBoostVisible;
            BoostIcon = _isBoostVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleConserveHeader()
        {
            IsConserveVisible = !_isConserveVisible;
            ConserveIcon = _isConserveVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleForkHeader()
        {
            IsForkVisible = !_isForkVisible;
            ForkIcon = _isForkVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleIntensifyHeader()
        {
            IsIntensifyVisible = !_isIntensifyVisible;
            IntensifyIcon = _isIntensifyVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleQuickenHeader()
        {
            IsQuickenVisible = !_isQuickenVisible;
            QuickenIcon = _isQuickenVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
        private async Task ToggleSharpenHeader()
        {
            IsSharpenVisible = !_isSharpenVisible;
            SharpenIcon = _isSharpenVisible ? FILENAME_EXPANDED : FILENAME_COLLAPSED;
        }
    }
}
