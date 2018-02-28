using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{

    public class SlimQuestion
    {
        [Key]
        public int QuestionID { get; set; }

        public string Qnum { get; set; }

        public string QuestionString { get; set; }

        public string AnswerA { get; set; }

        public string AnswerB { get; set; }

        public string AnswerC { get; set; }

        public string AnswerD { get; set; }

    }

}
