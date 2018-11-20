import { Injectable } from '@angular/core';
import { User } from '../classes/user';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { Team } from '../classes/team';
import { Encounter } from '../classes/encounter';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private USERS_URL = environment.WEB_API_URL + '/api/users/';

  constructor(private httpService: Http) { }

  getUsers(): Observable<Array<User>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.USERS_URL, requestOptions)
      .pipe(
        map((response: Response) => <Array<User>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getUserById(id: string): Observable<User | undefined> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.USERS_URL + id, requestOptions)
      .pipe(
        map((response: Response) => <User>response.json()),
        tap(data => console.log('Obtained user: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addUser(user: User) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.USERS_URL, user, requestOptions).pipe(
      tap((u: User) => console.log(`added user w/ id=${u.userName}`)),
      catchError(this.handleError)
    );
  }

  updateUser(user: User): Observable<any> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.put(this.USERS_URL + user.userName, user, requestOptions).pipe(
      tap(_ => console.log(`updated user id=${user.userName}`)),
      catchError(this.handleError)
    );
  }

  deleteUser(id: string) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.delete(this.USERS_URL + id, requestOptions);
  }

  getUserComments(): Observable<Array<Comment>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.USERS_URL + 'commentaries', requestOptions)
      .pipe(
        map((response: Response) => <Array<Comment>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getUserFollowedTeams(): Observable<Array<Team>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.get(this.USERS_URL + 'followers', requestOptions)
      .pipe(
        map((response: Response) => <Array<Team>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getFollowedTeamEncounters(): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.get(this.USERS_URL + 'encounters', requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json() || 'Server error');
  }
}
