using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Entities
{
    public  class Product: BaseEntity
    {
        [Required]
        public string ProductTitle { get;set; }

        [Required]
        public string Code { get; set; }
        [Required]
        public string Brand { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public long SubCategoryId {  get; set; }    

    }
}
