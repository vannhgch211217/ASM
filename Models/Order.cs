using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM.Models
{
	public class Order
	{
        [Key]
        public int OrderID { get; set; }

        [Display(Name = "User")]
        public virtual int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public DateTime OrderDate { get; set; }

        public float? TotalPrice { get; set; }
        public String Status { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}

