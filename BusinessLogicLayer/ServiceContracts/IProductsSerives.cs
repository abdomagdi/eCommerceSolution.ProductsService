using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceContracts
{
    public interface IProductsSerives
    {
        Task<List<ProductResponse?>> GetProducts();
        Task<List<ProductResponse?>> GetProducsByCondition(Expression<Func<Product, bool>> conditionExpression);
        Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
        Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);
        Task<ProductResponse?> UpdateProduct( ProductUpdateRequest productUpdateRequest);
        Task<bool> DeleteProduct(Guid productID);
    }
}
