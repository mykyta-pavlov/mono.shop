using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFilterForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterForCountSpecification(ProductSpecParams productParams)
            : base(ProductSpecificationFilters.ForProductSpecParams(productParams))
        {
        }
    }
}