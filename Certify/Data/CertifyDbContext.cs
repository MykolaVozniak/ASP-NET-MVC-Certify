using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Data.Models;
namespace Certify.Data
{
    public class CertifyDbContext : IdentityDbContext
    {
        public CertifyDbContext(DbContextOptions<CertifyDbContext> options)
            : base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Signature> Signatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.User)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<Signature>()
                .HasOne(s => s.User)
                .WithMany(u => u.Signatures)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Signature>()
                .HasOne(s => s.Document)
                .WithMany(d => d.Signatures)
                .HasForeignKey(s => s.DocumentId)
                .OnDelete(DeleteBehavior.NoAction);





        }
    }
}