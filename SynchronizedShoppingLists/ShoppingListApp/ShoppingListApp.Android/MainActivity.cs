using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using HockeyApp.Android;
using ShoppingListApp.Droid.Models;

namespace ShoppingListApp.Droid
{
    [Activity(Label = "ShoppingListApp",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private string _hockeyAppApiKey = ApiKeys.HockeyAppApi;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            try
            {

            base.OnCreate(bundle);
            }
            catch(Exception exxx)
            {
                throw exxx;
            }
            CrashManager.Register(this, _hockeyAppApiKey);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

