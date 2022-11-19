using Newtonsoft.Json;
namespace SocialMedia.Neo4jEnt
{
    public class User
    {
        public User(string userName, string firstName, string lastName, string email, string password)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        public User() { }
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        public override string ToString()
        {
            return $"{FirstName} {LastName} - username: {UserName}";
        }
    }
}