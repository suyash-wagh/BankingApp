using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankingApp.Models
{
    public class Transaction
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }

        public Transaction(User user, double amount, string ttype)
        {
            Id = user.Id;
            Name = user.Name;
            Created = DateTime.Now;
            Amount = amount;
            TransactionType = ttype;
        }

        public Transaction()
        {
        }
    }
}