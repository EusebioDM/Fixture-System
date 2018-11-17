import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { LoginService } from 'src/app/services/login/login.service';
import { Encounter } from 'src/app/classes/encounter';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  constructor(
    private loginService: LoginService,
    private usersService: UsersService
  ) { }

  isAdmin: boolean;
  userFollowedTeamsEncounters: Array<Encounter>;
  comments: Array<Comment>;

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.getData();
  }

  private getData() {
    this.usersService.getUserComments().subscribe(
      ((data: Array<Comment>) => { this.comments = data; }),
      ((error: any) => console.log(error))
    );
    this.usersService.getFollowedTeamEncounters().subscribe(
      ((data: Array<Encounter>) => { this.userFollowedTeamsEncounters = data; }),
      ((error: any) => console.log(error))
    );
  }
}
