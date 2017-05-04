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

namespace ShoppingListApp.Models
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public string ChatMessage { get; set; }
        public List<ShoppingList> AllShoppingLists { get; set; }
        public List<Item> AllItems { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public ICommand OnButtonClickedCommand { get; private set; }

        private readonly HttpClient _client;
        private readonly Client _signalRClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            AllShoppingLists = new List<ShoppingList>();
            AllItems = new List<Item>();
            Messages = new ObservableCollection<Message>();
            Name = "Androids";
            _client = new HttpClient();
            _signalRClient = new Client(Name,this);
            GetAllShoppingLists();
            _signalRClient.Connect();
            _signalRClient.OnMessageReceived += _signalRClient_OnMessageReceived;
            //_signalRClient.OnMessageReceiveded += _signalRClient_OnMessageReceiveded;
            OnButtonClickedCommand = new Command(() => OnButtonClicked());
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
                AllShoppingLists = JsonConvert.DeserializeObject<List<ShoppingList>>(contents);

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
           _signalRClient.Send(ChatMessage);
            ChatMessage = string.Empty;
           
        }
    }
}
