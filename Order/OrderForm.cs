using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ScrapbookBot.Order
{
    public class OrderForm
    {
        public long Id { get; }
        public string Name { get; }
        public string Description { get; }
        public List<FieldValue> Fields { get; }
        
        public OrderForm() { }

        public OrderForm(long id, string name, string description, List<FieldValue> fields)
        {
            Id = id;
            Name = name;
            Description = description;
            Fields = fields;
        }
        
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"\tID: \t \t{Id}");
            builder.AppendLine($"\tName: \t \t{Name}");
            builder.AppendLine($"\tDescription: \t{Description}");
            return builder.ToString();
        }
    }
}