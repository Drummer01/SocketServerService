namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.users")]
    public partial class users
    {
        public int id { get; set; }

        [Required]
        [StringLength(64)]
        public string name { get; set; }

        [Required]
        [StringLength(124)]
        public string password { get; set; }

        [Required]
        [StringLength(124)]
        public string hash { get; set; }

        [Required]
        [StringLength(256)]
        public string avatar { get; set; }

        public int lastonline { get; set; }
    }
}
