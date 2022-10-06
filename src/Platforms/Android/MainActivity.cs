using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Ashennes
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : KinderEngine.Maui.Platforms.Android.KinderEngineMainActivity
    {
    }
}