import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  baseUrl:string;
  user: object;

  constructor(private _router: Router, private _http:HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.checkSession();
  }

  NavigateToQuizSelection() {
    this._router.navigate(["/quizSelection"])
  }

  NavigateToChat() {
    this._router.navigate(["/chat"])
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
    
  }

}
