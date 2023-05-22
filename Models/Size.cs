using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testing1.Models
{
	public class Size
	{
        [Key]
        public int SizeID { get; set; }
        public int SizeNumber { get; set; }
    }
}

