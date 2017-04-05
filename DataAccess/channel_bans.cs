namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

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

        public int time { get; set; }

        public int expire { get; set; }

        public bool active { get; set; }

        public static bool isBanned(int uid, int cid)
        {
            using (DataModel ctx = new DataModel())
            {
                int now = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                return ctx.channel_bans.Any(b => b.uid == uid && b.cid == cid && ( b.expire > now || b.expire == 0 ) && b.active == true );
            }
        }

        public static void saveBan(int cid, int uid, int aid, string reason, int expire)
        {
            using (DataModel ctx = new DataModel())
            {
                channel_bans ban = new channel_bans();
                ban.cid = cid;
                ban.uid = uid;
                ban.aid = aid;
                ban.expire = expire;
                ban.reason = reason;
                ban.time = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                ban.active = true;

                ctx.channel_bans.Add(ban);
                ctx.SaveChanges();
            }
        }

        public static void removeBan(int cid, int uid)
        {
            using (DataModel ctx = new DataModel())
            {
                channel_bans ban = ctx.channel_bans.Where(b => b.cid == cid && b.uid == uid && b.active == true).First();
                ban.active = false;

                ctx.SaveChanges();
            }
        }
    }
}
