using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace testing1.Models
{
	public class Customer
	{
        [Key]
        public int CustomerID { get; set; }

        [Display(Name = "User")]
        public virtual int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public String Name { get; set; }

        [MaxLength(11), RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid phone number format.")]
        public String PhoneNumber { get; set; }
        public String Address { get; set; }
    }
}

