using System;
using System.Linq.Expressions;
using Core.Entities;
using Core.Specifications;

public class ProductSpecificationFilters
{
    public static Expression<Func<Product, bool>> ForProductSpecParams(ProductSpecParams productParams)
    {
        return x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId);
    }
}