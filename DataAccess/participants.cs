namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nltu-chat.participants")]
    public partial class participants
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public int chan_id { get; set; }

        public bool active { get; set; }
    }
}
