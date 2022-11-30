using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm.CodeGenerators;

namespace Ashennes.Models
{
    [GenerateViewModel]
    public partial class UserSettings
    {
        public const string IMAGE_MEDALLION = "magemedallion.webp";
        public const string IMAGE_MEDALLION_ARCH = "magemedallionarch.webp";
        public const int DEFAULT_LEVEL = 5;
        public const int DEFAULT_SPELLDAMAGE = 0;
        public const int DEFAULT_BARDDAMAGE = 4;
        public const bool DEFAULT_ASHENNES_BOOL = true;
        public const WizardType DEFAULT_WIZARDTYPE = WizardType.HumanWizard;
        public const MedallionType DEFAULT_ASHENNES = MedallionType.AshennesArchmageMedallion;
        public const int SORCERER_SPELLS_0TH = 6;
        public const int SORCERER_SPELLS_1ST = 5;
        public const int SORCERER_SPELLS_2ND = 3;
        [GenerateProperty]
        private bool _isAshenne = DEFAULT_ASHENNES_BOOL;
        [GenerateProperty]
        public string _tokenImage = string.Empty;
        /*[GenerateProperty]
        private bool _hasSpellStoring = true;
        [GenerateProperty]
        private bool _hasSpellSwapping = true;
        [GenerateProperty]
        private bool _hasCabalSetBonus = true;
        [GenerateProperty]
        private bool _hasArcaneSetBonus = true;*/
        [GenerateProperty]
        private bool _canViewSpellList = false;
        [GenerateProperty]
        private bool _canEditInitialSettings = true;
        [GenerateProperty]
        private int _bonusSpellDamage = DEFAULT_SPELLDAMAGE;
        [GenerateProperty]
        private bool _isBardBonusEnabled = false;
        [GenerateProperty]
        private int _bonusSkillCheckDamage = 0;
        [GenerateProperty]
        private int _bonusBardSpellDamage = DEFAULT_BARDDAMAGE;
        [GenerateProperty]
        private int _level = DEFAULT_LEVEL;
        [GenerateProperty]
        private WizardType _selectedClass = DEFAULT_WIZARDTYPE;
        [GenerateProperty]
        private MedallionType _selectedMedallion = DEFAULT_ASHENNES;
        [GenerateProperty(SetterAccessModifier = AccessModifier.Protected)]
        private System.Collections.ObjectModel.ObservableCollection<Spell> _spells = new System.Collections.ObjectModel.ObservableCollection<Spell>();
        [GenerateProperty]
        private Turn _currentTurn = new Turn();

        public UserSettings()
        {
            TokenImage = _isAshenne? IMAGE_MEDALLION_ARCH : IMAGE_MEDALLION;
        }

        protected void OnSelectedMedallionChanged()
        {
            IsAshenne = MedallionType.AshennesArchmageMedallion == _selectedMedallion;
            TokenImage = IsAshenne ? IMAGE_MEDALLION_ARCH : IMAGE_MEDALLION;
        }

        protected void OnIsBardBonusEnabledChanged()
        {
            OnBardSpellDamageChanged();
        }
        protected void OnBardSpellDamageChanged()
        {
            foreach(Spell spell in _spells)
            {
                spell.RefreshText();
            }
        }

        internal void UpdateSpellsFromClass(int level)
        {
            switch(_selectedClass)
            {
                case WizardType.HumanWizard:
                    populateSpellsForHumanWizard(level);
                    break;
                case WizardType.ElfWizard:
                    populateSpellsForElfWizard(level);
                    break;
                case WizardType.Illusionist:
                    populateSpellsForIllusionist();
                    break;
                case WizardType.Sorcerer:
                    populateSpellsForSorcerer();
                    break;
            }
            foreach (Spell spell in _spells)
            {
                spell.ResetUses();
            }
        }

        public void ResetRunState()
        {
            CanViewSpellList = false;
            CanEditInitialSettings = true;
            Level = DEFAULT_LEVEL;
            SelectedClass = DEFAULT_WIZARDTYPE;
            BonusBardSpellDamage = DEFAULT_BARDDAMAGE;
            BonusSpellDamage = DEFAULT_SPELLDAMAGE;
            SelectedMedallion = DEFAULT_ASHENNES;
        }

        private void populateSpellsForElfWizard(int level)
        {
            Spells.Clear();
            // Level 0
            Spells.Add(new Models.Spell(this, "Acid Splash", Models.CastActionType.SkillTestAttack, level == 4 ? 3 : 4)
            {
                DamageType = "Acid"
            });
            Spells.Add(new Models.Spell(this, "Shocking Grasp", Models.CastActionType.SkillTestAttack, 2)
            {
                DamageType = "Shock"
            });
            // Level 1
            if (level == 5)
            {
                Spells.Add(new Models.Spell(this, "Acid Ray", Models.CastActionType.Attack, 1, 1)
                {
                    DamageType = "Acid",
                    BaseDamage = 12,
                    TargetAc = 15
                });
            }
            Spells.Add(new Models.Spell(this, "Alertness", Models.CastActionType.None, 1, 1)
            {
                Text = "+10 to Initiative rolls (cast before DM announces Init.)"
            });

            if (level == 4)
            {
                Spells.Add(new Models.Spell(this, "Instant Safeguard", Models.CastActionType.None, 1, 1)
                {
                    Text = "Static AC 16; instantly cast, may take other actions"
                });
            }

            Spells.Add(new Models.Spell(this, "Magic Missile", Models.CastActionType.SkillTestAttack, 3, 1)
            {
                DamageType = "Force",
                BaseDamage = 8,
            });
            // Level 2

            Spells.Add(new Models.Spell(this, "Bull's Strength", Models.CastActionType.None, 1, 2)
            {
                Text = "Target recieves +4 to STR for the rest of the room"
            });


            Spells.Add(new Models.Spell(this, "Invisibility", Models.CastActionType.None, 1, 2)
            {
                Text = "You are invisible until you make a hostile action (1 room)"
            });

            Spells.Add(new Models.Spell(this, "Ray of Shock", Models.CastActionType.Attack, 1, 2)
            {
                DamageType = "Shock",
                BaseDamage = 18,
                TargetAc = 15
            });
            // Level 3
            if (level == 5)
            {
                Spells.Add(new Models.Spell(this, "Fireball", Models.CastActionType.AttackNoChallenge, 1, 3)
                {
                    DamageType = "Fire",
                    BaseDamage = 20,
                    IsAoe = true,
                });
                Spells.Add(new Models.Spell(this, "Ironskin", Models.CastActionType.None, 1, 3)
                {
                    Text = "Target ignores first 5 pts of damage per hit or effect (1 room)"
                });
            }
        }

        private void populateSpellsForHumanWizard(int level)
        {
            Spells.Clear();
            // Level 0
            Spells.Add(new Models.Spell(this, "Fire Dart", Models.CastActionType.SkillTestAttack, 3)
            {
                DamageType = "Fire",
            });
            Spells.Add(new Models.Spell(this, "Frost Dart", Models.CastActionType.SkillTestAttack, level == 4 ? 2 : 3)
            {
                DamageType = "Cold",
            });
            // Level 1
            if (level == 5)
            {
                Spells.Add(new Models.Spell(this, "Acid Ray", Models.CastActionType.Attack, 1, 1)
                {
                    DamageType = "Acid",
                    BaseDamage = 12,
                    TargetAc = 15
                });
            }

            Spells.Add(new Models.Spell(this, "Burning Hands", Models.CastActionType.SkillTestAttack, 1, 1)
            {
                DamageType = "Fire",
                BaseDamage = 6,
                IsAoe = true,
            });
            if (level == 4)
            {

                Spells.Add(new Models.Spell(this, "Instant Safeguard", Models.CastActionType.None, 1, 1)
                {
                    Text = "Grants AC 16; Instant Cast; May take other actions."
                });
            }

            Spells.Add(new Models.Spell(this, "Magic Missile", Models.CastActionType.SkillTestAttack, level == 4 ? 2 : 3, 1)
            {
                DamageType = "Force",
                BaseDamage = 8,
            });
            // Level 2

            Spells.Add(new Models.Spell(this, "Cat's Grace", Models.CastActionType.None, 1, 2)
            {
                Text = "Target recieves +4 to DEX for the rest of the room"
            });

            Spells.Add(new Models.Spell(this, "Ray of Shock", Models.CastActionType.Attack, 1, 2)
            {
                DamageType = "Shock",
                BaseDamage = 18,
                TargetAc = 15
            });

            Spells.Add(new Models.Spell(this, "Scorching Ray", Models.CastActionType.Attack, 1, 2)
            {
                DamageType = "Fire",
                BaseDamage = 18,
                TargetAc = 15
            });
            // Level 3
            if (level == 5)
            {
                Spells.Add(new Models.Spell(this, "Lesser Maze", Models.CastActionType.None, 1, 3)
                {
                    Text = "Remove monster for 1 round; players get 1 action; re-roll Initiative"
                });
                Spells.Add(new Models.Spell(this, "Lightning Storm", Models.CastActionType.AttackNoChallenge, 1, 3)
                {
                    DamageType = "Shock",
                    BaseDamage = 20,
                    IsAoe = true,
                });
            }
        }

        private void populateSpellsForIllusionist()
        {
            Spells.Clear();
            // Level 0
            Spells.Add(new Models.Spell(this, "Acid Splash", Models.CastActionType.None, 2)
            {
                Text = "Target is immune to gaze attacks"
            });
            Spells.Add(new Models.Spell(this, "Glow Orb", Models.CastActionType.SkillTestAttack, 4)
            {
                DamageType = "Force"
            });
            // Level 1
            Spells.Add(new Models.Spell(this, "Chromatic Orb", Models.CastActionType.Attack, 1, 1)
            {
                DamageType = "Force",
                BaseDamage = 12,
                TargetAc = 15
            });
            Spells.Add(new Models.Spell(this, "Color Spray", Models.CastActionType.None, 1, 1)
            {
                Text = "Will DC 15 or target is -4 to its attack rolls for the rest of combat."
            });
            Spells.Add(new Models.Spell(this, "Magic Missile", Models.CastActionType.SkillTestAttack, 3, 1)
            {
                DamageType = "Force",
                BaseDamage = 8,
            });
            // Level 2

            Spells.Add(new Models.Spell(this, "Horrid Vision", Models.CastActionType.None, 1, 2)
            {
                Text = "Will DC 15 or target cannot attack more then once/round"
            });
            Spells.Add(new Models.Spell(this, "Hypnotic Pattern", Models.CastActionType.None, 1, 2)
            {
                Text = "Will DC 20 or all monsters -4 to attack rolls for combat"
            });
            Spells.Add(new Models.Spell(this, "Invisibility", Models.CastActionType.None, 1, 2)
            {
                Text = "You are invisible until you make a hostile action (1 room)"
            });
            // Level 3
            Spells.Add(new Models.Spell(this, "Improved Invisibility", Models.CastActionType.None, 1, 3)
            {
                Text = "You are invisible until you make two hostile actions (1 room)"
            });
            Spells.Add(new Models.Spell(this, "Prismatic Spray", Models.CastActionType.AttackNoChallenge, 1, 3)
            {
                DamageType = "Force",
                BaseDamage = 10,
                IsAoe = true,
                Text = "Victims cannot move nor take any actions for next turn."
            });
        }
        private void populateSpellsForSorcerer()
        {
            Spells.Clear();
            // Level 0
            Spells.Add(new Models.Spell(this, "Fire Dart", Models.CastActionType.SkillTestAttack, 3)
            {
                DamageType = "Fire",
            });
            Spells.Add(new Models.Spell(this, "Frost Dart", Models.CastActionType.SkillTestAttack, 3)
            {
                DamageType = "Cold",
            });
            // Level 1
            Spells.Add(new Models.Spell(this, "Acid Ray", Models.CastActionType.Attack, 2, 1)
            {
                DamageType = "Acid",
                BaseDamage = 12,
                TargetAc = 15
            });
            Spells.Add(new Models.Spell(this, "Burning Hands", Models.CastActionType.SkillTestAttack, 1, 1)
            {
                DamageType = "Fire",
                BaseDamage = 6,
                IsAoe = true,
            });

            Spells.Add(new Models.Spell(this, "Magic Missile", Models.CastActionType.SkillTestAttack, 2, 1)
            {
                DamageType = "Force",
                BaseDamage = 8,
            });
            // Level 2

            Spells.Add(new Models.Spell(this, "Elevate Damage", Models.CastActionType.None, 1, 2)
            {
                Text = "Adds 15 pts to next spell you cast (1/room, not tracked by app)"
            });

            Spells.Add(new Models.Spell(this, "Ray of Shock", Models.CastActionType.Attack, 1, 2)
            {
                DamageType = "Shock",
                BaseDamage = 18,
                TargetAc = 15
            });

            Spells.Add(new Models.Spell(this, "Stone Storm", Models.CastActionType.AttackNoChallenge, 1, 2)
            {
                DamageType = "Force",
                BaseDamage = 15,
                IsAoe = true,
            });
        }

    }
}
