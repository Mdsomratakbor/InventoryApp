using ProductApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Services.Interfaces
{
    public interface IProductServices
    {

        Task<string> SaveProduct(Product model);
        Task<string> SaveCategory(Category model);
        Task<string> SaveSubCategory(SubCategory model);
        (IEnumerable<Product> Products, int TotalCount) GetProducts(int pageNumber, int pageSize, string? title = null, string? brand = null);
    }
}
