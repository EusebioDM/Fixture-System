import { Component, OnInit, Input, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Inject } from '@angular/core';
import { User } from '../../classes/user';
import { UsersService } from 'src/app/services/users.service';
import { UsersListComponent } from '../users/users.component';

@Component({
  selector: 'app-modify-user',
  templateUrl: './modify-user.component.html',
  styleUrls: ['./modify-user.component.css']
})
export class ModifyUserComponent implements OnInit {

  constructor(
    @Optional() public dialogRef: MatDialogRef<UsersListComponent>,
    private usersService: UsersService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: User
  ) { }

  @Input() userId: string;

  error: string;
  username: string;
  name: string;
  surname: string;
  mail: string;
  password: string;
  passwordRepeated: string;
  isAdmin: boolean;

  user: User;

  ngOnInit() {
    if (this.data) {
      this.username = this.data.userName;
      this.name = this.data.name;
      this.surname = this.data.surname;
      this.mail = this.data.mail;
    }
  }

  public submit() {
    const user = new User(this.username, this.name, this.surname, this.password, this.mail, this.isAdmin);
    user.userName = this.data.userName;
    this.usersService.updateUser(user).subscribe(
      (() => {
        this.dialogRef.close(user);
      }),
      (error) => this.error = error
    );
  }
}
