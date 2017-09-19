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
        public bool isAdmin { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class userPostsViewModel
    {
        public int ID { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }


        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("The Post")]
        public string Title { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}