using System;
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
    public partial class SpellUse
    {
        private bool _isUsed = false;
        [GenerateProperty]
        private Spell _owner = null;
        [GenerateProperty]
        private int _useIndex = 0;

        public SpellUse(int useIndex, Spell owner)
        {
            _useIndex = useIndex;
            _owner = owner;
        }
        public bool IsUsed
        {
            get => _isUsed;
            set
            {
                if (EqualityComparer<bool>.Default.Equals(_isUsed, value)) return;
                if (changeLockout)
                {
                    _isUsed = value;
                    RaisePropertyChanged(IsUsedChangedEventArgs);
                    _owner.UpdateNameDropdownText();
                    return;
                }

                try
                {
                    changeLockout = true;
                    if (value)
                    {
                        // We are checking a used box
                        for (int i = 0; i < _owner.Uses.Count; i++)
                        {
                            // look for and check the first use box we find
                            if (_owner.Uses[i].IsUsed != value)
                            {
                                _owner.Uses[i].IsUsed = value;
                                return;
                            }
                        }
                    }
                    else
                    {
                        // We are unchecking a used box
                        for (int i = _owner.Uses.Count - 1; i >= 0; i--)
                        {
                            // look for and check the first use box we find
                            if (_owner.Uses[i].IsUsed != value)
                            {
                                _owner.Uses[i].IsUsed = value;
                                return;
                            }
                        }
                    }
                }
                finally
                {
                    changeLockout = false;
                    RaisePropertyChanged(IsUsedChangedEventArgs);
                    _owner.UpdateNameDropdownText();
                }
            }
        }

        static PropertyChangedEventArgs IsUsedChangedEventArgs = new PropertyChangedEventArgs(nameof(IsUsed));
        static bool changeLockout = false;
    }
}
