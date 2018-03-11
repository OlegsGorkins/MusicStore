using System;
using System.Linq;
using MusicStore.Models;

namespace MusicStore.Data
{
    public static class SeedData
    {
        private static DateTime RandomYearInRange(int startYear, int endYear)
        {
            if (startYear > 0 && endYear <= DateTime.Today.Year)
            {
                Random gen = new Random();
                int year = gen.Next(startYear, endYear);
                int month = gen.Next(1, 12);
                int day = gen.Next(1, DateTime.DaysInMonth(year, month));
                return new DateTime(year, month, day);
            }
            else
                return DateTime.Today;
        }

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
                new Artist {Name = "Johann Sebastian Bach"},
                new Artist {Name = "Goran Bregović" },
                new Artist {Name = "Abre Noir"}
            };

            foreach (Artist item in artists)
                ArtistEnsureListed(context, item);

            context.SaveChanges();
            
            var albums = new Album[]
            {
                new Album {ArtistID = artists[0].ID, Title = "Partita for solo violin No. 1 in B minor",
                           ReleaseDate = RandomYearInRange(1717, 1723)},
                new Album {ArtistID = artists[0].ID, Title = "Partita for solo violin No. 2 in D minor",
                           ReleaseDate = RandomYearInRange(1717, 1723)},
                new Album {ArtistID = artists[0].ID, Title = "Partita for solo violin No. 3 in E major",
                           ReleaseDate = RandomYearInRange(1717, 1723)},
                new Album {ArtistID = artists[1].ID, Title = "Irish songs",
                           ReleaseDate = RandomYearInRange(1998, 1998)},
                new Album {ArtistID = artists[2].ID, Title = "Madurai",
                           ReleaseDate = RandomYearInRange(2006, 2006)}
            };

            foreach (Album item in albums)
                AlbumEnsureListed(context, item);

            context.SaveChanges();

            var tracks = new Track[]
            {
                new Track {AlbumID = albums[0].ID, Title = "Allemanda"},
                new Track {AlbumID = albums[0].ID, Title = "Double"},
                new Track {AlbumID = albums[0].ID, Title = "Corrente"},
                new Track {AlbumID = albums[0].ID, Title = "Double, presto"},
                new Track {AlbumID = albums[0].ID, Title = "Sarabande"},
                new Track {AlbumID = albums[1].ID, Title = "Allemanda"},
                new Track {AlbumID = albums[1].ID, Title = "Corrente"},
                new Track {AlbumID = albums[1].ID, Title = "Sarabande"},
                new Track {AlbumID = albums[1].ID, Title = "Giga"},
                new Track {AlbumID = albums[1].ID, Title = "Ciacona"},
                new Track {AlbumID = albums[2].ID, Title = "Preludio"},
                new Track {AlbumID = albums[2].ID, Title = "Loure"},
                new Track {AlbumID = albums[2].ID, Title = "Gavotee & Rondeau"},
                new Track {AlbumID = albums[2].ID, Title = "Menuet 1 - Menuet 2 - Menuet 1"},
                new Track {AlbumID = albums[2].ID, Title = "Bourrée"},
                new Track {AlbumID = albums[2].ID, Title = "Gigue"},
                new Track {AlbumID = albums[3].ID, Title = "Ev Chistr 'ta, Laou! (Cider-Drinking Song)"},
                new Track {AlbumID = albums[3].ID, Title = "The Landlord's Walk"},
                new Track {AlbumID = albums[3].ID, Title = "Worship"},
                new Track {AlbumID = albums[3].ID, Title = "Le Lys Vert (La Bottine Souriante)"},
                new Track {AlbumID = albums[3].ID, Title = "Stolen Child"},
                new Track {AlbumID = albums[3].ID, Title = "Nelson Mandela's Welcome To The City Of Glasgow"},
                new Track {AlbumID = albums[3].ID, Title = "An Innis Aigh (The Rankins)"},
                new Track {AlbumID = albums[3].ID, Title = "Daos-Tro Fisel (Dance From The Fisel Country)"},
                new Track {AlbumID = albums[3].ID, Title = "Stormcry"},
                new Track {AlbumID = albums[3].ID, Title = "Farewell To Ireland"},
                new Track {AlbumID = albums[3].ID, Title = "Marches (From The Vannes Country)"},
                new Track {AlbumID = albums[3].ID, Title = "A Mhairi Bhoidheach"},
                new Track {AlbumID = albums[3].ID, Title = "Callow Lake"},
                new Track {AlbumID = albums[3].ID, Title = "My Home - The Contradiction - Julia Delaney"},
                new Track {AlbumID = albums[3].ID, Title = "Lullaby"},
                new Track {AlbumID = albums[4].ID, Title = "Visions"},
                new Track {AlbumID = albums[4].ID, Title = "Madurai"},
                new Track {AlbumID = albums[4].ID, Title = "Welcome"},
                new Track {AlbumID = albums[4].ID, Title = "Nighttime"},
                new Track {AlbumID = albums[4].ID, Title = "Sahal"},
                new Track {AlbumID = albums[4].ID, Title = "Jaywalker"},
                new Track {AlbumID = albums[4].ID, Title = "Circles"},
                new Track {AlbumID = albums[4].ID, Title = "Ticket"},
                new Track {AlbumID = albums[4].ID, Title = "Essence"},
                new Track {AlbumID = albums[4].ID, Title = "Reflections"},
                new Track {AlbumID = albums[4].ID, Title = "Velvet"},
                new Track {AlbumID = albums[4].ID, Title = "Surface"},
                new Track {AlbumID = albums[4].ID, Title = "Talking"},
                new Track {AlbumID = albums[4].ID, Title = "Eclipse"},
                new Track {AlbumID = albums[4].ID, Title = "Namib"}
            };

            foreach (Track item in tracks)
                TrackEnsureListed(context, item);

            context.SaveChanges();
        }
    }
}
