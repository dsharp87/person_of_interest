using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{
    public class LoginUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
        

}