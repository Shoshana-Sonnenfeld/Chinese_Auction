namespace Server.Models
{
    public enum UserRole
    {
        User = 1, Manager = 2
    }
    public class User
    {
            public int Id { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string? Phone { get; set; }
            public UserRole Role { get; set; } = UserRole.User;// "manager" or "user"
            public List<Ticket> Tickets { get; set; }
    }
}
