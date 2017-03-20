namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Company")]
    public partial class Company
    {
        public int CompanyID { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string CompanyTell { get; set; }

        [StringLength(100)]
        public string CompanyWebsite { get; set; }

        [Column(TypeName = "text")]
        public string CompanyAbout { get; set; }

        public int OwnerID { get; set; }

        public int? LocationID { get; set; }

        [Column(TypeName = "text")]
        public string Image1 { get; set; }

        [Column(TypeName = "text")]
        public string Image2 { get; set; }

        [Column(TypeName = "text")]
        public string Image3 { get; set; }

        public virtual Account Account { get; set; }

        public virtual LocationX LocationX { get; set; }
    }
}
