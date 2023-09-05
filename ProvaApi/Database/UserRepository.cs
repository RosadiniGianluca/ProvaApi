using Microsoft.EntityFrameworkCore;

namespace ProvaApi.Database
{
    public class UserRepository
    {

        private readonly MyDbContext _database;

        public UserRepository(MyDbContext database)
        {
            _database = database;
        }

        public UserEntity GetUserById(int id)
        {
            return _database.Users.FirstOrDefault(user => user.Id == id);
        }

        public List<UserEntity> GetAllUsers()
        {
            return _database.Users.ToList();
        }

        public List<UserEntity> GetUsersByGender(int gender)
        {
            return _database.Users.Where(user => user.Gender == gender).ToList();
        }

        public void AddUser(UserEntity user)
        {
            _database.Users.Add(user);
            _database.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var userToDelete = _database.Users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete != null)
            {
                _database.Users.Remove(userToDelete);
                _database.SaveChanges();
            }
        }

        public void UpdateUser(UserEntity updatedUser)
        {
            var existingUser = _database.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser != null)
            {
                // Aggiorna le proprietà dell'utente esistente con i dati dell'utente aggiornato
                existingUser.UserName = updatedUser.UserName;
                existingUser.Name = updatedUser.Name;
                existingUser.Password = updatedUser.Password;
                existingUser.Surname = updatedUser.Surname;
                existingUser.DriverLicense = updatedUser.DriverLicense;
                existingUser.Gender = updatedUser.Gender;

                _database.SaveChanges();
            }
        }

    }
}
