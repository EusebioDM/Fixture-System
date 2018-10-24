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

  constructor(private auth: LoginService, private router: Router) { }

  public submit() {
    if (this.validateFields()) {

      this.auth.login(this.username, this.password)
        .pipe(first())
        .subscribe(
          result => this.router.navigate(['administrator']),
          err => this.error = 'Usuario y/o contraseña invalido/s'
        );
    }
  }

  // why doesn't work??
  private validateFields(): boolean {
    if (this.username === '' || this.password === '') {
      this.error = 'Debe completar los campos';
      return false;
    }
    return true;
  }
}
