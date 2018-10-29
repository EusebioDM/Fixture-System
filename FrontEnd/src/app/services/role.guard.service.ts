import { Injectable } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot
} from '@angular/router';
import { LoginService } from './login/login.service';
import * as jwtDecode from 'jwt-decode';

@Injectable()
export class RoleGuardService implements CanActivate {
  constructor(public auth: LoginService, public router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {

    const expectedRole = route.data.expectedRole;
    const token = localStorage.getItem('access_token');
    const tokenPayload = (token);


    const jwtData = tokenPayload.split('.')[1];
    const decodedJwtJsonData = window.atob(jwtData);
    const decodedJwtData = JSON.parse(decodedJwtJsonData);
    const role = decodedJwtData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (
      !this.auth.loggedIn ||
      role !== expectedRole
    ) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}
