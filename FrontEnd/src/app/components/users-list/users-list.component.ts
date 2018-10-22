import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from '../../classes/user';
import { UsersService } from '../../services/users.service';
import { MatTableDataSource, MatPaginator, MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {
  constructor(private usersService: UsersService) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  users: Array<User>;

  displayedColumns: string[] = ['userName', 'name', 'surname', 'mail'];
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
}
