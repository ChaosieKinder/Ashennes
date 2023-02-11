using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm.CodeGenerators;

namespace Ashennes.Models
{
    [GenerateViewModel]
    public partial class Spell
    {
        protected UserSettings _settings;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private string _name = string.Empty;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private string _nameDropdownDisplay = string.Empty;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private CastActionType _type = CastActionType.SkillTest;
        [GenerateProperty]
        private int _level = 0;
        [GenerateProperty]
        private int _maxUses = 1;
        [GenerateProperty]
        private int _baseDamage = 3;
        [GenerateProperty]
        private string _text = string.Empty;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private string _formattedText = string.Empty;
        [GenerateProperty]
        private string _damageType = string.Empty;
        [GenerateProperty]
        private bool _isAoe = false;
        [GenerateProperty]
        private int _targetAc = 0;
        [GenerateProperty]
        private List<SpellUse> _uses = new List<SpellUse>();
        protected void OnTextChanged()
        {
            _formattedText = FormatSpellString(
                _baseDamage,
                3,
                _settings.BonusSkillCheckDamage,
                _settings.BonusSpellDamage + (_settings.IsBardBonusEnabled && !_isAoe ? _settings.BonusBardSpellDamage : 0),
                _damageType,
                IsAoe ? -1 : 1);
            RaisePropertyChanged(FormattedTextChangedEventArgs);
        }
        protected void OnNameChanged()
        {
            UpdateNameDropdownText();
        }

        protected void OnUsesChanged()
        {
            UpdateNameDropdownText();
        }

        protected void OnMaxUsesChanged()
        {
            UpdateNameDropdownText();
        }

        public void RefreshText()
        {
            OnTextChanged();
        }
        private void _settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateNameDropdownText();
            RefreshText();
        }

        public void UpdateNameDropdownText()
        {
            int cnt = 0;
            foreach (SpellUse spellUse in _uses)
                cnt += spellUse.IsUsed ? 0 : 1;
            NameDropdownDisplay = $"{_name} ({cnt}/{_maxUses})";
        }


        public string FormatSpellStringExact(int baseDamage, int skillcheckDamage, int skillcheckBonusDamage, int bonusDamage, string damageType, int targets, bool success)
        {
            if (_type == CastActionType.None)
                return _text;

            // figure out targets text first
            string targetText = string.Empty;
            if (_isAoe && bonusDamage == 0)
                targetText = "all monsters";
            else if (_isAoe && bonusDamage > 0)
                targetText = $"all monsters, {bonusDamage} bonus damage split as you choose among targets";
            else if (targets == 1)
                targetText = $"1 target";
            else if (targets > 1)
                targetText = $"{targets} targets";

            // spell text
            switch (_type)
            {
                case CastActionType.AttackNoChallenge:
                    var adamage = baseDamage + (_isAoe ? 0 : bonusDamage);
                    return $"Does {adamage} pts of {damageType} to {targetText}";
                case CastActionType.Attack:
                    if (!success)
                        return "Attack Missed, No damage dealt.";
                    return $"Does {baseDamage + bonusDamage} pts of {damageType} to {targetText}";
                case CastActionType.SkillTest:
                case CastActionType.SkillTestAttack:
                    var bdamage = baseDamage + (success ? skillcheckDamage : 0) + (_isAoe ? 0 : bonusDamage) + (success ? skillcheckBonusDamage : 0);
                    return $"Does {bdamage} pts of {damageType} to {targetText}";
            }

            return "ERROR";
        }

        public string FormatSpellString(int baseDamage, int skillcheckDamage, int skillcheckBonusDamage, int bonusDamage, string damageType, int targets)
        {
            if (_type == CastActionType.None)
                return _text;

            // figure out targets text first
            string targetText = string.Empty;
            if (_isAoe && bonusDamage == 0)
                targetText = "all monsters";
            else if (_isAoe && bonusDamage > 0)
                targetText = $"all monsters, {bonusDamage} bonus damage split as you choose among targets";
            else if (targets == 1)
                targetText = $"1 target";
            else if (targets > 1)
                targetText = $"{targets} targets";

            // spell text
            switch (_type)
            {
                case CastActionType.AttackNoChallenge:
                    var initdamage = baseDamage + (_isAoe ? 0 : bonusDamage);
                    return $"Does {initdamage} pts of {damageType} to {targetText}";
                case CastActionType.Attack:
                    return $"Hit AC {_targetAc} to do {baseDamage + bonusDamage} pts of {damageType} to {targetText}";
                case CastActionType.SkillTest:
                case CastActionType.SkillTestAttack:
                    int minDamage = baseDamage + (_isAoe ? 0 : bonusDamage);
                    int maxDamage = minDamage + skillcheckDamage + skillcheckBonusDamage;
                    return $"(Skill ✔) Does {minDamage} or {maxDamage} pts of {damageType} to {targetText}";
            }

            return "ERROR";
        }


        public Spell(UserSettings settings, string name, CastActionType type, int maxUses = 1, int level = 0)
        {
            _settings = settings;
            _settings.PropertyChanged += _settings_PropertyChanged;
            _type = type;
            _maxUses = maxUses;
            _level = level;
            Name = name;
        }


        public void ResetUses()
        {
            Uses.Clear();
            for(int i=0;i< _maxUses; i++)
            {
                Uses.Add(new SpellUse(i, this));
            }
            UpdateNameDropdownText();
        }

    }

    
}
