namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Addon")]
    public partial class Addon
    {
        public int AddonID { get; set; }

        [Column("Addon", TypeName = "text")]
        [Required]
        public string Addon1 { get; set; }

        [Required]
        [StringLength(6)]
        public string isPostive { get; set; }

        public double AddonPrice { get; set; }

        public int PostID { get; set; }

        public virtual Post Post { get; set; }
    }
}
