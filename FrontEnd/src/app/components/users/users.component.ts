import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../classes/user';
import { UsersService } from '../../services/users.service';
import { MatTableDataSource, MatPaginator, MatSnackBar, MatDialog } from '@angular/material';

@Component({
  selector: 'app-users-list',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersListComponent implements OnInit {
  constructor(private usersService: UsersService, public dialog: MatDialog) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  users: Array<User>;

  displayedColumns: string[] = ['userName', 'name', 'surname', 'mail', 'btnModify', 'btnBorrar'];
  dataSource;

  ngOnInit() {
    this.usersService.getUsers().subscribe(
      ((data: Array<User>) => this.result(data)),
      ((error: any) => console.log(error))
    );

    this.dataSource = new MatTableDataSource<User>(this.users);
    this.dataSource.paginator = this.paginator;
  }

  private result(data: Array<User>): void {
    this.users = data;
    console.log(this.users);
  }

  deleteUser(id: string) {
    console.log('User to delete: ' + id);
    this.usersService.deleteUser(id);
  }

  getUser(id: string) {
    console.log('El usuario es: ' + this.usersService.getUserById(id));
  }

  openDialog() {
    const dialogRef = this.dialog.open(DialogContentExampleDialog);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}

@Component({
  selector: 'app-add-user',
  templateUrl: 'addUserForm.html',
  styleUrls: ['./users.component.css']
})

export class DialogContentExampleDialog { }
