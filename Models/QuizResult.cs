using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{

    public class QuizResult : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizResultID { get; set; }
        
        public string ResultString { get; set; }

        public int QuizID { get; set; }

        public Quiz Quiz { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

    }

}
