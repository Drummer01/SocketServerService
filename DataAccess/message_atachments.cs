namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.message_atachments")]
    public partial class message_atachments
    {
        public int id { get; set; }

        public int mid { get; set; }

        [Required]
        [StringLength(512)]
        public string link { get; set; }

        [Required]
        [StringLength(64)]
        public string type { get; set; }

        public bool hidden { get; set; }
    }
}
