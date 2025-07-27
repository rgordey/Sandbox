using AutoMapper;
using Domain;
namespace Application.Mappings
{
    public class CategoryPathResolver : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (source.Category == null) return string.Empty;

            var path = new List<string> { source.Category.Name };
            var parent = source.Category.Parent;
            while (parent != null)
            {
                path.Insert(0, parent.Name);
                parent = parent.Parent;
            }
            return string.Join(" > ", path);
        }
    }
}

