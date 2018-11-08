import { Injectable } from '@angular/core';
import { Encounter } from '../classes/encounter';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EncountersService {

  private encountersUrl = 'api/encounters';
  token = localStorage.getItem('access_token');

  constructor(private httpService: Http) { }

  getEncounters(): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.encountersUrl, requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getEnconutertById(id: string): Observable<Encounter> {
    const myHeaders = new Headers();
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.encountersUrl + '/' + id, requestOptions)
      .pipe(
        map((response: Response) => <Encounter>response.json()),
        tap(data => console.log('Obtained encounter: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addEncounter(encounter: Encounter) {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.encountersUrl, encounter, requestOptions).pipe(
      tap((e: Encounter) => console.log(`added encounter`)),
      catchError(this.handleError)
    );
  }

  deleteEncounter(id: string) {
    const myHeaders = new Headers({ Authorization: `Bearer ${this.token}` });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.delete(this.encountersUrl + '/' + id, requestOptions);
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json().error || 'Server error');
  }
}
