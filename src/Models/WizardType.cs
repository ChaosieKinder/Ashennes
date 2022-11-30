using System.ComponentModel;

namespace Ashennes.Models
{
    public enum WizardType
    {
        [Description("Human Wizard")]
        HumanWizard,
        [Description("Elf Wizard")]
        ElfWizard,
        Illusionist,
        Sorcerer
    }
}
