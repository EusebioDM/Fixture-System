import { Injectable } from '@angular/core';
import { Team } from '../classes/team';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TeamsService {

  token = localStorage.getItem('access_token');
  constructor(private httpService: Http) { }
  getTeams(): Observable<Array<Team>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get('/api/teams', requestOptions)
      .pipe(
        map((response: Response) => <Array<Team>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getTeamById(id: string): Observable<Team> {
    const myHeaders = new Headers();
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get('/api/teams', requestOptions)
      .pipe(
        map((response: Response) => <Team>response.json().find((team: Team) => team.name = id)),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
