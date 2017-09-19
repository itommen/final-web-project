using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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