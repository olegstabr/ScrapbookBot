using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScrapbookBot.Models.Template
{
    public class TemplateForm
    {
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("fields")]
        public List<TemplateField> Fields { get; set; }

        public TemplateForm() { }

        public TemplateForm(long? id, string name, string description, List<TemplateField> fields)
        {
            Id = id;
            Name = name;
            Description = description;
            Fields = fields;
        }
    }
}