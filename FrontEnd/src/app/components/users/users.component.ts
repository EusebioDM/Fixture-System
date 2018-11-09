import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../classes/user';
import { UsersService } from '../../services/users.service';
import { MatTableDataSource, MatPaginator, MatDialog, MatSort, MatDialogConfig } from '@angular/material';
import { AddUserComponent } from '../add-user/add-user.component';
import { ModifyUserComponent } from '../modify-user/modify-user.component';
import { YesNoDialogComponent } from '../yes-no-dialog/yes-no-dialog.component';

@Component({
  selector: 'app-users-list',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersListComponent implements OnInit {
  constructor(
    public usersService: UsersService,
    private dialog: MatDialog
  ) { }

  @ViewChild(AddUserComponent)
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['userName', 'name', 'surname', 'mail', 'role', 'btnModify', 'btnDelete'];
  dataSource;
  searchKey: string;

  userId: string;
  users: Array<User>;

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
    this.dataSource.filterPredicate = (data, filter) => {
      return this.displayedColumns.some(ele => {
        return ele !== 'btnModify' && ele !== 'btnDelete' && data[ele].toLowerCase().indexOf(filter) !== -1;
      });
    };
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

  onInsert() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(AddUserComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(
      ((user: User) => {
        if (user) {
          this.users.push(user);
          console.log('El nombre de usuario es ' + user.userName);
          this.loadTableDataSource();
        }
      })
    );
  }

  onDelete(id: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(YesNoDialogComponent, dialogConfig);
    dialogRef.componentInstance.title = 'Borrar usuario...';
    dialogRef.componentInstance.message = '¿Está seguro que quiere borrar al usuario ' + id + ' y todos los datos asociados a éste?';

    dialogRef.afterClosed().subscribe(
      ((result) => {
        if (result) {
          this.usersService.deleteUser(id).subscribe();
          this.updateDataSource(id);
        }
      })
    );
  }

  openDialogModifyUser(user: User) {
    const dialogRef = this.dialog.open(
      ModifyUserComponent,
      {
        data: { userName: user.userName, name: user.name, surname: user.surname, mail: user.mail }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // actualizar tabla
        this.ngOnInit();
      }
    });
  }

  deleteUser(userId: string) {
    this.usersService.deleteUser(userId).subscribe();
    this.updateDataSource(userId);
  }

  private updateDataSource(id: string) {

    let us;
    this.dataSource.data.forEach(user => {
      if (user.userName === id) {
        us = user;
      }
    });

    this.dataSource.data.splice(this.dataSource.data.indexOf(us), 1);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onRowClicked(row) {
    console.log('Row clicked: ', row);
  }
}

