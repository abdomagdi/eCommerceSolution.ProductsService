using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.RepositoryContracts
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> expressionCondation);
        Task<Product?> GetProductByContaiton(Expression<Func<Product, bool>> expressionCondation);
        Task<Product> AddProduct(Product product);
        Task<Product?> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Guid productID);
    }
}
