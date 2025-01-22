using System.ComponentModel.DataAnnotations;

namespace Dashboard.ViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
