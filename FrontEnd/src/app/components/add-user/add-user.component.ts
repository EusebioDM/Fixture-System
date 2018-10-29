import { Component, OnInit, EventEmitter, Output } from '@angular/core';
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

  @Output() usersToParent = new EventEmitter<User>();

  constructor(private usersService: UsersService) { }

  ngOnInit() {
  }

  public submit() {
    console.log('Se envia: ' + this.username);
    const user = new User(this.username, this.name, this.surname, this.password, this.mail, this.role);
    this.usersService.addUser(user).subscribe(result => {
      this.usersToParent.emit(user);
    });
  }
}
