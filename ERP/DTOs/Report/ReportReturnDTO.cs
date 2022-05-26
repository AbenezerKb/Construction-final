using System.Collections.Generic;

namespace ERP.DTOs.Report
{
    public class ReportReturnDTO<T>
    {

        public IEnumerable<T> Data { get; set; }

        public IQueryable Summary { get; set; }
    }
}
