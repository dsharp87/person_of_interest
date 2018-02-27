using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{

    public class Quiz : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizID { get; set; }

        public string Name { get; set; }

        public List<Question> Questions { get; set; }

        public Quiz() {
            Questions = new List<Question>();
        }

    }
}