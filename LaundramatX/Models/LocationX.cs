namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocationX")]
    public partial class LocationX
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocationX()
        {
            Accounts = new HashSet<Account>();
            Accounts1 = new HashSet<Account>();
            Accounts2 = new HashSet<Account>();
            Companies = new HashSet<Company>();
            Posts = new HashSet<Post>();
        }

        [Key]
        public int LocationID { get; set; }

        [Column(TypeName = "text")]
        public string LocationLat { get; set; }

        [Column(TypeName = "text")]
        public string LocationLon { get; set; }

        [StringLength(100)]
        public string LocationProvince { get; set; }

        [StringLength(100)]
        public string LocationCountry { get; set; }

        [StringLength(100)]
        public string LocationTownCity { get; set; }

        [Column(TypeName = "text")]
        public string LocationLocalName { get; set; }

        [Column(TypeName = "text")]
        public string LocationStreetName { get; set; }

        [Column(TypeName = "text")]
        public string LocationHouseShopNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Company> Companies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
