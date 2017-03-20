namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Chats = new HashSet<Chat>();
            Chats1 = new HashSet<Chat>();
            Comments = new HashSet<Comment>();
            Companies = new HashSet<Company>();
            Notifications = new HashSet<Notification>();
            Posts = new HashSet<Post>();
            PostHelpers = new HashSet<PostHelper>();
            Rates = new HashSet<Rate>();
            Rates1 = new HashSet<Rate>();
        }

        public int AccountID { get; set; }

        [Required]
        [StringLength(90)]
        public string Name { get; set; }

        [Required]
        [StringLength(90)]
        public string Surname { get; set; }

        public int Contact { get; set; }

        [Required]
        [StringLength(8)]
        public string Gender { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Pass { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePic { get; set; }

        public int? Age { get; set; }

        [StringLength(100)]
        public string DateCreated { get; set; }

        public int? LocationID { get; set; }

        public int? WorkLocationID { get; set; }

        public int? CurrentLocationID { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public virtual LocationX LocationX { get; set; }

        public virtual LocationX LocationX1 { get; set; }

        public virtual LocationX LocationX2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chats { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chat> Chats1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostHelper> PostHelpers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rates1 { get; set; }
    }
}
