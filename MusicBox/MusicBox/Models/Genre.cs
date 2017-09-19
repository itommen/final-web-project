// Sharon Grozman - 311429963 
// Elhen Shmailov - 313736639
// Tomer Parizer - 312465602
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicBox.Models
{
    public class Genre
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Genre Name")]
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}