namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.filter_subscribers")]
    public partial class filter_subscribers
    {
        public int id { get; set; }

        public int uid { get; set; }

        public int fid { get; set; }

        public bool active { get; set; }
    }
}
