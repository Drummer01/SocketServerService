namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.channel_permissions")]
    public partial class channel_permissions
    {
        public int id { get; set; }

        public int uid { get; set; }

        public int cid { get; set; }

        public int level { get; set; }
    }
}
