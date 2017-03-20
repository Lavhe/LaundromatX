namespace LaundramatX.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chat")]
    public partial class Chat
    {
        public int ChatID { get; set; }

        public int HelpID { get; set; }

        public int SenderID { get; set; }

        public int ReceiverID { get; set; }

        [Column(TypeName = "text")]
        public string Message { get; set; }

        [Required]
        [StringLength(100)]
        public string SendTime { get; set; }

        [Required]
        [StringLength(6)]
        public string Seen { get; set; }

        public virtual Account Account { get; set; }

        public virtual Account Account1 { get; set; }

        public virtual PostHelper PostHelper { get; set; }
    }
}
