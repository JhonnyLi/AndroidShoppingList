using ShoppingListApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ShoppingListApp
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel _vm;
        public MainPage()
        {
            _vm = new MainPageViewModel();
            InitializeComponent();
            //Navigation.PushModalAsync(new ShoppingListApp.Views.LoginPage());
            LoginModal();
            this.BindingContext = _vm;
     
        }

        async void LoginModal()
        {
            await Navigation.PushModalAsync(new Views.LoginPage(_vm.Name));
        }

    }
}
