using System;
using System.Linq;
using MusicStore.Models;

namespace MusicStore.Data
{
    public static class SeedData
    {
        private static void ArtistEnsureListed(MusicStoreContext context, Artist artist)
        {
            var artist_listed = context.Artists.FirstOrDefault(a => a.Name == artist.Name);

            if (artist_listed != null)
                artist.ID = artist_listed.ID;
            else
                context.Artists.Add(artist);
        }

        private static void AlbumEnsureListed(MusicStoreContext context, Album album)
        {
            var album_listed = context.Albums.FirstOrDefault(a => a.ArtistID == album.ArtistID && a.Title == album.Title);

            if (album_listed != null)
            {
                album.ID = album_listed.ID;
                album.ArtistID = album_listed.ArtistID;
            }
            else
                context.Albums.Add(album);
        }

        private static void TrackEnsureListed(MusicStoreContext context, Track track)
        {
            var track_listed = context.Tracks.FirstOrDefault(t => t.AlbumID == track.AlbumID && t.Title == track.Title);

            if (track_listed != null)
            {
                track.ID = track_listed.ID;
                track.AlbumID = track_listed.AlbumID;
            }
            else
                context.Tracks.Add(track);
        }

        public static void SeedDatabase(MusicStoreContext context)
        {
            var artists = new Artist[]
            {
                new Artist{Name = "Johann Sebastian Bach"}
            };

            foreach (Artist item in artists)
                ArtistEnsureListed(context, item);

            context.SaveChanges();
            
            var albums = new Album[]
            {
                new Album{ArtistID = artists[0].ID, Title = "Partita for solo violin No. 1 in B minor", ReleaseDate = new DateTime(2018, 3, 20)},
                new Album{ArtistID = artists[0].ID, Title = "Partita for solo violin No. 2 in D minor", ReleaseDate = new DateTime(2018, 3, 20)},
                new Album{ArtistID = artists[0].ID, Title = "Partita for solo violin No. 3 in E major", ReleaseDate = new DateTime(2018, 3, 20)}
            };

            foreach (Album item in albums)
                AlbumEnsureListed(context, item);

            context.SaveChanges();

            var tracks = new Track[]
            {
                new Track{AlbumID = albums[0].ID, Title = "Allemanda"},
                new Track{AlbumID = albums[0].ID, Title = "Double"},
                new Track{AlbumID = albums[0].ID, Title = "Corrente"},
                new Track{AlbumID = albums[0].ID, Title = "Double, presto"},
                new Track{AlbumID = albums[0].ID, Title = "Sarabande"},
                new Track{AlbumID = albums[1].ID, Title = "Allemanda"},
                new Track{AlbumID = albums[1].ID, Title = "Corrente"},
                new Track{AlbumID = albums[1].ID, Title = "Sarabande"},
                new Track{AlbumID = albums[1].ID, Title = "Giga"},
                new Track{AlbumID = albums[1].ID, Title = "Ciacona"},
                new Track{AlbumID = albums[2].ID, Title = "Preludio"},
                new Track{AlbumID = albums[2].ID, Title = "Loure"},
                new Track{AlbumID = albums[2].ID, Title = "Gavotee & Rondeau"},
                new Track{AlbumID = albums[2].ID, Title = "Menuet 1 - Menuet 2 - Menuet 1"},
                new Track{AlbumID = albums[2].ID, Title = "Bourrée"},
                new Track{AlbumID = albums[2].ID, Title = "Gigue"}
            };

            foreach (Track item in tracks)
                TrackEnsureListed(context, item);

            context.SaveChanges();
        }
    }
}
