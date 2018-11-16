import { Component, OnInit } from '@angular/core';
import { LoginService } from '../../services/login/login.service';

@Component({
  selector: 'app-admin-nav',
  templateUrl: './admin-nav.component.html',
  styleUrls: ['./admin-nav.component.css']
})
export class AdminNavComponent implements OnInit {

  constructor(private loginService: LoginService) { }

  loggedUser: string;

  ngOnInit() {
    this.loggedUser = this.loginService.getLoggedUserName();
  }

  public signOut() {
    this.loginService.logout();
  }
}
