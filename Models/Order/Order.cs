using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ScrapbookBot.Models.Order
{
    public class Order
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("customerPhone")]
        public string CustomerPhone { get; set; }
        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }
        [JsonProperty("deadlineDate")]
        public string DeadlineDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("orderForms")]
        public List<OrderForm> OrderForms { get; set; }

        public Order() { }

        public Order(long id, string customerName, string customerPhone, string createdDate, string deadlineDate, string status, List<OrderForm> orderForms)
        {
            Id = id;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            CreatedDate = createdDate;
            DeadlineDate = deadlineDate;
            Status = status;
            OrderForms = orderForms;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"ID: \t \t{Id}");
            builder.AppendLine($"Customer Name: \t{CustomerName}");
            builder.AppendLine($"Customer Phone: {CustomerPhone}");
            builder.AppendLine($"Created Date: \t{CreatedDate}");
            builder.AppendLine($"Deadline Date: \t{DeadlineDate}");
            builder.AppendLine($"Status: \t{Status}");
            builder.AppendLine("OrderForms: ");
            foreach (var form in OrderForms)
            {
                builder.AppendLine($"{form}");
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}