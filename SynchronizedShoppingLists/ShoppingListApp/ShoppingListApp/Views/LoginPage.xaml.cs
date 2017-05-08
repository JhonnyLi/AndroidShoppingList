using ShoppingListApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Json;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingListApp.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public ICommand LoginCommand { get; private set; }
        public LoginPage(string name)
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel(Navigation, name);
        }

        private void BtnFacebook_Clicked(object sender, EventArgs e)
        {
            var auth = new OAuth2Authenticator(
                clientId: ApiKeys.FacebookApi,
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"));

            auth.AllowCancel = true;
            auth.Completed += Auth_Completed;
            
            //var ui = auth.
            //auth.
            
        }

        private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var request = new OAuth2Request(
                    "GET",
                    new Uri("https://graph.facebook.com/me?fields=name,picture,cover,birthday"),
                    null,
                    e.Account);

                var fbResponse = await request.GetResponseAsync();

                //var fbUser = JsonValue.
            }
        }
        async void FacebookModal()
        {
            
        }
    }

    public class LoginPageViewModel : INotifyPropertyChanged
    //public class LoginPageViewModel : INavigation
    {
        public ICommand LoginCommand { get; private set; }
        public INavigation Navigate { get; set; }
        private string _userName { get; set; }
        public string UserName { get; set; }

        public LoginPageViewModel(INavigation nav, string name)
        {
            _userName = name;
            Navigate = nav;
            LoginCommand = new Command(() => Login());
        }
        private async void Login()
        {
            try
            {
                if (!String.IsNullOrEmpty(UserName))
                {
                    _userName = UserName;
                    await Navigate.PopModalAsync(true);
                }
            }
            catch(Exception e)
            {
                var message = e;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
