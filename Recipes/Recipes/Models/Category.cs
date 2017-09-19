using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicBox.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Food Category")]
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}