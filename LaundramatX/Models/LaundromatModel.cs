namespace LaundramatX.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LaundromatModel : DbContext
    {
        public LaundromatModel()
            : base("name=LaundromatModel")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Addon> Addons { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Clothing> Clothings { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<LocationX> LocationXes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostHelper> PostHelpers { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Pass)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.ProfilePic)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.DateCreated)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Chats)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.ReceiverID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Chats1)
                .WithRequired(e => e.Account1)
                .HasForeignKey(e => e.SenderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Comments)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Companies)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Notifications)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Posts)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.UserID);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.PostHelpers)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.HelperID);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Rates)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.RaterID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Rates1)
                .WithRequired(e => e.Account1)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Addon>()
                .Property(e => e.Addon1)
                .IsUnicode(false);

            modelBuilder.Entity<Addon>()
                .Property(e => e.isPostive)
                .IsUnicode(false);

            modelBuilder.Entity<Chat>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Chat>()
                .Property(e => e.SendTime)
                .IsUnicode(false);

            modelBuilder.Entity<Chat>()
                .Property(e => e.Seen)
                .IsUnicode(false);

            modelBuilder.Entity<Clothing>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.CommentMessage)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.CommentTime)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.CompanyTell)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.CompanyWebsite)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.CompanyAbout)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.Image1)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.Image2)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.Image3)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationLat)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationLon)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationProvince)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationCountry)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationTownCity)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationLocalName)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationStreetName)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .Property(e => e.LocationHouseShopNumber)
                .IsUnicode(false);

            modelBuilder.Entity<LocationX>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.LocationX)
                .HasForeignKey(e => e.CurrentLocationID);

            modelBuilder.Entity<LocationX>()
                .HasMany(e => e.Accounts1)
                .WithOptional(e => e.LocationX1)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<LocationX>()
                .HasMany(e => e.Accounts2)
                .WithOptional(e => e.LocationX2)
                .HasForeignKey(e => e.WorkLocationID);

            modelBuilder.Entity<Notification>()
                .Property(e => e.From)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.RefName)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Seen)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.NotificationTime)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.EventDateTime)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Ready)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.PostMessage)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.PostTime)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.PostDue)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Addons)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostHelper>()
                .Property(e => e.HelperTime)
                .IsUnicode(false);

            modelBuilder.Entity<PostHelper>()
                .Property(e => e.HelperAccepted)
                .IsUnicode(false);

            modelBuilder.Entity<PostHelper>()
                .HasMany(e => e.Chats)
                .WithRequired(e => e.PostHelper)
                .HasForeignKey(e => e.HelpID)
                .WillCascadeOnDelete(false);
        }
    }
}
