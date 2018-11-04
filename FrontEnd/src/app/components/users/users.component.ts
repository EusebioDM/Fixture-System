import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../classes/user';
import { UsersService } from '../../services/users.service';
import { MatTableDataSource, MatPaginator, MatDialog, MatSort } from '@angular/material';
import { AddUserComponent } from '../add-user/add-user.component';
import { ModifyUserComponent } from '../modify-user/modify-user.component';

@Component({
  selector: 'app-users-list',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersListComponent implements OnInit {
  constructor(public usersService: UsersService, private dialog: MatDialog) { }

  @ViewChild(AddUserComponent)
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  userId: string;
  users: Array<User>;

  displayedColumns: string[] = ['userName', 'name', 'surname', 'mail', 'btnModify', 'btnDelete'];
  dataSource;
  searchKey: string;

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
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  getUser(id: string) {
    console.log('El usuario es: ' + this.usersService.getUserById(id));
  }

  openDialogAddUser() {
    const dialogRef = this.dialog.open(AddUserComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result) {
        // actualizar tabla
        this.ngOnInit();
      }
    });
  }

  // no está funcionando
  usersToParent(added: User) {
    console.log('Entró desde el hijo con: ' + added.userName);
    this.dataSource.data.add(added);
    this.dataSource.paginator = this.paginator;
  }

  openDialogConfirmDeleteUser(id: string) {
    const dialogRef = this.dialog.open(DialogConfirmToDeleteUser);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteUser(id);
      }
    });
  }

  openDialogModifyUser(user: User) {
    console.log(user.userName);
    const dialogRef = this.dialog.open(
      ModifyUserComponent,
      {
        data: { userName: user.userName, name: user.name, surname: user.surname, mail: user.mail }
      });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result) {
        // actualizar tabla
        this.ngOnInit();
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

  onRowClicked(row) {
    console.log('Row clicked: ', row);
  }
}

@Component({
  selector: 'app-add-user',
  templateUrl: 'confirmToDeleteUser.html',
  styleUrls: ['./users.component.css']
})

export class DialogConfirmToDeleteUser { }

