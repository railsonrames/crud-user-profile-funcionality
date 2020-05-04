using Microsoft.EntityFrameworkCore;

namespace CrudUserProfileFuncionality.Models
{
    public partial class DataAppContext : DbContext
    {
        public DataAppContext()
        {
        }

        public DataAppContext(DbContextOptions<DataAppContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=local.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<Perfil>().HasKey(x => x.Id);
            modelBuilder.Entity<Funcionalidade>().HasKey(x => x.Id);

            modelBuilder.Entity<PerfilFuncionalidade>().HasKey(x => new { x.PerfilId, x.FuncionalidadeId });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Funcionalidade> Funcionalidades { get; set; }
        public DbSet<PerfilFuncionalidade> PerfilFuncionalidade { get; set; }
    }
}
