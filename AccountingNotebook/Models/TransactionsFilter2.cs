using System;

namespace AccountingNotebook.Models
{
    public class TransactionsFilter2
    {
        public Guid AccountId { get; set; } = new Guid();
        public SortDirection SortDirection { get; set; } = SortDirection.None;
        public SortField SortField { get; set; } = SortField.Date;
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public long? FromTimestamp { get; set; }
        public long? ToTimestamp { get; set; }
        public long? Timestamp { get; set; }
        public decimal? Price { get; set; }
        public decimal? FromPrice { get; set; }
        public decimal? ToPrice { get; set; }
    }
}
