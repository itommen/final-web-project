using System.ComponentModel;

namespace Recipes.ViewModels
{
    public class RecipeCommentViewModel
    {
        public int Id { get; set; }

        [DisplayName("The Recipe")]
        public string Title { get; set; }

        [DisplayName("Number Of Comments")]
        public int NumberOfComment { get; set; }
    }
}