using AccountingNotebook.Service.TransactionService;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingNotebook.Models
{
    public class Account
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal Balance { get; set; }
    }    
}
