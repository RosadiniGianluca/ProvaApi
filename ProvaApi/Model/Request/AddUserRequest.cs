namespace ProvaApi.Model.Request
{
    public class AddUserRequest
    {
        public string? UserName { get; set; }  // ? significa che il campo può essere null
        public string? Password { get; set; }
        public string? Name { get; set;}
        public string? Surname { get; set; }
        public int Gender { get; set; }
        public char DriverLicense { get; set; }
    }
}
