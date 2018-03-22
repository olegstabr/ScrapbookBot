using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace ScrapbookBot.Models.Order
{
    public class OrderForm
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("fields")]
        public List<FieldValue> Fields { get; set; }
        
        public OrderForm() { }

        public OrderForm(long? id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public OrderForm(long? id, string name, string description, List<FieldValue> fields)
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