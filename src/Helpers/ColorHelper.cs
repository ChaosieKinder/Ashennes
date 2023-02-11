using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashennes.Helpers
{
    internal static class ColorHelper
    {
        public static readonly Brush SelectedBackgroundBrush = new SolidColorBrush(Colors.Green);
        public static readonly Color SelectedBackgroundColor = Colors.Green;
        public static readonly Brush CancelBackgroundBrush = new SolidColorBrush(Colors.Red);
        public static readonly Color CancelBackgroundColor = Colors.Red;
        public static readonly Brush DisabledBackgroundBrush = new SolidColorBrush(Colors.Black);
        public static readonly Color DisabledBackgroundColor = Colors.Black;
        public static readonly Brush DeselectedBackgroundBrush = new SolidColorBrush(Colors.DarkGray);
        public static readonly Color DeselectedBackgroundColor = Colors.DarkGray;
        public static readonly Color DarkForegroundColor = Colors.Black;
        public static readonly Color LightForegroundColor = Colors.White;
    }
}
