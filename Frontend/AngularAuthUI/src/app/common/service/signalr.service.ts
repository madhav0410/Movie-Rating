import { Injectable } from '@angular/core';
import signalR, { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  
  private hubConnection!: HubConnection;
  constructor() { }

  startConnection = () => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.signalRHubUrl) // Replace with your SignalR Hub URL
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection started'))
      .catch(err => console.error('Error while starting SignalR connection: ' + err));
  }

  addReceiveCommentListener = (callback: (title: string, comment: string) => void) => {
    this.hubConnection.on('ReceiveComment', (title: string, comment: string) => {
      callback(title, comment);
    });
  }

  invokeAddComment = async (title: string, email:string, commentText: string) => {
    try {
      await this.hubConnection.invoke('AddComment', title, commentText);
    } catch (err) {
      console.error('Error while invoking AddComment: ' + err);
    }
  }
}
