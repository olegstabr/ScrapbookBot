using Newtonsoft.Json;
using ScrapbookBot.Template;

namespace ScrapbookBot.Order
{
    public class FieldValue
    {
        public long? Id { get; }
        public string Value { get; set; }
        public OrderForm OrderForm { get; }
        [JsonProperty("templateFieldTemplate")]
        public TemplateField TemplateField { get; }

        public FieldValue(long id, string value, OrderForm orderForm, TemplateField templateField)
        {
            Id = id;
            Value = value;
            OrderForm = orderForm;
            TemplateField = templateField;
        }
    }
}