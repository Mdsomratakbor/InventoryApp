using DataReadWriteFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReadWriteFramework
{
    public class QueryPattern : IQueryPattern
    {
        public string Query { get; set; }
        public List<Dictionary<string, string>> Parameters { get; set; } = new List<Dictionary<string, string>>();
    }
}
