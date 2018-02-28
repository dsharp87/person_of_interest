using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using person_of_interest.Models;
using Microsoft.AspNetCore.Http;
using System.Text;


namespace person_of_interest.Controllers
{
    //Static class for Session/ Session Extensions.
    public static class SessionExtensions
        {
            //Sets JSON object in Session
            public static void SetObjectAsJson(this ISession session, string key, object value)
            {
                session.SetString(key, JsonConvert.SerializeObject(value));
            }

            //Gets JSON objects from Session
            public static T GetObjectFromJson<T>(this ISession session, string key)
            {
                string value = session.GetString(key);
                return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
            }
        }
    [Route("[controller]")]
    public class LoginController: Controller
    {
        //Defines our context
        private ProjectContext _context;
        public LoginController(ProjectContext context)
        {
            _context = context;
        }

        //Register function.
        [HttpPost("[action]")]
        public void RegisterUser([FromBody] RegisterUserModel regUser)
        {
            var currentUser = _context.Users.SingleOrDefault(user => user.Email == regUser.Email);
            if (currentUser == null)
            {
                
                List<string> HashRes = HashSalt(regUser.Password);
                User newUser = new User
                {
                    FirstName = regUser.FirstName,
                    LastName = regUser.LastName,
                    Email = regUser.Email,
                    Password = HashRes[1],
                    Salt = HashRes[0],

                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetObjectAsJson("currentUser", newUser);
                return;
            }

            // public void LoginUser(User logUser)
            // {
            //     var currentUser = _context.Users.SingleOrDefault(user => user.Email == logUser.Email);
            //         HttpContext.Session.SetObjectAsJson("currentUser", currentUser);
            //         RedirectToAction("");
                
            //     return;
            // }
        }

        public class RegisterUserModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        //Login function.
        

        public static List<string> HashSalt(string Pass)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] Salt = new byte[128 / 8];
            using (var Rng = RandomNumberGenerator.Create())
            {
                Rng.GetBytes(Salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(Salt)}");
    
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string Hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                Pass,
                Salt,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
            Console.WriteLine($"Hashed: {Hashed}");
            List<string> HashPassString = new List<string>{Convert.ToBase64String(Salt), Hashed};
            return HashPassString;
        }
}}