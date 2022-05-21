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
        public double Amount { get; set; }

    }
}