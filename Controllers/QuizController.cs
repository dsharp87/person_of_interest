using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using person_of_interest.Models;
using SessionExtensions;

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

        [HttpGet("[action]")]
        public List<SlimQuiz> getAllQuizes()
        {
            List<Quiz> AllQuizes = _context.quizes.Include( quiz => quiz.Questions).ToList();
            List<SlimQuiz> AllSlimQuizes = new List<SlimQuiz>();
            foreach (var quiz in AllQuizes) {
                SlimQuiz SlimQuiz = new SlimQuiz {
                    QuizID = quiz.QuizID,
                    Name = quiz.Name,
                    Description = quiz.Description
                };
                foreach (var question in quiz.Questions) {
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
                };
                AllSlimQuizes.Add(SlimQuiz);
            }; 
            return AllSlimQuizes;
        }


        [HttpGet("[action]/{IdNum}")]
        public SlimQuiz GetQuiz(int IdNum)
        {
            Quiz Quiz = _context.quizes.Where( quiz => quiz.QuizID == IdNum).Include( quiz => quiz.Questions).SingleOrDefault();
            SlimQuiz SlimQuiz = new SlimQuiz {
                QuizID = Quiz.QuizID,
                Name = Quiz.Name,
                Description = Quiz.Description
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
        public Object SumbitResults([FromBody] QuizResultSubmitModel ResultModel) {
            System.Console.WriteLine(ResultModel);
            SlimUser currentUser = HttpContext.Session.GetObjectFromJson<SlimUser> ("currentUser");
            int LoggedUserID = currentUser.UserID;
            QuizResult ExistingResult = _context.quiz_results.SingleOrDefault( quiz_result => quiz_result.UserID == LoggedUserID && quiz_result.QuizID == ResultModel.QuizID);
            if (ExistingResult == null) {
                QuizResult QuizResult = new QuizResult {
                ResultString = ResultModel.ResultString,
                QuizID = ResultModel.QuizID,
                UserID = LoggedUserID
                };
                _context.quiz_results.Add(QuizResult);
                _context.SaveChanges();
                QuizResult SavedNewResult = _context.quiz_results.SingleOrDefault( quiz_result => quiz_result.UserID == QuizResult.UserID && quiz_result.QuizID == QuizResult.QuizID);
                if (SavedNewResult != null) {
                    SlimQuizResult SlimQuizResult  = new SlimQuizResult {
                    QuizResultID = SavedNewResult.QuizResultID,
                    ResultString = SavedNewResult.ResultString,
                    QuizID = SavedNewResult.QuizID,
                    UserID = SavedNewResult.UserID
                };
                    currentUser.QuizResults.Add(SlimQuizResult);
                    HttpContext.Session.SetObjectAsJson ("currentUser", currentUser);
                    return SavedNewResult;
                }
                return BadRequest ("Your quiz failed for some reason");
            }
            ExistingResult.ResultString = ResultModel.ResultString;
            _context.SaveChanges();
            QuizResult SavedResult = _context.quiz_results.SingleOrDefault( quiz_result => quiz_result.UserID == ExistingResult.UserID && quiz_result.QuizID == ExistingResult.QuizID);
            if (SavedResult != null) {
                SlimQuizResult SlimQuizResult  = new SlimQuizResult {
                    QuizResultID = ExistingResult.QuizResultID,
                    ResultString = ExistingResult.ResultString,
                    QuizID = ExistingResult.QuizID,
                    UserID = ExistingResult.UserID
                };
                    //HARDCODED QUIZ RESULT!!! WILL NEED FORLOOP
                    currentUser.QuizResults[0] = SlimQuizResult;
                    //HARDCODED QUIZ RESULT!!! WILL NEED FORLOOP
                    
                HttpContext.Session.SetObjectAsJson ("currentUser", currentUser);
                
                return SavedResult;
            }
            return BadRequest ("Your quiz failed for some reason");
        }
    }
}
