using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ScrapbookBot.Models.Order;
using ScrapbookBot.Models.Template;

namespace ScrapbookBot.Http
{
    public class HttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();

        public HttpClient()
        {
            _httpClient.BaseAddress = new Uri("https://frozen-sea-73750.herokuapp.com/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> PostOrderAsync(Order order)
        {
            var response = await _httpClient.PostAsJsonAsync("order", order);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Order>> GetOrderAsync()
        {
            var response = await _httpClient.GetAsync("https://frozen-sea-73750.herokuapp.com/order");
            List<Order> orders = null;

            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<List<Order>>();
            }
            
            return orders;
        }

        public async Task<List<TemplateForm>> GetTemplateFormsAsync()
        {
            var response = await _httpClient.GetAsync("https://frozen-sea-73750.herokuapp.com/template/form");
            List<TemplateForm> templateForms = null;

            if (response.IsSuccessStatusCode)
            {
                templateForms = await response.Content.ReadAsAsync<List<TemplateForm>>();
            }

            return templateForms;
        }

        public async Task<Order> PostTemplateFormIntoOrderAsync(int formId)
        {
            var response = await _httpClient.PostAsJsonAsync($"template/form/{formId}/order", formId);
            
            Order order = null;
            if (response.IsSuccessStatusCode)
            {
                order = await response.Content.ReadAsAsync<Order>();
            }
            
            return order;
        }
    }
}