using System.Collections.Generic;

namespace ScrapbookBot.Order
{
    public class OrderForm
    {
        public long Id { get; }
        public string Name { get; }
        public string Description { get; }
        public List<FieldValue> Fields { get; }

        public OrderForm(long id, string name, string description, List<FieldValue> fields)
        {
            Id = id;
            Name = name;
            Description = description;
            Fields = fields;
        }
    }
}