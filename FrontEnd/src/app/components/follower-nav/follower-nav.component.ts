import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-follower-nav',
  templateUrl: './follower-nav.component.html',
  styleUrls: ['./follower-nav.component.css']
})
export class FollowerNavComponent implements OnInit {

  constructor(private loginService: LoginService) { }

  ngOnInit() {
  }

  signOut() {
    this.loginService.logout();
  }
}
