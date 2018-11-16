import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';
import { LoginService } from 'src/app/services/login/login.service';

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
  comments: Array<Comment>;

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.getData();
  }

  private getData() {
    this.usersService.getUserComments().subscribe(
      ((data: Array<Comment>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Comment>): void {
    this.comments = data;
  }
}
