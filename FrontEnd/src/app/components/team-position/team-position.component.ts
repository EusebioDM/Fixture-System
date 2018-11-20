import { Component, OnInit, ViewChild } from '@angular/core';
import { Sport } from 'src/app/classes/sport';
import { SportsService } from 'src/app/services/sports.service';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { TeamPosition } from 'src/app/classes/team-position';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-team-position',
  templateUrl: './team-position.component.html',
  styleUrls: ['./team-position.component.css']
})
export class TeamPositionComponent implements OnInit {

  constructor(
    private loginService: LoginService,
    private sportsService: SportsService
    ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  isAdmin: boolean;
  displayedColumns: string[] = ['team', 'position'];
  dataSource;
  searchKey: string;
  selectedSport: string;
  sports: Array<Sport>;
  teamPosition: Array<TeamPosition>;

  ngOnInit() {
    this.isAdmin = (this.loginService.getLoggedUserRole() === 'Administrator');
    this.getSportData();
  }

  private getSportData() {
    this.sportsService.getSports().subscribe(
      ((data: Array<Sport>) => { this.sports = data; }),
      ((error: any) => console.log(error))
    );
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<TeamPosition>(this.teamPosition);
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

  onSelectSport() {
    this.sportsService.getTeamPositionsBySport(this.selectedSport).subscribe(
      ((data: Array<TeamPosition>) => {
        this.teamPosition = data;
        this.loadTableDataSource();
      }),
      ((error: any) => console.log(error))
    );
  }
}
