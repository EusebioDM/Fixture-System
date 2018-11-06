import { Component, OnInit, Input, Optional } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material';
import { Inject } from '@angular/core';
import { User } from '../../classes/user';
import { from } from 'rxjs';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-modify-user',
  templateUrl: './modify-user.component.html',
  styleUrls: ['./modify-user.component.css']
})
export class ModifyUserComponent implements OnInit {

  constructor(
    private usersService: UsersService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: User
  ) { }

  @Input() userId: string;

  username: string;
  name: string;
  surname: string;
  mail: string;
  password: string;
  passwordRepeated: string;
  role: string;


  ngOnInit() {
    if (this.data) {
      this.username = this.data.userName;
      this.name = this.data.name;
      this.surname = this.data.surname;
      this.mail = this.data.mail;
    }
  }

  public submit() {

    const user = new User(this.username, this.name, this.surname, this.password, this.mail, this.role);
    user.userName = this.data.userName;
    this.usersService.updateUser(user).subscribe(result => {
      console.log('Se actualizó: ' + user.userName);
    });
  }
}
