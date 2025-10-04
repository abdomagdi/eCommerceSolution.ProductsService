using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ProductsRepository(ApplicationDbContext _context) : IProductsRepository
    {
        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            Product? existitingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productID);
            if (existitingProduct == null)
            {
                return false;
            }
            _context.Products.Remove(existitingProduct);
            int affectedRows =await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public Task<Product?> GetProductByContaiton(Expression<Func<Product, bool>> expressionCondation)
        {
            return _context.Products.FirstOrDefaultAsync(expressionCondation);
        }

        public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> expressionCondation)
        {
            return await _context.Products.Where(expressionCondation).ToListAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);
            if(existingProduct == null)
            {
                return null;
            }
            existingProduct.ProductName = product.ProductName;
            existingProduct.Category = product.Category;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.QuantityInStock = product.QuantityInStock;
            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}
