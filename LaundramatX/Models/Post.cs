namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Addons = new HashSet<Addon>();
            Clothings = new HashSet<Clothing>();
            Comments = new HashSet<Comment>();
            PostHelpers = new HashSet<PostHelper>();
        }

        public int PostID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string PostMessage { get; set; }

        [Required]
        [StringLength(50)]
        public string PostTime { get; set; }

        [Required]
        [StringLength(50)]
        public string PostDue { get; set; }

        public double PostPrice { get; set; }

        [Column(TypeName = "text")]
        public string Status { get; set; }

        public int? LocationID { get; set; }

        public int? PostViews { get; set; }

        public int? PostType { get; set; }

        public int? UserID { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Addon> Addons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Clothing> Clothings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual LocationX LocationX { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostHelper> PostHelpers { get; set; }
    }
}
