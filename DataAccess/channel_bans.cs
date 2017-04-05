namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.channel_bans")]
    public partial class channel_bans
    {
        public int id { get; set; }

        public int cid { get; set; }

        public int uid { get; set; }

        public int aid { get; set; }

        [Required]
        [StringLength(128)]
        public string reason { get; set; }

        public int expire { get; set; }

        public int time { get; set; }

        public bool active { get; set; }
    }
}
