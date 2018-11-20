import { Injectable } from '@angular/core';
import { Encounter } from '../classes/encounter';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { Fixture } from '../classes/fixture';
import { CommentIn } from '../classes/comment-in';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EncountersService {

  private ENCOUNTERS_URL = environment.WEB_API_URL + '/api/encounters/';

  constructor(private httpService: Http) { }

  getEncounters(): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.ENCOUNTERS_URL, requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getEnconutersById(id: string): Observable<Encounter> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.ENCOUNTERS_URL + id, requestOptions)
      .pipe(
        map((response: Response) => <Encounter>response.json()),
        tap(data => console.log('Obtained encounter: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  getEnconutersFromToDate(dateFrom: string, dateTo: string): Observable<Array<Encounter>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.ENCOUNTERS_URL + '?start=' + dateFrom + '&end=' + dateTo, requestOptions)
      .pipe(
        map((response: Response) => <Array<Encounter>>response.json()),
        tap(data => console.log('Obtained encounter: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addEncounter(encounter: Encounter) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.ENCOUNTERS_URL, encounter, requestOptions).pipe(
      tap((e: Encounter) => console.log(`added encounter`)),
      catchError(this.handleError)
    );
  }

  updateEncounter(encounter: Encounter): Observable<any> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });
    return this.httpService.put(this.ENCOUNTERS_URL + encounter.id, encounter, requestOptions).pipe(
      tap(_ => console.log(`updated user id=${encounter.id}`)),
      catchError(this.handleError)
    );
  }

  deleteEncounter(id: string) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.delete(this.ENCOUNTERS_URL + id, requestOptions);
  }

  GetAvailableFixtureGenerators(): Observable<Array<string>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.ENCOUNTERS_URL + 'fixture', requestOptions)
      .pipe(
        map((response: Response) => <Array<string>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  createFixture(fixture: Fixture) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.ENCOUNTERS_URL + 'fixture', fixture, requestOptions).pipe(
      tap((f: Fixture) => console.log(`generated fixture`)),
      catchError(this.handleError)
    );
  }

  getEncounterComments(id: string): Observable<Array<Comment>> {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.get(this.ENCOUNTERS_URL + id + '/commentaries', requestOptions)
      .pipe(
        map((response: Response) => <Array<Comment>>response.json()),
        tap(data => console.log('Obtained data: ' + JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  addCommentToEncounter(encounterId: string, message: CommentIn) {
    const myHeaders = new Headers({ Authorization: 'Bearer ' + localStorage.getItem('access_token') });
    myHeaders.append('Accept', 'application/json');
    const requestOptions = new RequestOptions({ headers: myHeaders });

    return this.httpService.post(this.ENCOUNTERS_URL + encounterId + '/commentaries', message, requestOptions).pipe(
      tap((s: string) => console.log(`added comment`)),
      catchError(this.handleError)
    );
  }

  private handleError(error: Response) {
    console.error(error);
    return throwError(error.json() || 'Server error');
  }
}
