namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostHelper")]
    public partial class PostHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostHelper()
        {
            Chats = new HashSet<Chat>();
            Orders = new HashSet<Order>();
        }

        public int ID { get; set; }

        public int? PostID { get; set; }

        public int? HelperID { get; set; }

        [Required]
        [StringLength(50)]
        public string HelperTime { get; set; }

        [Required]
        [StringLength(50)]
        public string HelperAccepted { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual Post Post { get; set; }
    }
}
