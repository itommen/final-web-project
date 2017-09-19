using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Models
{
    public class Client
    {
        public int ID { get; set; }
  
        public Gender Gender { get; set; }

        [Required]
        [DisplayName("Logon Name")]
        public string ClientName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [DisplayName("Administrator")]
        public bool IsAdmin { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Recipe> Recipes { get; set; }
    }
}