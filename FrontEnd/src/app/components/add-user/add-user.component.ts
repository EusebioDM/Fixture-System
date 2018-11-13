import { Component, OnInit, EventEmitter, Output, Inject } from '@angular/core';
import { UsersService } from '../../services/users.service';
import { User } from '../../classes/user';
import { FormBuilder, FormGroup, Validators, FormControl, AbstractControl, ValidationErrors } from '@angular/forms';
import { compareValidator } from 'src/app/shared/compare-validator.directive';
import { uniqueUsernameValidator } from 'src/app/shared/unique-username-validator.directive';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { UsersListComponent } from '../users/users.component';
import { InstantErrorStateMatcher } from 'src/app/shared/instant-error-state-matcher';

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

  /*
  public noWhitespaceValidator(control: AbstractControl): ValidationErrors | null {
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

  addUserForm: FormGroup;
  error: string;

  @Output() usersToParent = new EventEmitter<User>();

  matcher = new InstantErrorStateMatcher();

  ngOnInit() {
    this.createAddUserForm();
  }

  createAddUserForm() {
    this.addUserForm = this.formBuilder.group({
      userName: ['',
        // null,
        Validators.required,
        // this.noWhitespaceValidator,
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

  public submit() {
    const user = this.addUserForm.value;
    this.usersService.addUser(user).subscribe(
      (() => {
        this.dialogRef.close(user);
      }),
      (error) => this.error = error
    );
  }
}
