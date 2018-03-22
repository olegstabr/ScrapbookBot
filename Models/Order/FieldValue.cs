using Newtonsoft.Json;
using ScrapbookBot.Models.Template;

namespace ScrapbookBot.Models.Order
{
    public class FieldValue
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("orderForm", ItemReferenceLoopHandling = ReferenceLoopHandling.Serialize, IsReference = true)]
        public OrderForm OrderForm { get; set; }
        [JsonProperty("templateFieldTemplate")]
        public TemplateField TemplateFieldTemplate { get; set; }

        public FieldValue() { }

        public FieldValue(long id, string value, OrderForm orderForm, TemplateField templateFieldTemplate)
        {
            Id = id;
            Value = value;
            OrderForm = orderForm;
            TemplateFieldTemplate = templateFieldTemplate;
        }
    }
}