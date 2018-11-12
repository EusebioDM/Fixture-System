import { Component, OnInit, ViewChild, Pipe, PipeTransform } from '@angular/core';
import { EncountersService } from 'src/app/services/encounters.service';
import { MatSort, MatPaginator, MatTableDataSource, MatDialogConfig, MatDialog } from '@angular/material';
import { Encounter } from 'src/app/classes/encounter';
import { Sport } from 'src/app/classes/sport';
import { SportsService } from 'src/app/services/sports.service';
import { TeamsService } from 'src/app/services/teams.service';
import { Team } from 'src/app/classes/team';
import { AddEncounterComponent } from '../add-encounter/add-encounter.component';

@Component({
  selector: 'app-encounters',
  templateUrl: './encounters.component.html',
  styleUrls: ['./encounters.component.css']
})
export class EncountersComponent implements OnInit {

  constructor(
    private sportsService: SportsService,
    private teamsService: TeamsService,
    private encountersService: EncountersService,
    private dialog: MatDialog
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['sport', 'teams', 'date', 'actionDelete'];
  dataSource;
  searchKey: string;
  sports: Array<Sport>;
  teams: Array<Team>;
  encounters: Array<Encounter>;

  ngOnInit() {
    this.getSportsData();
    this.getTeamsData();
    this.getEncountersData();
  }

  getSportsData(): any {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => { this.sports = data; }),
      ((error: any) => console.log(error))
    );
  }

  getTeamsData(): any {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => { this.teams = data; }),
      ((error: any) => console.log(error))
    );
  }

  private getEncountersData() {
    this.encountersService.getEncounters().subscribe(
      ((data: Array<Encounter>) => this.result(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Encounter>): void {
    this.encounters = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Encounter>(this.encounters);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  onInsert() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(AddEncounterComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(
      ((/*user: User*/) => {
        // if (user) {
          // this.getData();
       // }
      })
    );
  }
}

@Pipe({ name: 'listToSingleString' })
export class ListToSingleString implements PipeTransform {
  transform(teamsIds: Array<string>): string {
    let singleStringTeams = '';
    teamsIds.forEach(teamId => {
      const teamName = teamId.split('_');
      singleStringTeams += teamName[0] + ' - ';
    });

    return singleStringTeams;
  }
}
