namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("nltu-chat.channel_permissions")]
    public partial class channel_permissions
    {
        public int id { get; set; }

        public int uid { get; set; }

        public int cid { get; set; }

        public int level { get; set; }

        public static bool isAllowed(int uid, int cid, ChannelPermissionsLevel requiredPermission)
        {
            using (DataModel ctx = new DataModel())
            {
                System.Diagnostics.Debug.WriteLine(uid);
                System.Diagnostics.Debug.WriteLine(cid);
                return ctx.channel_permissions.Any(p => p.uid == uid && ( p.cid == cid || p.cid == 0 ) && p.level >= (int)requiredPermission );
            }
        }
    }
}
