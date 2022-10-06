using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashennes.Helpers
{
    internal static class ColorHelper
    {
        public static readonly Brush SelectedBackgroundColor = new SolidColorBrush(Colors.Green);
        public static readonly Brush CancelBackgroundColor = new SolidColorBrush(Colors.Red);
        public static readonly Brush DisabledBackgroundColor = new SolidColorBrush(Colors.Black);
        public static readonly Brush DeselectedBackgroundColor = new SolidColorBrush(Colors.DarkGray);
        public static readonly Color DarkForegroundColor = Colors.Black;
        public static readonly Color LightForegroundColor = Colors.White;
    }
}
