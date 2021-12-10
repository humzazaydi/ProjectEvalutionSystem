namespace ProjectEvalutionSystem.Models.Auth
{
    public class LoginDTO
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public UserRole whoLogin { get; set; }
    }

    public enum UserRole
    {
        SuperAdmin,
        Teacher,
        Student
    }
}