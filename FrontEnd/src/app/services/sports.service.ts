import { Injectable } from '@angular/core';
import { Sport } from '../classes/sport';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SportsService {

  private sportsUrl = 'api/sports';
  token = localStorage.getItem('access_token');

  constructor(private httpService: Http) { }

  getSports(): Observable<Array<Sport>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.sportsUrl, requestOptions)
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

    return this.httpService.get(this.sportsUrl, requestOptions)
      .pipe(
        map((response: Response) => <Sport>response.json().find((s: Sport) => s.name = id)),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
