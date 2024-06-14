import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Movie } from '../../../common/models/movie';
import { Observable } from 'rxjs';
import { Response } from '../../../common/models/response';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private Url = environment.apiUrl

  constructor(private httpClient: HttpClient) { }

  public addMovie = (model: Movie): Observable<Response<null>> => {
    return this.httpClient.post<Response<null>>(`${this.Url}/admin/addmovie`, model);
  }

  public editMovie = (model: Movie): Observable<Response<null>> => {
    return this.httpClient.post<Response<null>>(`${this.Url}/admin/updatemovie`, model)
  }

  public deleteMovie = (id: number | null): Observable<Response<null>> => {
    return this.httpClient.delete<Response<null>>(`${this.Url}/admin/deletemovie?id=${id}`)
  }

}
