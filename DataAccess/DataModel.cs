namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<channel_bans> channel_bans { get; set; }
        public virtual DbSet<channel_permissions> channel_permissions { get; set; }
        public virtual DbSet<channels> channels { get; set; }
        public virtual DbSet<filter_mapping> filter_mapping { get; set; }
        public virtual DbSet<filter_subscribers> filter_subscribers { get; set; }
        public virtual DbSet<filters> filters { get; set; }
        public virtual DbSet<message_atachments> message_atachments { get; set; }
        public virtual DbSet<messages> messages { get; set; }
        public virtual DbSet<participants> participants { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<channel_bans>()
                .Property(e => e.reason)
                .IsUnicode(false);

            modelBuilder.Entity<channels>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<channels>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<channels>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<channels>()
                .Property(e => e.thumb)
                .IsUnicode(false);

            modelBuilder.Entity<filter_mapping>()
                .Property(e => e.original)
                .IsUnicode(false);

            modelBuilder.Entity<filter_mapping>()
                .Property(e => e.replacement)
                .IsUnicode(false);

            modelBuilder.Entity<filters>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<filters>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<message_atachments>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<message_atachments>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<messages>()
                .Property(e => e.message)
                .IsUnicode(false);

            modelBuilder.Entity<messages>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.hash)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.avatar)
                .IsUnicode(false);
        }
    }
}
