using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YTCG_Deck_Builder_API.Models.Entitities;

namespace YTCG_Deck_Builder_API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<PostRating> PostRatings { get; set; }
        public DbSet<ReplyRating> ReplyRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Deck>()
            //    .HasOne(d => d.User)
            //    .WithMany(u => u.Decks)
            //    .HasForeignKey(d => d.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Card>()
            //    .HasOne(c => c.User)
            //    .WithMany(u => u.Cards)
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Deck)
                .WithMany(d => d.Cards)
                .HasForeignKey(c => c.DeckId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PostRating>()
                .HasOne(pr => pr.Post)
                .WithMany(p => p.PostRatings)
                .HasForeignKey(pr => pr.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReplyRating>()
                .HasOne(rr => rr.Reply)
                .WithMany(r => r.ReplyRatings)
                .HasForeignKey(rr => rr.ReplyId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Post>()
            //    .HasOne(p => p.User)
            //    .WithMany(u => u.Posts)
            //    .HasForeignKey(p => p.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Reply>()
            //    .HasOne(r => r.User)
            //    .WithMany(u => u.Replies)
            //    .HasForeignKey(r => r.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
