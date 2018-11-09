import { Component, OnInit, EventEmitter, Output, Inject } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { User } from '../../classes/user';
import { FormBuilder, FormGroup, Validators, FormControl, FormGroupDirective, NgForm, AbstractControl } from '@angular/forms';
import { compareValidator } from 'src/app/shared/compare-validator.directive';
import { uniqueUsernameValidator } from 'src/app/shared/unique-username-validator.directive';
import { ErrorStateMatcher, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { UsersListComponent } from '../users/users.component';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: AddUserComponent,
    public dialogRef: MatDialogRef<UsersListComponent>,
    private formBuilder: FormBuilder,
    private usersService: UsersService
  ) { }

  addUserForm: FormGroup;
  error: string;

  @Output() usersToParent = new EventEmitter<User>();

  ngOnInit() {
    this.createAddUserForm();
  }

  createAddUserForm() {
    this.addUserForm = this.formBuilder.group({
      userName: ['',
        // null,
        Validators.required,
        // this.noWhitespaceValidator
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

  /*
  public noWhitespaceValidator(control: FormControl) {
    const isWhitespace = (control.value || '').trim().length === 0;
    const isValid = !isWhitespace;
    return isValid ? null : { 'whitespace': true };
  }
  */

  get userName() {
    return this.addUserForm.get('userName');
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
      ((result: User) => {
        console.log('El nombre del usuario desde el add-user es: ' + user.userName);
        this.dialogRef.close(user);
      }),
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
