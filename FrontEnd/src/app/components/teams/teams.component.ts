import { Component, OnInit, ViewChild } from '@angular/core';
import { Team } from 'src/app/classes/team';
import { MatPaginator, MatTableDataSource, MatDialog, MatSort, MatDialogConfig } from '@angular/material';
import { TeamsService } from 'src/app/services/teams.service';
import { AddTeamComponent } from '../add-team/add-team.component';
import { YesNoDialogComponent } from '../yes-no-dialog/yes-no-dialog.component';
import { ModifyTeamComponent } from '../modify-team/modify-team.component';

@Component({
  selector: 'app-teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.css']
})
export class TeamsComponent implements OnInit {

  constructor(
    private teamsService: TeamsService,
    private dialog: MatDialog
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['name', 'sportName', 'logo', 'btnModify', 'btnDelete'];
  dataSource;
  searchKey: string;
  teams: Array<Team>;

  ngOnInit() {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Team>): void {
    this.teams = data;
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

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  onInsert() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(AddTeamComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(
      ((team: Team) => {
        if (team) {
          this.teams.push(team);
          this.loadTableDataSource();
        }
      })
    );
  }

  onDelete(teamName: string, sportId: string) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(YesNoDialogComponent, dialogConfig);
    dialogRef.componentInstance.title = 'Borrar equipo...';
    dialogRef.componentInstance.message = '¿Está seguro que quiere borrar al equipo ' + teamName
      + ' perteneciente al deporte ' + sportId + '.';

    dialogRef.afterClosed().subscribe(
      ((result) => {
        if (result) {
          this.teamsService.deleteTeam(teamName + '_' + sportId).subscribe();
          this.updateDataSource(teamName + '_' + sportId);
        }
      })
    );
  }

  private updateDataSource(id: string) {
    let tm;
    this.dataSource.data.forEach(team => {
      const idTeam = (team.name + '_' + team.sportName);
      if (idTeam === id) {
        tm = team;
      }
    });

    this.dataSource.data.splice(this.dataSource.data.indexOf(tm), 1);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onModify(team: Team) {
    const dialogRef = this.dialog.open(
      ModifyTeamComponent,
      {
        data: { name: team.name, sportName: team.sportName, logo: team.logo }
      });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result) {
        // actualizar tabla
        this.ngOnInit();
      }
    });
  }
}
