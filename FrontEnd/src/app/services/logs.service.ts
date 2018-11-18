import { Injectable } from '@angular/core';
import { Log } from '../classes/log';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LogsService {

  users: Array<Log>;
  private logsUrl = 'api/log';
  token = localStorage.getItem('access_token');

  constructor(private httpService: Http) { }

  getLogs(): Observable<Array<Log>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.logsUrl, requestOptions)
      .pipe(
        map((response: Response) => <Array<Log>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getLogsFromToDate(dateFrom: string, dateTo: string): Observable<Array<Log>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.logsUrl + '?start=' + dateFrom + '&end=' + dateTo, requestOptions)
      .pipe(
        map((response: Response) => <Array<Log>>response.json()),
        tap(data => console.log('Obtained encounter: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
