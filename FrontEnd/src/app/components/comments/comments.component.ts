import { Component, OnInit } from '@angular/core';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  constructor(
    private usersService: UsersService
  ) { }

  comments: Array<Comment>;

  ngOnInit() {
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
