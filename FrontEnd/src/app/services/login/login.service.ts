import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable()
export class LoginService {

  private LOGIN_URL = environment.WEB_API_URL + '/api/auth';

  constructor(
    private http: HttpClient,
    private router: Router) { }

  login(username: string, password: string): Observable<boolean> {
    return this.http.post<{ token: string }>(this.LOGIN_URL, { username: username, password: password })
      .pipe(
        map(result => {
          localStorage.setItem('access_token', result.token);
          return true;
        })
      );
  }

  logout() {
    localStorage.removeItem('access_token');
    this.router.navigate(['login']);
    console.log('Token ' + localStorage.getItem('access_token'));
  }

  public get loggedIn(): boolean {
    return (localStorage.getItem('access_token') !== null);
  }

  getLoggedUserName(): string {
    const token = localStorage.getItem('access_token');
    if (token !== null) {
      const tokenPayload = (token);
      const jwtData = tokenPayload.split('.')[1];
      const decodedJwtJsonData = window.atob(jwtData);
      const decodedJwtData = JSON.parse(decodedJwtJsonData);
      const userName = decodedJwtData['UserName'];
      return userName;
    }
  }

  getLoggedUserRole(): string {
    const token = localStorage.getItem('access_token');
    if (token !== null) {
      const tokenPayload = (token);
      const jwtData = tokenPayload.split('.')[1];
      const decodedJwtJsonData = window.atob(jwtData);
      const decodedJwtData = JSON.parse(decodedJwtJsonData);
      const role = decodedJwtData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return role;
    }
  }
}
