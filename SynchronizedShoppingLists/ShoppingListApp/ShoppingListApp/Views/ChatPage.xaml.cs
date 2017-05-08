using ShoppingListApp.Models;
using ShoppingListApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingListApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public MainPageViewModel _vm;

        public ChatPage(MainPageViewModel vm)
        {
            _vm = vm;
            InitializeComponent();
            BindingContext = vm;
        }

        //private void ToolbarChatBtn_Clicked()
        //{
        //    Navigation.PopModalAsync();
        //}

        private void Entry_Completed(object sender, EventArgs e)
        {
            _vm.SendChatMessage();
            ChatEntry.Focus();
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PopModalAsync();
        //}
    }
}
