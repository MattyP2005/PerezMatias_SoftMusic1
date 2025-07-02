using SpotifyClone.Models;
using System.Collections.Generic;

namespace SpotifyClone.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Album> Albums { get; set; } 

        public List<Playlist> PlaylistsPublicas { get; set; } 

        public List<Cancion> CancionesSueltas { get; set; } 
    }
}
