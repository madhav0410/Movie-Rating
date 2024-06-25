import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Movie, PaginatedMovies } from '../models/movie';
import { BehaviorSubject, Observable } from 'rxjs';
import { Response } from '../models/response';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  
  private Url= environment.apiUrl;
  constructor(private httpClient: HttpClient) { }

  public movieList: BehaviorSubject<Movie[]> = new BehaviorSubject<Movie[]>([]);
  public currentPage: BehaviorSubject<number> = new BehaviorSubject<number>(1);

  public getMovies = (page:number,pageSize:number,selectedGenres: string[],searchQuery: string): Observable<Response<PaginatedMovies>> => {

    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('genres', selectedGenres.join(','))
      .set('searchQuery', searchQuery);

    return this.httpClient.get<Response<PaginatedMovies>>(`${this.Url}/movie/getallmovies`, { params })
  } 

  public getMovieByTitle = (title: string): Observable<Response<Movie>> => {
    return this.httpClient.get<Response<Movie>>(`${this.Url}/movie/getmovie?title=${title}`)
  }
}
