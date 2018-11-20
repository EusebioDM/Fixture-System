import { Injectable } from '@angular/core';
import { Team } from '../classes/team';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { Encounter } from '../classes/encounter';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TeamsService {

  private TEAMS_URL = environment.WEB_API_URL + '/api/teams/';

  constructor(private httpService: Http) { }

  getTeams(): Observable<Array<Team>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.TEAMS_URL, requestOptions)
      .pipe(
        map((response: Response) => <Array<Team>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getTeamById(id: string): Observable<Team> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.TEAMS_URL + id, requestOptions)
      .pipe(
        map((response: Response) => <Team>response.json()),
        tap(data => console.log('Obtained team: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addTeam(team: Team) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.TEAMS_URL, team, requestOptions).pipe(
      tap((t: Team) => console.log(`added team w/ id=${t.name}`)),
      catchError(this.handleError)
    );
  }

  updateTeam(team: Team): Observable<any> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.put(this.TEAMS_URL + team.name + '_' + team.sportName, team, requestOptions).pipe(
      tap(_ => console.log(`updated team id=${team.name + '_' + team.sportName}`)),
      catchError(this.handleError)
    );
  }

  deleteTeam(id: string) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    console.log(this.TEAMS_URL + id);
    return this.httpService.delete(this.TEAMS_URL + id, requestOptions);
  }

  getEncountersByTeams(teamId: string): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.TEAMS_URL + teamId + '/encounters', requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addFollowedTeamToLoggedUser(id: string) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.TEAMS_URL + id + '/follower', null, requestOptions).pipe(
      tap((t: Team) => console.log(`added team w/ id=${t.name}`)),
      catchError(this.handleError)
    );
  }

  deleteFollowedTeamToLoggedUser(id: string) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.delete(this.TEAMS_URL + id + '/follower', requestOptions);
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().Name || 'Server error');
  }
}
