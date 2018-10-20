import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  title = 'login';
  password: string;
  username: string;
  constructor(private _service: LoginService) {

  }

  ngOnInit(): void {
     this._service.getToken();
  }

}
