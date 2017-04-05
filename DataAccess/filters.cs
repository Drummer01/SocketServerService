namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.filters")]
    public partial class filters
    {
        public int id { get; set; }

        [Required]
        [StringLength(64)]
        public string name { get; set; }

        [Required]
        [StringLength(512)]
        public string description { get; set; }
    }
}
