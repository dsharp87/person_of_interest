import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-quiz',
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})
export class QuizComponent implements OnInit {
  QuizTitle: string;
  QuizDescription: string;
  QuizID:number;
  Answers: object;
  Questions: Question[];
  baseUrl:String;
  QuizError: string;
  user:object
  QuizIsDone: boolean;


  constructor(private _route: ActivatedRoute, private _router: Router, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.QuizError = "";
    this.Questions = [];
    this._route.params.subscribe((params: Params) => this.QuizID = params.id);
    this._http.get(baseUrl + `Quizes/Quiz/GetQuiz/${this.QuizID}`).subscribe(result => {
      // console.log(result);
      this.Questions = result["questions"];
      this.QuizTitle = result["name"]
      this.QuizDescription = result["description"];
    }, error => console.error(error))

    this.Answers = {
      a1:"",
      a2:"",
      a3:"",
      a4:"",
      a5:"",
      a6:"",
      a7:"",
      a8:"",
      a9:"",
      a10:"",
      a11:"",
      a12:"",
      a13:"",
      a14:"",
      a15:"",
    };
    this.QuizIsDone = false;
   }

   // CSS YOUR MOM!!!!

  SubmitAnswers() {
    // console.log(this.Answers, "component SubmitAnswer");
    // console.log(this.baseUrl + 'Quizes/Quiz/SumbitResults', "url i'm going to hit");
    let StringAssembler = ""
    for (let key in this.Answers) {
      StringAssembler += this.Answers[key];
    }
    let ResultModel = {
      ResultString: StringAssembler,
      QuizID: this.QuizID
    }
    this._http.post(this.baseUrl + 'Quizes/Quiz/SumbitResults', ResultModel).subscribe(result => {
      // console.log(result);
      this._router.navigate(["/landing"]);
    }, error => {
      console.error(error);
      this.QuizError = error.error;
    });
  }

  NavigateToHome() {
    this._router.navigate(["/landing"])
  }


  checkSession(){
    // console.log(this.baseUrl+'User/CheckSession');
    this._http.get(this.baseUrl+'User/CheckSession').subscribe(
      (result) => {
        if (result == null) {
          this._router.navigate(["/login"])
        }
        this.user = result;
        // this.find_online_users();
      }, error => console.error(error)
    )
  }

  ngOnInit() {
    this.checkSession();
    this.QuizIsDone = false;
  }
  
  QuizCompleted()
  {
    this.QuizIsDone = true;
  }
 
}

interface Question {
  Question: string;
  AnswerA: string;
  AnswerB: string;
  AnswerC: string;
  AnswerD: string;

}
