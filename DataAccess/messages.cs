namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.messages")]
    public partial class messages
    {
        public int id { get; set; }

        public int uid { get; set; }

        public int cid { get; set; }

        [Required]
        [StringLength(512)]
        public string message { get; set; }

        public int time { get; set; }

        [Required]
        [StringLength(64)]
        public string type { get; set; }
    }
}
