import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-quiz',
  templateUrl: './quiz.component.html',
  styleUrls: ['./quiz.component.css']
})
export class QuizComponent implements OnInit {
  QuizTitle: string = "Quiz Title Here"
  Id:number = 0;
  Answer: object;
  Questions: Question[];


  constructor(private _route: ActivatedRoute, private _router: Router, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.Answer = {
      TestAnswer: ""
    };
    this._http.get<Question[]>(baseUrl + 'Quizes/Quiz/GetQuiz').subscribe(result => {
      this.Questions = result;
    }, error => console.error(error))
   }

  SubmitAnswer() {
    console.log(this.Answer, "component SubmitAnswer");
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
