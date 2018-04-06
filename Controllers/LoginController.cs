using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using person_of_interest.Models;
using SessionExtensions;

namespace person_of_interest.Controllers {

    [Route ("[controller]")]
    public class LoginController : Controller {
        //Defines our context
        private ProjectContext _context;
        public LoginController (ProjectContext context) {
            _context = context;
        }

        //Register function.
        [HttpPost ("[action]")]
        public Object RegisterUser ([FromBody] RegisterUserModel regUser) {
            var currentUser = _context.users.SingleOrDefault (user => user.Email == regUser.Email);
            if (currentUser == null) {

                // List<string> HashRes = HashSalt (regUser.Password, CreateByteSalt ());
                User newUser = new User {
                    FirstName = regUser.FirstName,
                    LastName = regUser.LastName,
                    Email = regUser.Email,
                    Password = regUser.Password,
                    // Password = HashRes[1],
                    // Salt = HashRes[0],
                };
                _context.Add(newUser);
                _context.SaveChanges();
                var JustAddedUser = _context.users.SingleOrDefault (user => user.Email == regUser.Email);
                List<SlimQuizResult> QuizResults = new List<SlimQuizResult>();
                SlimUser NewSlimUser = new SlimUser {
                    FirstName = JustAddedUser.FirstName,
                    LastName = JustAddedUser.LastName,
                    ConnectionID = JustAddedUser.ConnectionID,
                    UserID = JustAddedUser.UserID,
                    QuizResults = QuizResults
                };

                HttpContext.Session.SetObjectAsJson("currentUser", NewSlimUser);
                return NewSlimUser;
            }
            return BadRequest ("User with this email already exists. Please specify use a different email address");
        }

        [HttpPost ("[action]")]
        public Object LoginUser ([FromBody] LoginUserModel logUser) {
            var currentUser = _context.users.Include(user => user.QuizResults).SingleOrDefault (user => user.Email == logUser.Email);
            if (currentUser == null) {
                //SOME ERROR MESSAGE FOR FRONT END
                return BadRequest ("User email does not exist");
            }
            List<SlimQuizResult> SlimQuizResults = new List<SlimQuizResult>();
            foreach (var OneSlimQuizResult in currentUser.QuizResults) {
                SlimQuizResult SlimQuizResult  = new SlimQuizResult {
                    QuizResultID = OneSlimQuizResult.QuizResultID,
                    ResultString = OneSlimQuizResult.ResultString,
                    QuizID = OneSlimQuizResult.QuizID,
                    UserID = OneSlimQuizResult.UserID
                };
                SlimQuizResults.Add(SlimQuizResult);
            }
            SlimUser NewSlimUser = new SlimUser {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                ConnectionID = currentUser.ConnectionID,
                UserID = currentUser.UserID,
                QuizResults = SlimQuizResults
            };
            //Compare passwords
            // byte[] Salt = Convert.FromBase64String (currentUser.Salt);
            // string HashSaltedPswd = CreatePasswordHash(currentUser.Password, Salt);
            
            // if (HashSaltedPswd == currentUser.Password){
            if (logUser.Password == currentUser.Password) {
                HttpContext.Session.SetObjectAsJson ("currentUser", NewSlimUser);
                HttpContext.Session.SetInt32 ("UserID", currentUser.UserID);
                return NewSlimUser;
            } else {
                return BadRequest ("Password does not match!");
            };
        }

        public static byte[] CreateByteSalt () {
            byte[] Salt = new byte[128 / 8];
            using (var Rng = RandomNumberGenerator.Create ()) {
                Rng.GetBytes (Salt);
            }
            return Salt;
        }

        public static string CreatePasswordHash (string Pass, byte[] Salt) {
            string HashedPassString = Convert.ToBase64String (KeyDerivation.Pbkdf2 (
                Pass,
                Salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
            return HashedPassString;
        }

        public static List<string> HashSalt (string Pass, byte[] Salt) {
            // generate a 128-bit salt using a secure PRNG
            // byte[] Salt = new byte[128 / 8];
            // using (var Rng = RandomNumberGenerator.Create ()) {
            //     Rng.GetBytes (Salt);
            // }
            // Console.WriteLine ($"Salt: {Convert.ToBase64String(Salt)}");
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            
            // string Hashed = Convert.ToBase64String (KeyDerivation.Pbkdf2 (
            //     Pass,
            //     Salt,
            //     KeyDerivationPrf.HMACSHA1,
            //     10000,
            //     256 / 8));
            // Console.WriteLine ($"Hashed: {Hashed}");
            // List<string> HashPassString = new List<string> { Convert.ToBase64String (Salt), Hashed };
            // return HashPassString;
            string Hashed = CreatePasswordHash(Pass, Salt);
            List<string> HashPassString = new List<string> { Convert.ToBase64String (Salt), Hashed };
            return HashPassString;
        }
    }
}