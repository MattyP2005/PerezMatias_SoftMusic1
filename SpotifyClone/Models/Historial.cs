namespace SpotifyClone.Models
{
    public class Historial
    {
        public int Id { get; set; }


        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public int CancionId { get; set; }

        public Cancion Cancion { get; set; }

        public DateTime FechaHora { get; set; }
    }
}
