namespace BackKFHShortcuts.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public int KFH_Id { get; set; }
        public bool IsAdmin { get; set; }
        public List<ProductRequest> ProductRequests { get; set; }
        public List<RewardRequest> Rewards { get; set; }
        public List<Message> Messages { get; set; }
        public static User Create(int id, string email, string password, string firstName, string Lastname, int points, int kfh_id, bool isAdmin = false)
        {
            return new User
            {
                Id = id,
                Email = email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                IsAdmin = isAdmin,
                FirstName = firstName,
                LastName = Lastname,
                Points = points,
                KFH_Id = kfh_id
            };
        }
        public bool VerifyPassword(string pwd) => BCrypt.Net.BCrypt.EnhancedVerify(pwd, this.Password);
    }
}
