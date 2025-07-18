namespace TVBroad.Web.Models.Admin
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public List<string> AllRoles { get; set; }
        public string SelectedRole { get; set; }
    }
}
