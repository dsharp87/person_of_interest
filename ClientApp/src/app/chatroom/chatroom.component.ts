import { Component, OnInit, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HubConnection } from '@aspnet/signalr-client';
import { Http } from '@angular/http';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit {
  private _hubConnection: HubConnection;
  private _http:Http
  public async: any;
  message = '';
  messages: string[] = [];
  user: object;
  online_users : object[];
  baseUrl : String;

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.user = {first_name : "Per"};
    this.baseUrl = baseUrl;
   }

  public sendMessage(): void {
    const data = `${this.user['first_name']}: ${this.message}`;

    this._hubConnection.invoke('Send', data);
    // this.messages.push(data);
  }


  ngOnInit() {
    // this.checkSession();
    // this.find_online_users();
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
