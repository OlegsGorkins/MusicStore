using MusicStore.Models;

namespace MusicStore.ViewModels
{
    public class AlbumTrackViewModel
    {
        public Album Album { get; set; }
        public Track Track { get; set; }

        public AlbumTrackViewModel(Album album, Track track)
        {
            this.Album = album;
            this.Track = track;
        }
    }
}
