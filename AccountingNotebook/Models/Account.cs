using AccountingNotebook.Service.TransactionService;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingNotebook.Models
{
    public class Account
    {
        [Required]
        public Guid AccountId { get; set; } = new Guid();

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "Account's name";

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Balance { get; set; } = 0;
    }
}
