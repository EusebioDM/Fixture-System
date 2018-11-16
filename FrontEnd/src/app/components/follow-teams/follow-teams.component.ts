import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, MatPaginator, MatTableDataSource } from '@angular/material';
import { Team } from '../../classes/team';
import { TeamsService } from '../../services/teams.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-follow-teams',
  templateUrl: './follow-teams.component.html',
  styleUrls: ['./follow-teams.component.css']
})
export class FollowTeamsComponent implements OnInit {

  constructor(
    private usersSerivice: UsersService,
    private teamsService: TeamsService
  ) { }

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  displayedColumns: string[] = ['teamName', 'sportName', 'actionFollow'];
  dataSource;
  searchKey: string;
  temas: Array<Team>;
  teamsFollowed: Array<string>;

  ngOnInit() {
    this.getData();
  }

  private getData() {
    this.teamsService.getTeams().subscribe(
      ((data: Array<Team>) => this.result(data)),
      ((error: any) => console.log(error))
    );

    this.usersSerivice.getUserFollowedTeams().subscribe(
      ((data: Array<string>) => this.resultAux(data)),
      ((error: any) => console.log(error))
    );
  }

  private result(data: Array<Team>): void {
    this.temas = data;
    this.loadTableDataSource();
  }

  private loadTableDataSource() {
    this.dataSource = new MatTableDataSource<Team>(this.temas);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  private resultAux(data: Array<string>) {
    debugger;
    this.teamsFollowed = data;
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filter = this.searchKey.trim().toLowerCase();
  }

  onFollow(team: Team) {
    this.teamsService.addFollowedTeamToLoggedUser(team.name + '_' + team.sportName).subscribe();
  }
}
