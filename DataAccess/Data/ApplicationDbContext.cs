 using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using CodingWiki_Model.Models;

namespace DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        //public DbSet<Genere> Generes { get; set; }
        public DbSet<BookDetail> BookDetails{ get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookAuthorMap> BookAuthorMaps { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("Publishers");

                // Primary key
                entity.HasKey(p => p.Publisher_Id);
                entity.Property(p => p.Publisher_Id)
                      .ValueGeneratedOnAdd();

                // Required name
                entity.Property(p => p.Name)
                      .IsRequired();

                // Optional: limit Location length if desired
                // entity.Property(p => p.Location).HasMaxLength(200);

                // Relationship: Publisher 1 - * Books
                entity.HasMany(p => p.Books)
                      .WithOne(b => b.Publisher)
                      .HasForeignKey(b => b.Publisher_Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BookDetail>(entity =>
            {
                entity.ToTable("BookDetails");

                // Primary key
                entity.HasKey(bd => bd.BookDetail_Id);
                entity.Property(bd => bd.BookDetail_Id)
                      .ValueGeneratedOnAdd();

                // Optional: configure column types/lengths here if needed
                // entity.Property(bd => bd.Weight).HasPrecision(8, 2);
            });

            // Configure one-to-one relationship: Book 1 - 1 BookDetail
            
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");

                // Primary key
                entity.HasKey(b => b.BookId);
                entity.Property(b => b.BookId).ValueGeneratedOnAdd();

                // ISBN: required + max length 20
                entity.Property(b => b.ISBN)
                      .IsRequired()
                      .HasMaxLength(20);

                // Optional: Title is left as a non-nullable string, remove .IsRequired() if you want it optional
                entity.Property(b => b.Title)
                      .IsRequired(false);

                // Ignore computed/not-mapped property
                entity.Ignore(b => b.PriceRange);

                // One-to-one with BookDetail (BookDetail holds FK)
                entity.HasOne(b => b.BookDetail)
                      .WithOne(d => d.Book)
                      .HasForeignKey<BookDetail>(d => d.BookId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Navigation collection to join table (configured on join side as well)
                entity.HasMany(b => b.BookAuthorMap)
                      .WithOne(bam => bam.Book)
                      .HasForeignKey(bam => bam.Book_Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // existing commented/other configurations (preserved)
            //modelBuilder.Entity<Book>().Property(u => u.Price).HasPrecision(10, 5);

            // BookAuthorMap join entity configuration (composite PK, relationships)
            modelBuilder.Entity<BookAuthorMap>(entity =>
            {
                entity.ToTable("BookAuthorMaps");

                // Composite primary key
                entity.HasKey(bam => new { bam.Book_Id, bam.Author_Id });

                // Keys are provided by the application (no identity)
                entity.Property(bam => bam.Book_Id).ValueGeneratedNever();
                entity.Property(bam => bam.Author_Id).ValueGeneratedNever();

                // Relationship to Book
                entity.HasOne(bam => bam.Book)
                      .WithMany(b => b.BookAuthorMap)
                      .HasForeignKey(bam => bam.Book_Id)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relationship to Author
                entity.HasOne(bam => bam.Author)
                      .WithMany(a => a.BookAuthorMap)
                      .HasForeignKey(bam => bam.Author_Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Authors");

                // Primary key
                entity.HasKey(a => a.Author_Id);
                entity.Property(a => a.Author_Id).ValueGeneratedOnAdd();

                // Property rules
                entity.Property(a => a.FirstName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(a => a.LastName)
                      .IsRequired();

                // Ignore computed property
                entity.Ignore(a => a.FullName);

                // Relationship: Author 1 - * BookAuthorMap
                entity.HasMany(a => a.BookAuthorMap)
                      .WithOne(bam => bam.Author)
                      .HasForeignKey(bam => bam.Author_Id)
                      .OnDelete(DeleteBehavior.Cascade);
            });



        }
    }
}
