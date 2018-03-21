
namespace ScrapbookBot.Template
{
    public class TemplateField
    {
        public long Id { get; }
        public string Name { get; }
        public string Description { get; }

        public TemplateField(long id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}