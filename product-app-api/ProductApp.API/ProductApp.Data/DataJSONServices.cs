using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReadWriteFramework.Interfaces
{
    public abstract class DataJSONServices : IDataJSONServices
    {
        public virtual string DataSetToJSON(DataSet dataset)
        {
            throw new NotImplementedException();
        }
        public virtual string DataTableToJSON(DataTable dataTable)
        {
            //DataTable dt = (DataTable)dsData.Tables[0];
            // JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            //  jsSerializer.MaxJsonLength = 500000000;

            //  List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            // Dictionary<string, object> childRow;
            // foreach (DataRow row in dataTable.Rows)
            // {
            //     childRow = new Dictionary<string, object>();
            //  foreach (DataColumn col in dataTable.Columns)
            //     {
            //       childRow.Add(col.ColumnName, row[col]);
            //     }
            //   parentRow.Add(childRow);
            // }

            //dataTable.Dispose();
            return "";// jsSerializer.Serialize(parentRow);
        }
    }
}
