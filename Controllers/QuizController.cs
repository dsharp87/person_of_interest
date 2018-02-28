using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using person_of_interest.Models;

namespace person_of_interest.Controllers
{

    [Route("Quizes/[controller]")]
    public class QuizController : Controller
    {

        private ProjectContext _context;

        public QuizController(ProjectContext context)
        {
            _context = context;
        }


        [HttpGet("[action]/{IdNum}")]
        public SlimQuiz GetQuiz(int IdNum)
        {
            Quiz Quiz = _context.quizes.Where( quiz => quiz.QuizID == IdNum).Include( quiz => quiz.Questions).SingleOrDefault();
            SlimQuiz SlimQuiz = new SlimQuiz {
                QuizID = Quiz.QuizID,
                Name = Quiz.Name
            };
            foreach (var question in Quiz.Questions) {
                SlimQuestion SlimQuestion = new SlimQuestion {
                    QuestionID = question.QuestionID,
                    Qnum = question.Qnum,
                    QuestionString = question.QuestionString,
                    AnswerA = question.AnswerA,
                    AnswerB = question.AnswerB,
                    AnswerC = question.AnswerC,
                    AnswerD = question.AnswerD
                };
                SlimQuiz.Questions.Add(SlimQuestion);   
            }
            return SlimQuiz;
        }

        [HttpPost ("[action]")]
        public QuizResult SumbitResults([FromBody] QuizResultSubmitModel ResultModel) {
            System.Console.WriteLine(ResultModel);
            QuizResult QuizResult = new QuizResult {
                ResultString = ResultModel.ResultString,
                QuizID = ResultModel.QuizID,
                UserID = 1
            };
            _context.quiz_results.Add(QuizResult);
            _context.SaveChanges();
            QuizResult SavedResult = _context.quiz_results.SingleOrDefault( quiz_result => quiz_result.UserID == QuizResult.UserID && quiz_result.QuizID == QuizResult.QuizID);
        return SavedResult;
        }
    }
}
