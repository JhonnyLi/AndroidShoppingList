using ShoppingListApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
