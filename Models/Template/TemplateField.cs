using Newtonsoft.Json;

namespace ScrapbookBot.Models.Template
{
    public class TemplateField
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        public TemplateField()  { }

        public TemplateField(long id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}