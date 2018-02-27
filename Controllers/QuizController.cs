using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace person_of_interest.Controllers
{

    [Route("Quizes/[controller]")]
    public class QuizController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<Question> GetQuiz()
        {
            return Enumerable.Range(1, 3).Select(Qnum => new Question
            {
                QuestionString = $"I am a question {Qnum}",
                AnswerA = "I am Answer A",
                AnswerB = "I am Answer B",
                AnswerC = "I am Answer C",
                AnswerD = "I am Answer D"
            });
        }
    
    
        public class Question
        {
            public string QuestionString { get; set; }
            public string AnswerA { get; set; }

            public string AnswerB { get; set; }

            public string AnswerC { get; set; }

            public string AnswerD { get; set; }
        }

    
    }
}
