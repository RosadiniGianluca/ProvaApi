namespace ProvaApi.Model.Request
{
    // DTO: Data Transfer Object (oggetto che trasferisce dati), serve per trasferire dati da una classe all'altra
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string? UserName { get; set; }  // ? significa che il campo può essere null
        public string? Password { get; set; }
        public string? Name { get; set;}
        public string? Surname { get; set; }
        public int Gender { get; set; }
        public char DriverLicense { get; set; }
    }
}
