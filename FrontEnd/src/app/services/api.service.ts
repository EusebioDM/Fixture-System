import { Injectable } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private token: string;

  private loginData = {
    userName: 'admin',
    password: 'admin'
  };

  constructor(public http: Http) { }

  public login(): Promise<any> {
    if (this.token) {
      return new Promise(resolve => { resolve(this.token); });
    } else {
      return new Promise((resolve, reject) => {
        const body = JSON.stringify(this.loginData);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        this.http.post('api/auth', body, options);
      });
    }
  }

  public get(path: string): Promise<any> {
    return new Promise((resolve, reject) => {

    });
  }
}
