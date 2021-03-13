using Core.Entities;

namespace Core.Specifications
{
    public class TypesWithCategoriesSpecification : BaseSpecification<ProductType>
    {
        public TypesWithCategoriesSpecification()
        {
            AddInclude(x => x.ProductCategory);
        }
    }
}