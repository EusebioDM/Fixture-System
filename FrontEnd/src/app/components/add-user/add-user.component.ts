import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { User } from '../../classes/user';
import { FormControl, FormGroupDirective, NgForm, Validators, ValidatorFn } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  constructor(private usersService: UsersService) { }

  username: string;
  name: string;
  surname: string;
  mail: string;
  password: string;
  passwordRepeated: string;
  role: string;

  @Output() usersToParent = new EventEmitter<User>();

  passwordMatchFormControl = new FormControl('', [
    Validators.required
  ]);

  requiredFormControl = new FormControl('', [
    Validators.required,
  ]);

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);


  matcher = new FormErrorStateMatcher();

  ngOnInit() {
  }

  onRepeatPasswordChange() {
    if (this.password !== this.passwordRepeated) {
      console.log('Las contraseñas no coinciden');
    }
  }

  public submit() {
    console.log('Se envia: ' + this.username);
    const user = new User(this.username, this.name, this.surname, this.password, this.mail, this.role);
    this.usersService.addUser(user).subscribe(result => {
      this.usersToParent.emit(user);
    });
  }
}

export class FormErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
