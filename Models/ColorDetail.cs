using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
	public class ColorDetail
	{
        [Key]
        public int ColorDetailID { get; set; }
        [MaxLength(255)]
        public String Color { get; set; }
    }
}