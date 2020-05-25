namespace DatingApp.API.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
    }
}