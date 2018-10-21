import { Injectable } from '@angular/core';
import { User } from '../classes/user';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  token = localStorage.getItem('access_token');
  constructor(private httpService: Http) { }
  getUsers(): Observable<Array<User>> {
    const myHeaders = new Headers({ Authorization:  `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get('/api/users', requestOptions)
      .pipe(
        map((response: Response) => <Array<User>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getUserById(id: string): Observable<User> {
    const myHeaders = new Headers();
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get('/api/users', requestOptions)
      .pipe(
        map((response: Response) => <User>response.json().find((user: User) => user.userName = id)),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
