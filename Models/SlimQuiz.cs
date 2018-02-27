using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{

    public class SlimQuiz
    {
        [Key]
        public int QuizID { get; set; }

        public string Name { get; set; }

        public List<SlimQuestion> Questions { get; set; }

        public SlimQuiz() {
            Questions = new List<SlimQuestion>();
        }

    }
}