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
            base.OnModelCreating(modelBuilder);

            ConfigurarLivro(modelBuilder);
            ConfigurarUsuario(modelBuilder);
            ConfigurarEmprestimo(modelBuilder);
        }

        private static void ConfigurarLivro(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("Livros");

                entity.HasKey(l => l.Id);

                entity.Property(l => l.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(l => l.Titulo)
                    .IsRequired()
                    .HasColumnType("nvarchar(150)")
                    .HasMaxLength(Livro.TituloMaxLength);

                entity.Property(l => l.Autor)
                    .IsRequired()
                    .HasColumnType("nvarchar(120)")
                    .HasMaxLength(Livro.AutorMaxLength);

                entity.Property(l => l.AnoPublicacao)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(l => l.Disponivel)
                    .IsRequired()
                    .HasColumnType("bit");

                entity.HasIndex(l => l.Titulo)
                    .HasDatabaseName("IX_Livros_Titulo");

                entity.HasIndex(l => l.Autor)
                    .HasDatabaseName("IX_Livros_Autor");
            });
        }

        private static void ConfigurarUsuario(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.Nome)
                    .IsRequired()
                    .HasColumnType("nvarchar(100)")
                    .HasMaxLength(Usuario.NomeMaxLength);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasColumnType("nvarchar(254)")
                    .HasMaxLength(Usuario.EmailMaxLength);

                entity.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuarios_Email");
            });
        }

        private static void ConfigurarEmprestimo(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.ToTable("Emprestimos");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.LivroId)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(e => e.UsuarioId)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(e => e.DataEmprestimo)
                    .IsRequired()
                    .HasColumnType("datetime2");

                entity.Property(e => e.DataPrevistaDevolucao)
                    .IsRequired()
                    .HasColumnType("datetime2");

                entity.Property(e => e.DataDevolucao)
                    .HasColumnType("datetime2");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<int>()
                    .HasColumnType("int");

                entity.HasOne(e => e.Livro)
                    .WithMany()
                    .HasForeignKey(e => e.LivroId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.LivroId)
                    .HasDatabaseName("IX_Emprestimos_LivroId");

                entity.HasIndex(e => e.UsuarioId)
                    .HasDatabaseName("IX_Emprestimos_UsuarioId");

                entity.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_Emprestimos_Status");

                entity.HasIndex(e => e.DataPrevistaDevolucao)
                    .HasDatabaseName("IX_Emprestimos_DataPrevistaDevolucao");
            });
        }
    }
}