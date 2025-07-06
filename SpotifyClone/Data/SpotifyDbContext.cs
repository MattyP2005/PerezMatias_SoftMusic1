using SpotifyClone.Models;
using Microsoft.EntityFrameworkCore;

namespace SpotifyClone.Data
{
    public class SpotifyDbContext : DbContext
    {
        public SpotifyDbContext(DbContextOptions<SpotifyDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cancion> Canciones { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<PlaylistCancion> PlaylistCanciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Historial → Usuario y Canción
            modelBuilder.Entity<Historial>()
                .HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Historial>()
                .HasOne(h => h.Cancion)
                .WithMany()
                .HasForeignKey(h => h.CancionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cancion → Usuario
            modelBuilder.Entity<Cancion>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // PlaylistCancion (composite key)
            modelBuilder.Entity<PlaylistCancion>()
                .HasKey(pc => new { pc.PlaylistId, pc.CancionId });

            modelBuilder.Entity<PlaylistCancion>()
                .HasOne(pc => pc.Playlist)
                .WithMany(p => p.Canciones)
                .HasForeignKey(pc => pc.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlaylistCancion>()
                .HasOne(pc => pc.Cancion)
                .WithMany()
                .HasForeignKey(pc => pc.CancionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Playlist → Usuario
            modelBuilder.Entity<Playlist>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // FormaPago
            modelBuilder.Entity<FormaPago>()
                .HasOne(fp => fp.Usuario)
                .WithMany()
                .HasForeignKey(fp => fp.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Álbum → Artista (usuario)
            modelBuilder.Entity<Album>()
                .HasOne(a => a.Artista)
                .WithMany()
                .HasForeignKey(a => a.ArtistaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cancion → Álbum
            modelBuilder.Entity<AlbumCancion>()
                .HasKey(ac => new { ac.AlbumId, ac.CancionId });

            modelBuilder.Entity<AlbumCancion>()
                .HasOne(ac => ac.Album)
                .WithMany(a => a.AlbumCanciones)
                .HasForeignKey(ac => ac.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AlbumCancion>()
                .HasOne(ac => ac.Cancion)
                .WithMany(c => c.AlbumCanciones)
                .HasForeignKey(ac => ac.CancionId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Historial> Historiales { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<FormaPago> FormasPago { get; set; }

        public DbSet<Artista> Artistas { get; set; }

        public DbSet<AlbumCancion> AlbumCanciones { get; set; }
    }
}
