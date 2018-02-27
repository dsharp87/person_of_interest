import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HubConnection } from '@aspnet/signalr-client';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit {
  private _hubConnection: HubConnection;
  public async: any;
  message = '';
  messages: string[] = [];

  constructor() { }

  public sendMessage(): void {
    const data = `UserName: ${this.message}`;

    this._hubConnection.invoke('Send', data);
    this.messages.push(data);
  }


  ngOnInit() {
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

}
