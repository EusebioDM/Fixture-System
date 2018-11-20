import { Injectable } from '@angular/core';
import { Log } from '../classes/log';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LogsService {

  private LOGS_URL = environment.WEB_API_URL + '/api/log';

  constructor(private httpService: Http) { }

  getLogs(): Observable<Array<Log>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.LOGS_URL, requestOptions)
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

    return this.httpService.get(this.LOGS_URL + '?start=' + dateFrom + '&end=' + dateTo, requestOptions)
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
