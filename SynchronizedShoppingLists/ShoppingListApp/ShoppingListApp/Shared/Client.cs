using Microsoft.AspNet.SignalR.Client;
using ShoppingListApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListApp.Shared
{
    public class Client
    {
        private readonly string _platform;
        private MainPageViewModel _vm;
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;

        public event EventHandler<string> OnMessageReceived;

        public Client(string platform, MainPageViewModel vm)
        {
            _platform = platform;
            _vm = vm;
            _connection = new HubConnection("http://sync.jhonny.se/");
            _proxy = _connection.CreateHubProxy("SyncHub");
        }
        public async Task Connect()
        {
            try
            {
                await _connection.Start();

                _proxy.On("broadcastMessage", (string platform, string message) =>
                {
                    _vm.Messages.Add(new Message() { Name = platform, Text = message });
                    //if (OnMessageReceived != null)
                        //OnMessageReceived(this, string.Format("{0}: {1}", platform, message));
                });
                _vm.Messages.Add(new Message { Name = _platform, Text = "Connected" });
                //await Send(_platform + " : Connected");
            }
            catch (Exception e)
            {
                var err = e.Message;
            }
        }

        public Task Send(string message)
        {
            return _proxy.Invoke("Send", _platform, message);
        }
    }
}
