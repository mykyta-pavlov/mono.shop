using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesSpecification(ProductSpecParams productParams) 
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) &&
                (!productParams.SizeAvailableXxs.HasValue || (x.SizeQuantityXxs != 0) == productParams.SizeAvailableXxs) &&
                (!productParams.SizeAvailableXs.HasValue || (x.SizeQuantityXs != 0) == productParams.SizeAvailableXs) &&
                (!productParams.SizeAvailableS.HasValue || (x.SizeQuantityS != 0) == productParams.SizeAvailableS) &&
                (!productParams.SizeAvailableM.HasValue || (x.SizeQuantityM != 0) == productParams.SizeAvailableM) &&
                (!productParams.SizeAvailableL.HasValue || (x.SizeQuantityL != 0) == productParams.SizeAvailableL) &&
                (!productParams.SizeAvailableXl.HasValue || (x.SizeQuantityXl != 0) == productParams.SizeAvailableXl) &&
                (!productParams.SizeAvailableXxl.HasValue || (x.SizeQuantityXxl != 0) == productParams.SizeAvailableXxl)
            )
        {
            AddInclude(x => x.ProductType);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
        }
    }
}