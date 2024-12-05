namespace Dashboard.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<EditRoleViewModel> Roles { get; set; }
    }
}
