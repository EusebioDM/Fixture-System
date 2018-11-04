import { Injectable } from '@angular/core';
import { User } from '../classes/user';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  users: Array<User>;
  private usersUrl = 'api/users';
  token = localStorage.getItem('access_token');

  constructor(private httpService: Http) { }

  getUsers(): Observable<Array<User>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.usersUrl, requestOptions)
      .pipe(
        map((response: Response) => <Array<User>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getUserById(id: string): Observable<User | undefined> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.usersUrl + '/' + id, requestOptions)
      .pipe(
        map((response: Response) => <User>response.json()),
        tap(data => console.log('Obtained user: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addUser(user: User) {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.usersUrl, user, requestOptions).pipe(
      tap((u: User) => console.log(`added user w/ id=${u.userName}`)),
      catchError(this.handleError)
    );
  }

  deleteUser(id: string) {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.delete(this.usersUrl + '/' + id, requestOptions);
  }

  updateUser(user: User): Observable<any> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.put(this.usersUrl + '/' + user.userName, user, requestOptions).pipe(
      tap(_ => console.log(`updated user id=${user.userName}`)),
      catchError(this.handleError)
    );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
