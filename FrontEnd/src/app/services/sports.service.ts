import { Injectable } from '@angular/core';
import { Sport } from '../classes/sport';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { Encounter } from '../classes/encounter';
import { Team } from '../classes/team';
import { TeamPosition } from '../classes/team-position';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SportsService {

  private SPORTS_URL = environment.WEB_API_URL + '/api/sports/';

  constructor(private httpService: Http) { }

  getSports(): Observable<Array<Sport>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.SPORTS_URL, requestOptions)
      .pipe(
        map((response: Response) => <Array<Sport>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getSportById(id: string): Observable<Sport> {
    const myHeaders = new Headers();
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.SPORTS_URL + id, requestOptions)
      .pipe(
        map((response: Response) => <Sport>response.json()),
        tap(data => console.log('Obtained sport: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addSport(sport: Sport) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.SPORTS_URL, sport, requestOptions).pipe(
      tap((s: Sport) => console.log(`added sport w/ id=${s.name}`)),
      catchError(this.handleError)
    );
  }

  deleteSport(id: string) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.delete(this.SPORTS_URL + id, requestOptions);
  }

  getTeamsBySport(sportId: string): Observable<Array<Team>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.SPORTS_URL + sportId + '/teams', requestOptions)
      .pipe(
        map((response: Response) => <Array<Team>>response.json()),
        tap(data => console.log('Obtained sport: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getEncountersBySport(sportId: string): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.SPORTS_URL + sportId + '/encounters', requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained sport: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getTeamPositionsBySport(sportId: string): Observable<Array<TeamPosition>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.SPORTS_URL + sportId + '/results', requestOptions)
      .pipe(
        map((response: Response) => <Array<TeamPosition>>response.json()),
        tap(data => console.log('Obtained sport: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json() || 'Server error');
  }
}
