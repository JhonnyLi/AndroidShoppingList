using ShoppingListApp.Models;
using ShoppingListApp.Views;
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
        static NavigationPage _ChatNavPage;
        public MainPageViewModel _vm;
        public MainPage(MainPageViewModel vm)
        {
            _vm = vm;
            InitializeComponent();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ApiKeys.IsLoggedIn)
            {
                if (!App._AlreadyCalled)
                {
                    Navigation.PushModalAsync(new ShoppingListApp.LoginPage());
                    App._AlreadyCalled = true;
                }
            }
        }

        async void ToolbarChatBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChatPage(_vm));
        }

        private void EntryAddItem_Completed(object sender, EventArgs e)
        {
            var senderEntry = sender as Entry;
            Item newItem = new Item()
            {
                Name = _vm.NewItem,
                Comment = ""
            };
            _vm.ListItems.Add(newItem);
        }
    }
}
