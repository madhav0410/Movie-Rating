import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { Response } from '../../../common/models/response';
import { environment } from '../../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  
  public avgRating: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  private Url= environment.apiUrl

  constructor(private httpClient: HttpClient,private router: Router) { }

  public getUserRating = (email:string,title:string): Observable<Response<number>> => {
    return this.httpClient.get<Response<number>>(`${this.Url}/user/get-user-rating?email=${email}&title=${title}`)
  }
  
  public getAvgRating = (title:string): Observable<Response<number>> => {
    return this.httpClient.get<Response<number>>(`${this.Url}/user/get-avg-rating?title=${title}`)
  }
  
  public updateRating = (email:string,title:string,rating:number): Observable<Response<null>> => {
    return this.httpClient.post<Response<null>>(`${this.Url}/user/update-rating?email=${email}&title=${title}&rating=${rating}`,{})
  }
}
