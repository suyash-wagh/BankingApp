using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingApp.Models
{
    public class TransactionViewModel
    {
        public string TransactionType { get; set; }

        [DataType(DataType.Currency)]
        [Required( ErrorMessage ="Please enter Amount")]
        [Range(500, 100000, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double Amount { get; set; }

    }
}