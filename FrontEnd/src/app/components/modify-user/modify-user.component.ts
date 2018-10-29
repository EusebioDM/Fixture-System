import { Component, OnInit } from '@angular/core';
import { User } from '../../classes/user';
import { from } from 'rxjs';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-modify-user',
  templateUrl: './modify-user.component.html',
  styleUrls: ['./modify-user.component.css']
})
export class ModifyUserComponent implements OnInit {

  constructor(private usersService: UsersService) { }

  username: string;
  name: string;
  surname: string;
  mail: string;
  password: string;
  passwordRepeated: string;
  role: string;


  ngOnInit() {
  }

  public submit() {
    const user = new User(this.username, this.name, this.surname, this.password, this.mail, this.role);
    this.usersService.updateUser(user).subscribe(result => {
      console.log('Se actualizó: ' + user.userName);
    });
  }

}
