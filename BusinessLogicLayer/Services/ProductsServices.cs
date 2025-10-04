using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ProductsServices(IValidator<ProductAddRequest> _productAddRequestValidator,
        IValidator<ProductUpdateRequest> _productUpdateRequestValidator, IMapper _mapper,
        IProductsRepository _productsRepository) : IProductsSerives
    {
        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            if (productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }
            ValidationResult? validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);
            if(!validationResult.IsValid)
            {
                string errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errorMessages);
            }
            Product productInput = _mapper.Map<Product>(productAddRequest);
            Product? addedProduct = await _productsRepository.AddProduct(productInput);
            if (addedProduct == null) 
                return null;
            ProductResponse? productResponse = _mapper.Map<ProductResponse>(addedProduct);
            return productResponse;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            Product? existingProduct= await _productsRepository.GetProductByContaiton(p => p.ProductID == productID);
            if(existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productID} not found.");
            }
            bool isDeleted= await _productsRepository.DeleteProduct(productID);
            return isDeleted;
        }

        public async Task<List<ProductResponse?>> GetProducsByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            IEnumerable<Product?> products = await _productsRepository.GetProductsByCondition(conditionExpression);
            IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses.ToList();
        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression)
        {
            Product? product = await _productsRepository.GetProductByContaiton(conditionExpression);
            if(product == null)
            {
                return null;
            }
            ProductResponse? productResponse = _mapper.Map<ProductResponse>(product);
            return productResponse;
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            IEnumerable<Product?> products = await  _productsRepository.GetAllProducts();
            IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return productResponses.ToList();

        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            Product? existingProduct = await _productsRepository.GetProductByContaiton(p => p.ProductID == productUpdateRequest.ProductID);
            if(existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {productUpdateRequest.ProductID} not found.");
            }
            ValidationResult? validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);
            if(!validationResult.IsValid)
            {
                string errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errorMessages);
            }

            Product productInput = _mapper.Map<Product>(productUpdateRequest);
            Product? updatedProduct = await _productsRepository.UpdateProduct(productInput);
            if (updatedProduct == null)
                return null;
            ProductResponse? productResponse = _mapper.Map<ProductResponse>(updatedProduct);
            return productResponse;
        }
    }
}
