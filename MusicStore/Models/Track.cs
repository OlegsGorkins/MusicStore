using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class Track
    {
        public int ID { get; set; }
        [Display(Name = "Album")]
        public int AlbumID { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        public Album Album { get; set; }
    }
}
