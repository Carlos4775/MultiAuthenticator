namespace Data.ViewModels
{
    public class UserLogin
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Relationships
        public int RoleId { get; set; }
    }
}
