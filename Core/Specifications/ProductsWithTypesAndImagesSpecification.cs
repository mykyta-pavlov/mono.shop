using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class ProductsWithTypesAndImagesSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndImagesSpecification(ProductSpecParams productParams) 
            : base(x =>
                (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(q => q.Include(i => i.ImageUrls).ThenInclude(i => i.Product));
            AddInclude(q => q.Include(i => i.ProductSizeQuantity).ThenInclude(i => i.Product));

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

        public ProductsWithTypesAndImagesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(q => q.Include(i => i.ImageUrls).ThenInclude(i => i.Product));
            AddInclude(q => q.Include(i => i.ProductSizeQuantity).ThenInclude(i => i.Product));
        }
    }
}