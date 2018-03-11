using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class Album
    {
        public int ID { get; set;}
        [Display(Name = "Artist")]
        public int ArtistID { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime ReleaseDate { get; set; }

        public Artist Artist { get; set; }
        public List<Track> Tracks { get; set; }
    }
}
