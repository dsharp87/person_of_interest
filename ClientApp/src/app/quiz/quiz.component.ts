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
  Answer: object;
  Questions: object;


  constructor(private _route: ActivatedRoute, private _router: Router, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.Answer = {
      TestAnswer: ""
    };
    this._http.get(baseUrl + 'Quizes/Quiz/GetQuiz/1').subscribe(result => {
      console.log(result);
      this.Questions = result["questions"];
      this.QuizTitle = result["name"];
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
