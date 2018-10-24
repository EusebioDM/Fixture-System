import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../classes/user';
import { UsersService } from '../../services/users.service';
import { MatTableDataSource, MatPaginator, MatDialog } from '@angular/material';
import { AddUserComponent } from '../add-user/add-user.component';

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
  }

  private result(data: Array<User>): void {
    this.users = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<User>(this.users);
    this.dataSource.paginator = this.paginator;
  }

  getUser(id: string) {
    console.log('El usuario es: ' + this.usersService.getUserById(id));
  }

  openDialogAddUser() {
    const dialogRef = this.dialog.open(AddUserComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  openDialogConfirmDeleteUser(id: string) {
    const dialogRef = this.dialog.open(DialogConfirmToDeleteUser);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteUser(id);
      }
    });
  }

  deleteUser(id: string) {
    this.usersService.deleteUser(id).subscribe();
    this.updateDataSource(id);
  }

  private updateDataSource(id: string) {

    let us;
    this.dataSource.data.forEach(user => {
      if (user.userName === id) {
        us = user;
      }
    });

    this.dataSource.data.splice(this.dataSource.data.indexOf(us), 1);
    this.dataSource.paginator = this.paginator;
  }
}

@Component({
  selector: 'app-add-user',
  templateUrl: 'confirmToDeleteUser.html',
  styleUrls: ['./users.component.css']
})

export class DialogConfirmToDeleteUser { }

