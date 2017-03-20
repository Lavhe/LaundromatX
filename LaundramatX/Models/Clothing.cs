namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Clothing")]
    public partial class Clothing
    {
        public int ID { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public int? PostID { get; set; }

        public virtual Post Post { get; set; }
    }
}
