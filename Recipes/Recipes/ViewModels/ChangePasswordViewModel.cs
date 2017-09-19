using System.ComponentModel.DataAnnotations;

namespace Recipes.ViewModels
{
    public class ChangePasswordViewModel : SetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
    }
}