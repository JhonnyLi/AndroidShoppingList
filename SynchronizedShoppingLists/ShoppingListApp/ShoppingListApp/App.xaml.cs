using ShoppingListApp.Models;
using ShoppingListApp;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using Xamarin.Auth;
using Xamarin.Forms;

namespace ShoppingListApp
{
    public partial class App : Application
    {
        static NavigationPage _NavPage;
        public static bool _AlreadyCalled = false; //Hacklösning för att komma förbi en känd bugg : https://bugzilla.xamarin.com/show_bug.cgi?id=44211
        public static MainPageViewModel _vm;
        public App()
        {
            _vm = new MainPageViewModel();
            InitializeComponent();
            MainPage = GetMainPage(_vm);
            //MainPage = new NavigationPage(new ShoppingListApp.MainPage());
        }

        public static Page GetMainPage(MainPageViewModel vm)
        {
            var loginPage = new MainPage(vm);
            _NavPage = new NavigationPage(loginPage);
            return _NavPage;
        }
        

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() => {
                    _vm.UserName = UserInformation.UserName;
                    _vm.LoggedIn = "True";
                    _NavPage.Navigation.PopModalAsync();
                });
            }
        }
        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
    
    //Tom klass som ersätts med plattformspecifik kod.
    public class LoginPage : ContentPage
    {
       
    }
}
