using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Web.Data
{
    public class BibliotecaDbContext : DbContext
    {
        public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Livro> Livros => Set<Livro>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamentos principais
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Livro)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Usuario)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}