namespace AccountingNotebook.Models
{
    public class TransactionsFilter
    {
        public SortDirection SortDirection { get; set; }
        public SortField SortField { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public TransactionsFilter() { }

        public TransactionsFilter(
            SortDirection direction,
            SortField field,
            int pageSize,
            int pageNumber)
        {
            this.SortDirection = direction;
            this.SortField = field;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
