import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public username: string;
  public password: string;
  public error: string;

  constructor(private loginService: LoginService, private router: Router) { }

  public submit() {


    this.loginService.login(this.username, this.password)
      .pipe(first())
      .subscribe(
        result => {
          if (this.loginService.getLoggedUserRole() === 'Administrator') {
            this.router.navigate(['users']);
          } else {
            this.router.navigate(['calendar']);
          }
        },
        err => this.error = 'Usuario y/o contraseña invalido/s'
      );
  }
}

