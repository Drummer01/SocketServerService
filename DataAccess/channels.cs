namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("nltu-chat.channels")]
    public partial class channels
    {
        public int id { get; set; }

        [Required]
        [StringLength(64)]
        public string name { get; set; }

        [StringLength(64)]
        public string password { get; set; }

        public int max_users { get; set; }

        [Required]
        [StringLength(64)]
        public string title { get; set; }

        public bool is_locked { get; set; }

        public int created_at { get; set; }

        public bool hidden { get; set; }

        [StringLength(512)]
        public string thumb { get; set; }

        public int creator_id { get; set; }

        public static channels[] all()
        {
            using (DataModel ctx = new DataModel())
            {
                return ctx.channels.Where(c => c.hidden == false).ToArray();
            }
        }
    }
}
