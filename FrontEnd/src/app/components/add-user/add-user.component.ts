import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { User } from '../../classes/user';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  username: string;
  name: string;
  surname: string;
  mail: string;
  password: string;
  passwordRepeated: string;
  role: string;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
  }

  public submit() {
    const user = new User(this.username, this.name, this.surname, this.password, this.mail, this.role);
    console.log(user);
    this.usersService.addUser(user);
    console.log('El form se está enviando ' + this.username + ' ' + this.role);
  }


}
