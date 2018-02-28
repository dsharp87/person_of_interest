using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace person_of_interest.Models
{

    public class QuizResultSubmitModel
    {        
        public string ResultString { get; set; }

        public int QuizID { get; set; }


    }

}
