using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
	public class Product
	{
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "User")]
        public virtual int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        [Display(Name = "Category")]
        public virtual int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [Display(Name = "Size")]
        public virtual int SizeID { get; set; }
        [ForeignKey("SizeID")]
        public Size Size { get; set; }

        [Display(Name = "ColorDetail")]
        public virtual int ColorDetailID { get; set; }
        [ForeignKey("ColorDetailID")]
        public ColorDetail ColorDetail { get; set; }

        public String ProductName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public String Description { get; set; }
        public String Image { get; set; }
    }
}

