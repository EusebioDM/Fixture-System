import { Injectable } from '@angular/core';
import { Team } from '../classes/team';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { Encounter } from '../classes/encounter';

@Injectable({
  providedIn: 'root'
})
export class TeamsService {

  private teamsUrl = 'api/teams';
  token = localStorage.getItem('access_token');

  constructor(private httpService: Http) { }

  getTeams(): Observable<Array<Team>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.teamsUrl, requestOptions)
      .pipe(
        map((response: Response) => <Array<Team>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getTeamById(id: string): Observable<Team> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.teamsUrl + '/' + id, requestOptions)
      .pipe(
        map((response: Response) => <Team>response.json()),
        tap(data => console.log('Obtained team: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addTeam(team: Team) {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.teamsUrl, team, requestOptions).pipe(
      tap((t: Team) => console.log(`added team w/ id=${t.name}`)),
      catchError(this.handleError)
    );
  }

  updateTeam(team: Team): Observable<any> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.put(this.teamsUrl + '/' + team.name + '_' + team.sportName, team, requestOptions).pipe(
      tap(_ => console.log(`updated team id=${team.name + '_' + team.sportName}`)),
      catchError(this.handleError)
    );
  }

  deleteTeam(id: string) {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    console.log(this.teamsUrl + id);
    return this.httpService.delete(this.teamsUrl + '/' + id, requestOptions);
  }

  getEncountersByTeams(teamId: string): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.teamsUrl + teamId + '/encounters', requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
