import { Component, OnInit, ViewChild } from '@angular/core';
import { Team } from 'src/app/classes/team';
import { MatPaginator, MatTableDataSource, MatDialog, MatSort } from '@angular/material';
import { TeamsService } from 'src/app/services/teams.service';
import { AddTeamComponent } from '../add-team/add-team.component';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {

  constructor(private teamsService: TeamsService, private dialog: MatDialog) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  teams: Array<Team>;

  displayedColumns: string[] = ['name', 'sportName', 'logo', 'btnModify', 'btnDelete'];
  dataSource;
  searchKey: string;

  ngOnInit() {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Team>): void {
    this.teams = data;
    console.log('Este es el array de teams: ' + this.teams);
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Team>(this.teams);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.dataSource.filterPredicate = (data, filter) => {
      return this.displayedColumns.some(ele => {
        return ele !== 'btnModify' && ele !== 'btnDelete' && data[ele].toLowerCase().indexOf(filter) !== -1;
      });
    };
  }

  onInsert() {
    const dialogRef = this.dialog.open(AddTeamComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result) {
        // actualizar tabla
        this.ngOnInit();
      }
    });
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }
}
