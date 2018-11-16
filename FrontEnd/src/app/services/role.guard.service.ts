import { Injectable } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot
} from '@angular/router';
import { LoginService } from './login/login.service';

@Injectable()
export class RoleGuardService implements CanActivate {
  constructor(public auth: LoginService, public router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {

    const expectedRole = route.data.expectedRole;
    const role = this.auth.getLoggedUserRole();

    if (
      !this.auth.loggedIn ||
      !expectedRole.includes(role)
    ) {
      this.router.navigate(['login']);
      return false;
    }
    return true;
  }
}
