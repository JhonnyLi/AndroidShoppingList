using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNet.SignalR.Client;
using ShoppingListApp.Shared;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Windows.Input;
using ShoppingListApp.Views;

namespace ShoppingListApp.Models
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly HttpClient _client;
        private Client _signalRClient;

        private string _loggedIn; //Kontrollerar om man är inloggad.

        private string _userName; //Från Facebook
        private string _chatMessage; //Meddelandet som skall skickas
        private string _activeListName;
        private string _newItem; //Det som skall sparas som en ny item i listan.
        private ShoppingList _activeList { get; set; } //Den aktiva listan

        public string LoggedIn
        {
            get { return _loggedIn; }
            set
            {
                _signalRClient = new Client(UserName, this);
                _signalRClient.OnMessageReceived += _signalRClient_OnMessageReceived;
                _signalRClient.Connect();
                SetPropertyField(nameof(LoggedIn), ref _loggedIn, value);
            }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetPropertyField(nameof(UserName), ref _userName, value); }
        }
        public string ChatMessage
        {
            get { return _chatMessage; }
            set { SetPropertyField(nameof(ChatMessage), ref _chatMessage, value); }
        }
        public string ActiveListName
        {
            get { return _activeListName; }
            set { SetPropertyField(nameof(ActiveListName), ref _activeListName, value); }
        }
        public string NewItem
        {
            get { return _newItem; }
            set { SetPropertyField(nameof(NewItem), ref _newItem, value); }
        }

        public ObservableCollection<ShoppingList> AllShoppingLists { get; set; }
        public ObservableCollection<Item> AllItems { get; set; }

        public ObservableCollection<Item> ListItems { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

        public ICommand OnButtonClickedCommand { get; private set; }
        //public ICommand ToolbarChatBtn_ClickedCommand { get; private set; }
        public MainPageViewModel()
        {
            AllShoppingLists = new ObservableCollection<ShoppingList>();
            AllItems = new ObservableCollection<Item>();
            Messages = new ObservableCollection<Message>();
            ListItems = new ObservableCollection<Item>();
            UserName = UserInformation.UserName;
            _client = new HttpClient();
            //_signalRClient = new Client(UserName, this);
            GetAllShoppingLists();
            //_signalRClient.Connect();
            //_signalRClient.OnMessageReceived += _signalRClient_OnMessageReceived;
            //_signalRClient.OnMessageReceiveded += _signalRClient_OnMessageReceiveded;
            OnButtonClickedCommand = new Command(() => OnButtonClicked());
            //ToolbarChatBtn_ClickedCommand = new Command(() => ToolbarChatBtn_Clicked());
        }

        private void _signalRClient_OnMessageReceived(object sender, string e)
        {
            var newMessage = new Message()
            {
                Name = "Remote",
                Text = e
            };
            Messages.Add(newMessage);
        }

        private async Task<List<ShoppingList>> GetAllShoppingLists()
        {
            var Url = "http://sync.jhonny.se/api/Values/";
            List<ShoppingList> model = new List<ShoppingList>();
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task<string> contentsTask = _client.GetStringAsync(Url);
                string contents = await contentsTask.ConfigureAwait(false);
                //var respons = JsonConvert.DeserializeObject<List<ShoppingList>>(contents);
                AllShoppingLists = JsonConvert.DeserializeObject<ObservableCollection<ShoppingList>>(contents);
                _activeList = AllShoppingLists.First();
                ActiveListName = _activeList.Name;
                foreach (var item in _activeList.Items.OrderBy(n => n.Name))
                {
                    ListItems.Add(item);
                }

            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return model;
        }

        //private void onMessageRecieved(object sender, Message m)
        //{
        //    var dummy = m;
        //}

        private void OnButtonClicked()
        {
            SendChatMessage();
        }

        public void SendChatMessage()
        {
            _signalRClient.Send(UserName, ChatMessage);
            ChatMessage = string.Empty;
        }
        
    }
}
