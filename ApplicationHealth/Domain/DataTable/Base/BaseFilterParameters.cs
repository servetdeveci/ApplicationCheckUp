namespace ApplicationHealth.Domain.DataTable.Base
{
    public class BaseFilterParameters
    {
        public int start { get; set; }
        public int length { get; set; }
        public string draw { get; set; }
        public string sortColumnName { get; set; }
        public string sortColumnDirection { get; set; }
        public string mainFilter { get; set; }
    }
}
