using System;

namespace AccountingNotebook.Models
{
    public class TransactionsFilter
    {
        public Guid AccountId { get; set; }
        public SortDirection SortDirection { get; set; }
        public SortField SortField { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public TransactionsFilter() { }
    }
}
