using Microsoft.AspNetCore.Mvc;
using ProvaApi.Database;
using ProvaApi.Model.Request;
using System.Security.Cryptography.X509Certificates;

namespace ProvaApi.Controllers
{
    [Route("api/[controller]")]  // Questa annotazione serve per dire che questa classe è un controller
    [ApiController]
    public class UserController : ControllerBase
    {
        private static FakeDatabase database = new FakeDatabase();

        [HttpGet]
        public IActionResult AllUsers(int? gender)
        {
            if(gender == null)
            {
                // Mappa tutti i generi come stringa
                List<UserModel> usersGenderResponses = database.Users.Select(MapUserEntityToUserModel).ToList();
                return Ok(usersGenderResponses);
            }
            else
            {
                string genderString;
                switch(gender)
                {
                    case 1:
                        genderString = "Male";
                        break;
                    case 2:
                        genderString = "Female";
                        break;
                    case 3:
                        genderString = "Other";
                        break;
                    default:
                        return BadRequest("Attenzione: Genere non valido");
                }

                // Crea una lista con tutti gli utenti del genere richiesto
                var usersWithGender = database.Users.Where(user => user.Gender == gender).ToList();

                if(usersWithGender.Count == 0)
                {
                    return NotFound("Attenzione: Nessun utente trovato");
                }

                // Mappa la lista di utenti con il genere convertito a stringa
                List<UserModel> usersGenderResponses = usersWithGender.Select(MapUserEntityToUserModel).ToList();

                return Ok(new
                {
                    Message = "Utenti trovati",
                    Gender = "Genere Cercato: " + genderString,
                    Users = usersGenderResponses
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            // Trova il primo utente con l'id richiesto nel db
            var user = database.Users.FirstOrDefault(user => user.Id == id);
            if (user == null)
            {
                return NotFound("Attenzione: Utente non trovato"); // NotFound è un metodo del framework che restituisce uno statusCode 404
            }
            return Ok(MapUserEntityToUserModel(user));
        }

        [HttpDelete]
        public IActionResult DeleteUser(int? id, int? gender)
        {
            if(id != null)
            {
                var user = database.Users.FirstOrDefault(x => x.Id == id);
                if(user == null)
                {
                    return NotFound("Attenzione: Utente non trovato");
                }
                database.Users.Remove(user);
                UserModel userModel = MapUserEntityToUserModel(user);
                return Ok(new
                {
                    Message = "Utente eliminato",
                    User = userModel
                });
            }
            else if(gender != null)
            {
                var usersToDelete = database.Users.Where(user => user.Gender == gender).ToList();

                if(usersToDelete.Count == 0)
                {
                    return NotFound("Attenzione: Nessun utente trovato");
                }
                foreach(var user in usersToDelete)
                {
                    database.Users.Remove(user);
                }
                List<UserModel> usersModel = usersToDelete.Select(MapUserEntityToUserModel).ToList();
                return Ok(new
                {
                    Message = "Utenti eliminati",
                    Users = usersModel
                });
            }
            else
            {
                return BadRequest("Attenzione: Id o Genere non specificati");
            }
        }

        // Aggiunta utenti: i dati sono molti e strutturati, quindi i dati in POST devono essere passati nel BODY della richiesta
        [HttpPost]
        public IActionResult AddUser([FromBody]AddUserRequest request)
        {
            database.AddUser(new UserEntity
            {
                Name = request.Name,
                Password = request.Password,
                Surname = request.Surname,
                UserName = request.UserName, 
                Gender = request.Gender,
                DriverLicense = request.DriverLicense
            }); 
            return Ok("Utente aggiunto");
        }

        // Modifica utenti: i dati sono molti e strutturati, quindi i dati in PUT devono essere passati nel BODY della richiesta
        [HttpPut]
        public IActionResult UpdateUsers([FromBody]UpdateUserRequest request)
        {
            var currentUser = database.Users.FirstOrDefault(user => user.Id == request.Id);

            if(currentUser == null)
            {
                return NotFound("Attenzione: Utente non trovato");
            }

            currentUser.UserName = request.UserName;
            currentUser.Name = request.Name;
            currentUser.Password = request.Password;
            currentUser.Surname = request.Surname;
            currentUser.DriverLicense = request.DriverLicense;
            currentUser.Gender = request.Gender;
            return Ok("Utente modificato");

        }

        private UserModel MapUserEntityToUserModel(UserEntity user)
        {
            string genderString = GetUserGenderString(user.Gender);
            return new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Name = user.Name,
                Surname = user.Surname,
                Gender = genderString,
                DriverLicense = user.DriverLicense
            };
        }

        // Questo metodo mappa un intero che rappresenta il genere in una stringa
        private string GetUserGenderString(int gender)
        {
            switch (gender)
            {
                case 1:
                    return "Male";
                case 2:
                    return "Female";
                case 3:
                    return "Other";
                default:
                    return "Unknown Gender";
            }
        }
    }

}
