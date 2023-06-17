using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
	public class OrderDetail
	{
        [Key]
        public int OrderDetailID { get; set; }

        [Display(Name = "Order")]
        public virtual int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order { get; set; }

        [Display(Name = "Product")]
        public virtual int? ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public float UnitPrice { get; set; }
    }
}

