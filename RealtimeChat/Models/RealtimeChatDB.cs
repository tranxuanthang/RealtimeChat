namespace RealtimeChat.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RealtimeChatDB : DbContext
    {
        public RealtimeChatDB()
            : base("name=RealtimeChatDB")
        {
        }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<RoomMember> RoomMembers { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .Property(e => e.RoomName)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.RoomDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.RoomMembers)
                .WithRequired(e => e.Room)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.ChatMessages)
                .WithOptional(e => e.Room)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ChatMessages)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.SenderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ChatMessages1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.ToUserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RoomMembers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Rooms)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatorID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Users1)
                .WithMany(e => e.Users)
                .Map(m => m.ToTable("PrivateMessages").MapLeftKey("SenderID").MapRightKey("ToUserID"));
        }
    }
}
