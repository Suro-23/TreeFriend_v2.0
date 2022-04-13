using Microsoft.EntityFrameworkCore;
using TreeFriend.Models.Entity;

namespace TreeFriend.Models {
    public class TreeFriendDbContext: DbContext {
        public TreeFriendDbContext(DbContextOptions<TreeFriendDbContext> options) : base(options) {

        }

        //思雯&凱琳的登入頁面
        public DbSet<User> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Hashtag> hashtags { get; set; }
        public DbSet<UserDetail> usersDetail { get; set; }
        public DbSet<HashtagDetail> hashtagDetails { get; set; }
        public DbSet<SkillPost> skillPosts { get; set; }
        
        public DbSet<SkillPostMessage> skillPostMessages{ get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //指定日期使用SQL getdate() 自動取得當前時間
            modelBuilder.Entity<User>()
                .Property(u => u.CreateDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Product>()
                .Property(p => p.StarDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<UserDetail>()
                .Property(u => u.UpdateTime)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SkillPost>()
                .Property(p => p.CreateDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<HashtagDetail>()
                .HasKey(d => new { d.HashtagId, d.SkillPostId });
            modelBuilder.Entity<User>()
                .HasMany(u => u.SkillPosts)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SkillPost>()
                .HasMany(s => s.Hashtags)
                .WithOne(s => s.SkillPost)
                .HasForeignKey(s => s.SkillPostId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Hashtag>()
                .HasMany(h => h.Hashtags)
                .WithOne(h => h.Hashtag)
                .HasForeignKey(h => h.HashtagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>(bulider => {
                bulider.HasKey(x => new { x.UserId, x.LectureId });
                bulider.HasOne(x => x.User).WithMany(x => x.OrderDetails).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                bulider.HasOne(x => x.Lecture).WithMany(x => x.OrderDetails).HasForeignKey(x => x.LectureId).OnDelete(DeleteBehavior.NoAction);
            });
            modelBuilder.Entity<Lecture>().HasOne(x => x.User).WithMany(x => x.Lectures).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<PersonalPost>()
            //    .Property(p => p.CreateDate)
            //    .HasDefaultValueSql("getdate()");

            //modelBuilder.Entity <PersonalPostMessage>()
            //    .Property(p => p.CreateDate)
            //    .HasDefaultValueSql("getdate()");
        }
    }
}
