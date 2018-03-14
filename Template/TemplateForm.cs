namespace ScrapbookBot.Template
{
    public class TemplateForm
    {
        public long Id { get; }
        public string Name { get; }
        public string Description { get; }
        public TemplateField Fields { get; }

        public TemplateForm(long id, string name, string description, TemplateField fields)
        {
            Id = id;
            Name = name;
            Description = description;
            Fields = fields;
        }
    }
}