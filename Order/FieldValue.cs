using ScrapbookBot.Template;

namespace ScrapbookBot.Order
{
    public class FieldValue
    {
        public long Id { get; }
        public string Value { get; }
        public OrderForm OrderForm { get; }
        public TemplateField TemplateField;

        public FieldValue(long id, string value, OrderForm orderForm, TemplateField templateField)
        {
            TemplateField = templateField;
            Id = id;
            Value = value;
            OrderForm = orderForm;
        }
    }
}