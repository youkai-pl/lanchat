using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.OS;
using Xamarin.Essentials;

namespace Lanchat.Xamarin.Droid
{
    [Activity(Label = "Lanchat 2", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private bool lieAboutCurrentFocus;
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            lieAboutCurrentFocus = true;
            var result = base.DispatchTouchEvent(ev);
            lieAboutCurrentFocus = false;

            return result;
        }

        public override View CurrentFocus
        {
            get
            {
                if (lieAboutCurrentFocus)
                {
                    return null;
                }

                return base.CurrentFocus;
            }
        }
    }
}