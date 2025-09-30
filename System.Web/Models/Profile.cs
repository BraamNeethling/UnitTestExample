namespace Systems.Web.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public List<string> Interests { get; set; } = new List<string>();
        public User User { get; set; } = new User();
    }
}
