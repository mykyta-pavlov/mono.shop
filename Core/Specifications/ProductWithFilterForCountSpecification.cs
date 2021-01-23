using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFilterForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterForCountSpecification(ProductSpecParams productParams)
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) &&
                (!productParams.SizeAvailableXxs.HasValue || productParams.SizeAvailableXxs == true) &&
                (!productParams.SizeAvailableXs.HasValue || productParams.SizeAvailableXs == true) &&
                (!productParams.SizeAvailableS.HasValue || productParams.SizeAvailableS == true) &&
                (!productParams.SizeAvailableM.HasValue || productParams.SizeAvailableM == true) &&
                (!productParams.SizeAvailableL.HasValue || productParams.SizeAvailableL == true) &&
                (!productParams.SizeAvailableXl.HasValue || productParams.SizeAvailableXl == true) &&
                (!productParams.SizeAvailableXxl.HasValue || productParams.SizeAvailableXxl == true)
            )
        {

        }
    }
}