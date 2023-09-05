using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvaApi.Database;
using ProvaApi.Model.Request;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;

// Classe Controller: contiene i metodi che rispondono alle richieste HTTP
namespace ProvaApi.Controllers
{
    [Route("api/[controller]")]  // Questa annotazione serve per dire che questa classe è un controller
    [ApiController]
    public class UserController : ControllerBase
    {
        // Inietta il DbContext nel controller
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult AllUsers(int? gender)
        {
            if(gender == null)
            {   
                // Ottieni tutti gli utenti dal db
                List<UserEntity> users = _userRepository.GetAllUsers();
                // Mappa tutti i generi come stringa
                List<UserModel> usersGenderResponses = users.Select(MapUserEntityToUserModel).ToList();
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
                var usersWithGenderInt = _userRepository.GetUsersByGender((int)gender);

                if(usersWithGenderInt.Count == 0)
                {
                    return NotFound("Attenzione: Nessun utente trovato");
                }

                // Mappa la lista di utenti con il genere convertito a stringa
                List<UserModel> usersGenderResponses = usersWithGenderInt.Select(MapUserEntityToUserModel).ToList();

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
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound("Attenzione: Utente non trovato"); // NotFound è un metodo del framework che restituisce uno statusCode 404
            }
            return Ok(MapUserEntityToUserModel(user));
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);
            return Ok("Utente eliminato");
        }

        // Aggiunta utenti: i dati sono molti e strutturati, quindi i dati in POST devono essere passati nel BODY della richiesta
        [HttpPost]
        public IActionResult AddUser([FromBody]AddUserRequest request)
        {
            // Crea un nuovo oggetto UserEntity a partire dai dati ricevuti nella richiesta
            var newUser = new UserEntity
            {
                UserName = request.UserName,
                Password = request.Password,
                Name = request.Name,
                Surname = request.Surname,
                Gender = request.Gender,
                DriverLicense = request.DriverLicense
            };

            // Aggiungi il nuovo utente al database utilizzando il contesto MyDbContext
            _userRepository.AddUser(newUser);

            // Restituisci una risposta appropriata
            return Ok("Utente aggiunto");
        }


        // Modifica utenti: i dati sono molti e strutturati, quindi i dati in PUT devono essere passati nel BODY della richiesta
        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserEntity updatedUser)
        {
            _userRepository.UpdateUser(updatedUser);
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
