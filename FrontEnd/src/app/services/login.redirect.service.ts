import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { LoginService } from './login/login.service';

@Injectable({
  providedIn: 'root'
})
export class LoginRedirectService implements CanActivate {

  constructor(public auth: LoginService, public router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {

    const role = this.auth.getLoggedUserRole();
    if (this.auth.loggedIn) {
      if (role === 'Administrator') {
        this.router.navigate(['users']);
        return true;
      } else if (role === 'Follower') {
        this.router.navigate(['calendar']);
        return true;
      }
      return false;
    }
  }
}
