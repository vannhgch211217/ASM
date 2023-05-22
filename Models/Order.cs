using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testing1.Models
{
	public class Order
	{
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "Customer")]
        public virtual int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public float? TotalPrice { get; set; }
        public String Status { get; set; }
    }
}

