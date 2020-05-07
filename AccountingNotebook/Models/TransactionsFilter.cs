using AccountingNotebook.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace AccountingNotebook.Models
{
    public class TransactionsFilter
    {
        public Guid AccountId { get; set; } = new Guid();
        public SortDirection SortDirection { get; set; } = SortDirection.Standart;
        public SortField SortField { get; set; } = SortField.Date;
        public AmountParams AmountParams { get; set; } = AmountParams.Two;
        public FilterOption FilterOption { get; set; } = FilterOption.Sorting;
        public CompareDirection CompareDirection { get; set; } = CompareDirection.LessThen;
        public int PageSize { get; set; } = 1;
        public int PageNumber { get; set; } = 1;
        public DateTime BotDate { get; set; } = new DateTime(0,0,0,0,0,0);
        public DateTime TopDate { get; set; } = DateTime.Now;
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Price { get; set; } = 1000M;
        public decimal PriceBot { get; set; } = 0M;
        public decimal PriceTop { get; set; } = 1000M;
    }
}
