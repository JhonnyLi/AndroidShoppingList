using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ShoppingListApp.Models
{
    class MainPageViewModel
    {
        public List<ShoppingList> AllShoppingLists = new List<ShoppingList>();
        public List<Item> AllItems = new List<Item>();

        private readonly HttpClient _client;
        public MainPageViewModel()
        {
            _client = new HttpClient();
            GetAllShoppingLists();

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
                var dummy = "";
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var breakit = "";
            }
            return model;
        }
    }
}
