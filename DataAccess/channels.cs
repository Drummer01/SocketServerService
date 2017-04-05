namespace DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.Validation;
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

        public const int CREATION_LIMIT = 3;
        public const int CREATION_TIME_LIMIT = 86400; //one day

        public static IEnumerable<channels> getAll()
        {
            try
            {
                using (DataModel ctx = new DataModel())
                {
                    try
                    {
                        return ctx.channels.Where(c => c.hidden == false).ToList();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static channels get(int cid)
        {
            using (DataModel ctx = new DataModel())
            {
                return ctx.channels.Where(c => c.id == cid && c.hidden == false).SingleOrDefault();
            }
        }

        public static int createNew(int creator, string name, string title, string password)
        {
            using (DataModel ctx = new DataModel())
            {
                int now = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                int count = ctx.channels.Where(c => c.creator_id == creator && now - CREATION_TIME_LIMIT >= c.created_at).Count();
                if (count > CREATION_LIMIT)
                {
                    return -1;
                }
                channels chan = new channels()
                {
                    max_users = 100,
                    creator_id = creator,
                    is_locked = (password != null),
                    name = name,
                    title = title,
                    created_at = now,
                    hidden = false,
                    thumb = string.Empty,
                    password = password?.Trim() ?? string.Empty
                };

                channel_permissions perm = new channel_permissions()
                {
                    uid = creator,
                    cid = chan.id,
                    level = (int)ChannelPermissionsLevel.Administrator

                };
                try
                {
                    ctx.channels.Add(chan);
                    ctx.channel_permissions.Add(perm);
                    ctx.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var v in e.EntityValidationErrors)
                    {
                        foreach (var item in v.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(item.PropertyName + " | " + item.ErrorMessage);
                        }
                    }
                    throw e;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e) //DbContext
                {
                    throw e;
                }

                return chan.id;
            }
        }

        public static bool remove(int cid)
        {
            using (DataModel ctx = new DataModel())
            {
                channels chan = ctx.channels.SingleOrDefault(c => c.id == cid);
                if (chan != null)
                {
                    chan.hidden = true;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static bool restore(int cid)
        {
            using (DataModel ctx = new DataModel())
            {
                channels chan = ctx.channels.SingleOrDefault(c => c.id == cid);
                if (chan != null)
                {
                    chan.hidden = false;
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static bool update(string name, string title, string password)
        {
            return true;
        }
    }
}
