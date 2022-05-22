using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankingApp.Models
{
    public class User
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Initial Balance is required to register.")]
        public double Balance { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}