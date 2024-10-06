using DataReadWriteFramework.Interfaces;
using ProductApp.Entities;
using ProductApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Services
{
    public class ProductServices:IProductServices
    {
        private readonly IDataCommonRepository _context = null;
        public ProductServices(IDataCommonRepository context)
        {
            _context = context;
        }

        private string SubCategorySaveQuery() =>
    "INSERT INTO SubCategory (Id, Title, Description, CreatedAt, UpdatedAt, IsDeleted, CategoryId) " +
    "VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)";

        private string CategorySaveQuery() =>
    "INSERT INTO Category (Id, Title, Description, CreatedAt, UpdatedAt, IsDeleted) " +
    "VALUES (@param1, @param2, @param3, @param4, @param5, @param6)";

        private string ProductSaveQuery() =>
       "INSERT INTO Product (ID, ProductTitle, Code, Brand, Price, Description, CreatedAt, IsDeleted, CategoryId, SubCategoryId) " +
       "VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10)";

        public async Task<string> SaveProduct(Product model)
        {
            try
            {
                List<IQueryPattern> listOfQuery = new List<IQueryPattern>();
                // Generate a new ID for the Product table.
                int newId = await _context.GetDataOneRowColumAsync<int>("SELECT isnull(MAX(ID), 0) + 1 FROM Product");

                // Prepare the query with the parameters for the Product table.
                listOfQuery.Add(_context.AddQuery(ProductSaveQuery(), _context.AddParameter(new string[] {
            newId.ToString(),
            model.ProductTitle, // assuming ProductTitle is a string
            model.Code,         // assuming Code is a string
            model.Brand,        // assuming Brand is a string
            model.Price.ToString(), // assuming Price is a decimal, converting to string
            model.Description,  // assuming Description is a string
            model.CreatedAt.ToString(), // assuming CreatedAt is DateTime
            model.IsDeleted ? "1" : "0", // assuming IsDeleted is a boolean
            model.CategoryId.ToString(),  // assuming CategoryId is a bigint
            model.SubCategoryId.ToString() // nullable SubCategoryId
        })));

                // Execute the query and return the result.
                bool output = await _context.SaveChangesAsync(listOfQuery);
                return output == true ? "1" : "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> SaveCategory(Category model)
        {
            try
            {
                List<IQueryPattern> listOfQuery = new List<IQueryPattern>();

                // Generate a new ID for the Category table.
                int newId = await _context.GetDataOneRowColumAsync<int>("SELECT isnull(MAX(Id), 0) + 1 FROM Category");

                // Prepare the query with the parameters for the Category table.
                listOfQuery.Add(_context.AddQuery(CategorySaveQuery(), _context.AddParameter(new string[] {
            newId.ToString(),
            model.Title, // assuming Title is a string
            model.Description ?? DBNull.Value.ToString(), // nullable Description
            model.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), // assuming CreatedAt is DateTime
            model.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? DBNull.Value.ToString(), // nullable UpdatedAt
            model.IsDeleted ? "1" : "0" // assuming IsDeleted is a boolean
        })));

                // Execute the query and return the result.
                bool output = await _context.SaveChangesAsync(listOfQuery);
                return output == true ? "1" : "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> SaveSubCategory(SubCategory model)
        {
            try
            {
                List<IQueryPattern> listOfQuery = new List<IQueryPattern>();

                // Generate a new ID for the SubCategory table.
                int newId = await _context.GetDataOneRowColumAsync<int>("SELECT isnull(MAX(Id), 0) + 1 FROM SubCategory");

                // Prepare the query with the parameters for the SubCategory table.
                listOfQuery.Add(_context.AddQuery(SubCategorySaveQuery(), _context.AddParameter(new string[] {
            newId.ToString(),
            model.Title, // assuming Title is a string
            model.Description ?? DBNull.Value.ToString(), // nullable Description
            model.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), // assuming CreatedAt is DateTime
            model.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") ?? DBNull.Value.ToString(), // nullable UpdatedAt
            model.IsDeleted ? "1" : "0", // assuming IsDeleted is a boolean
            model.CategoryId.ToString() // assuming CategoryId is a bigint (Foreign Key to Category)
        })));

                // Execute the query and return the result.
                bool output = await _context.SaveChangesAsync(listOfQuery);
                return output == true ? "1" : "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //public (IEnumerable<Product> Products, int TotalCount) GetProducts(int pageNumber, int pageSize, string? title = "", string? brand = "")
        //{
        //    return ( (from product in _context.GetDataTable("EXEC GetProducts @param1, @param2, @param3, @param4", _context.AddParameter(new string[] { pageNumber.ToString(), pageSize.ToString(), title?.ToString(), brand?.ToString() })).AsEnumerable()
        //     select new Product()
        //     {
        //         Id = product.Field<int>("Id"),
        //         ProductTitle = product.Field<string>("ProductTitle"),
        //         Code = product.Field<string>("Code"),
        //         Brand = product.Field<string>("Brand"),
        //         Price = product.Field<decimal>("Price"),
        //         Description = product.Field<string>("Description"),
        //         CreatedAt = product.Field<DateTime>("CreatedAt"),
        //         IsDeleted = product.Field<bool>("IsDeleted"),
        //     }).ToList(), 0);

        // //  return (products, 0);
        //}

        public (IEnumerable<Product> Products, int TotalCount) GetProducts(int pageNumber, int pageSize, string? title = "", string? brand = "")
        {
            var totalCountParameter = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

            var parameters = new Dictionary<string, string>
    {
        { "@param1", pageNumber.ToString() },
        { "@param2", pageSize.ToString() },
        { "@param3", title ?? string.Empty },
        { "@param4", brand ?? string.Empty }
    };

            var (productsData, totalCount) = _context.GetDataTable("EXEC GetProducts @param1, @param2, @param3, @param4, @TotalCount OUTPUT", parameters, totalCountParameter);

            var products = (from product in productsData.AsEnumerable()
                            select new Product()
                            {
                                Id = product.Field<int>("Id"),
                                ProductTitle = product.Field<string>("ProductTitle"),
                                Code = product.Field<string>("Code"),
                                Brand = product.Field<string>("Brand"),
                                Price = product.Field<decimal>("Price"),
                                Description = product.Field<string>("Description"),
                                CreatedAt = product.Field<DateTime>("CreatedAt"),
                                IsDeleted = product.Field<bool>("IsDeleted"),
                            }).ToList();

            return (products, totalCount);
        }


        //public (IEnumerable<Product> Products, int TotalCount) GetProducts(int pageNumber, int pageSize, string? title = null, string? brand = null)
        //{
        //    // Calculate the offset for pagination
        //    int offset = (pageNumber - 1) * pageSize;

        //    // Base query
        //    var query = "SELECT Id, ProductTitle, Code, Brand, Price, Description, CreatedAt, UpdatedAt, IsDeleted FROM Product WHERE 1=1";

        //    // Adding filter conditions
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        query += " AND Title LIKE @title";
        //    }

        //    if (!string.IsNullOrEmpty(brand))
        //    {
        //        query += " AND Brand LIKE @brand";
        //    }

        //    // Query to get the total count of products
        //    string countQuery = "SELECT COUNT(*) FROM Product WHERE 1=1";
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        countQuery += " AND Title LIKE @title";
        //    }

        //    if (!string.IsNullOrEmpty(brand))
        //    {
        //        countQuery += " AND Brand LIKE @brand";
        //    }

        //    // Get total count of filtered products
        //    var parameters = new List<string>();
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        parameters.Add($"%{title}%");
        //    }

        //    if (!string.IsNullOrEmpty(brand))
        //    {
        //        parameters.Add($"%{brand}%");
        //    }

        //    int totalCount = _context.GetDataTable(countQuery, _context.AddParameter(parameters.ToArray())).AsEnumerable().FirstOrDefault().Field<int>(0);

        //    // Query to get the paginated and filtered product list
        //    query += " ORDER BY CreatedAt DESC " +  // Change ordering as needed
        //             " OFFSET @param1 ROWS FETCH NEXT @param2 ROWS ONLY";

        //    // Execute the main product query
        //    var products = (from product in _context.GetDataTable(query, _context.AddParameter(new string[] { offset.ToString(), pageSize.ToString() }))
        //                   .AsEnumerable()
        //                    select new Product()
        //                    {
        //                        Id = product.Field<int>("Id"),
        //                        ProductTitle = product.Field<string>("Title"),
        //                        Code = product.Field<string>("Code"),
        //                        Brand = product.Field<string>("Brand"),
        //                        Price = product.Field<decimal>("Price"),
        //                        Description = product.Field<string>("Description"),
        //                        CreatedAt = product.Field<DateTime>("CreatedAt"),
        //                        UpdatedAt = product.Field<DateTime>("UpdatedAt"),
        //                        IsDeleted = product.Field<bool>("IsDeleted"),
        //                    }).ToList();

        //    return (products, totalCount);
        //}
    }
}
