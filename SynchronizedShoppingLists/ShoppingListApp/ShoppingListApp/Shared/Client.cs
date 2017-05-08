using Microsoft.AspNet.SignalR.Client;
using ShoppingListApp.Models;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListApp.Shared
{
    public class Client
    {
        private readonly string _userName;
        private MainPageViewModel _vm;
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;

        public event EventHandler<string> OnMessageReceived;

        public Client(string userName, MainPageViewModel vm)
        {
            _userName = userName;
            _vm = vm;
            var queryData = new Dictionary<string, string>();
            queryData.Add("username", _userName);
            //_connection = new HubConnection("http://localhost:3768/signalr");
            _connection = new HubConnection("http://sync.jhonny.se/", queryData);
            //_connection.Items.Add("userName", _userName);
            _proxy = _connection.CreateHubProxy("SyncHub");
            
        }
        public async Task Connect()
        {
            try
            {
                
                await _connection.Start();

                _proxy.On("broadcastMessage", (string userName, string message) =>
                {
                    _vm.Messages.Add(new Message() { Name = userName, Text = message });
                    //if (OnMessageReceived != null)
                        //OnMessageReceived(this, string.Format("{0}: {1}", platform, message));
                });
                _proxy.On("connectionMessage", (string userName, string message) =>
                {
                    _vm.Messages.Add(new Message() { Name = userName, Text = message });
                    //if (OnMessageReceived != null)
                    //OnMessageReceived(this, string.Format("{0}: {1}", platform, message));
                });
                //_proxy.On("contextMessage", (string serial) =>
                //{
                //    var contextObject = JsonValue.Parse(serial);
                //    var dummy = "";
                //    //if (OnMessageReceived != null)
                //    //OnMessageReceived(this, string.Format("{0}: {1}", platform, message));
                //});
                //_vm.Messages.Add(new Message { Name = _vm.UserName, Text = "Connected" });
                //await Send(_platform + " : Connected");
            }
            catch (HttpRequestException e)
            {
                var err = e.Message;
            }
        }

        public Task Send(string name, string message)
        {
            return _proxy.Invoke("Send", name, message);
        }
    }
}
