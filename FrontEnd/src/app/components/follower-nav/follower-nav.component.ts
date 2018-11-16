import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-follower-nav',
  templateUrl: './follower-nav.component.html',
  styleUrls: ['./follower-nav.component.css']
})
export class FollowerNavComponent implements OnInit {

  constructor(private loginService: LoginService) { }

  loggedUser: string;

  ngOnInit() {
    this.loggedUser = this.loginService.getLoggedUserName();
  }

  signOut() {
    this.loginService.logout();
  }
}
