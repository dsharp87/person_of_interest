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
  Id:number = 0;
  Answers: object;
  Questions: Question[];
  baseUrl:String;
  QuizError: string;


  constructor(private _route: ActivatedRoute, private _router: Router, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.QuizError = "";
    this._http.get(baseUrl + 'Quizes/Quiz/GetQuiz/1').subscribe(result => {
      console.log(result);
      this.Questions = result["questions"];
      this.QuizTitle = result["name"];
    }, error => console.error(error))
    this.Answers = {
      QuizID: 1,
      a1:"",
      a2:"",
      a3:"",
      a4:"",
      a5:""
    };
   }

  SubmitAnswers() {
    console.log(this.Answers, "component SubmitAnswer");
    console.log(this.baseUrl + 'Quizes/Quiz/SumbitResults', "url i'm going to hit");
    let ResultModel = {
      ResultString: (this.Answers["a1"] + this.Answers["a2"] + this.Answers["a3"] + this.Answers["a4"] + this.Answers["a5"]),
      QuizID: this.Answers["QuizID"]
    }
    this._http.post(this.baseUrl + 'Quizes/Quiz/SumbitResults', ResultModel).subscribe(result => {
      console.log(result);
      this._router.navigate(["/"]);
    }, error => {
      console.error(error);
      this.QuizError = error.error;
    });
  }


  ngOnInit() {
    this._route.params.subscribe((params: Params) => this.Id = params.id);
  }

  
}

interface Question {
  Question: string;
  AnswerA: string;
  AnswerB: string;
  AnswerC: string;
  AnswerD: string;

}
