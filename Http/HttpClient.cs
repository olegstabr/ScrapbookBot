using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScrapbookBot.Http
{
    public class HttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();

        public HttpClient()
        {
            _httpClient.BaseAddress = new Uri("https://frozen-sea-73750.herokuapp.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> PostOrderAsync(Order.Order order)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("order", order);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Order.Order>> GetOrderAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://frozen-sea-73750.herokuapp.com/order");
            List<Order.Order> orders = null;

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<Order.Order>>(str);
            }
            
            return orders;
        }
    }
}