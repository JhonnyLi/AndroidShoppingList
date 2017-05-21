﻿using Microsoft.AspNet.SignalR.Client;
using ShoppingListApp.Models;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

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
            _connection = new HubConnection("http://sync.jhonny.se/", queryData);
            _proxy = _connection.CreateHubProxy("SyncHub");
            
        }
        public async Task Connect()
        {
            try
            {
                _proxy.On("listMessage", (string userName, ShoppingList list) =>
                {
                    _vm.ListItems.Clear();
                    foreach(var item in list.Items)
                    {
                        _vm.ListItems.Add(item);
                    }
                    _vm._activeList = list;
                    _vm.Messages.Add(new Message() { Name = userName, Text = "listUpdateMessage" });
                });

                _proxy.On("broadcastMessage", (string userName, string message) =>
                {
                    _vm.Messages.Add(new Message() { Name = userName, Text = message });
                });
                _proxy.On("connectionMessage", (string userName, string message) =>
                {
                    _vm.Messages.Add(new Message() { Name = userName, Text = message });
                });
                await _connection.Start();
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
        public Task SendList(string updatedList)
        {
            return _proxy.Invoke("SendList", updatedList);
        }
    }
}
