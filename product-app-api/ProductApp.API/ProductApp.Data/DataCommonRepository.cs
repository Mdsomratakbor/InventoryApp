using DataReadWriteFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReadWriteFramework
{
    public  class DataCommonRepository : DataRepository,  IDataCommonRepository
    {
        public DataCommonRepository(string connectionString) : base(connectionString)
        {

        }
    }
}
