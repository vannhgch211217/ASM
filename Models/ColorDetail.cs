using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testing1.Models
{
	public class ColorDetail
	{
        [Key]
        public int ColorDetailID { get; set; }
        public String Color { get; set; }
    }
}