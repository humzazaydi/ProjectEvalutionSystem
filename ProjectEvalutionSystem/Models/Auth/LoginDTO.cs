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
        Teacher = 2,
        Student =3
    }

    public class ChangeSettingsDTO
    {
        public int id { get; set; }
        public string new_password { get; set; }
        public string fullname { get; set; }
        public string email_address { get; set; }
        public UserRole user_role { get; set; }
    }
}