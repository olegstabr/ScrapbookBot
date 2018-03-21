using System.Collections.Generic;

namespace ScrapbookBot.Template
{
    public class TemplateForm
    {
        public long Id { get; }
        public string Name { get; }
        public string Description { get; }
        public List<TemplateField> Fields { get; }
        
        public TemplateForm(long id, string name, string description, List<TemplateField> fields)
        {
            Id = id;
            Name = name;
            Description = description;
            Fields = fields;
        }
    }
}