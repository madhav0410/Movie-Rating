import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Login } from '../models/login';
import { Signup } from '../models/signup';
import { Response } from '../../../common/models/response';
import { ResetPassword } from '../models/resetPassword';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private Url = environment.apiUrl;

  constructor(private httpClient: HttpClient, private router: Router) {}

  public signUp(model: Signup): Observable<Response<null>> {
    return this.httpClient.post<Response<null>>(`${this.Url}/auth/signup`, model)
  }

  public login(model: Login): Observable<Response<string>> {
    return this.httpClient.post<Response<string>>(`${this.Url}/auth/login`, model)
  }

  public signOut(): void {
    localStorage.clear()
    this.router.navigate(['/login'])
  }

  public storeToken(tokenValue: string): void {
    localStorage.setItem('token', tokenValue)
  }

  public getToken(): string | null{
    return localStorage.getItem('token')
  }

  public isLoggedIn(): boolean {
    return !!localStorage.getItem('token')
  }

  public getEmailFromToken(): string {
    const payLoad: any = this.decodedToken()
    return payLoad ? payLoad.email : "";
  }
  
  public getRoleFromToken(): string {
    const payLoad: any = this.decodedToken()
    return payLoad ? payLoad.role : "";
  }

  public forgotPassword(email: string): Observable<Response<null>> {
    return this.httpClient.post<Response<null>>(`${this.Url}/auth/forgotPassword?email=${email}`, {});
  }

  public resetPassword = (email: string, time: string, model: ResetPassword): Observable<Response<null>> => {
    return this.httpClient.post<Response<null>>(`${this.Url}/auth/resetPassword?email=${email}&time=${time}`, model)
  }

  public getDashboardUrl(): string {
    const userRole: string | null = this.getRoleFromToken() || null
    return userRole == "1" ? `/admin/dashboard` : (userRole == "2" ? `/user/dashboard` : `/`)
  }

  private decodedToken() {
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    return jwtHelper.decodeToken(token)
  }

}
