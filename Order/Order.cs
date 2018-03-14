using System;
using System.Collections.Generic;

namespace ScrapbookBot.Order
{
    public class Order
    {
        public long Id { get; }
        public string CustomerName { get; }
        public string CustomerPhone { get; }
        public DateTime CreatedDate { get; }
        public DateTime DeadlineDate { get; }
        public string Status { get; }
        public List<OrderForm> OrderForms { get; }

        public Order(long id, string customerName, string customerPhone, DateTime createdDate, DateTime deadlineDate, string status, List<OrderForm> orderForms)
        {
            Id = id;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            CreatedDate = createdDate;
            DeadlineDate = deadlineDate;
            Status = status;
            OrderForms = orderForms;
        }
    }
}