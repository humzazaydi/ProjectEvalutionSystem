namespace ProjectEvalutionSystem.Models.Auth
{
    public class LoginDTO
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public UserRole whoLogin { get; set; }
    }

    public class LoginSessionDTO
    {
        public string fullname { get; set; }
        public string emailaddress { get; set; }
        public UserRole user_role { get; set; }
    }

    public enum UserRole
    {
        SuperAdmin = 1,
        Student = 2,
        Teacher=3
    }
}