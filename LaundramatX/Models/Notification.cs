namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notification")]
    public partial class Notification
    {
        [Key]
        public int NotifyID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string From { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        public int? RefID { get; set; }

        [StringLength(50)]
        public string RefName { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Message { get; set; }

        [Required]
        [StringLength(10)]
        public string Seen { get; set; }

        [Required]
        [StringLength(100)]
        public string NotificationTime { get; set; }

        public virtual Account Account { get; set; }
    }
}
