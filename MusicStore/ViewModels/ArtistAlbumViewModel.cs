using MusicStore.Models;

namespace MusicStore.ViewModels
{
    public class ArtistAlbumViewModel
    {
        public Artist Artist { get; set; }
        public Album Album { get; set; }

        public ArtistAlbumViewModel(Artist artist, Album album)
        {
            this.Artist = artist;
            this.Album = album;
        }
    }
}
