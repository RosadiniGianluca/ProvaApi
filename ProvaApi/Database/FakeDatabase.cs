namespace ProvaApi.Database
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Gender { get; set;}
        public char DriverLicense { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public char DriverLicense { get; set;}
    }
    

    public class FakeDatabase
    {
        private static int globalId = 0;  // variabile statica che viene incrementata ad ogni creazione di un nuovo utente

        // Simulo tabella utenti
        public List<UserEntity> Users { get; set; }

        public FakeDatabase()
        {
            Users = new List<UserEntity>();

            AddUserInternal(new UserEntity
            {
                UserName = "User1",
                Password = "Password1",
                Name = "Name1",
                Surname = "Surname1",
                Gender = 1,
                DriverLicense = 'A'
            });

            AddUserInternal(new UserEntity
            {
                UserName = "User2",
                Password = "Password2",
                Name = "Name2",
                Surname = "Surname2", 
                Gender = 2, 
                DriverLicense = 'B'
            });

            AddUserInternal(new UserEntity
            {
                UserName = "User3",
                Password = "Password3",
                Name = "Name3",
                Surname = "Surname3", 
                Gender = 2,
                DriverLicense = 'B'
            });

            AddUserInternal(new UserEntity
            {
                UserName = "User4",
                Password = "Password4",
                Name = "Name4",
                Surname = "Surname4", 
                Gender = 3,
                DriverLicense = 'B'
            });

            AddUserInternal(new UserEntity
            {
                UserName = "User5",
                Password = "Password5",
                Name = "Name5",
                Surname = "Surname5",
                Gender = 2,
                DriverLicense = 'B'
            });

            AddUserInternal(new UserEntity
            {
                UserName = "User6",
                Password = "Password6",
                Name = "Name6",
                Surname = "Surname6",
                Gender = 2,
                DriverLicense = 'C'
            });
        }


        //metodo per la creazione di un nuovo utente
        public void AddUser(UserEntity user)
        {
            AddUserInternal(user);
        }

        private void AddUserInternal(UserEntity user)
        {
            user.Id = ++globalId;
            Users.Add(user);
        }

    }

    
}
