using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class Artist
    {
        public int ID { get; set; }

        [Display(Name = "Artist")]
        public string Name { get; set; }

        public List<Album> Albums { get; set; }
    }
}
