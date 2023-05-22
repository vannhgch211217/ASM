using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testing1.Models
{
	public class Category
	{
        [Key]
        public int CategoryID { get; set; }
        public String Name { get; set; }
    }
}

