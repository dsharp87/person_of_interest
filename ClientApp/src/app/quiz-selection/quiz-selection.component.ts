import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-quiz-selection',
  templateUrl: './quiz-selection.component.html',
  styleUrls: ['./quiz-selection.component.css']
})

export class QuizSelectionComponent implements OnInit {
  baseUrl:String;
  user:object;
  quizes:any;


  constructor(private _route: ActivatedRoute, private _router: Router, private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.quizes = [];
  }
  
  
  getQuizes() {
    this._http.get(this.baseUrl + 'Quizes/Quiz/getAllQuizes').subscribe(
      (result) => {
        console.log(result);
        this.quizes = result; 
      }, error => {
        console.error(error)
      });
  }

  NavigateToQuizNumber(quizID:number) {
    this._router.navigate([`/quiz/${quizID}`]);
  }

  NavigateToHome() {
    this._router.navigate(["/landing"])
  }

  checkSession(){
    console.log(this.baseUrl+'User/CheckSession');
    this._http.get(this.baseUrl+'User/CheckSession').subscribe(
      (result) => {
        if (result == null) {
          this._router.navigate(["/login"])
        }
        // console.log("my result is", result);
        this.user = result;
        // this.find_online_users();
      }, error => console.error(error)
    )
  }

  ngOnInit() {
    this.checkSession();
    this.getQuizes();
  }

}

interface Quiz {
  name: string
  quizID: number
  questions: object[];
}

