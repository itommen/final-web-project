using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Models
{
    public class Comment
    {
        public int ID { get; set; }

        [Required]
        [ForeignKey("Client")]
        public int ClientID { get; set; }

        [Required]
        [ForeignKey("Recipe")]
        public int RecipeID { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created at")]
        public DateTime CreationDate { get; set; }

        public virtual Recipe Recipe { get; set; }

        public virtual Client Client { get; set; }
    }
}