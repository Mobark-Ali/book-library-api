using BookLibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryApi.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasIndex(b => b.ISBN)
            .IsUnique()
            .HasFilter("[ISBN] IS NOT NULL");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
