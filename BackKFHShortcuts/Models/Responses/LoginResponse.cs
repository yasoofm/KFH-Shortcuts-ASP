namespace BackKFHShortcuts.Models.Responses
{
    public class LoginResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int KFH_Id { get; set; }
        public string Token { get; set; }
    }
}
