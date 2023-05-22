using System;
using System.ComponentModel.DataAnnotations;

namespace testing1.Models
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
		public Byte Role { get; set; }

		//Testing push nè
	}
}

