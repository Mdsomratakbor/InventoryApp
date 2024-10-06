
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductApp.API.Dtos;
using ProductApp.Entities;
using ProductApp.Services;
using ProductApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.API.Controllers
{
    [ApiController]

    public class ProductController : ControllerBase
    {


        private readonly IProductServices _proudctService;
        public ProductController(IConfiguration configuration, IProductServices productServices)
        {
            _proudctService = productServices;
    
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/category/create")]
        public async Task<ResponseMessage> CreateCategory(CategoryDTO model)
        {
            Category category = new Category()
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                IsDeleted = false // Assuming it's a new category
            };

            string isCreated = await _proudctService.SaveCategory(category);
            if (isCreated == "1")
            {
                return new ResponseMessage(HttpStatusCode.OK, true, "Category created successfully", category);
            }

            return new ResponseMessage(HttpStatusCode.BadRequest, false, "Failed to create category", null);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/subcategory/create")]
        public async Task<ResponseMessage> CreateSubCategory(SubCategoryDTO model)
        {
            SubCategory subCategory = new SubCategory()
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                IsDeleted = false, // Assuming it's a new subcategory
                CategoryId = model.CategoryId // Linking to a category
            };

            string isCreated = await _proudctService.SaveSubCategory(subCategory);
            if (isCreated == "1")
            {
                return new ResponseMessage(HttpStatusCode.OK, true, "Subcategory created successfully", subCategory);
            }

            return new ResponseMessage(HttpStatusCode.BadRequest, false, "Failed to create subcategory", null);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/product/create")]
        public async Task<ResponseMessage> CreateProduct(ProductDTO model)
        {
            Product product = new Product()
            {
                ProductTitle = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                Brand = model.Brand,
                Code = model.Code,
                Price = model.Price,
                IsDeleted = false, // Assuming it's a new product
                CategoryId = model.CategoryId, // Linking to a category
                SubCategoryId = model.SubCategoryId // Linking to a subcategory
            };

            string isCreated = await _proudctService.SaveProduct(product);
            if (isCreated == "1")
            {
                return new ResponseMessage(HttpStatusCode.OK, true, "Product created successfully", product);
            }

            return new ResponseMessage(HttpStatusCode.BadRequest, false, "Failed to create product", null);
        }

        [HttpGet]
        [Route("api/product/list")]
        public ResponseMessage GetProducts(string? title = "", string? brand = "", int pageNumber = 1, int pageSize = 10)
        {
            // Call the service method to get products with filtering and pagination
            var (products, totalCount) = _proudctService.GetProducts(pageNumber, pageSize, title, brand);

            // Prepare a list of DTOs
            var productDtos = new List<ProductListDto>();

            // Loop through products and bind to DTO
            foreach (var product in products)
            {
                var productDto = new ProductListDto
                {
                    Id = product.Id,
                    Title = product.ProductTitle,
                    Code = product.Code,
                    Brand = product.Brand,
                    Price = product.Price,
                    Description = product.Description,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    IsDeleted = product.IsDeleted
                };

                productDtos.Add(productDto);
            }

            // Prepare the response message
            return new ResponseMessage(HttpStatusCode.OK, true, "Success", new
            {
                Products = productDtos, // Use the list of DTOs
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }



    }
}
