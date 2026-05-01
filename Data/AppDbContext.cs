using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PDFsignture.Models;

namespace PDFsignture.Data
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships if needed
            // 1. SignedDocument -> Document Relationship
            builder.Entity<SignedDocument>()
                .HasOne(sd => sd.Document)
                .WithMany(d => d.SignedDocuments)
                .HasForeignKey(sd => sd.DocumentId)
                .OnDelete(DeleteBehavior.Cascade); // If original document is deleted, delete its signed history

            // 2. SignedDocument -> Signature Relationship
            builder.Entity<SignedDocument>()
                .HasOne(sd => sd.Signature)
                .WithMany(s => s.SignedDocuments)
                .HasForeignKey(sd => sd.SignatureId)
                .OnDelete(DeleteBehavior.Restrict); // Important: Don't delete signature if it was used in a PDF

            // 3. Document -> IdentityUser (Shadow Property or Manual)
            // Assuming we use the string UserId from Identity
            builder.Entity<Document>()
                .HasIndex(d => d.UserId);

            // 4. Signature -> IdentityUser 
            builder.Entity<Signature>()
                .HasIndex(s => s.UserId);
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Signature> Signatures { get; set; }
        public DbSet<SignedDocument> SignedDocuments { get; set; }
    }
}
