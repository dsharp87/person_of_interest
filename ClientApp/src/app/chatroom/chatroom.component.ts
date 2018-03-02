import { Component, OnInit, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HubConnection } from '@aspnet/signalr-client';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit {
  private _hubConnection: HubConnection;
  // private _http:Http
  public async: any;
  message = '';
  messages: string[] = [];
  user: object;
  online_users : any;
  baseUrl : String;
  chatID : String;
  chatPerson : String;
  OnlineList : any;

  constructor(private _router: Router, private _http:HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.user = {firstName : "Per", userID:1, quizResults:[]};
    this.baseUrl = baseUrl;
    this.chatID = "";
    this.chatPerson = "";
    this.online_users = [];
   }

  public sendMessage(): void {
    const data = `${this.user['firstName']}: ${this.message}`;

    this._hubConnection.invoke('Send', data, this.chatID);
    this.messages.push(data);
  }

  public ChatUser(connectionID, targetPerson){
    this.chatID = connectionID;
    this.chatPerson = targetPerson;
  }


  ngOnInit() {
    this.checkSession();
    // this._hubConnection = new HubConnection('/chathub');
    // this._hubConnection.on('Send', (data: any) => {
    //   const received = `${data}`;
    //   this.messages.push(received);
    // });

    // this._hubConnection.start()
    //   .then(() => {
    //     console.log('Hub connection started')
    //   })
    //   .catch(err => {
    //     console.log('Error while establishing connection')
    //   });
  }

  checkSession(){
    console.log(this.baseUrl+'User/CheckSessionCookieMaker');
    this._http.get(this.baseUrl+'User/CheckSessionCookieMaker').subscribe(
      (result) => {
        if (result == null) {
          this._router.navigate(["/login"])
        }
        console.log("my result is", result);
        this.user = result;
        this.find_online_users();
        this._hubConnection = new HubConnection('/chathub');
        this._hubConnection.on('Send', (data: any) => {
          const received = `${data}`;
          this.messages.push(received);
        });
    
        this._hubConnection.start()
          .then(() => {
            console.log('Hub connection started')
          })
          .catch(err => {
            console.log('Error while establishing connection')
          });

      }, error => console.error(error)
    )
  }

  find_online_users(){
    this._http.get(this.baseUrl+'Chat/OnlineUsers').subscribe(
      (result) => {
        this.online_users = [];
        this.OnlineList = result
        console.log(this.OnlineList);
        for(var u of this.OnlineList){
          console.log(u,"line96")
          if(u.userID == this.user['userID']){
            continue
          }
          u['matchRes'] =[];
          for(var q of u.quizResults){
            var qID = q.quizID;
            var len = q.resultString.length;
            var matchCount = 0;
            var userQidIdx = this.user['quizResults'].findIndex(q => q.quizID == qID);
            if(userQidIdx == -1){
              continue
            }
            for(let i=0; i<len;i++){
              if(q.resultString[i]==this.user['quizResults'][userQidIdx].resultString[i]){
                matchCount++
              }
            }
            u['matchRes'].push([q.quizID, matchCount/len]);
          }
          
          this.online_users.push(u);
        };

        // this.online_users = this.OnlineList;

      }, error => console.error(error)
    );
  }

}
