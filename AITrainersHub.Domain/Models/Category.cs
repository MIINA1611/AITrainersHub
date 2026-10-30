namespace AITrainersHub.Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<PlatformCategory> PlatformCategories { get; set; } = new List<PlatformCategory>();


        public Category()
        {
            
        }
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public void Update(string? name, string? description)
        {
            Name = name ?? Name;
            Description = description ?? Description;
        }
    }
}
