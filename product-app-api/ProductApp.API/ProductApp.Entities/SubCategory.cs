using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Entities
{
    public class SubCategory: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }


        public Category Category { get; set; }
    }
}
