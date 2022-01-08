using System;

namespace ApplicationHealth.Domain.DataTable.Base
{
    public abstract class BaseDataTable
    {
        public string draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public DateTime SystemDateTime { get; set; } = DateTime.Now;
    }
}
