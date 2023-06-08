using System;
using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
	public class User
	{
		[Key]
		public int UserID { get; set; }
		public String Email { get; set; }
		
		[MinLength(8)]
		public String Password { get; set; }

		/*
		 * Byte 0 (Admin), 1 (Customer), 2 (Supplier)
		 */ 
		public String Role { get; set; }

		public String Name { get; set; }

        [MaxLength(13), RegularExpression(@"^\d{4}\d{3}\d{3}$", ErrorMessage = "Invalid phone number format.")]
        public String PhoneNumber { get; set; }

        public String? Address { get; set; }
    }
}

