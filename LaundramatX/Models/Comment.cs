namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public int CommentID { get; set; }

        public int? PostID { get; set; }

        public int? UserID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string CommentMessage { get; set; }

        [Required]
        [StringLength(50)]
        public string CommentTime { get; set; }

        public virtual Account Account { get; set; }

        public virtual Post Post { get; set; }
    }
}
