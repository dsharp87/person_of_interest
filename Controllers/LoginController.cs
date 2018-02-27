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

//ADD CONTEXT FILE and MODELS, then these ugly red lines will go away.
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
    public class LoginController: Controller
    {
        //Defines our context
        private ProjectContext _context;
        public LoginController(ProjectContext context)
        {
            _context = context;
        }

        //Register function.
        public IActionResult RegisterUser(User regUser)
        {
            var currentUser = _context.users.SingleOrDefault(user => user.Email == regUser.Email);
            if(ModelState.IsValid && currentUser == null)
                {
                    User newUser = new User
                    {
                        FirstName = regUser.FirstName,
                        LastName = regUser.LastName,
                        Email = regUser.Email,
                        Password = regUser.Password,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    _context.Add(newUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetObjectAsJson("currentUser", newUser);
                    return RedirectToAction("Success");
                }else{
                    ViewBag.errors = ModelState.Values;
                    return View("Index");
                }
        }
        //Login function.
        public IActionResult LoginUser(User logUser)
        {
            var currentUser = _context.users.SingleOrDefault(user => user.Email == logUser.Email);
            if(currentUser != null && currentUser.Password == logUser.Password)
            {
                HttpContext.Session.SetObjectAsJson("currentUser", currentUser);
                RedirectToAction("Success");
            }else{
            ViewBag.Error = "Incorrect Email/ Password combination";
            return View("Index");
            }
            
            return RedirectToAction("Success");
        }

        //PASSWORD STUFF
        public class HashWithSaltResult
        {
            public string Salt { get; }
            public string Digest { get; set; }
        
            public HashWithSaltResult(string salt, string digest)
            {
                Salt = salt;
                Digest = digest;
            }
        }
        //RNG CLASS
        public class RNG
        {
            public string GenerateRandomCryptographicKey(int keyLength)
            {           
                return Convert.ToBase64String(GenerateRandomCryptographicBytes(keyLength));
            }
 
            public byte[] GenerateRandomCryptographicBytes(int keyLength)
            {
                RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                byte[] randomBytes = new byte[keyLength];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return randomBytes;
            }
        }
        //Password hash, salt function to be used in TestPasswordHasher
        public class PasswordWithSaltHasher
        {
            public HashWithSaltResult HashWithSalt(string Password, int saltLength, HashAlgorithm hashAlgo)
            {
                RNG rng = new RNG();
                byte[] saltBytes = rng.GenerateRandomCryptographicBytes(saltLength);
                byte[] PasswordAsBytes = Encoding.UTF8.GetBytes(Password); 
                List<byte> PasswordWithSaltBytes = new List<byte>();
                PasswordWithSaltBytes.AddRange(PasswordAsBytes);
                PasswordWithSaltBytes.AddRange(saltBytes);
                byte[] digestBytes = hashAlgo.ComputeHash(PasswordWithSaltBytes.ToArray());
                return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(digestBytes));
            }
        }

        private static void TestPasswordHasher()
        {
            PasswordWithSaltHasher pwHasher = new PasswordWithSaltHasher();
            HashWithSaltResult hashResultSha256 = pwHasher.HashWithSalt("ultra_safe_P455w0rD", 64, SHA256.Create());
            HashWithSaltResult hashResultSha512 = pwHasher.HashWithSalt("ultra_safe_P455w0rD", 64, SHA512.Create());
        
            Console.WriteLine(hashResultSha256.Salt);
            Console.WriteLine(hashResultSha256.Digest);
            Console.WriteLine();
            Console.WriteLine(hashResultSha512.Salt);
            Console.WriteLine(hashResultSha512.Digest);
        }

    }
}