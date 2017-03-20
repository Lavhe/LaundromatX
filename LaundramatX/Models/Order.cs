namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int OrderID { get; set; }

        public int? PostHelperID { get; set; }

        [StringLength(100)]
        public string EventDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string Status { get; set; }

        [Required]
        [StringLength(6)]
        public string Ready { get; set; }

        public virtual PostHelper PostHelper { get; set; }
    }
}
