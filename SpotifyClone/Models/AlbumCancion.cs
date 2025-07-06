namespace SpotifyClone.Models
{
    public class AlbumCancion
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int CancionId { get; set; }
        public Cancion Cancion { get; set; }
    }
}
