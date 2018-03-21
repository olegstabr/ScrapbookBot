using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ScrapbookBot.Order
{
    public class Order
    {
        public bool IsFullInfo => CustomerName != default && CustomerName != string.Empty && CustomerPhone != default &&
                                  CustomerPhone != string.Empty && CreatedDate != default &&
                                  CreatedDate != string.Empty && DeadlineDate != default &&
                                  DeadlineDate != string.Empty && Status != default && Status != string.Empty &&
                                  OrderForms != null;

        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CreatedDate { get; set; }
        public string DeadlineDate { get; set; }
        public string Status { get; set; }
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