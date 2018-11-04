import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { User } from '../../classes/user';
import { FormBuilder, FormGroup, Validators, FormControl, FormGroupDirective, NgForm, AbstractControl } from '@angular/forms';
import { compareValidator } from 'src/app/shared/compare-validator.directive';
import { uniqueUsernameValidator } from 'src/app/shared/unique-username-validator.directive';
import { ErrorStateMatcher } from '@angular/material';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private usersService: UsersService) { }

  addUserForm: FormGroup;
  error: string;

  @Output() usersToParent = new EventEmitter<User>();

  ngOnInit() {
    this.createAddUserForm();
  }

  createAddUserForm() {
    this.addUserForm = this.formBuilder.group({
      username: ['',
        null,
        uniqueUsernameValidator(this.usersService) // async
      ],
      name: ['',
        Validators.required
      ],
      surname: ['',
        Validators.required
      ],
      mail: ['',
        Validators.email
      ],
      password: ['',
        Validators.required
      ],
      passwordConfirm: ['',
        compareValidator('password')
      ],
      role: ['',
        Validators.required
      ],
    });
  }

  get username() {
    return this.addUserForm.get('username');
  }

  get name() {
    return this.addUserForm.get('name');
  }

  get surname() {
    return this.addUserForm.get('surname');
  }

  get mail() {
    return this.addUserForm.get('mail');
  }

  get password() {
    return this.addUserForm.get('password');
  }

  get passwordConfirm() {
    return this.addUserForm.get('passwordConfirm');
  }

  get role() {
    return this.addUserForm.get('role');
  }

  public submit() {
    const user = this.addUserForm.value;
    this.usersService.addUser(user).subscribe(
      result => { },
      err => this.error = 'Error al agregar usuario'
    );
  }

  matcher = new MyErrorStateMatcher();
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
