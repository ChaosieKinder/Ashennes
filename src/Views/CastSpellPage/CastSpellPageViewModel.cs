using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm.CodeGenerators;
using System.Windows.Input;
using Ashennes.Models;
using CommunityToolkit.Mvvm.Input;
using ColorHelper = Ashennes.Helpers.ColorHelper;

namespace Ashennes.Views.CastSpellPage
{
    [GenerateViewModel]
    public partial class CastSpellPageViewModel : IQueryAttributable
    {
        private static readonly string[] AlterEnergyTypesStatic = new string[] { "Alter", "Alter: Fire", "Alter: Shock", "Alter: Cold" };
        

        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private SpellCast _spell;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private UserSettings _settings;
        [GenerateProperty]
        private Models.Spell _selectedSpell = null;
        [GenerateProperty]
        private string _alterSelectedEnergyType = "Alter";
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private bool _editMode = false;
        [GenerateProperty]
        private bool _mecSelected = false;
        [GenerateProperty]
        private bool _canMec = false;
        [GenerateProperty]
        private Color _mecForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _mecBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private bool _canAlter = false;
        [GenerateProperty]
        private Brush _alterBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _alterForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _assureBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _assureForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private bool _canBoost = false;
        [GenerateProperty]
        private Brush _boostBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _boostForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _conserveBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _conserveForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private bool _canFork = false;
        [GenerateProperty]
        private Brush _forkBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _forkForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _intensifyBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _intensifyForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _quickenBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _quickenForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private bool _canSharpen = false;
        [GenerateProperty]
        private Brush _sharpenBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
        [GenerateProperty]
        private Color _sharpenForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private string _saveText = "Cast Spell";
        public string[] AlterEnergyTypes => AlterEnergyTypesStatic;

        [ActivatorUtilitiesConstructor]
        public CastSpellPageViewModel(UserSettings settings)
        {
            _settings = settings;
            _spell = new SpellCast(settings);
            ToggleMECCommand = new AsyncRelayCommand(ToggleMEC);
            ToggleAssureCommand = new AsyncRelayCommand(ToggleAssure, CanToggleAssure);
            ToggleBoostCommand = new AsyncRelayCommand(ToggleBoost, CanToggleBoost);
            ToggleConserveCommand = new AsyncRelayCommand(ToggleConserve, CanToggleConserve);
            ToggleForkCommand = new AsyncRelayCommand(ToggleFork, CanToggleFork);
            ToggleIntensifyCommand = new AsyncRelayCommand(ToggleIntensify, CanToggleIntensify);
            ToggleQuickenCommand = new AsyncRelayCommand(ToggleQuicken, CanToggleQuicken);
            ToggleSharpenCommand = new AsyncRelayCommand(ToggleSharpen, CanToggleSharpen);
            CastASpellCommand = new AsyncRelayCommand(CastASpell);
        }

        protected void OnSpellChanged()
        {
            OnSelectedSpellChanged();
        }

        protected void OnEditModeChanged()
        {
            SaveText = EditMode ? "Back to Spell List" : "Cast Spell";
        }

        protected async Task CastASpell()
        {
            // Set the selected spell def to the spellcast
            _spell.SpellDefinition = _selectedSpell;
            if(!EditMode) // skip adding if we are editing
               _settings.CurrentTurn.Spells.Add(_spell);
            try
            {
                Reset();
            }
            catch(Exception e)
            {

            }
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(Constants.Nav.MainPage_SpellQueue);
            
        }

        private void Reset()
        {
            Spell = new SpellCast(_settings);
            SelectedSpell = null;
            AlterSelectedEnergyType = "Alter";
            MecSelected = false;
            CanMec = false;
            CanFork = false;
            CanSharpen = false;
            CanAlter = false;
            CanBoost = false;
            EditMode = false;
            OnPowersSelectionChanged();
        }


        protected void OnSelectedSpellChanged()
        {
            if(SelectedSpell is null)
            {
                CanMec = false;
                CanAlter = false;
                CanBoost = false;
                CanFork = false;
                CanSharpen = false;
                MecSelected = false;
                _spell.SpellDefinition = null;
            }
            else
            {
                CanMec = (SelectedSpell is not null && SelectedSpell.Type != CastActionType.None);
                CanAlter = SelectedSpell is not null && Settings.IsAshenne && (SelectedSpell.Type != CastActionType.None);
                CanBoost = SelectedSpell is not null && (_spell.BoostSelected || (Settings.IsAshenne && SelectedSpell.Type != CastActionType.None && !SelectedSpell.IsAoe && _spell.PowerUsedCount < 1));
                CanFork = SelectedSpell is not null && SelectedSpell.Type != CastActionType.None && !SelectedSpell.IsAoe;
                CanSharpen = SelectedSpell is not null && (SelectedSpell.Type == CastActionType.Attack);
                MecSelected = SelectedSpell is not null && _canMec ? _mecSelected : false;
                _spell.SpellDefinition = SelectedSpell;
            }
            _spell.AlterSelected = _canAlter ? _spell.AlterSelected : false;
            _spell.BoostSelected = _canBoost ? _spell.BoostSelected : false;
            _spell.ForkSelected = _canFork ? _spell.ForkSelected : false;
            _spell.SharpenSelected = _canSharpen ? _spell.SharpenSelected : false;
        }

        protected void OnMecSelectedChanged()
        {
            _spell.MecSelected = _mecSelected;
            MecBackgroundBrush = _mecSelected ? ColorHelper.SelectedBackgroundColor : ColorHelper.DeselectedBackgroundColor;
            MecForegroundColor = _mecSelected ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor;
        }

        protected void OnPowersSelectionChanged()
        {
            // Update background brushes for each button
            AlterBackgroundBrush = _spell.AlterSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleAlter()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            AssureBackgroundBrush = _spell.AssureSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleAssure()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            BoostBackgroundBrush = _spell.BoostSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleBoost()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            ConserveBackgroundBrush = _spell.ConserveSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleConserve()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            ForkBackgroundBrush = _spell.ForkSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleFork()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            IntensifyBackgroundBrush = _spell.IntensifySelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleIntensify()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            QuickenBackgroundBrush = _spell.QuickenSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleQuicken()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            SharpenBackgroundBrush = _spell.SharpenSelected
                ? ColorHelper.SelectedBackgroundColor
                : CanToggleSharpen()
                    ? ColorHelper.DeselectedBackgroundColor
                    : ColorHelper.DisabledBackgroundColor;

            // Correct foreground brushes based on background brushes.
            AlterForegroundColor = (AlterBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            AssureForegroundColor = (AssureBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            BoostForegroundColor = (BoostBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            ConserveForegroundColor = (ConserveBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            ForkForegroundColor = (ForkBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            IntensifyForegroundColor = (IntensifyBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            QuickenForegroundColor = (QuickenBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);
            SharpenForegroundColor = (SharpenBackgroundBrush == ColorHelper.SelectedBackgroundColor ? ColorHelper.LightForegroundColor : ColorHelper.DarkForegroundColor);

        }


        #region ToggleButton Commands
        private async Task ToggleMEC()
        {
            MecSelected = !_mecSelected;
        }
        protected void OnAlterSelectedEnergyTypeChanged()
        {
            _spell.AlterDamageType = _alterSelectedEnergyType;
            OnPowersSelectionChanged();
        }

        private bool CanToggleAlter()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.AlterSelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }

        private async Task ToggleAssure()
        {
            _spell.AssureSelected = !_spell.AssureSelected;
            OnPowersSelectionChanged();
        }
        private bool CanToggleAssure()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.AssureSelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }


        private async Task ToggleBoost()
        {
            _spell.BoostSelected = !_spell.BoostSelected;
            OnPowersSelectionChanged();
        }

        private bool CanToggleBoost()
        {
            if (_spell.BoostSelected)
                return true;
            return _spell.PowerUsedCount < 1;
        }

        private async Task ToggleConserve()
        {
            _spell.ConserveSelected = !_spell.ConserveSelected;
            OnPowersSelectionChanged();
        }
        private bool CanToggleConserve()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.ConserveSelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }

        private async Task ToggleFork()
        {
            _spell.ForkSelected = !_spell.ForkSelected;
            OnPowersSelectionChanged();
        }
        private bool CanToggleFork()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.ForkSelected && !_spell.BoostSelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }

        private async Task ToggleIntensify()
        {
            _spell.IntensifySelected = !_spell.IntensifySelected;
            OnPowersSelectionChanged();
        }

        private bool CanToggleIntensify()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.IntensifySelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }
        private async Task ToggleQuicken()
        {
            _spell.QuickenSelected = !_spell.QuickenSelected;
            OnPowersSelectionChanged();
        }

        private bool CanToggleQuicken()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.QuickenSelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }
        private async Task ToggleSharpen()
        {
            _spell.SharpenSelected = !_spell.SharpenSelected;
            OnPowersSelectionChanged();
        }
        private bool CanToggleSharpen()
        {
            if (_spell.BoostSelected)
                return false;
            if (_spell.SharpenSelected)
                return true;
            return _spell.PowerUsedCount < (_settings.IsAshenne ? 6 : 4);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.ContainsKey("edit"))
            {
                EditMode = true;
                Spell = query["edit"] as SpellCast;
                MecSelected = _spell.MecSelected;
                OnPowersSelectionChanged();
            }
        }

        public ICommand ToggleMECCommand { get; }
        public ICommand ToggleAssureCommand { get; }
        public ICommand ToggleBoostCommand { get; }
        public ICommand ToggleConserveCommand { get; }
        public ICommand ToggleForkCommand { get; }
        public ICommand ToggleIntensifyCommand { get; }
        public ICommand ToggleQuickenCommand { get; }
        public ICommand ToggleSharpenCommand { get; }
        public ICommand CastASpellCommand { get; }

        #endregion

    }
}
