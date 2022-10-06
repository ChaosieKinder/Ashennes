using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Mvvm.CodeGenerators;
using ColorHelper = Ashennes.Helpers.ColorHelper;


namespace Ashennes.Models
{
    [GenerateViewModel]
    public partial class SpellCast
    {
        [GenerateProperty]
        private Spell _spellDefinition;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private string _spellText = "";
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private string _spellExactText = "";
        [GenerateProperty]
        private bool _mecSelected = false;
        [GenerateProperty]
        private bool _alterSelected = false;
        [GenerateProperty]
        private string _alterDamageType = "";
        [GenerateProperty]
        private bool _isSpellSuccessful = true;
        [GenerateProperty]
        private bool _isSpellFailable = true;
        [GenerateProperty]
        private string _spellStatusSucceedText = "Skill Test Pass";
        [GenerateProperty]
        private string _spellStatusFailText = "Skill Test Fail";
        [GenerateProperty]
        private bool _assureSelected = false;
        [GenerateProperty]
        private bool _boostSelected = false;
        [GenerateProperty]
        private bool _conserveSelected = false;
        [GenerateProperty]
        private bool _forkSelected = false;
        [GenerateProperty]
        private bool _intensifySelected = false;
        [GenerateProperty]
        private bool _quickenSelected = false;
        [GenerateProperty]
        private bool _sharpenSelected = false;
        [GenerateProperty]
        private int _hpCost = 0;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private int _powerUsedCount = 0;
        [GenerateProperty]
        private Color _successBtnForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _successBtnBackgroundBrush = ColorHelper.SelectedBackgroundColor;
        [GenerateProperty]
        private Color _failBtnForegroundColor = ColorHelper.DarkForegroundColor;
        [GenerateProperty]
        private Brush _failBtnBackgroundBrush = ColorHelper.DeselectedBackgroundColor;

        private StringBuilder spellTextBuilder = new StringBuilder();
        private StringBuilder spellTextExactBuilder = new StringBuilder();

        private UserSettings _settings;
        public ICommand MarkSpellPassCommand { get; }
        public ICommand MarkSpellFailCommand { get; }

        public SpellCast(UserSettings settings)
        {
            _settings = settings;
            _settings.PropertyChanged += _settings_PropertyChanged;
            MarkSpellPassCommand = new AsyncRelayCommand(MarkSpellPass);
            MarkSpellFailCommand = new AsyncRelayCommand(MarkSpellFail);
        }

        private void _settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _updateSpellText();
        }

        public async Task MarkSpellPass()
        {
            IsSpellSuccessful = true;
            SuccessBtnBackgroundBrush = ColorHelper.SelectedBackgroundColor;
            SuccessBtnForegroundColor = ColorHelper.LightForegroundColor;
            FailBtnBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
            FailBtnForegroundColor = ColorHelper.DarkForegroundColor;
            _updateSpellText();
        }

        public async Task MarkSpellFail()
        {
            IsSpellSuccessful = false;
            SuccessBtnBackgroundBrush = ColorHelper.DeselectedBackgroundColor;
            SuccessBtnForegroundColor = ColorHelper.DarkForegroundColor;
            FailBtnBackgroundBrush = ColorHelper.SelectedBackgroundColor;
            FailBtnForegroundColor = ColorHelper.LightForegroundColor;
            _updateSpellText();
        }

        protected void _updateSpellText()
        {
            spellTextBuilder.Clear();
            spellTextExactBuilder.Clear();
            if (_spellDefinition is not null)
            {
                string energyType = _alterSelected
                    ? _alterDamageType.Replace("Alter: ", "")
                    : _spellDefinition.DamageType;

                bool useSkillTestDamage = _spellDefinition.Type == CastActionType.SkillTest ||
                                        _spellDefinition.Type == CastActionType.SkillTestAttack;

                var modifiedSkillCheckDamage = useSkillTestDamage
                    ? (3 + _settings.BonusSkillCheckDamage) * (MecSelected && !_spellDefinition.IsAoe ? 2 : 1)
                    : 0;

                var modifiedBaseDamage = _spellDefinition.IsAoe
                    ? _spellDefinition.BaseDamage
                    : (_spellDefinition.BaseDamage * (MecSelected ? 2 : 1));


                var modifiedBonusDamage = _settings.BonusSpellDamage
                    + (_settings.IsBardBonusEnabled ? _settings.BonusBardSpellDamage : 0)
                    + (_spellDefinition.IsAoe && MecSelected
                        ? _spellDefinition.BaseDamage
                        : 0)
                    + (_spellDefinition.IsAoe && MecSelected && useSkillTestDamage && IsSpellSuccessful
                        ? 3 + _settings.BonusSkillCheckDamage
                        : 0);

                spellTextBuilder.AppendLine(_spellDefinition.FormatSpellString(
                    modifiedBaseDamage,
                    modifiedSkillCheckDamage,
                    modifiedBonusDamage,
                    energyType,
                    ForkSelected ? 2 : 1));
                spellTextExactBuilder.AppendLine(_spellDefinition.FormatSpellStringExact(
                    modifiedBaseDamage,
                    modifiedSkillCheckDamage,
                    modifiedBonusDamage,
                    energyType,
                    ForkSelected ? 2 : 1,
                    IsSpellSuccessful
                    ));
                if (MecSelected)
                {
                    spellTextBuilder.AppendLine("This spell is evoked (MEC).");
                    spellTextExactBuilder.AppendLine("This spell was evoked (MEC).");
                }
                
                if (SharpenSelected)
                {
                    spellTextBuilder.AppendLine("This spell crits on 18-20.");
                    if (IsSpellSuccessful)
                        spellTextExactBuilder.AppendLine("This spell crits on 18-20.");
                }
                if (AssureSelected)
                {
                    spellTextBuilder.AppendLine("This spell's success is Assured");
                    if(IsSpellSuccessful)
                        spellTextExactBuilder.AppendLine("This spell's success is Assured");
                }
                if (BoostSelected)
                {
                    spellTextBuilder.AppendLine("This spell's damage will be used to boost an ally's attack.");
                    spellTextExactBuilder.AppendLine("This spell's damage will be used to boost an ally's attack.");
                }
                if (ConserveSelected)
                {
                    spellTextBuilder.AppendLine("This spell will not be checked off your character card");
                    spellTextExactBuilder.AppendLine("This spell will not be checked off your character card");
                }
                if (IntensifySelected)
                {
                    spellTextBuilder.AppendLine("This spell is effected by a maximum of 25% spell resistance.");
                    if (IsSpellSuccessful)
                        spellTextExactBuilder.AppendLine("This spell is effected by a maximum of 25% spell resistance.");
                }
                if (QuickenSelected)
                {
                    if (_powerUsedCount == 1)
                    {
                        spellTextBuilder.AppendLine("This spell is being cast as an Instant Action.");
                        spellTextExactBuilder.AppendLine("This spell is being cast as an Instant Action.");
                    }
                    else
                    {
                        spellTextBuilder.AppendLine("This spell is being cast as a Free Action.");
                        spellTextExactBuilder.AppendLine("This spell is being cast as a Free Action.");
                    }
                }

            }
            SpellText = spellTextBuilder.ToString();
            SpellExactText = spellTextExactBuilder.ToString();

        }

        protected void OnSpellDefinitionChanged()
        {
            _updateSpellText();
            if (_spellDefinition is null)
                return;
            switch(_spellDefinition.Type)
            {
                case CastActionType.Attack:
                    IsSpellFailable = true;
                    SpellStatusFailText = "Attack Miss";
                    SpellStatusSucceedText = "Attack Hit";
                    break;
                case CastActionType.SkillTest:
                case CastActionType.SkillTestAttack:
                    IsSpellFailable = true;
                    SpellStatusFailText = "Skill Test Fail";
                    SpellStatusSucceedText = "Skill Test Pass";
                    break;
                default:
                    IsSpellFailable = false;
                    break;
            }
        }
        protected void OnMecSelectedChanged()
        {
            HpCost += _mecSelected ? 25 : -25;
            _updateSpellText();
        }
        protected void OnAlterSelectedChanged()
        {
            HpCost += _alterSelected ? 5 : -5;
            PowerUsedCount += _alterSelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnAlterDamageTypeChanged()
        {
            if (AlterSelected
                && AlterDamageType == "Alter")
            {
                AlterSelected = false;
            }
            else if(!AlterSelected
                && AlterDamageType != "Alter")
            {
                AlterSelected = true;
            }
            _updateSpellText();
        }
        protected void OnAssureSelectedChanged()
        {
            HpCost += _assureSelected ? 5 : -5;
            PowerUsedCount += _assureSelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnBoostSelectedChanged()
        {
            HpCost += _boostSelected ? 5 : -5;
            PowerUsedCount += _boostSelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnConserveSelectedChanged()
        {
            HpCost += _conserveSelected ? 5 : -5;
            PowerUsedCount += _conserveSelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnForkSelectedChanged()
        {
            HpCost += _forkSelected ? 5 : -5;
            PowerUsedCount += _forkSelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnIntensifySelectedChanged()
        {
            HpCost += _intensifySelected ? 5 : -5;
            PowerUsedCount += _intensifySelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnQuickenSelectedChanged()
        {
            HpCost += _quickenSelected ? 5 : -5;
            PowerUsedCount += _quickenSelected ? 1 : -1;
            _updateSpellText();
        }
        protected void OnSharpenSelectedChanged()
        {
            HpCost += _sharpenSelected ? 5 : -5;
            PowerUsedCount += _sharpenSelected ? 1 : -1;
            _updateSpellText();
        }
    }
}
