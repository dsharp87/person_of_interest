import { Component, OnInit, Inject } from '@angular/core';
import { Http } from '@angular/http';


@Component({
  selector: 'app-select-user',
  templateUrl: './select-user.component.html',
  styleUrls: ['./select-user.component.css']
})
export class SelectUserComponent implements OnInit {
  online_users : object[];
  user : object;
  baseUrl : String;
  constructor(private _http:Http, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.checkSession();
    this.find_online_users();
  }

  checkSession(){
    this._http.get(this.baseUrl+'User/GetSession').subscribe(
      (result) => {
        console.log(result);
        this.user = result;
      }, error => console.error(error)
    )
  }

  find_online_users(){
    this._http.get(this.baseUrl+'Chat/OnlineUsers').subscribe(
      (result) => {
        console.log(result);
        this.online_users = [result];
      }, error => console.error(error)
    );
  }

}
