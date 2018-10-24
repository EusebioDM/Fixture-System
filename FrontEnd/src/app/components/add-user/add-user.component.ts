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
    //let user = new User(this.username, this.name, this.surname, this.mail, this.password, this.role);
    //this.usersService.addUser();
    console.log('El form se está enviando ' + this.username + ' ' + this.role);
  }


}
