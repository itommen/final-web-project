using System.ComponentModel;

namespace Recipes.ViewModels
{
    public class UserRecipesViewModel
    {
        public int ID { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }


        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("The Recipe")]
        public string Title { get; set; }
    }
}