using System; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{
    public class SlimUser
        {
            [Key]
            public int UserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ConnectionID { get; set; }
        }
}