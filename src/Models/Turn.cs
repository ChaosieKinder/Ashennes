using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Mvvm.CodeGenerators;

namespace Ashennes.Models
{
    [GenerateViewModel]
    public partial class Turn
    {
        [GenerateProperty]
        public ObservableCollection<SpellCast> _spells = new ObservableCollection<SpellCast>();
        [GenerateProperty]
        public string _hpCostText = "";
        public Turn()
        {
            _spells.CollectionChanged += _spells_CollectionChanged;
        }

        private void _spells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            int hpCost = 0;
            foreach(var spell in _spells)
            {
                hpCost += spell.HpCost;
            }
            HpCostText = $"This spell chain will cost {hpCost} hp to cast.";
        }
    }
}
