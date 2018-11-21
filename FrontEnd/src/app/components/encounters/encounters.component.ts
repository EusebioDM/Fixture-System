import { Component, OnInit, ViewChild, Pipe, PipeTransform } from '@angular/core';
import { EncountersService } from 'src/app/services/encounters.service';
import { MatSort, MatPaginator, MatTableDataSource, MatDialogConfig, MatDialog } from '@angular/material';
import { Encounter } from 'src/app/classes/encounter';
import { Sport } from 'src/app/classes/sport';
import { SportsService } from 'src/app/services/sports.service';
import { TeamsService } from 'src/app/services/teams.service';
import { Team } from 'src/app/classes/team';
import { AddEncounterComponent } from '../add-encounter/add-encounter.component';
import { YesNoDialogComponent } from '../yes-no-dialog/yes-no-dialog.component';
import { AddEncountersResultComponent } from '../add-encounters-result/add-encounters-result.component';
import { GenerateFixtureComponent } from '../generate-fixture/generate-fixture.component';

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

  selectedSport: string;
  startDate: Date;
  endDate: Date;
  displayedColumns: string[] = ['sport', 'teams', 'date', 'actionResults', 'actionDelete'];
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
    const dialogRef = this.dialog.open(AddEncounterComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(
      ((encounter: Encounter) => {
        if (encounter) {
          this.ngOnInit();
        }
      })
    );
  }

  onGenerateFixture() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(GenerateFixtureComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(
      ((isGenerated: boolean) => {
        if (isGenerated) {
          this.ngOnInit();
        }
      })
    );
  }

  onAddResults(encounter: Encounter) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(AddEncountersResultComponent, dialogConfig);
    dialogRef.componentInstance.encounter = encounter;

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.ngOnInit();
      }
    });
  }

  onDelete(encounter: Encounter) {

    const encounterId = encounter.id;

    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    const dialogRef = this.dialog.open(YesNoDialogComponent, dialogConfig);
    dialogRef.componentInstance.title = 'Borrar encuentro...';
    dialogRef.componentInstance.message = '¿Está seguro que quiere borrar el encuentro?';

    dialogRef.afterClosed().subscribe(
      ((result) => {
        if (result) {
          this.encountersService.deleteEncounter(encounterId).subscribe(
            () => { this.ngOnInit(); }
          );
        }
      })
    );
  }

  onFilterBySport() {
    if (this.selectedSport) {
      this.sportsService.getEncountersBySport(this.selectedSport).subscribe(
        ((data: Array<Encounter>) => {
          this.encounters = data;
          this.loadTableDataSource();
        }),
        ((error: any) => console.log(error))
      );
    } else {
      this.getEncountersData();
    }
  }

  onFilterByDate() {

    if (this.startDate && this.endDate) {
      const startMouth = this.startDate.getMonth() + 1;
      const endMouth = this.endDate.getMonth() + 1;

      const dateTo = this.startDate.getFullYear() + '-' + startMouth + '-' + this.startDate.getDate();
      const dateFor = this.endDate.getFullYear() + '-' + endMouth + '-' + this.endDate.getDate();

      this.encountersService.getEnconutersFromToDate(dateTo, dateFor).subscribe(
        ((data: Array<Encounter>) => {
          this.encounters = data;
          this.loadTableDataSource();
        }),
        ((error: any) => console.log(error))
      );
    } else {
      this.getEncountersData();
    }
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
