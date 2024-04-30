using Ashennes.Views;

namespace Ashennes
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            InitializeComponent();
            MainPage = shell;
            UserAppTheme = AppTheme.Dark;

            ICollection<ResourceDictionary> mergedDictionaries = Current.Resources.MergedDictionaries;
            if (mergedDictionaries is not null)
            {
                mergedDictionaries.Clear();
#if ANDROID
                mergedDictionaries.Add(new Ashennes.Resources.StylesAndroid());
#elif IOS
                mergedDictionaries.Add(new Ashennes.Resources.StylesIos());
#endif
            }
        }

    }
}