namespace Application
{
    public sealed class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; } = "None";
    }
}
